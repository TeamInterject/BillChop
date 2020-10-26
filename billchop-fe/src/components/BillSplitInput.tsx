import * as React from "react";
import Form from "react-bootstrap/Form";
import Button from "react-bootstrap/Button";

interface IState {
  inputValue: string;
}

interface IProps {
  onSplit: (amount: number) => void;
}

export default class BillSplitInput extends React.Component<IProps, IState> {
  constructor(props: IProps) {
    super(props);
    this.handleOnClick = this.handleOnClick.bind(this);
    this.state = {
      inputValue: "",
    };
  }

  handleOnClick(): void {
    const { onSplit } = this.props;
    const { inputValue } = this.state;
    onSplit(+inputValue);
    this.setState({
      inputValue: "",
    });
  }

  render(): JSX.Element {
    const { inputValue } = this.state;
    return (
      <div>
        <div className="m-2">
          <Form.Label>Expense amount:</Form.Label>
          <Form.Control
            placeholder="Enter the amount"
            onChange={(e) => this.setState({ inputValue: e.target.value })}
            value={inputValue}
          />
        </div>
        <div className="m-2">
          <Button variant="outline-primary" onClick={this.handleOnClick}>
            Split
          </Button>
        </div>
      </div>
    );
  }
}
