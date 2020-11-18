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
import RegistrationPage from "./pages/RegistrationPage";
import { Col, Container, Row } from "react-bootstrap";
import LoadingSpinner from "./components/LoadingSpinner";
import LoadingContext from "./backend/helpers/LoadingContext";

export interface IAppState {
  currentUser?: User;
  isLoading: boolean;
}

export default class App extends React.Component<unknown, IAppState> {
  constructor(props: unknown) {
    super(props);
    this.state = {
      isLoading: false,
    };
  }

  public componentDidMount(): void {
    UserContext.userObservable.subscribe((user) =>
      this.setState({ currentUser: user }),
    );
    LoadingContext.loadingObservable.subscribe((isLoading) => {
      this.setState({ isLoading });
    });
  }

  public logout = (): void => {
    UserContext.logout();
  };

  public render(): React.ReactNode {
    const { currentUser, isLoading } = this.state;

    return (
      <Router history={BrowserHistory}>
        <Container fluid className="vh-100 d-flex flex-column" style={{ overflowX: "hidden" }}>
          <LoadingSpinner isLoading={isLoading} />
          {currentUser &&
          <Row>
            <Col className="flex-shrink-0 p-0 border">
              <NavigationBar currentUser={currentUser} logout={this.logout} />
            </Col>
          </Row>
          }
          <Row className="flex-fill border">
            <Col>
              <Switch>
                <Route exact path="/login">
                  <LoginPage />
                </Route>
                <Route exact path="/register">
                  <RegistrationPage />
                </Route>
                <PrivateRoute path="/profile">
                  <h1>In construction</h1>
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
            </Col>
          </Row>
        </Container>
      </Router>
    );
  }
}
