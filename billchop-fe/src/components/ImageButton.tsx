import * as React from "react";
import Button from "react-bootstrap/Button";

export interface IImageButtonProps {
  imageSource: string;
  alt: string;
  onClick?: () => void;
}

export default class ImageButton extends React.Component<IImageButtonProps> {
  render(): JSX.Element {
    const { imageSource, alt, onClick } = this.props;
    return (
      <Button variant="link" onClick={onClick}>
        <img src={imageSource} height="32px" width="32px" alt={alt} />
      </Button>
    );
  }
}
