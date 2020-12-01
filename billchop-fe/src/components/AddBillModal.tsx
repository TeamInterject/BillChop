import * as React from "react";
import { Button, Form, Modal } from "react-bootstrap";
import CurrencyInput from "./CurrencyInput";

interface IAddBillModalProps {
  showModal: boolean;
  onHide: () => void;
  onAdd: (name: string, total: number) => void;
}

interface IAddBillModalState {
  nameInputValue: string;
  totalAmountInputValue: string;
}

export default class AddBillModal extends React.Component<
  IAddBillModalProps,
  IAddBillModalState
> {
  constructor(props: IAddBillModalProps) {
    super(props);
    this.state = {
      nameInputValue: "",
      totalAmountInputValue: "",
    };
  }

  handleOnSubmit = (event: React.BaseSyntheticEvent): void => {
    event.stopPropagation();
    event.preventDefault();

    const { onAdd } = this.props;
    const { nameInputValue, totalAmountInputValue } = this.state;
    onAdd(nameInputValue, parseFloat(totalAmountInputValue));
    this.setState({ nameInputValue: "", totalAmountInputValue: "" });
  };

  handleOnHide = (): void => {
    const { onHide } = this.props;

    this.setState({ nameInputValue: "", totalAmountInputValue: "" });
    onHide();
  };

  handleNameChange = (event: React.ChangeEvent<HTMLInputElement>): void => {
    this.setState({ nameInputValue: event.target.value });
  };

  handleAmountChange = (event: React.ChangeEvent<HTMLInputElement>): void => {
    this.setState({ totalAmountInputValue: event.target.value });
  };

  render(): JSX.Element {
    const { showModal } = this.props;
    const { nameInputValue, totalAmountInputValue } = this.state;

    return (
      <Modal
        show={showModal}
        onHide={this.handleOnHide}
        aria-labelledby="contained-modal-title-vcenter"
        centered
      >
        <Modal.Header closeButton>
          <Modal.Title id="contained-modal-title-vcenter">
            Add new bill
          </Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Form onSubmit={this.handleOnSubmit}>
            <Form.Row>
              <Form.Label>Name:</Form.Label>
              <Form.Control
                required
                placeholder="Enter the name of the bill"
                onChange={this.handleNameChange}
                value={nameInputValue}
              />
            </Form.Row>
            <Form.Row className="mt-2">
              <CurrencyInput
                required
                fullWidth
                min={0.01}
                onChange={this.handleAmountChange}
                value={parseFloat(totalAmountInputValue)}
                label="Total amount:"
                placeholder="Enter the total amount of the bill"
              />
            </Form.Row>
            <Form.Row className="justify-content-end">
              <Button className="mt-3" type="submit">
                Add
              </Button>
            </Form.Row>
          </Form>
        </Modal.Body>
      </Modal>
    );
  }
}
