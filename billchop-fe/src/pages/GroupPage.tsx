import Axios from "axios";
import * as React from "react";
import Group from "../api/Group";
import GroupsTabs from "../components/GroupsTabs";
import { CURRENT_USER_ID } from "../api/User";

const BASE_URL_API_GROUPS = "https://localhost:44333/api/groups/";

export default class GroupPage extends React.Component<
  unknown,
  { groups: Group[] }
> {
  constructor(props = {}) {
    super(props);

    this.state = {
      groups: [],
    };

    this.getGroups = this.getGroups.bind(this);
    this.createNewGroup = this.createNewGroup.bind(this);

    this.getGroups();
  }

  getGroups(): void {
    Axios.get(`${BASE_URL_API_GROUPS}?userId=${CURRENT_USER_ID}`).then(
      (response) => {
        this.setState({
          groups: response.data,
        });
      }
    );
  }

  createNewGroup(groupName: string): void {
    Axios.post(BASE_URL_API_GROUPS, { name: groupName }).then(
      (createResponse) => {
        Axios.post(
          `${
            BASE_URL_API_GROUPS + (createResponse.data as Group).Id
          }/add-user/${CURRENT_USER_ID}`
        ).then((response) => {
          const { groups } = this.state;
          this.setState({
            groups: groups.concat(response.data),
          });
        });
      }
    );
  }

  render(): JSX.Element {
    const { groups } = this.state;
    return (
      <GroupsTabs groups={groups} onCreateNewGroup={this.createNewGroup} />
    );
  }
}
