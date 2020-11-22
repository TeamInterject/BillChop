import * as React from "react";
import { Accordion, Card } from "react-bootstrap";
import Bill from "../backend/models/Bill";
import Group from "../backend/models/Group";
import BillIcon from "../assets/bill-icon.svg";
import Dictionary from "../util/Dictionary";
import GroupTable from "./GroupTable";
import "../styles/bill-list-accordion.css";
import toEuros from "../util/Currency";

export interface IBillsListAccordionProps {
  group: Group;
  bills: Bill[];
}

export default class BillsListAccordion extends React.Component<
  IBillsListAccordionProps
> {
  generateExpenseAmounts = (bill: Bill): Dictionary<number> => {
    const expenseAmounts: Dictionary<number> = {};

    bill.Loans.forEach((loan) => {
      expenseAmounts[loan.Loanee.Id] = expenseAmounts[loan.Loanee.Id] ?? 0;
      expenseAmounts[loan.Loanee.Id] += loan.Amount;
    });

    return expenseAmounts;
  };

  renderBillCards = (): JSX.Element[] => {
    const { bills, group } = this.props;

    return bills.map((bill) => {
      return (
        <Card key={bill.Id}>
          <Accordion.Toggle className="bill-list-accordion" as={Card.Header} eventKey={bill.Id}>
            <img className="mr-2" src={BillIcon} height="32px" width="32px" alt="Bill icon" />
            <div className="ml-2 d-flex justify-content-between align-items-center flex-grow-1">
              <div><span style={{ fontWeight: 500 }}>{bill.Name}</span> {/*TODO add date here*/}</div>
              <div>{toEuros(bill.Total)}</div>
            </div>
          </Accordion.Toggle>
          <Accordion.Collapse eventKey={bill.Id}>
            <Card.Body>
              <GroupTable
                group={group}
                expenseAmounts={this.generateExpenseAmounts(bill)}
                showMembersOnlyWithExpenses
                loanerId={bill.Loaner.Id}
              />
            </Card.Body>
          </Accordion.Collapse>
        </Card>
      );
    });
  };

  render(): JSX.Element {
    return <Accordion className="m-2 mb-4">{this.renderBillCards()}</Accordion>;
  }
}
