import Axios from "axios";
import * as React from "react";
import Group from "../api/Group";
import { GroupsTabs } from "../components/GroupsTabs";

export class GroupPage extends React.Component {
  constructor(props = {}) {
    super(props);
    this.getGroups = this.getGroups.bind(this);
    this.createNewGroup = this.createNewGroup.bind(this);
    // this.getGroups();
  }

  state: { groups: Group[] } = {
    groups: [],
  };

  getGroups() {
    Axios.get("url").then((response) => {
      this.setState({
        groups: response.data,
      });
    });
  }

  createNewGroup(groupName: string) {
    Axios.post("url").then((response) => {
      this.setState({
        groups: response.data,
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
