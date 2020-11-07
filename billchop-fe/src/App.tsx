import React from "react";
import { Router, Redirect, Route, Switch } from "react-router-dom";
import GroupsPage from "./pages/GroupsPage";
import CreateGroupPage from "./pages/CreateGroupPage";
import "./App.css";
import NavigationBar from "./components/NavigationBar";
import { PrivateRoute } from "./PrivateRoute";
import BrowserHistory from "./backend/helpers/History";
import User from "./backend/models/User";
import UserContext from "./backend/helpers/UserContext";
import LoginPage from "./pages/LoginPage";

export interface IAppState {
  currentUser?: User;
}

export default class App extends React.Component<unknown, IAppState> {
  constructor(props: unknown) {
    super(props);
    this.state = {};
  }

  public componentDidMount(): void {
    UserContext.userObservable.subscribe((user) =>
      this.setState({ currentUser: user })
    );
  }

  public logout = (): void => {
    UserContext.logout();
  };

  public render(): React.ReactNode {
    const { currentUser } = this.state;

    return (
      <Router history={BrowserHistory}>
        <div className="mainContainer">
          <NavigationBar currentUser={currentUser} logout={this.logout} />
          <div className="mainContainer__content">
            <Switch>
              <Route exact path="/login">
                <LoginPage />
              </Route>
              <PrivateRoute path="/profile">
                <h1>In constructon</h1>
              </PrivateRoute>
              <PrivateRoute path="/groups">
                <GroupsPage />
              </PrivateRoute>
              <PrivateRoute path="/createGroup">
                <CreateGroupPage />
              </PrivateRoute>
              <PrivateRoute exact path="/">
                <Redirect to="/groups" />
              </PrivateRoute>
            </Switch>
          </div>
        </div>
      </Router>
    );
  }
}
