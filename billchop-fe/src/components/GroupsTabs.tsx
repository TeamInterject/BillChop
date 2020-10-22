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
    alignItems: "center"
};

interface IGroupsTabsProps {
    groups: Group[];
    onCreateNewGroup: (groupName: string) => void
}

export class GroupsTabs extends React.Component<IGroupsTabsProps, {}> {
    constructor(props: IGroupsTabsProps) {
        super(props);
        this.renderGroups = this.renderGroups.bind(this);
        this.renderTabPanes = this.renderTabPanes.bind(this);
    }

    renderGroups() {
        const listGroupItems = this.props.groups.map(group => 
            <ListGroup.Item action href={`#${group.name}`}>{group.name}</ListGroup.Item>
        );
        listGroupItems.push(<ListGroup.Item action href="#createNewGroup">Create new group...</ListGroup.Item>);
        return listGroupItems;
    }

    renderTabPanes() {
        const tabPanes = this.props.groups.map(group =>
            <Tab.Pane eventKey={`#${group.name}`}>
                <GroupTable group={group} />
            </Tab.Pane>
        );
        tabPanes.push(<Tab.Pane eventKey="#createNewGroup"><CreateGroupForm onCreateNewGroup={this.props.onCreateNewGroup}/></Tab.Pane>);
        return tabPanes;
    }

    render() {
        return (
            <Tab.Container id="groupsTabs" defaultActiveKey="#roommates">
                <Row sm={2}>
                    <Col sm={"auto"} >
                        <ListGroup>{this.renderGroups()}</ListGroup>
                    </Col>
                    <div style={divStyle}>
                        <Col sm={"auto"} >
                            <Tab.Content>{this.renderTabPanes()}</Tab.Content>
                        </Col>
                    </div>
                </Row>
            </Tab.Container>
        );
    }
}
