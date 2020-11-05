import * as React from "react";
import Table from "react-bootstrap/Table";
import Form from "react-bootstrap/Form";
import Button from "react-bootstrap/Button";
import Group from "../api/Group";
import Dictionary from "../util/Dictionary";

interface IGroupTableState {
  nameInputValue: string;
}

export interface IGroupTableProps {
  group: Group;
  expenseAmounts?: Dictionary<number>;
  onAddNewMember: (name: string) => void;
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

  handleOnNameInputChange = (event: React.BaseSyntheticEvent): void => {
    const eventTargetValue = event.target.value;
    this.setState({ nameInputValue: eventTargetValue });
  };

  handleOnAddNewMember = (): void => {
    const { onAddNewMember } = this.props;
    const { nameInputValue } = this.state;
    onAddNewMember(nameInputValue);
    this.setState({ nameInputValue: "" });
  };

  renderTableContent = (): React.ReactNode => {
    const tableContent = [];
    const { nameInputValue } = this.state;
    const { group, expenseAmounts } = this.props;

    tableContent.push(
      group.Users?.map((user) => (
        <tr>
          <td>{user.Name}</td>
          <td>{expenseAmounts ? expenseAmounts[user.Id]?.toFixed(2) : ""}</td>
        </tr>
      ))
    );
    tableContent.push(
      <tr>
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
      </tr>
    );
    return tableContent;
  };

  render(): JSX.Element {
    return (
      <div>
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
      </div>
    );
  }
}
