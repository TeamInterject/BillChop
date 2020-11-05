import * as React from "react";
import { Col, Container, Row } from "react-bootstrap";
import ImageButton from "./ImageButton";
import AddBillIcon from "../assets/add-bill-icon.svg";
import AddBillModal from "./AddBillModal";

export interface IGroupPageHeaderProps {
  onAddNewBill: (name: string, total: number) => void;
}

export interface IGroupPageHeaderState {
  showAddBillModal: boolean;
}

export default class GroupPageHeader extends React.Component<
  IGroupPageHeaderProps,
  IGroupPageHeaderState
> {
  constructor(props: IGroupPageHeaderProps) {
    super(props);
    this.state = {
      showAddBillModal: false,
    };
  }

  handleOnAddNewBill = (name: string, total: number): void => {
    const { onAddNewBill } = this.props;
    onAddNewBill(name, total);
    this.setState({ showAddBillModal: false });
  };

  handleOnAddBillModalHide = (): void => {
    this.setState({ showAddBillModal: false });
  };

  render(): JSX.Element {
    const { showAddBillModal } = this.state;
    return (
      <div>
        <Container className="mt-2">
          <Row>
            <Col>
              <ImageButton
                imageSource={AddBillIcon}
                alt="Add Bill"
                onClick={() => this.setState({ showAddBillModal: true })}
              />
            </Col>
          </Row>
          <AddBillModal
            showModal={showAddBillModal}
            onHide={this.handleOnAddBillModalHide}
            onAdd={this.handleOnAddNewBill}
          />
        </Container>
      </div>
    );
  }
}
