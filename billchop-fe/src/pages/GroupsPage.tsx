import * as React from "react";
import { produce } from "immer";
import Group from "../backend/models/Group";
import Sidebar, { ISidebarTab } from "../components/Sidebar";
import NoGroupSelectedSubPage from "./NoGroupSelectedSubPage";
import "../styles/groups-page.css";
import GroupSubPage from "./GroupSubPage";
import GroupClient from "../backend/clients/GroupClient";
import UserContext from "../backend/helpers/UserContext";
import { Col, Row } from "react-bootstrap";

interface IGroupsPageState {
  groups: Group[];
  selectedGroupId?: string;
}

export default class GroupsPage extends React.Component<
  unknown,
  IGroupsPageState
> {
  private groupClient = new GroupClient();

  constructor(props = {}) {
    super(props);

    this.state = {
      groups: [],
    };
  }

  componentDidMount(): void {
    this.getGroups();
  }

  getGroups = async (): Promise<void> => {
    const currentUserId = UserContext.authenticatedUser.Id;

    this.groupClient
      .getGroups(currentUserId)
      .then((groups) => this.setState({ groups }));
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

  handleAddNewMember = (newMemberId: string): void => {
    const { groups, selectedGroupId } = this.state;
    const selectedGroupIdx = groups.findIndex((g) => g.Id === selectedGroupId);
    const selectedGroup = groups[selectedGroupIdx];

    if (selectedGroup === undefined) {
      this.getGroups();
      return;
    }

    const updateGroups = (updatedGroup: Group): void => {
      const updatedGroups = produce(groups, (draftGroups) => {
        draftGroups[selectedGroupIdx] = updatedGroup;
      });

      this.setState({ groups: updatedGroups });
    };

    this.groupClient.addUserToGroup(selectedGroup.Id, newMemberId).then((updatedGroup) => updateGroups(updatedGroup));
  };

  render(): JSX.Element {
    const { selectedGroupId } = this.state;
    const { groups } = this.state;
    const selectedGroup = groups.find((group) => group.Id === selectedGroupId);
    return (
      <Row className="h-100">
        <Col className="p-0 sidebar-column border-right" md={2}>
          <Sidebar
            sidebarTabs={this.getGroupsSidebarTabs()}
            onTabClick={this.handleOnGroupTabSelect}
          />
        </Col>
        <Col className="p-0">
          {selectedGroup ? (
            <GroupSubPage
              group={selectedGroup}
              onAddNewMember={this.handleAddNewMember}
            />
          ) : (
            <NoGroupSelectedSubPage />
          )}
        </Col>
      </Row>
    );
  }
}
