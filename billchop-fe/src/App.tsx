import React from "react";
import GroupPage from "./pages/GroupPage";
import "./App.css";

export default class App extends React.Component {
  public render(): React.ReactNode {
    return (
      <div className="mainContainer">
        <GroupPage />
      </div>
    );
  }
}
