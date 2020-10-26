import * as React from "react";
import Tab from "react-bootstrap/Tab";
import Col from "react-bootstrap/Col";
import Row from "react-bootstrap/Row";
import ListGroup from "react-bootstrap/ListGroup";
import GroupTable from "./GroupTable";
import { CreateGroupForm } from "./CreateGroupForm";
import Group from "../api/Group";

const divStyle: React.CSSProperties = {
  display: "flex",
  justifyContent: "center",
  alignItems: "center",
};

interface IGroupsTabsProps {
  groups: Group[];
  onCreateNewGroup: (groupName: string) => void;
}

export default class GroupsTabs extends React.Component<IGroupsTabsProps, {}> {
  constructor(props: IGroupsTabsProps) {
    super(props);
    this.renderGroups = this.renderGroups.bind(this);
    this.renderTabPanes = this.renderTabPanes.bind(this);
  }

  renderGroups(): JSX.Element[] {
    const { groups } = this.props;
    const listGroupItems = groups.map((group) => (
      <ListGroup.Item action href={`#${group.Name}`}>
        {group.Name}
      </ListGroup.Item>
    ));
    listGroupItems.push(
      <ListGroup.Item action href="#createNewGroup">
        Create new group...
      </ListGroup.Item>
    );
    return listGroupItems;
  }

  renderTabPanes(): JSX.Element[] {
    const { groups } = this.props;
    const tabPanes = groups.map((group) => (
      <Tab.Pane eventKey={`#${group.Name}`}>
        <GroupTable group={group} />
      </Tab.Pane>
    ));
    const { onCreateNewGroup } = this.props;
    tabPanes.push(
      <Tab.Pane eventKey="#createNewGroup">
        <CreateGroupForm onCreateNewGroup={onCreateNewGroup} />
      </Tab.Pane>
    );
    return tabPanes;
  }

  render(): JSX.Element {
    return (
      <Tab.Container id="groupsTabs">
        <Row sm={2}>
          <Col sm={"auto"}>
            <ListGroup>{this.renderGroups()}</ListGroup>
          </Col>
          <div style={divStyle}>
            <Col sm={"auto"}>
              <Tab.Content>{this.renderTabPanes()}</Tab.Content>
            </Col>
          </div>
        </Row>
      </Tab.Container>
    );
  }
}
