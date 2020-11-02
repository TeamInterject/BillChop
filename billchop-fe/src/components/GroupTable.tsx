import * as React from "react";
import Table from "react-bootstrap/Table";
import Form from "react-bootstrap/Form";
import Button from "react-bootstrap/Button";
import Axios from "axios";
import Group from "../api/Group";
import BillSplitInput from "./BillSplitInput";
import User, { CURRENT_USER_ID } from "../api/User";
import Loan from "../api/Loan";

const BASE_URL_API_GROUPS = "https://localhost:44333/api/groups/";
const BASE_URL_API_USERS = "https://localhost:44333/api/users/";
const BASE_URL_API_BILLS = "https://localhost:44333/api/bills/";
const BASE_URL_API_LOANS = "https://localhost:44333/api/loans/";

interface Dictionary<T> {
  [Key: string]: T;
}

export interface IGroupTableProps {
  group: Group;
}

interface IGroupTableState {
  group?: Group;
  nameInputValue: string;
  expenseAmounts?: Dictionary<number>;
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
      expenseAmounts: {},
    };
  }

  componentDidMount(): void {
    this.getUserLoans();
  }

  getUserLoans = (): void => {
    const { group: stateGroup } = this.state;
    const { group: propsGroup } = this.props;
    const group = stateGroup ?? propsGroup;

    Axios.get(
      `${BASE_URL_API_LOANS}?loanerId=${CURRENT_USER_ID}&groupId=${group.Id}`
    ).then((loansResponse) => {
      const expenseAmounts: Dictionary<number> = {};

      loansResponse.data.forEach((loan: Loan) => {
        expenseAmounts[loan.Loanee.Id] = expenseAmounts[loan.Loanee.Id] ?? 0;
        expenseAmounts[loan.Loanee.Id] += loan.Amount;
      });

      this.setState((prevState) => ({
        ...prevState,
        expenseAmounts,
      }));
    });
  };

  handleOnAddNewMember(): void {
    const { nameInputValue } = this.state;
    const { group: stateGroup } = this.state;
    const { group: propsGroup } = this.props;
    const group = stateGroup ?? propsGroup;

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
    const { group: stateGroup } = this.state;
    const { group: propsGroup } = this.props;
    const group = stateGroup ?? propsGroup;

    Axios.post(BASE_URL_API_BILLS, {
      name: "Bill",
      total: amount,
      loanerId: CURRENT_USER_ID,
      groupContextId: group.Id,
    }).then(() => this.getUserLoans());
  }

  renderTableContent(): React.ReactNode {
    const tableContent = [];
    const { group: stateGroup, expenseAmounts, nameInputValue } = this.state;
    const { group: propsGroup } = this.props;
    const group = stateGroup ?? propsGroup;

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
            onChange={(e) => {
              const eventTargetValue = e.target.value;
              this.setState((prevState) => ({
                ...prevState,
                nameInputValue: eventTargetValue,
              }));
            }}
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
