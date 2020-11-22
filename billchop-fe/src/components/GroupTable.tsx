import * as React from "react";
import Table from "react-bootstrap/Table";
import UserContext from "../backend/helpers/UserContext";
import Group from "../backend/models/Group";
import Dictionary from "../util/Dictionary";

export interface IGroupTableProps {
  group: Group;
  expenseAmounts: Dictionary<number>;
  colorCode?: boolean;
  showMembersOnlyWithExpenses?: boolean;
  loanerId?: string;
}

export default class GroupTable extends React.Component<IGroupTableProps> {
  getExpenseStyling(
    expense: number | undefined | null,
  ): React.CSSProperties | undefined {
    const { colorCode } = this.props;

    if (!colorCode || !expense || (expense > -0.01 && expense < 0.01))
      return undefined;

    if (expense > 0) return { color: "green" };

    return { color: "red" };
  }

  renderTableContent = (): React.ReactNode => {
    const tableContent = [];
    const {
      group,
      expenseAmounts,
      showMembersOnlyWithExpenses,
      loanerId,
    } = this.props;

    let groupUsers = group.Users;
    const currentUserId = UserContext.authenticatedUser.Id;

    if (showMembersOnlyWithExpenses && expenseAmounts !== undefined) {
      groupUsers = group.Users?.filter((user) => {
        return expenseAmounts[user.Id] !== undefined;
      });
    }

    tableContent.push(
      groupUsers.sort((user) => user.Id === currentUserId ? -1 : 0)
        .map((user) => {
          const expense = expenseAmounts[user.Id]
            ? expenseAmounts[user.Id].toFixed(2).replace("-0.00", "0.00")
            : "0.00";
          return (
            <tr key={user.Id}>
              <td>{user.Id === UserContext.authenticatedUser.Id ? "You" : user.Name} {user.Id === loanerId && "(Payer)"}</td>
              <td style={this.getExpenseStyling(expenseAmounts[user.Id])}>
                {expense}
              </td>
            </tr>
          );
        }),
    );
    return tableContent;
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
