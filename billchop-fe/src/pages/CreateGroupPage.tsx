import Axios from "axios";
import * as React from "react";
import { Button, Form } from "react-bootstrap";
import { Redirect } from "react-router-dom";
import Group from "../api/Group";
import { CURRENT_USER_ID } from "../api/User";
import "../styles/create-group-page.css";

const BASE_URL_API_GROUPS = "https://localhost:44333/api/groups/";

export interface IState {
  inputValue: string;
  shouldRedirect: boolean;
}

export default class CreateGroupPage extends React.Component<unknown, IState> {
  constructor(props = {}) {
    super(props);
    this.state = {
      inputValue: "",
      shouldRedirect: false,
    };

    this.onCreateNewGroup = this.onCreateNewGroup.bind(this);
  }

  onCreateNewGroup(groupName: string): void {
    Axios.post(BASE_URL_API_GROUPS, { name: groupName }).then(
      (createResponse) => {
        Axios.post(
          `${
            BASE_URL_API_GROUPS + (createResponse.data as Group).Id
          }/add-user/${CURRENT_USER_ID}`
        ).then(() => {
          this.setState({
            inputValue: "",
            shouldRedirect: true,
          });
        });
      }
    );
  }

  render(): React.ReactNode {
    const { inputValue, shouldRedirect } = this.state;

    return shouldRedirect ? (
      <Redirect to="/groups" />
    ) : (
      <div className="create-group-page__container d-flex flex-column align-items-center justify-content-center">
        <div>
          <Form.Label>Group name:</Form.Label>
          <Form.Control
            placeholder="Enter the group name"
            onChange={(e) => this.setState({ inputValue: e.target.value })}
          />
          <div className="mt-2">
            <Button
              variant="outline-primary"
              onClick={() => this.onCreateNewGroup(inputValue)}
            >
              Create
            </Button>
          </div>
        </div>
      </div>
    );
  }
}
