import React from "react";
import GroupsPage from "./pages/GroupsPage";
import "./App.css";

export default class App extends React.Component {
  public render(): React.ReactNode {
    return (
      <div className="mainContainer">
        <GroupsPage />
      </div>
    );
  }
}
