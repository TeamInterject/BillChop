import Axios from "axios";
import * as React from "react";
import Bill from "../api/Bill";
import Group from "../api/Group";
import Loan from "../api/Loan";
import { CURRENT_USER_ID } from "../api/User";
import BillsListAccordion from "../components/BillsListAccordion";
import GroupPageHeader from "../components/GroupPageHeader";
import GroupTable from "../components/GroupTable";
import Dictionary from "../util/Dictionary";

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
        <GroupPageHeader onAddNewBill={this.handleOnAddNewBill} />
        <GroupTable
          group={group}
          expenseAmounts={expenseAmounts}
          onAddNewMember={onAddNewMember}
        />
        <BillsListAccordion group={group} bills={bills} />
      </div>
    );
  }
}
