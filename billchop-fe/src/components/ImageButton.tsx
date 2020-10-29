import * as React from "react";
import Button from "react-bootstrap/Button";

interface IProps {
  onClick: () => void;
  imageSource: string;
}

export default class ImageButton extends React.Component<IProps> {
  render(): JSX.Element {
    const { onClick, imageSource } = this.props;
    return (
      <Button variant="light" onClick={onClick}>
        <img src={imageSource} height="32px" width="32px" alt="Button" />
      </Button>
    );
  }
}
