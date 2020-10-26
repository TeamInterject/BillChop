import * as React from "react";
import { BillSplitInput } from "./BillSplitInput";
import Table from "react-bootstrap/Table";
import Group from "../api/Group";
import Form from "react-bootstrap/Form";
import Button from "react-bootstrap/Button";
import Axios from "axios";
import User from "../api/User";
import { CURRENT_USER_ID } from "../api/User";
import Loan from "../api/Loan";

const BASE_URL_API_GROUPS = "https://localhost:44333/api/groups/";
const BASE_URL_API_USERS = "https://localhost:44333/api/users/";
const BASE_URL_API_BILLS = "https://localhost:44333/api/bills/";

interface IGroupTableProps {
  group: Group;
}

interface IGroupTableState {
  group?: Group;
  nameInputValue: string;
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
    this.handleOnAddNewMember = this.handleOnAddNewMember.bind(this);
    this.state = {
      group: undefined,
      nameInputValue: "",
      expenseAmounts: undefined
    }
  }

  handleOnAddNewMember() {
    Axios.post(BASE_URL_API_USERS, {
      name: this.state.nameInputValue,
    }).then((userResponse) => {
      const newUserId = (userResponse.data as User).Id;
      const group = this.state.group ?? this.props.group;

      Axios.post(
        BASE_URL_API_GROUPS + group.Id + "/add-user/" + newUserId
      ).then((response) => {
        this.setState({
          group: response.data,
          nameInputValue: "",
          expenseAmounts: undefined
        });
      });
    });
  }

  handleOnSplit(amount: number) {
    const group = this.state.group ?? this.props.group;

    Axios.post(BASE_URL_API_BILLS, {
      name: "Bill",
      total: amount,
      loanerId: CURRENT_USER_ID,
      groupContextId: group.Id,
    }).then((response) => {
      console.log(response.data);
      this.setState({
        ...this.state,
        expenseAmounts: response.data.Loans.map((loan: Loan) => loan.Amount),
      });
    });
  }

  renderTableContent() {
    const tableContent = [];
    const group = this.state?.group ?? this.props.group;
    const expenseAmounts = this.state?.expenseAmounts;
    tableContent.push(
      group.Users?.map((user, index) => (
        <tr>
          <td>{user.Name}</td>
          <td>{expenseAmounts ? expenseAmounts[index] : ""}</td>
        </tr>
      ))
    );
    tableContent.push(
      <tr>
        <td>
          <Form.Control
            placeholder="New member's name:"
            onChange={(e) =>
              this.setState({ ...this.state, nameInputValue: e.target.value })
            }
            value={this.state?.nameInputValue ?? ""}
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
