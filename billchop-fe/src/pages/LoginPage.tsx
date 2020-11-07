import React from "react";
import { Form, Button, Row, Col, Container } from "react-bootstrap";
import UserContext from "../backend/helpers/UserContext";
import BrowserHistory from "../backend/helpers/History";

interface LoginPageState {
  email: string;
}

export default class LoginPage extends React.Component<
  unknown,
  LoginPageState
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

  verifyLogin(): void {
    UserContext.isLoggedIn().then((isLoggedIn) => {
      if (isLoggedIn) BrowserHistory.push("/");
    });
  }

  render(): React.ReactNode {
    const { email } = this.state;
    return (
      <Container>
        <Row className="justify-content-md-center">
          <Col>
            <Form onSubmit={this.handleLogin}>
              <Form.Group>
                <Form.Label>Email</Form.Label>
                <Form.Control
                  required
                  type="email"
                  placeholder="name@domain.com"
                  value={email}
                  onChange={this.handleEmail}
                />
              </Form.Group>
              <Button variant="primary" type="submit">
                Login
              </Button>
            </Form>
          </Col>
        </Row>
      </Container>
    );
  }
}
