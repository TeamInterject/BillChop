import Axios from "axios";
import * as React from "react";
import Group from "../api/Group";
import Sidebar, { ISidebarTab } from "../components/Sidebar";
import { CURRENT_USER_ID } from "../api/User";
import NoGroupSelectedSubPage from "./NoGroupSelectedSubPage";
import "../styles/group-page.css";
import GroupSubPage from "./GroupSubPage";

const BASE_URL_API_GROUPS = "https://localhost:44333/api/groups/";

interface IState {
  groups: Group[];
  selectedGroupId: string | undefined;
}

export default class GroupsPage extends React.Component<unknown, IState> {
  constructor(props = {}) {
    super(props);

    this.state = {
      groups: [],
      selectedGroupId: undefined,
    };

    this.getGroups = this.getGroups.bind(this);
    this.getGroupsSidebarTabs = this.getGroupsSidebarTabs.bind(this);
    this.handleOnGroupTabSelect = this.handleOnGroupTabSelect.bind(this);
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
      };
    });
  }

  handleOnGroupTabSelect(groupId: string): void {
    this.setState((prevState) => ({
      ...prevState,
      selectedGroupId: groupId,
    }));
  }

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
          <GroupSubPage group={selectedGroup} />
        ) : (
          <NoGroupSelectedSubPage />
        )}
      </div>
    );
  }
}
