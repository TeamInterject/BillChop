import * as React from "react";
import Table from "react-bootstrap/Table";
import UserContext from "../backend/helpers/UserContext";
import Group from "../backend/models/Group";
import User from "../backend/models/User";
import toEuros from "../util/toEuros";
import Dictionary from "../util/Dictionary";

export interface IGroupTableProps {
  group: Group;
  expenseAmounts: Dictionary<number>;
  colorCode?: boolean;
  showMembersOnlyWithExpenses?: boolean;
  loanerId?: string;
}

function GroupTableRow(props: {
  user: User, 
  expenseAmounts: Dictionary<number>, 
  colorCode?: boolean, 
  loanerId?: string,
}) {
  const {user, expenseAmounts, colorCode, loanerId} = props;

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
      <td>{user.Id === UserContext.authenticatedUser.Id ? "You" : user.Name} {user.Id === loanerId && "(Payer)"}</td>
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
      showMembersOnlyWithExpenses,
      loanerId,
      colorCode,
    } = this.props;

    let groupUsers = [...group.Users];
    const currentUserId = UserContext.authenticatedUser.Id;

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
