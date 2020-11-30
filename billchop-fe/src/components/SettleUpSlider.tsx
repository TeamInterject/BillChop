import React from "react";
import { Button, Col, Form } from "react-bootstrap";
import CurrencyInput from "./CurrencyInput";

export interface ISettleUpSliderProps {
  loanerName: string;
  loanAmount: number;
  onSettle: () => void;
}

interface ISettleUpSliderState {
  settleAmount: number;
}

export default class SettleUpSlider extends React.Component<
  ISettleUpSliderProps,
  ISettleUpSliderState
> {
  constructor(props: ISettleUpSliderProps) {
    super(props);

    this.state = {
      settleAmount: 0,
    };
  }

  handleSettleAmountChange = (event: React.ChangeEvent<HTMLInputElement>): void => {
    this.setState({ settleAmount: parseFloat(event.currentTarget.value) });
  };

  handleSettle = (): void => {
    const { onSettle } = this.props;
    onSettle();
  };

  render(): JSX.Element {
    const { loanerName, loanAmount} = this.props;
    const { settleAmount } = this.state;
    return (
      <Form className="mt-4" onSubmit={this.handleSettle}>
        <Form.Group controlId="settleUpRange">
          <Form.Row>
            <Col className="ml-2 mr-2 mb-1">
              <div className="d-flex justify-content-between align-items-center">
                <span>You owe {`${loanerName} ${loanAmount}€`}</span>
                <div className="d-flex justify-content-end align-items-center">
                  <span className="mr-2">Pay:</span>
                  <CurrencyInput
                    onChange={this.handleSettleAmountChange}
                    value={settleAmount}
                    max={loanAmount}
                  />
                </div>
              </div>
            </Col>
          </Form.Row>
          <Form.Row>
            <Col className="d-flex align-items-center ml-2 mr-2">
              <Form.Control
                type="range"
                min={0}
                max={loanAmount}
                step={0.01}
                onChange={this.handleSettleAmountChange}
                value={settleAmount}
              />
            </Col>
          </Form.Row>
          <Form.Row>
            <Col className="d-flex justify-content-end ml-2 mr-2 mt-1">
              <Button
                variant="outline-primary"
                type="submit"
                disabled={settleAmount === 0}
              >
                Settle up
              </Button>
            </Col>
          </Form.Row>
        </Form.Group>
      </Form>
    );
  }
}
