import * as React from "react";
import { produce } from "immer";
import Group from "../backend/models/Group";
import Sidebar, { ISidebarTab } from "../components/Sidebar";
import NoGroupSelectedSubPage from "./NoGroupSelectedSubPage";
import GroupSubPage from "./GroupSubPage";
import GroupClient from "../backend/clients/GroupClient";
import UserContext from "../backend/helpers/UserContext";
import { Col, Row } from "react-bootstrap";
import SettleUpSubPage from "./SettleUpSubPage";

interface IGroupsPageState {
  groups: Group[];
  selectedGroupId?: string;
  showSettleUp: boolean;
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
      showSettleUp: false,
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
    this.setState({ selectedGroupId: groupId, showSettleUp: false });
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

  handleOpenSettleUp = (): void => {
    this.setState({ showSettleUp: true });
  };

  handleCloseSettleUp = (): void => {
    this.setState({ showSettleUp: false });
  };

  getLoansToSettle = (): { // [TM] NOTE this is just a draft object, when implementing new model in BE feel free to change it however you seem fit.
    Id: string;
    loanerName: string;
    amountToSettle: number;
  }[] => {
    return [
      {
        Id: "1",
        loanerName: "Ainoras",
        amountToSettle: 100,
      },
      {
        Id: "2",
        loanerName: "Martynas",
        amountToSettle: 350,
      },
      {
        Id: "3",
        loanerName: "Maurice",
        amountToSettle: 225,
      },
    ];
  };

  render(): JSX.Element {
    const { selectedGroupId, showSettleUp } = this.state;
    const { groups } = this.state;
    const selectedGroup = groups.find((group) => group.Id === selectedGroupId);
    return (
      <Row className="h-100">
        <Col className="p-0 border-right" sm={2}>
          <Sidebar
            sidebarTabs={this.getGroupsSidebarTabs()}
            onTabClick={this.handleOnGroupTabSelect}
          />
        </Col>
        <Col className="p-0">
          {selectedGroup ?
            showSettleUp ?
              <SettleUpSubPage
                onCloseSettleUp={this.handleCloseSettleUp}
                onSettle={this.handleCloseSettleUp}
                loansToSettle={this.getLoansToSettle()}
              />
              :
              (
                <GroupSubPage
                  group={selectedGroup}
                  onAddNewMember={this.handleAddNewMember}
                  onOpenSettleUp={this.handleOpenSettleUp}
                />
              ) : (
              <NoGroupSelectedSubPage />
            )}
        </Col>
      </Row>
    );
  }
}
