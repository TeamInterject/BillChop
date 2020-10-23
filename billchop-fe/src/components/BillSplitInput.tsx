import * as React from "react";
import Form from "react-bootstrap/Form";
import Button from "react-bootstrap/Button";

export class BillSplitInput extends React.Component<{}, {}> {
  render() {
    return (
      <div>
        <div className="m-2">
          <Form.Label>Expense amount:</Form.Label>
          <Form.Control placeholder="Enter the amount" />
        </div>
        <div className="m-2">
          <Button variant="outline-primary">Split</Button>
        </div>
      </div>
    );
  }
}
