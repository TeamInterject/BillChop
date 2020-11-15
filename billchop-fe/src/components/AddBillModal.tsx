import * as React from "react";
import { Button, Form, Modal } from "react-bootstrap";

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
    onAdd(nameInputValue, +totalAmountInputValue);
    this.setState({ nameInputValue: "", totalAmountInputValue: "" });
  };

  handleOnHide = (): void => {
    const { onHide } = this.props;

    this.setState({ nameInputValue: "", totalAmountInputValue: "" });
    onHide();
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
                placeholder="Enter the name of the bill"
                onChange={(e) =>
                  this.setState({ nameInputValue: e.target.value })
                }
                value={nameInputValue}
              />
            </Form.Row>
            <Form.Row>
              <Form.Label className="mt-2">Total amount:</Form.Label>
              <Form.Control
                placeholder="Enter the total amount of the bill"
                onChange={(e) =>
                  this.setState({ totalAmountInputValue: e.target.value })
                }
                value={totalAmountInputValue}
                type="number"
              />
            </Form.Row>
            <Form.Row>
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
