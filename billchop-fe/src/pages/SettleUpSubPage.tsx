import React from "react";
import { Button, Col, Modal, Row } from "react-bootstrap";
import ArrowBackIcon from "../assets/arrow-back-icon.svg";
import ImageButton from "../components/ImageButton";
import SettleUpSlider from "../components/SettleUpSlider";
import "../styles/groups-page.css";
import DoneIcon from "../assets/done-icon.svg";

export interface ISettleUpSubPageProps {
  onCloseSettleUp: () => void;
}

interface ISettleUpSubPabeState {
  loansToSettle: { // [TM] NOTE this is just a draft object, when implementing new model in BE feel free to change it however you seem fit.
    Id: string;
    loanerName: string;
    amountToSettle: number;
  }[];
}

export default class SettleUpSubPage extends React.Component<
  ISettleUpSubPageProps,
  ISettleUpSubPabeState
> {
  constructor(props: ISettleUpSubPageProps) {
    super(props);
    this.state = {
      loansToSettle: [],
    };
  }

  componentDidMount(): void {
    this.getExpectedPayments();
  }

  getExpectedPayments = (): void => {
    this.setState({
      loansToSettle: [
        {
          Id: "1",
          loanerName: "Ainoras",
          amountToSettle: 100,
        },
        {
          Id: "2",
          loanerName: "Martynas",
          amountToSettle: 350,
        },
        {
          Id: "3",
          loanerName: "Maurice",
          amountToSettle: 225,
        },
      ],
    });
  };

  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  handleSettle = (Id: string, settleAmount: number): void => {
    //TODO call BE
  };

  renderSettleUpSliders = (): JSX.Element => {
    const { loansToSettle } = this.state;

    return (
      <Col className="settle-up-subpage__sliders">
        {
          loansToSettle.map((loan) => {
            return <SettleUpSlider
              key={loan.Id}
              loanToSettle={loan}
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
    const { onCloseSettleUp } = this.props;
    const { loansToSettle } = this.state;

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
