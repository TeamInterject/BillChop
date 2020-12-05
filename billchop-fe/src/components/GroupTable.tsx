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
  loanerId?: string;
}

function GroupTableRow(props: {
  user: User, 
  expenseAmounts: Dictionary<number>, 
  currentUserId: string,
  colorCode?: boolean, 
  loanerId?: string,
}) {
  const {user, expenseAmounts, currentUserId, colorCode, loanerId} = props;

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
        {formattedExpense}
      </td>
    </tr>
  );
}

export default class GroupTable extends React.Component<IGroupTableProps> {
  renderTableContent = (): React.ReactNode => {
    const {
      group,
      expenseAmounts,
      currentUserId,
      showMembersOnlyWithExpenses,
      loanerId,
      colorCode,
    } = this.props;

    let groupUsers = [...group.Users];

    if (showMembersOnlyWithExpenses && expenseAmounts !== undefined) {
      groupUsers = group.Users?.filter((user) => {
        return expenseAmounts[user.Id] !== undefined;
      });
    }

    groupUsers.sort((user) => user.Id === currentUserId ? -1 : 0);
    
    return groupUsers
      .map((user) => (
        <GroupTableRow 
          key={user.Id} 
          user={user}
          expenseAmounts={expenseAmounts}
          currentUserId={currentUserId}
          colorCode={colorCode}
          loanerId={loanerId}
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
