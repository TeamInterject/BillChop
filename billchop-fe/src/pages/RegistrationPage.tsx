import React from "react";
import { Button, Card, Col, Container, Form } from "react-bootstrap";

export default class RegistrationPage extends React.Component {
  handleRegister = (): void => {

  }

  render() {
    return (
      <div className="container-vertical-center">
        <Container className="col-lg-4">
          <Card className="shadow mb-5 bg-white rounded">
            <Card.Header as="h3">Registration</Card.Header>
            <Card.Body>
              <Form>
                <Form.Row>
                  <Col>
                    <Form.Group controlId="formName">
                      <Form.Label>Name</Form.Label>
                      <Form.Control pattern="[a-z][A-Z]" placeholder="Enter your name" required />
                    </Form.Group>
                  </Col>
                </Form.Row>
                <Form.Row>
                  <Col>
                    <Form.Group controlId="formEmail">
                      <Form.Label>Email</Form.Label>
                      <Form.Control type="email" placeholder="name@domain.com" required />
                    </Form.Group>
                  </Col>
                </Form.Row>
                <Form.Row>
                  <Col>
                    <Button type="submit">Register</Button>
                  </Col>
                  <Col className="d-flex justify-content-end">
                    <Button variant="light">Login</Button>
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