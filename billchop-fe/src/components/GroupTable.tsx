import * as React from "react";
import { BillSplitInput } from "./BillSplitInput";
import Table from "react-bootstrap/Table";
import Group from "../api/Group";
import Form from "react-bootstrap/Form";
import Button from "react-bootstrap/Button";
import Axios from "axios";

interface IGroupTableProps {
  group: Group;
}

interface IGroupTableState {
  group: Group;
  inputValue: string;
  expenseAmounts?: number[];
}

export default class GroupTable extends React.Component<
  IGroupTableProps,
  IGroupTableState
> {
  constructor(props: IGroupTableProps) {
    super(props);
    this.renderTableContent = this.renderTableContent.bind(this);
    this.handleOnSplit = this.handleOnSplit.bind(this);
  }

  handleOnClick() {
    Axios.post("url").then((response) => {
      this.setState({
        ...this.state,
        group: response.data,
      });
    });
  }

  handleOnSplit(amount: number) {
    Axios.post("url").then((response) => {
      this.setState({
        ...this.state,
        expenseAmounts: response.data,
      });
    });
  }

  renderTableContent() {
    const tableContent = this.props.group.users.map((user, index) => (
      <tr>
        <td>user.name</td>
        <td>this.state.expenseAmounts[index] || ""</td>
      </tr>
    ));
    tableContent.push(
      <tr>
        <td>
          <Form.Control
            placeholder="Expense new member's name:"
            onChange={(e) =>
              this.setState({ ...this.state, inputValue: e.target.value })
            }
          />
        </td>
        <td>
          <Button variant="outline" onClick={this.handleOnClick}>
            Add
          </Button>
        </td>
      </tr>
    );
    return tableContent;
  }

  render() {
    return (
      <div>
        <BillSplitInput onSplit={this.handleOnSplit} />
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
