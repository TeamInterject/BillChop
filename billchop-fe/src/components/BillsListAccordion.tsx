import * as React from "react";
import { Accordion, Button, Card } from "react-bootstrap";
import Bill from "../backend/models/Bill";
import Group from "../backend/models/Group";
import BillIcon from "../assets/bill-icon.svg";
import Dictionary from "../util/Dictionary";
import GroupTable from "./GroupTable";

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
          <Card.Header>
            <img src={BillIcon} height="32px" width="32px" alt="Bill icon" />
            <Accordion.Toggle as={Button} variant="link" eventKey={bill.Id}>
              {bill.Name}
              {/* TODO Add date */}
            </Accordion.Toggle>
          </Card.Header>
          <Accordion.Collapse eventKey={bill.Id}>
            <Card.Body>
              <GroupTable
                group={group}
                expenseAmounts={this.generateExpenseAmounts(bill)}
                showMembersOnlyWithExpenses
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
