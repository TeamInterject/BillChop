import * as React from "react";
import "../styles/groups-page.css";
import InfoIcon from "../assets/info-icon.svg";

export default class NoGroupSelectedSubPage extends React.Component {
  render(): JSX.Element {
    return (
      <div className="h-100 d-flex flex-column align-items-center justify-content-center">
        <img src={InfoIcon} height="48px" width="48px" alt="Groups icon" />
        <p>
          No group selected. Please create new group or select an existing one.
        </p>
      </div>
    );
  }
}
