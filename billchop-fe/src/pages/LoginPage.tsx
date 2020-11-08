import React from "react";
import { Form, Button, Col, Container, Card } from "react-bootstrap";
import UserContext from "../backend/helpers/UserContext";
import BrowserHistory from "../backend/helpers/History";

interface ILoginPageState {
  email: string;
}

export default class LoginPage extends React.Component<
  unknown,
  ILoginPageState
  > {
  constructor(props: unknown) {
    super(props);

    this.state = { email: "" };
  }

  componentDidMount(): void {
    this.verifyLogin();
  }

  handleEmail = (event: React.ChangeEvent<HTMLInputElement>): void => {
    this.setState({ email: event.target.value });
  };

  handleLogin = async (e: React.FormEvent<HTMLFormElement>): Promise<void> => {
    e.preventDefault();

    const { email } = this.state;

    await UserContext.login(email);
    this.verifyLogin();
  };

  handleRegister = (): void => {
    BrowserHistory.push("/register");
  };

  verifyLogin(): void {
    UserContext.isLoggedIn().then((isLoggedIn) => {
      if (isLoggedIn) BrowserHistory.push("/");
    });
  }

  render(): React.ReactNode {
    const { email } = this.state;
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
                    <Button type="submit">
                      Login
                    </Button>
                  </Col>
                  <Col className="d-flex justify-content-end">
                    <Button variant="light" onClick={this.handleRegister}>Register</Button>
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
