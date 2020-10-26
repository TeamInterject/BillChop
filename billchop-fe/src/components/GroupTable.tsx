import * as React from "react";
import Table from "react-bootstrap/Table";
import Form from "react-bootstrap/Form";
import Button from "react-bootstrap/Button";
import Axios from "axios";
import Group from "../api/Group";
import { BillSplitInput } from "./BillSplitInput";
import User, { CURRENT_USER_ID } from "../api/User";
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
      expenseAmounts: undefined,
    }
  }

  handleOnAddNewMember(): void {
    const { nameInputValue } = this.state;
    const { group } = this.state ?? this.props;
    Axios.post(BASE_URL_API_USERS, {
      name: nameInputValue,
    }).then((userResponse) => {
      const newUserId = (userResponse.data as User).Id;
      Axios.post(
        `${BASE_URL_API_GROUPS + group.Id}/add-user/${newUserId}`
      ).then((response) => {
        this.setState({
          group: response.data,
          nameInputValue: "",
          expenseAmounts: undefined,
        });
      });
    });
  }

  handleOnSplit(amount: number): void {
    const { group } = this.state ?? this.props;

    Axios.post(BASE_URL_API_BILLS, {
      name: "Bill",
      total: amount,
      loanerId: CURRENT_USER_ID,
      groupContextId: group.Id,
    }).then((response) => {
      this.setState((prevState) => ({
        ...prevState,
        expenseAmounts: response.data.Loans.map((loan: Loan) => loan.Amount),
      }));
    });
  }

  renderTableContent(): (JSX.Element | JSX.Element[])[] {
    const tableContent = [];
    const { group } = this.state ?? this.props;
    const { expenseAmounts } = this.state;
    const { nameInputValue } = this.state;
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
              this.setState((prevState) => ({
                ...prevState,
                nameInputValue: e.target.value,
              }))
            }
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
  }

  render(): JSX.Element {
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
