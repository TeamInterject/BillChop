import * as React from "react";
import { Button, Form } from "react-bootstrap";
import { Redirect } from "react-router-dom";
import GroupClient from "../backend/clients/GroupClient";
import UserContext from "../backend/helpers/UserContext";
import "../styles/create-group-page.css";

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

  onCreateNewGroup = async (event: React.BaseSyntheticEvent): Promise<void> => {
    event.preventDefault();
    event.stopPropagation();
    const { inputValue: groupName } = this.state;

    const currentUserId = await UserContext.getOrCreateTestUser();

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
      <div className="create-group-page__container d-flex flex-column align-items-center justify-content-center">
        <div>
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
        </div>
      </div>
    );
  }
}
