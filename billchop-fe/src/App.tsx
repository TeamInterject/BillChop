import React from "react";
import {
  BrowserRouter as Router,
  Redirect,
  Route,
  Switch,
} from "react-router-dom";
import GroupsPage from "./pages/GroupsPage";
import CreateGroupPage from "./pages/CreateGroupPage";
import "./App.css";
import NavigationBar from "./components/NavigationBar";

export default class App extends React.Component {
  public render(): React.ReactNode {
    return (
      <Router>
        <div className="mainContainer">
          <div className="mainContainer__pageHeader">
            <NavigationBar />
          </div>
          <div className="mainContainer__content">
            <Switch>
              <Route exact path="/">
                <Redirect to="/groups" />
              </Route>
              <Route path="/profile">
                <h1>In constructon</h1>
              </Route>
              <Route path="/groups">
                <GroupsPage />
              </Route>
              <Route path="/createGroup">
                <CreateGroupPage />
              </Route>
            </Switch>
          </div>
        </div>
      </Router>
    );
  }
}
