import * as React from "react";
import ListGroup from "react-bootstrap/ListGroup";
import "../styles/group-page.css";

export interface ISidebarTab {
  groupName: string;
  groupId: string;
}

export interface ISidebarProps {
  sidebarTabs: ISidebarTab[];
  onTabClick: (groupId: string) => void;
}

export default class Sidebar extends React.Component<ISidebarProps> {
  renderGroupsTabs = (): JSX.Element[] => {
    const { sidebarTabs, onTabClick } = this.props;
    return sidebarTabs.map((tab) => {
      return (
        <ListGroup.Item
          action
          key={tab.groupId}
          href={`#${tab.groupId}`}
          onClick={() => onTabClick(tab.groupId)}
        >
          {tab.groupName}
        </ListGroup.Item>
      );
    });
  }

  render(): JSX.Element {
    return (
      <div className="sidebar">
        <ListGroup>{this.renderGroupsTabs()}</ListGroup>
      </div>
    );
  }
}
