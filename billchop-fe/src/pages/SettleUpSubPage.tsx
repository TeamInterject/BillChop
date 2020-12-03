import React from "react";
import { Col, Row } from "react-bootstrap";
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

  renderInfoMessage = (): JSX.Element => {
    return (
      <Col className="d-flex flex-column align-items-center justify-content-center">
        <img src={DoneIcon} height="48px" width="48px" alt="Groups icon" />
        <p className="text-center m-2">
          All loans are settled up.
        </p>
      </Col>
    );
  };

  render(): JSX.Element {
    const { loansToSettle, onCloseSettleUp } = this.props;

    return (
      <div className="h-100 w-100 subpage-container">
        <Row>
          <Col>
            <ImageButton imageSource={ArrowBackIcon} tooltipText="Go back" onClick={onCloseSettleUp} />
          </Col>
        </Row>
        <Row className="h-100">
          {loansToSettle.length === 0 ? this.renderInfoMessage() : this.renderSettleUpSliders()}
        </Row>
      </div>
    );
  }
}
