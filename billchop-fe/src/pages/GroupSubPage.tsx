import * as React from "react";
import { Col, Container, Row } from "react-bootstrap";
import { produce } from "immer";
import BillClient from "../backend/clients/BillClient";
import LoanClient from "../backend/clients/LoanClient";
import Bill from "../backend/models/Bill";
import Group from "../backend/models/Group";
import Loan from "../backend/models/Loan";
import BillsListAccordion from "../components/BillsListAccordion";
import GroupPageHeader from "../components/GroupPageHeader";
import GroupTable from "../components/GroupTable";
import MonthlySpendingGraph, { IBarChartDataset } from "../components/BarChart";
import Dictionary from "../util/Dictionary";
import getMonthName from "../util/Months";
import UserContext from "../backend/helpers/UserContext";

export interface IGroupSubPageProps {
  group: Group;
  onAddNewMember: (name: string) => void;
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

  private loanClient = new LoanClient();

  constructor(props: IGroupSubPageProps) {
    super(props);
    this.state = {
      bills: [],
      expenseAmounts: undefined,
    };
  }

  componentDidMount(): void {
    this.getUserLoans();
    this.getGroupBills();
  }

  componentDidUpdate(prevProps: IGroupSubPageProps): void {
    const { group } = this.props;
    if (group.Id !== prevProps.group.Id) {
      this.getUserLoans();
      this.getGroupBills();
    }
  }

  getGroupBills = (): void => {
    const { group } = this.props;

    this.billClient
      .getBills({ groupId: group.Id })
      .then((bills) => this.setState({ bills }));
  };

  getUserLoans = (): void => {
    const { group } = this.props;

    const currentUserId = UserContext.authenticatedUser.Id;

    this.loanClient
      .getProvidedLoans({ loanerId: currentUserId, groupId: group.Id })
      .then((loans) => {
        const expenseAmounts = this.buildUserExpenses(loans);
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
    const { bills } = this.state;

    const currentUserId = UserContext.authenticatedUser.Id;

    this.billClient
      .postBill({
        name,
        total,
        loanerId: currentUserId,
        groupContextId: group.Id,
      })
      .then((bill) => {
        const newBills = produce(bills, (draftBills) => {
          draftBills.unshift(bill);
        });
        this.setState({ bills: newBills });
        this.getUserLoans();
      });
  };

  private buildUserExpenses(loans: Loan[]): Dictionary<number> {
    const expenseAmounts: Dictionary<number> = {};

    loans.forEach((loan: Loan) => {
      expenseAmounts[loan.Loanee.Id] = expenseAmounts[loan.Loanee.Id] ?? 0;
      expenseAmounts[loan.Loanee.Id] += loan.Amount;
    });

    return expenseAmounts;
  }

  render(): JSX.Element {
    const { group, onAddNewMember } = this.props;
    const { expenseAmounts, bills } = this.state;

    return (
      <div className="group-page__sub-page-container">
        <Container>
          <Row>
            <Col>
              <GroupPageHeader onAddNewBill={this.handleOnAddNewBill} />
            </Col>
          </Row>
          <Row>
            <Col>
              <GroupTable
                group={group}
                expenseAmounts={expenseAmounts}
                onAddNewMember={onAddNewMember}
              />
            </Col>
            <Col>
              <MonthlySpendingGraph
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
        </Container>
      </div>
    );
  }
}
