import React from "react";
import { Button, Col, Modal, Row } from "react-bootstrap";
import ArrowBackIcon from "../assets/arrow-back-icon.svg";
import ImageButton from "../components/ImageButton";
import SettleUpSlider from "../components/SettleUpSlider";
import "../styles/groups-page.css";
import DoneIcon from "../assets/done-icon.svg";

export interface ISettleUpSubPageProps {
  loansToSettle: { // [TM] NOTE this is just a draft object, when implementing new model in BE feel free to change it however you seem fit.
    Id: string;
    loanerName: string;
    amountToSettle: number;
  }[];
  onSettle: (Id: string, settleAmount: number) => void;
  onCloseSettleUp: () => void;
}

export default class SettleUpSubPage extends React.Component<ISettleUpSubPageProps> {
  renderSettleUpSliders = (): JSX.Element => {
    const { onSettle, loansToSettle } = this.props;

    return (
      <Col className="settle-up-subpage__sliders">
        {
          loansToSettle.map((loan) => {
            return <SettleUpSlider
              key={loan.Id}
              loanToSettle={loan}
              onSettle={onSettle}
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
            All loans are settled up.
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
    const { loansToSettle, onCloseSettleUp } = this.props;

    return (
      <div className="h-100 w-100 subpage-container">
        {
          loansToSettle.length === 0 ? 
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
