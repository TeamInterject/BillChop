import Axios from "axios";
import * as React from "react";
import { Col, Container, Row } from "react-bootstrap";
import Bill from "../backend/models/Bill";
import Group from "../backend/models/Group";
import Loan from "../backend/models/Loan";
import { CURRENT_USER_ID } from "../backend/models/User";
import BillsListAccordion from "../components/BillsListAccordion";
import GroupPageHeader from "../components/GroupPageHeader";
import GroupTable from "../components/GroupTable";
import MonthlySpendingGraph, { IBarChartDataset } from "../components/BarChart";
import Dictionary from "../util/Dictionary";
import getMonthName from "../util/Months";

const BASE_URL_API_BILLS = "https://localhost:44333/api/bills/";
const BASE_URL_API_LOANS = "https://localhost:44333/api/loans/";

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

    Axios.get(`${BASE_URL_API_BILLS}?groupId=${group.Id}`).then((response) => {
      this.setState({ bills: response.data });
    });
  };

  getUserLoans = (): void => {
    const { group } = this.props;

    Axios.get(
      `${BASE_URL_API_LOANS}?loanerId=${CURRENT_USER_ID}&groupId=${group.Id}`
    ).then((loansResponse) => {
      const expenseAmounts: Dictionary<number> = {};

      loansResponse.data.forEach((loan: Loan) => {
        expenseAmounts[loan.Loanee.Id] = expenseAmounts[loan.Loanee.Id] ?? 0;
        expenseAmounts[loan.Loanee.Id] += loan.Amount;
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

    let month = -1;
    bills.forEach((bill) => {
      const billMonth = new Date(bill.CreationTime).getMonth();
      if (billMonth !== month) {
        month = billMonth;
        datasets.push({ label: getMonthName(month), data: 0 });
      }
      datasets[datasets.length - 1].data += bill.Total;
    });

    return datasets;
  };

  handleOnAddNewBill = (name: string, total: number): void => {
    const { group } = this.props;
    const { bills } = this.state;

    Axios.post(BASE_URL_API_BILLS, {
      name,
      total,
      loanerId: CURRENT_USER_ID,
      groupContextId: group.Id,
    }).then((response) => {
      bills.unshift(response.data as Bill);
      this.setState({ bills });
      this.getUserLoans();
    });
  };

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
