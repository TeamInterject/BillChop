import React from "react";
import { Button, Card, Col, Container, Form, Toast } from "react-bootstrap";
import BrowserHistory from "../backend/helpers/History";
import LoadingContext from "../backend/helpers/LoadingContext";
import UserContext from "../backend/helpers/UserContext";

interface IRegistrationPageState {
  name: string;
  email: string;
  password: string;
  confirmPassword: string;
  showRegisterError: boolean;
}

export default class RegistrationPage extends React.Component<
  unknown,
  IRegistrationPageState
  > {
  constructor(props: unknown) {
    super(props);

    this.state = {
      name: "",
      email: "",
      password: "",
      confirmPassword: "",
      showRegisterError: false,
    };
  }

  handleName = (event: React.ChangeEvent<HTMLInputElement>): void => {
    this.setState({ name: event.target.value });
  };

  handleEmail = (event: React.ChangeEvent<HTMLInputElement>): void => {
    this.setState({ email: event.target.value });
  };

  handlePassword = (event: React.ChangeEvent<HTMLInputElement>): void => {
    this.setState({ password: event.target.value });
    event.currentTarget.setCustomValidity("");
  };

  handleConfirmPassword = (event: React.ChangeEvent<HTMLInputElement>): void => {
    const { password } = this.state;

    this.setState({ confirmPassword: event.target.value });
    if (event.target.value === password) {
      event.currentTarget.setCustomValidity("");
    }
  };

  handleRegister = async (e: React.FormEvent<HTMLFormElement>): Promise<void> => {
    e.preventDefault();

    const { name, email } = this.state;

    const registerResult = await UserContext.register(name, email);
    registerResult
      ? BrowserHistory.push("/") : this.handleRegisterError();
  };

  handleRegisterError = (): void => {
    this.setState({ showRegisterError: true });
    LoadingContext.isLoading = false; // TODO move this setter to user client, when catching axios error response
  };

  handleLogin = (): void => {
    BrowserHistory.push("/login");
  };

  handleInvalidPassword = (e: React.FormEvent<HTMLInputElement>): void => {
    const passwordValidationMessage = "Password must have minimum eight characters, at least one letter, one number and one special character";
    e.currentTarget.setCustomValidity(passwordValidationMessage);
  };

  handleInvalidConfirmPassword = (e: React.FormEvent<HTMLInputElement>): void => {
    e.currentTarget.setCustomValidity("Passwords must match");
  };

  handleErrorToastClose = (): void => {
    this.setState({ showRegisterError: false });
  };

  render(): JSX.Element {
    const { name, email, password, confirmPassword, showRegisterError } = this.state;

    return (
      <div className="container-vertical-center">
        <Container className="col-lg-4">
          <Card className="shadow mb-5 bg-white rounded">
            <Card.Header as="h3">Registration</Card.Header>
            <Card.Body>
              <Form onSubmit={this.handleRegister}>
                <Form.Row>
                  <Col>
                    <Form.Group controlId="formName">
                      <Form.Label>Name</Form.Label>
                      <Form.Control
                        pattern="[A-Z][a-z]+"
                        placeholder="Enter your name"
                        required
                        value={name}
                        onChange={this.handleName}
                      />
                    </Form.Group>
                  </Col>
                </Form.Row>
                <Form.Row>
                  <Col>
                    <Form.Group controlId="formEmail">
                      <Form.Label>Email</Form.Label>
                      <Form.Control
                        type="email"
                        placeholder="name@domain.com"
                        required
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
                    <Form.Group controlId="formConfirmPassword">
                      <Form.Label>Confirm Password</Form.Label>
                      <Form.Control
                        required
                        pattern={password}
                        onInvalid={this.handleInvalidConfirmPassword}
                        type="password"
                        placeholder="*********"
                        value={confirmPassword}
                        onChange={this.handleConfirmPassword}
                      />
                    </Form.Group>
                  </Col>
                </Form.Row>
                <Form.Row>
                  <Col>
                    <Button type="submit">Register</Button>
                  </Col>
                  <Col className="d-flex justify-content-end">
                    <Button variant="light" onClick={this.handleLogin}>Login</Button>
                  </Col>
                </Form.Row>
                <Form.Row>
                  <Col className="d-flex justify-content-center">
                    <Toast className="mt-3" show={showRegisterError} onClose={this.handleErrorToastClose}>
                      <Toast.Header><strong className="mr-auto text-danger">Oops...</strong></Toast.Header>
                      <Toast.Body>Something wrong happened during registration :/ Please try again later</Toast.Body>
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