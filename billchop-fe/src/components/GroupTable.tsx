import * as React from "react";
import Table from "react-bootstrap/Table";
import Group from "../backend/models/Group";
import User from "../backend/models/User";
import toEuros from "../util/toEuros";
import Dictionary from "../util/Dictionary";

export interface IGroupTableProps {
  group: Group;
  expenseAmounts: Dictionary<number>;
  currentUserId: string;
  colorCode?: boolean;
  showMembersOnlyWithExpenses?: boolean;
  skipCurrentUserAmount?: boolean;
  loanerId?: string;
}

function GroupTableRow(props: {
  user: User, 
  expenseAmounts: Dictionary<number>, 
  currentUserId: string,
  colorCode?: boolean, 
  loanerId?: string,
  skipAmount?: boolean,
}) {
  const {user, expenseAmounts, currentUserId, colorCode, loanerId, skipAmount} = props;

  function getExpenseStyling(expense: number | undefined | null): React.CSSProperties | undefined {
    if (!colorCode || !expense || (expense > -0.01 && expense < 0.01))
      return undefined;

    if (expense > 0) 
      return { color: "green" };

    return { color: "red" };
  }

  const formattedExpense = expenseAmounts[user.Id] ? toEuros(expenseAmounts[user.Id]) : "0.00€";
  return (
    <tr>
      <td>{user.Id === currentUserId ? "You" : user.Name} {user.Id === loanerId && "(Payer)"}</td>
      <td style={getExpenseStyling(expenseAmounts[user.Id])}>
        {skipAmount ? "-" : formattedExpense}
      </td>
    </tr>
  );
}

export default class GroupTable extends React.Component<IGroupTableProps> {
  currentUserComparer = (userA: User, userB: User): number => {
    const {
      currentUserId,
    } = this.props;

    if (userA.Id === currentUserId)
      return -1;

    if (userB.Id === currentUserId)
      return 1;

    return 0;
  };

  renderTableContent = (): React.ReactNode => {
    const {
      group,
      expenseAmounts,
      currentUserId,
      showMembersOnlyWithExpenses,
      loanerId,
      colorCode,
      skipCurrentUserAmount,
    } = this.props;

    let groupUsers = [...group.Users];

    if (showMembersOnlyWithExpenses && expenseAmounts !== undefined) {
      groupUsers = group.Users.filter((user) => {
        return expenseAmounts[user.Id] !== undefined;
      });
    }

    groupUsers.sort(this.currentUserComparer);
    
    return groupUsers
      .map((user) => (
        <GroupTableRow 
          key={user.Id} 
          user={user}
          expenseAmounts={expenseAmounts}
          currentUserId={currentUserId}
          colorCode={colorCode}
          loanerId={loanerId}
          skipAmount={skipCurrentUserAmount && user.Id === currentUserId}
        />),
      );
  };

  render(): JSX.Element {
    return (
      <div className="m-2">
        <Table striped bordered hover>
          <thead>
            <tr>
              <th>Name</th>
              <th>Amount</th>
            </tr>
          </thead>
          <tbody>{this.renderTableContent()}</tbody>
        </Table>
      </div>
    );
  }
}
