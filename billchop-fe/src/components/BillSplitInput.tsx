import * as React from "react";
import Form from "react-bootstrap/Form";
import Button from "react-bootstrap/Button";

interface IState {
  inputValue: string
}

interface IProps {
  onSplit: (amount: number) => void
}

export class BillSplitInput extends React.Component<
  IProps,
  IState
> {
  constructor(props: IProps) {
    super(props);
    this.handleOnClick = this.handleOnClick.bind(this);
    this.state = {
      inputValue: ""
    }
  }

  handleOnClick() {
    this.props.onSplit(+this.state.inputValue)
    this.setState({
      inputValue: ""
    });
  }

  render() {
    return (
      <div>
        <div className="m-2">
          <Form.Label>Expense amount:</Form.Label>
          <Form.Control
            placeholder="Enter the amount"
            onChange={(e) => this.setState({ inputValue: e.target.value })}
            value={this.state.inputValue}
          />
        </div>
        <div className="m-2">
          <Button
            variant="outline-primary"
            onClick={this.handleOnClick}
          >
            Split
          </Button>
        </div>
      </div>
    );
  }
}
