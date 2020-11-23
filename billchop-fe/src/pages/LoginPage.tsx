import React from "react";
import { Form, Button, Col, Container, Card, Toast } from "react-bootstrap";
import UserContext from "../backend/helpers/UserContext";
import BrowserHistory from "../backend/helpers/History";
import LoadingContext from "../backend/helpers/LoadingContext";

interface ILoginPageState {
  email: string;
  password: string;
  showLoginError: boolean;
}

export default class LoginPage extends React.Component<
  unknown,
  ILoginPageState
  > {
  constructor(props: unknown) {
    super(props);

    this.state = { 
      email: "",
      password: "",
      showLoginError: false,
    };
  }

  componentDidMount(): void {
    this.verifyLogin(true);
  }

  handleEmail = (event: React.ChangeEvent<HTMLInputElement>): void => {
    this.setState({ email: event.target.value });
  };

  handlePassword = (event: React.ChangeEvent<HTMLInputElement>): void => {
    this.setState({ password: event.target.value });
    event.currentTarget.setCustomValidity("");
  };

  handleLogin = async (e: React.FormEvent<HTMLFormElement>): Promise<void> => {
    e.preventDefault();

    const { email } = this.state;

    await UserContext.login(email);
    this.verifyLogin(false);
  };

  handleRegister = (): void => {
    BrowserHistory.push("/register");
  };

  verifyLogin(componentDidMount: boolean): void {
    UserContext.isLoggedIn().then((isLoggedIn) => {
      if (isLoggedIn) {
        BrowserHistory.push("/");
      } else if (!componentDidMount) {
        this.setState({ showLoginError: true });
        LoadingContext.isLoading = false; // TODO move this setter to user client, when catching axios error response
      }
    });
  }

  handleErrorToastClose = (): void => {
    this.setState({ showLoginError: false });
  };

  handleInvalidPassword = (e: React.FormEvent<HTMLInputElement>): void => {
    const passwordValidationMessage = "Password must have minimum eight characters, at least one letter, one number and one special character";
    e.currentTarget.setCustomValidity(passwordValidationMessage);
  };

  render(): React.ReactNode {
    const { email, password, showLoginError } = this.state;

    return (
      <div className="container-vertical-center">
        <Container className="col-lg-4">
          <Card className="shadow mb-5 bg-white rounded">
            <Card.Header as="h3">Login</Card.Header>
            <Card.Body>
              <Form onSubmit={this.handleLogin}>
                <Form.Row>
                  <Col>
                    <Form.Group controlId="formEmail">
                      <Form.Label>Email</Form.Label>
                      <Form.Control
                        required
                        type="email"
                        placeholder="name@domain.com"
                        value={email}
                        onChange={this.handleEmail}
                      />
                    </Form.Group>
                  </Col>
                </Form.Row>
                <Form.Row>
                  <Col>
                    <Form.Group controlId="formPassword">
                      <Form.Label>Password</Form.Label>
                      <Form.Control
                        required
                        pattern="^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$"
                        onInvalid={this.handleInvalidPassword}
                        type="password"
                        placeholder="*********"
                        value={password}
                        onChange={this.handlePassword}
                      />
                    </Form.Group>
                  </Col>
                </Form.Row>
                <Form.Row>
                  <Col>
                    <Button type="submit">
                      Login
                    </Button>
                  </Col>
                  <Col className="d-flex justify-content-end">
                    <Button variant="light" onClick={this.handleRegister}>Register</Button>
                  </Col>
                </Form.Row>
                <Form.Row>
                  <Col className="d-flex justify-content-center">
                    <Toast className="mt-3" show={showLoginError} onClose={this.handleErrorToastClose}>
                      <Toast.Header><strong className="mr-auto text-danger">Oops...</strong></Toast.Header>
                      <Toast.Body>Something wrong happened during login :/ Please try again later</Toast.Body>
                    </Toast>
                  </Col>
                </Form.Row>
              </Form>
            </Card.Body>
          </Card>
        </Container>
      </div>
    );
  }
}
