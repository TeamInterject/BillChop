import React from "react";
import { Button, Card, Col, Container, Form, Toast } from "react-bootstrap";
import BrowserHistory from "../backend/helpers/History";
import UserContext from "../backend/helpers/UserContext";

interface IRegistrationPageState {
  name: string;
  email: string;
  showRegisterError: boolean;
}

export default class RegistrationPage extends React.Component<unknown, IRegistrationPageState> {
  constructor(props: unknown) {
    super(props);

    this.state = {
      name: "",
      email: "",
      showRegisterError: false,
    };
  }

  handleName = (event: React.ChangeEvent<HTMLInputElement>): void => {
    this.setState({ name: event.target.value });
  };

  handleEmail = (event: React.ChangeEvent<HTMLInputElement>): void => {
    this.setState({ email: event.target.value });
  };

  handleRegister = async (e: React.FormEvent<HTMLFormElement>): Promise<void> => {
    e.preventDefault();

    const { name, email } = this.state;

    const registerResult = await UserContext.register(name, email);
    registerResult ? BrowserHistory.push("/") : this.setState({ showRegisterError: true });
  };

  handleLogin = (): void => {
    BrowserHistory.push("/login");
  }

  handleErrorToastClose = (): void => {
    this.setState({ showRegisterError: false });
  };

  render() {
    const { name, email, showRegisterError } = this.state;

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