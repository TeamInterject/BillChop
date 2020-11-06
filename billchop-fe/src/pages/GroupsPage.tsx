import Axios from "axios";
import * as React from "react";
import Group from "../api/Group";
import Sidebar, { ISidebarTab } from "../components/Sidebar";
import User, { CURRENT_USER_ID } from "../api/User";
import NoGroupSelectedSubPage from "./NoGroupSelectedSubPage";
import "../styles/group-page.css";
import GroupSubPage from "./GroupSubPage";

const BASE_URL_API_GROUPS = "https://localhost:44333/api/groups/";
const BASE_URL_API_USERS = "https://localhost:44333/api/users/";

interface IGroupsPageState {
  groups: Group[];
  selectedGroupId?: string;
}

export default class GroupsPage extends React.Component<
  unknown,
  IGroupsPageState
> {
  constructor(props = {}) {
    super(props);

    this.state = {
      groups: [],
    };
  }

  componentDidMount(): void {
    this.getGroups();
  }

  getGroups = (): void => {
    Axios.get(`${BASE_URL_API_GROUPS}?userId=${CURRENT_USER_ID}`).then(
      (response) => {
        this.setState({
          groups: response.data,
        });
      }
    );
  };

  getGroupsSidebarTabs = (): ISidebarTab[] => {
    const { groups } = this.state;
    return groups.map((group) => {
      return {
        groupName: group.Name,
        groupId: group.Id,
      };
    });
  };

  handleOnGroupTabSelect = (groupId: string): void => {
    this.setState({ selectedGroupId: groupId });
  };

  handleOnAddNewMember = (name: string): void => {
    const { groups, selectedGroupId } = this.state;
    const group = groups.find((g) => g.Id === selectedGroupId);

    if (group === undefined) {
      this.getGroups();
      return;
    }

    Axios.post(BASE_URL_API_USERS, {
      name,
      email: `${name.replace(" ", ".")}@gmail.com`,
    }).then((userResponse) => {
      const newUserId = (userResponse.data as User).Id;
      Axios.post(
        `${BASE_URL_API_GROUPS + group.Id}/add-user/${newUserId}`
      ).then((response) => {
        const index = groups.findIndex((g) => g.Id === selectedGroupId);
        groups[index] = response.data;
        this.setState({
          groups,
        });
      });
    });
  };

  render(): JSX.Element {
    const { selectedGroupId } = this.state;
    const { groups } = this.state;
    const selectedGroup = groups.find((group) => group.Id === selectedGroupId);
    return (
      <div>
        <Sidebar
          sidebarTabs={this.getGroupsSidebarTabs()}
          onTabClick={this.handleOnGroupTabSelect}
        />
        {selectedGroup ? (
          <GroupSubPage
            group={selectedGroup}
            onAddNewMember={this.handleOnAddNewMember}
          />
        ) : (
          <NoGroupSelectedSubPage />
        )}
      </div>
    );
  }
}
