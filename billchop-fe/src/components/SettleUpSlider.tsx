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

  handleSettle = (event: React.FormEvent<HTMLFormElement>): void => {
    
  };

  render(): JSX.Element {
    const { loanerName, loanAmount} = this.props;
    const { settleAmount } = this.state;
    return (
      <Form className="mt-2" onSubmit={this.handleSettle}>
        <Form.Group controlId="settleUpRange">
          <Form.Row>
            <Col className="ml-1 mr-1">
              <div className="d-flex justify-content-between align-items-center">
                <span>You owe {`${loanerName} ${loanAmount}€`}</span>
                <div className="d-flex align-items-center">
                  <span className="mr-2">Pay:</span>
                  <CurrencyInput
                    onChange={this.handleSettleAmountChange}
                    value={settleAmount}
                    max={loanAmount}
                  />
                </div>
              </div>
            </Col>
            <Col className="ml-1 mr-1" md={1} />
          </Form.Row>
          <Form.Row>
            <Col className="d-flex align-items-center ml-1 mr-1">
              <Form.Control
                type="range"
                min={0}
                max={loanAmount}
                step={0.01}
                onChange={this.handleSettleAmountChange}
                value={settleAmount}
              />
            </Col>
            <Col className="ml-1 mr-1" md={1}>
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
