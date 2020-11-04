import * as React from "react";
import Form from "react-bootstrap/Form";
import Button from "react-bootstrap/Button";

interface IBillSplitInputState {
  inputValue: string;
}

export interface IBillSplitInputProps {
  onSplit: (amount: number) => void;
}

export default class BillSplitInput extends React.Component<
  IBillSplitInputProps,
  IBillSplitInputState
> {
  constructor(props: IBillSplitInputProps) {
    super(props);
    this.state = {
      inputValue: "",
    };
  }

  handleOnSubmit = (event: React.BaseSyntheticEvent): void => {
    event.stopPropagation();
    event.preventDefault();
    const { onSplit } = this.props;
    const { inputValue } = this.state;
    onSplit(+inputValue);
    this.setState({
      inputValue: "",
    });
  };

  render(): JSX.Element {
    const { inputValue } = this.state;
    return (
      <div>
        <Form onSubmit={this.handleOnSubmit}>
          <div className="m-2">
            <Form.Label>Expense amount:</Form.Label>
            <Form.Control
              placeholder="Enter the amount"
              onChange={(e) => this.setState({ inputValue: e.target.value })}
              value={inputValue}
            />
          </div>
          <div className="m-2">
            <Button variant="outline-primary" type="submit">
              Split
            </Button>
          </div>
        </Form>
      </div>
    );
  }
}
