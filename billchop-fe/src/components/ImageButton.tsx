import * as React from "react";
import { OverlayTrigger, Tooltip } from "react-bootstrap";
import Button from "react-bootstrap/Button";

export interface IImageButtonProps {
  imageSource: string;
  tooltipText: string;
  onClick?: () => void;
}

export default class ImageButton extends React.Component<IImageButtonProps> {
  render(): JSX.Element {
    const { imageSource, tooltipText, onClick } = this.props;
    return (
      <OverlayTrigger
        placement="top"
        overlay={<Tooltip id="image-button-tooltip">{tooltipText}</Tooltip>}
      >
        <Button variant="link" onClick={onClick}>
          <img src={imageSource} height="32px" width="32px" alt={tooltipText} />
        </Button>
      </OverlayTrigger>
    );
  }
}
