import React from "react";
import { Button, Col, Modal, Row } from "react-bootstrap";
import ArrowBackIcon from "../assets/arrow-back-icon.svg";
import ImageButton from "../components/ImageButton";
import SettleUpSlider from "../components/SettleUpSlider";
import "../styles/groups-page.css";
import DoneIcon from "../assets/done-icon.svg";
import Payment from "../backend/models/Payment";
import PaymentClient from "../backend/clients/PaymentClient";
import UserContext from "../backend/helpers/UserContext";

export interface ISettleUpSubPageProps {
  groupId: string;
  onCloseSettleUp: () => void;
}

interface ISettleUpSubPageState {
  expectedPayments: Payment[];
}

export default class SettleUpSubPage extends React.Component<
  ISettleUpSubPageProps,
  ISettleUpSubPageState
> {
  constructor(props: ISettleUpSubPageProps) {
    super(props);
    this.state = {
      expectedPayments: [],
    };
  }

  private paymentClient = new PaymentClient();

  componentDidMount(): void {
    this.getExpectedPayments();
  }

  getExpectedPayments = (): void => {
    const { groupId } = this.props;
    const userId = UserContext.authenticatedUser.Id;

    this.paymentClient.getExpectedPayments({
      userId,
      groupId,
    }).then((payments) => {
      const expectedPayments = payments.filter((payment) => payment.Payer.Id === userId);
      this.setState({ expectedPayments });
    });
  };

  handleSettle = (receiverId: string, settleAmount: number): void => {
    const { groupId } = this.props;

    this.paymentClient.createPayment({
      Amount: settleAmount,
      PayerId: UserContext.authenticatedUser.Id,
      ReceiverId: receiverId,
      GroupContextId: groupId,
    }).then(() => this.getExpectedPayments());
  };

  renderSettleUpSliders = (): JSX.Element => {
    const { expectedPayments } = this.state;

    return (
      <Col className="settle-up-subpage__sliders">
        {
          expectedPayments.map((payment) => {
            return <SettleUpSlider
              key={payment.Id}
              paymentToMake={payment}
              onSettle={this.handleSettle}
            />;
          })
        }
      </Col>
    );
  };

  renderInfoModal = (): JSX.Element => {
    const { onCloseSettleUp } = this.props;
    return (
      <Modal show centered onHide={onCloseSettleUp}>
        <Modal.Body className="d-flex flex-column align-items-center justify-content-center">
          <img src={DoneIcon} height="48px" width="48px" alt="Groups icon" />
          <p className="text-center m-2">
            Everything is settled up!
          </p>
        </Modal.Body>
        <Modal.Footer className="justify-content-center">
          <Button variant="primary" onClick={onCloseSettleUp}>
            Close
          </Button>
        </Modal.Footer>
      </Modal>
    );
  };

  render(): JSX.Element {
    const { onCloseSettleUp } = this.props;
    const { expectedPayments } = this.state;

    return (
      <div className="h-100 w-100 subpage-container">
        {
          expectedPayments.length === 0 ? 
            this.renderInfoModal()
            :
            <div>
              <Row>
                <Col>
                  <ImageButton imageSource={ArrowBackIcon} tooltipText="Go back" onClick={onCloseSettleUp} />
                </Col>
              </Row>
              <Row>
                {this.renderSettleUpSliders()}
              </Row>
            </div>
        }
      </div>
    );
  }
}
