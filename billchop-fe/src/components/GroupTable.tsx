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
}

export default class GroupTable extends React.Component<
  IGroupTableProps,
  IGroupTableState
> {
  constructor(props: IGroupTableProps) {
    super(props);
    this.renderTableContent = this.renderTableContent.bind(this);
  }

  handleOnClick() {
    Axios.post("url").then((response) => {
      this.setState({
        ...this.state,
        group: response.data as Group,
      });
    });
  }

  renderTableContent() {
    const tableContent = this.props.group.users.map((user) => (
      <tr>
        <td>user.name</td>
        <td></td>
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
        <BillSplitInput />
        <div className="m-2">
          <Table striped bordered hover>
            <thead>
              <tr>
                <th>Name</th>
                <th>Amount</th>
              </tr>
            </thead>
            {this.renderTableContent()}
            <tbody>
              <tr>
                <td>Mark</td>
                <td></td>
              </tr>
              <tr>
                <td>Jacob</td>
                <td></td>
              </tr>
            </tbody>
          </Table>
        </div>
      </div>
    );
  }
}
