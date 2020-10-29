import * as React from "react";
import "../styles/group-page.css";
import GroupIcon from "../assets/group-icon.svg";

export default class NoGroupSelectedSubPage extends React.Component {
  render(): JSX.Element {
    return (
      <div className="group-page-sub-page-container d-flex flex-column align-items-center justify-content-center">
        <img src={GroupIcon} height="48px" width="48px" alt="Groups icon" />
        <p>
          No group selected. Please create new group or select an existing one.
        </p>
      </div>
    );
  }
}
