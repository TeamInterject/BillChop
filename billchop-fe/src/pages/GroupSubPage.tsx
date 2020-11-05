import Axios from "axios";
import * as React from "react";
import Group from "../api/Group";
import Loan from "../api/Loan";
import { CURRENT_USER_ID } from "../api/User";
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
  expenseAmounts?: Dictionary<number>;
}

export default class GroupSubPage extends React.Component<
  IGroupSubPageProps,
  IGroupSubPageState
> {
  constructor(props: IGroupSubPageProps) {
    super(props);
    this.state = {
      expenseAmounts: undefined,
    };
  }

  componentDidMount(): void {
    this.getUserLoans();
  }

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

    Axios.post(BASE_URL_API_BILLS, {
      name,
      total,
      loanerId: CURRENT_USER_ID,
      groupContextId: group.Id,
    }).then(() => this.getUserLoans());
  };

  render(): JSX.Element {
    const { group, onAddNewMember } = this.props;
    const { expenseAmounts } = this.state;

    return (
      <div className="group-page__sub-page-container">
        <GroupPageHeader onAddNewBill={this.handleOnAddNewBill} />
        <GroupTable
          group={group}
          expenseAmounts={expenseAmounts}
          onAddNewMember={onAddNewMember}
        />
      </div>
    );
  }
}
