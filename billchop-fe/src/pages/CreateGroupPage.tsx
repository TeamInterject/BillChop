import * as React from "react";
import { Button, Col, Form, Row } from "react-bootstrap";
import { Redirect } from "react-router-dom";
import GroupClient from "../backend/clients/GroupClient";
import UserContext from "../backend/helpers/UserContext";

interface ICreateGroupPageState {
  inputValue: string;
  shouldRedirect: boolean;
}

export default class CreateGroupPage extends React.Component<
  unknown,
  ICreateGroupPageState
> {
  private groupClient = new GroupClient();

  constructor(props = {}) {
    super(props);
    this.state = {
      inputValue: "",
      shouldRedirect: false,
    };
  }

  onCreateNewGroup = (event: React.BaseSyntheticEvent): void => {
    event.preventDefault();
    event.stopPropagation();
    const { inputValue: groupName } = this.state;

    const currentUserId = UserContext.authenticatedUser.Id;

    this.groupClient
      .postGroup({ name: groupName })
      .then((group) => this.groupClient.addUserToGroup(group.Id, currentUserId))
      .then(() => this.setState({ shouldRedirect: true }));
  };

  handleOnFormControlChange = (event: React.BaseSyntheticEvent): void => {
    this.setState({ inputValue: event.currentTarget.value });
  };

  render(): React.ReactNode {
    const { shouldRedirect } = this.state;

    return shouldRedirect ? (
      <Redirect to="/groups" />
    ) : (
      <Row className="h-100 d-flex flex-column align-items-center justify-content-center">
        <Col md={3}>
          <Form onSubmit={this.onCreateNewGroup}>
            <Form.Label>Group name:</Form.Label>
            <Form.Control
              placeholder="Enter the group name"
              onChange={this.handleOnFormControlChange}
            />
            <div className="mt-2">
              <Button variant="outline-primary" type="submit">
                Create
              </Button>
            </div>
          </Form>
        </Col>
      </Row>
    );
  }
}
