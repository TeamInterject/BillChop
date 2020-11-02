import Axios from "axios";
import * as React from "react";
import { Button, Form } from "react-bootstrap";
import { Redirect } from "react-router-dom";
import Group from "../api/Group";
import { CURRENT_USER_ID } from "../api/User";
import "../styles/create-group-page.css";

const BASE_URL_API_GROUPS = "https://localhost:44333/api/groups/";

interface ICreateGroupPageState {
  inputValue: string;
  shouldRedirect: boolean;
}

export default class CreateGroupPage extends React.Component<
  unknown,
  ICreateGroupPageState
> {
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
    Axios.post(BASE_URL_API_GROUPS, { name: groupName }).then(
      (createResponse) => {
        Axios.post(
          `${
            BASE_URL_API_GROUPS + (createResponse.data as Group).Id
          }/add-user/${CURRENT_USER_ID}`
        ).then(() => {
          this.setState({
            shouldRedirect: true,
          });
        });
      }
    );
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
