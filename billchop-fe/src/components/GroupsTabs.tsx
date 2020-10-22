import * as React from "react";
import Tab from "react-bootstrap/Tab";
import Col from "react-bootstrap/Col";
import Row from "react-bootstrap/Row";
import ListGroup from "react-bootstrap/ListGroup";
import GroupTable from "./GroupTable";

export class GroupsTabs extends React.Component<{}, {}> {
    render() {
        return (
            <Tab.Container id="groupsTabs" defaultActiveKey="#roommates">
                <Row>
                <Col sm={"auto"} >
                    <ListGroup>
                        <ListGroup.Item action href="#roommates">Roommates</ListGroup.Item>
                        <ListGroup.Item action href="#friends">Friends</ListGroup.Item>
                        <ListGroup.Item action href="#addNewGroup">Add new group...</ListGroup.Item>
                    </ListGroup>
                </Col>
                <Col>
                    <Tab.Content>
                        <Tab.Pane eventKey="#roommates">
                            <GroupTable />
                        </Tab.Pane>
                        <Tab.Pane eventKey="#friends">
                            <GroupTable />
                        </Tab.Pane>
                    </Tab.Content>
                </Col>
                </Row>
            </Tab.Container>
        );
    }
}