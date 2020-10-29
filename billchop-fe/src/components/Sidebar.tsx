import * as React from "react";
import ListGroup from "react-bootstrap/ListGroup";
import "../styles/group-page.css";

export interface ISidebarTab {
  groupName: string;
  groupId: string;
}

interface IProps {
  sidebarTabs: ISidebarTab[];
  onTabClick: (groupId: string) => void;
  onCreateNewGroup: (groupName: string) => void;
}

export default class Sidebar extends React.Component<IProps> {
  constructor(props: IProps) {
    super(props);
    this.renderGroupsTabs = this.renderGroupsTabs.bind(this);
  }

  renderGroupsTabs(): JSX.Element[] {
    const { sidebarTabs, onTabClick } = this.props;
    return sidebarTabs.map((tab) => (
      <ListGroup.Item
        action
        key={tab.groupId}
        href={`#${tab.groupId}`}
        onClick={() => onTabClick(tab.groupId)}
      >
        {tab.groupName}
      </ListGroup.Item>
    ));
  }

  // renderTabPanes(): JSX.Element[] {
  //   const { groups } = this.props;
  //   const tabPanes = groups.map((group) => (
  //     <Tab.Pane eventKey={`#${group.Name}`}>
  //       <GroupTable group={group} />
  //     </Tab.Pane>
  //   ));
  //   const { onCreateNewGroup } = this.props;
  //   tabPanes.push(
  //     <Tab.Pane eventKey="#createNewGroup">
  //       <CreateGroupForm onCreateNewGroup={onCreateNewGroup} />
  //     </Tab.Pane>
  //   );
  //   return tabPanes;
  // }

  render(): JSX.Element {
    return (
      <div className="sidebar">
        <ListGroup>{this.renderGroupsTabs()}</ListGroup>
      </div>
    );
  }
}
