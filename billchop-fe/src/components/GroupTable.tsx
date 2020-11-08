import * as React from "react";
import Table from "react-bootstrap/Table";
import Form from "react-bootstrap/Form";
import Button from "react-bootstrap/Button";
import Group from "../backend/models/Group";
import Dictionary from "../util/Dictionary";

interface IGroupTableState {
  nameInputValue: string;
}

export interface IGroupTableProps {
  group: Group;
  expenseAmounts: Dictionary<number>;
  colorCode?: boolean;
  onAddNewMember?: (name: string) => void;
  showMembersOnlyWithExpenses?: boolean;
}

export default class GroupTable extends React.Component<
  IGroupTableProps,
  IGroupTableState
> {
  constructor(props: IGroupTableProps) {
    super(props);
    this.state = {
      nameInputValue: "",
    };
  }

  getExpenseStyling(
    expense: number | undefined | null,
  ): React.CSSProperties | undefined {
    const { colorCode } = this.props;

    if (!colorCode || !expense || (expense > -0.01 && expense < 0.01))
      return undefined;

    if (expense > 0) return { color: "green" };

    return { color: "red" };
  }

  handleOnNameInputChange = (event: React.BaseSyntheticEvent): void => {
    const eventTargetValue = event.target.value;
    this.setState({ nameInputValue: eventTargetValue });
  };

  handleOnAddNewMember = (): void => {
    const { onAddNewMember } = this.props;
    const { nameInputValue } = this.state;
    if (onAddNewMember) onAddNewMember(nameInputValue);
    this.setState({ nameInputValue: "" });
  };

  renderTableContent = (): React.ReactNode => {
    const tableContent = [];
    const { nameInputValue } = this.state;
    const {
      group,
      expenseAmounts,
      onAddNewMember,
      showMembersOnlyWithExpenses,
    } = this.props;

    let groupUsers = group.Users;

    if (showMembersOnlyWithExpenses && expenseAmounts !== undefined) {
      groupUsers = group.Users?.filter((user) => {
        return expenseAmounts[user.Id] !== undefined;
      });
    }

    tableContent.push(
      groupUsers.map((user) => {
        const expense = expenseAmounts[user.Id]
          ? expenseAmounts[user.Id].toFixed(2)
          : "0.00";
        return (
          <tr key={user.Id}>
            <td>{user.Name}</td>
            <td style={this.getExpenseStyling(expenseAmounts[user.Id])}>
              {expense}
            </td>
          </tr>
        );
      }),
    );
    if (onAddNewMember !== undefined) {
      tableContent.push(
        <tr key="addNewMemberRow">
          <td>
            <Form.Control
              placeholder="New member's name:"
              onChange={this.handleOnNameInputChange}
              value={nameInputValue ?? ""}
            />
          </td>
          <td>
            <Button variant="outline" onClick={this.handleOnAddNewMember}>
              Add
            </Button>
          </td>
        </tr>,
      );
    }
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
