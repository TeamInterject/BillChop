import React from "react";
import { Col, Row } from "react-bootstrap";
import ArrowBackIcon from "../assets/arrow-back-icon.svg";
import ImageButton from "../components/ImageButton";
import SettleUpSlider from "../components/SettleUpSlider";
import "../styles/groups-page.css";

export interface ISettleUpSubPageProps {
  onSettle: () => void;
  onCloseSettleUp: () => void;
}

export default class SettleUpSubPage extends React.Component<ISettleUpSubPageProps> {
  renderSettleUpSliders = (): JSX.Element[] => {
    const { onSettle } = this.props;

    return [
      <SettleUpSlider
        key="1"
        loanerName="Ainoras"
        loanAmount={100}
        onSettle={onSettle}
      />,
      <SettleUpSlider
        key="2"
        loanerName="Martynas"
        loanAmount={360}
        onSettle={onSettle}
      />,
      <SettleUpSlider
        key="3"
        loanerName="Daniel"
        loanAmount={500}
        onSettle={onSettle}
      />,
      <SettleUpSlider
        key="4"
        loanerName="Henrik"
        loanAmount={500}
        onSettle={onSettle}
      />,<SettleUpSlider
        key="5"
        loanerName="Tibor"
        loanAmount={500}
        onSettle={onSettle}
      />,
    ];
  };

  render(): JSX.Element {
    const { onCloseSettleUp } = this.props;

    return (
      <div className="h-100 w-100 subpage-container">
        <Row>
          <Col>
            <ImageButton imageSource={ArrowBackIcon} tooltipText="Go back" onClick={onCloseSettleUp} />
          </Col>
        </Row>
        <Row>
          <Col className="settle-up-subpage__sliders">
            {this.renderSettleUpSliders()}
          </Col>
        </Row>
      </div>
    );
  }
}
