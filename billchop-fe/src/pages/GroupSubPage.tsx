import * as React from "react";
import { Col, Row } from "react-bootstrap";
import BillClient from "../backend/clients/BillClient";
import Bill from "../backend/models/Bill";
import Group from "../backend/models/Group";
import BillsListAccordion from "../components/BillsListAccordion";
import GroupPageHeader from "../components/GroupPageHeader";
import GroupTable from "../components/GroupTable";
import BarChart, { IBarChartDataset } from "../components/BarChart";
import Dictionary from "../util/Dictionary";
import getMonthName from "../util/getMonthName";
import UserContext from "../backend/helpers/UserContext";
import "../styles/groups-page.css";
import PaymentClient from "../backend/clients/PaymentClient";
import produce from "immer";

export enum LoanType {
  Provided,
  Received,
}

export interface IGroupSubPageProps {
  group: Group;
  onAddNewMember: (name: string) => void;
  onOpenSettleUp: () => void;
}

export interface IGroupSubPageState {
  bills: Bill[];
  expenseAmounts?: Dictionary<number>;
}

export default class GroupSubPage extends React.Component<
  IGroupSubPageProps,
  IGroupSubPageState
> {
  private billClient = new BillClient();

  private paymentClient = new PaymentClient();

  constructor(props: IGroupSubPageProps) {
    super(props);
    this.state = {
      bills: [],
      expenseAmounts: undefined,
    };
  }

  componentDidMount(): void {
    this.getExpectedPayments();
    this.getGroupBills();
  }

  componentDidUpdate(prevProps: IGroupSubPageProps): void {
    const { group } = this.props;
    if (group.Id !== prevProps.group.Id) {
      this.getExpectedPayments();
      this.getGroupBills();
    }
  }

  getGroupBills = (): void => {
    const { group } = this.props;

    this.billClient
      .getBills({ groupId: group.Id })
      .then((bills) => {
        bills = bills.sort((x, y) => y.CreationTime.localeCompare(x.CreationTime));
        this.setState({ bills });
      });
  };

  getExpectedPayments = (): void => {
    const { group } = this.props;

    this.paymentClient.getExpectedPayments({
      userId: UserContext.authenticatedUser.Id,
      groupId: group.Id,
    }).then((payments) => {
      const expenseAmounts: Dictionary<number> = {};
      const currentUserId = UserContext.authenticatedUser.Id;

      produce(expenseAmounts, (draftAmounts) => {
        payments.forEach((payment) => {
          expenseAmounts[payment.Receiver.Id] = draftAmounts[payment.Receiver.Id] ?? 0;
          if (payment.Payer.Id === currentUserId) {
            expenseAmounts[payment.Receiver.Id] -= payment.Amount;
          } else {
            expenseAmounts[payment.Receiver.Id] += payment.Amount;
          }
        });
      });

      this.setState({ expenseAmounts });
    });
  };

  getRecentGroupSpendingDatasets = (): IBarChartDataset<number>[] => {
    let { bills } = this.state;
    const datasets: IBarChartDataset<number>[] = [];
    const currentMonth = new Date().getMonth();

    bills = bills.filter((bill) => {
      const billMonth = new Date(bill.CreationTime).getMonth();
      return (
        billMonth === currentMonth ||
        billMonth === currentMonth - 1 ||
        billMonth === currentMonth - 2
      );
    });

    bills.forEach((bill) => {
      const billMonth = new Date(bill.CreationTime).getMonth();

      datasets[billMonth] = datasets[billMonth] ?? {
        label: getMonthName(billMonth),
        data: 0,
      };
      datasets[billMonth].data += bill.Total;
    });

    return Object.values(datasets);
  };

  handleOnAddNewBill = (name: string, total: number): void => {
    const { group } = this.props;

    const currentUserId = UserContext.authenticatedUser.Id;

    this.billClient
      .postBill({
        name,
        total,
        loanerId: currentUserId,
        groupContextId: group.Id,
      })
      .then(() => {
        this.getGroupBills();
        this.getExpectedPayments();
      });
  };

  render(): JSX.Element {
    const { group, onAddNewMember, onOpenSettleUp } = this.props;
    const { expenseAmounts, bills } = this.state;

    return (
      <div className="p-3 h-100 w-100 subpage-container">
        <Row>
          <Col>
            <GroupPageHeader
              groupId={group.Id}
              onAddNewBill={this.handleOnAddNewBill}
              onAddNewMember={onAddNewMember}
              onOpenSettleUp={onOpenSettleUp}
            />
          </Col>
        </Row>
        <Row>
          <Col>
            <GroupTable
              group={group}
              expenseAmounts={expenseAmounts ?? {}}
              colorCode
            />
          </Col>
          <Col>
            <BarChart
              datasets={this.getRecentGroupSpendingDatasets()}
              headingText="Total group spending"
            />
          </Col>
        </Row>
        <Row>
          <Col>
            <BillsListAccordion group={group} bills={bills} />
          </Col>
        </Row>
      </div>
    );
  }
}
