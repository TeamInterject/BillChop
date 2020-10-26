import Axios from "axios";
import * as React from "react";
import Group from "../api/Group";
import { GroupsTabs } from "../components/GroupsTabs";
import { CURRENT_USER_ID } from "../api/User";

const BASE_URL_API_GROUPS = "https://localhost:44333/api/groups/";

export class GroupPage extends React.Component {
  constructor(props = {}) {
    super(props);
    this.getGroups = this.getGroups.bind(this);
    this.createNewGroup = this.createNewGroup.bind(this);

    this.getGroups();
  }

  state: { groups: Group[] } = {
    groups: [],
  };

  getGroups() {
    Axios.get(
      BASE_URL_API_GROUPS + "?userId=" + CURRENT_USER_ID
    ).then((response) => {
      this.setState({
        groups: response.data,
      });
    });
  }

  createNewGroup(groupName: string) {
    Axios.post(BASE_URL_API_GROUPS, { name: groupName }).then((createResponse) => {
      Axios.post(
        BASE_URL_API_GROUPS + (createResponse.data as Group).Id + "/add-user/" + CURRENT_USER_ID
      ).then((response) => {
        this.setState({
          groups: this.state.groups.concat(response.data),
        });
      });
    });
  }

  render() {
    return (
      <GroupsTabs
        groups={this.state.groups}
        onCreateNewGroup={this.createNewGroup}
      />
    );
  }
}
