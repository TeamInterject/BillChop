import Axios from "axios";
import * as React from "react";
import Group from "../api/Group";
import Sidebar, { ISidebarTab } from "../components/Sidebar";
import { CURRENT_USER_ID } from "../api/User";
import NoGroupSelectedSubPage from "./NoGroupSelectedSubPage";
import "../styles/group-page.css";

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
    this.getGroupsSidebarTabs = this.getGroupsSidebarTabs.bind(this);
  }

  componentDidMount(): void {
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

  getGroupsSidebarTabs(): ISidebarTab[] {
    const { groups } = this.state;
    return groups.map((group) => {
      return {
        groupName: group.Name,
        groupId: group.Id,
      }
    });
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
    return (
      <div>
        <Sidebar
          sidebarTabs={this.getGroupsSidebarTabs()}
          onTabClick={() => {}}
          onCreateNewGroup={this.createNewGroup}
        />
        <NoGroupSelectedSubPage />
      </div>
    );
  }
}
