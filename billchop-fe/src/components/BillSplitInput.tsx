import * as React from "react";
import Form from "react-bootstrap/Form";
import Button from "react-bootstrap/Button";

export class BillSplitInput extends React.Component<
  { onSplit: (amount: number) => void },
  { inputValue: string }
> {
  render() {
    return (
      <div>
        <div className="m-2">
          <Form.Label>Expense amount:</Form.Label>
          <Form.Control
            placeholder="Enter the amount"
            onChange={(e) => this.setState({ inputValue: e.target.value })}
          />
        </div>
        <div className="m-2">
          <Button variant="outline-primary" onClick={() => this.props.onSplit(+this.state.inputValue)}>Split</Button>
        </div>
      </div>
    );
  }
}
