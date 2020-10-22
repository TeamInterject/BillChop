import * as React from "react";
import Tab from "react-bootstrap/Tab";
import Col from "react-bootstrap/Col";
import Row from "react-bootstrap/Row";
import ListGroup from "react-bootstrap/ListGroup";
import GroupTable from "./GroupTable";
import { CreateGroupForm } from "./CreateGroupForm";

const divStyle: React.CSSProperties = {
    display: "flex",
    justifyContent: "center",
    alignItems: "center"
 };

export class GroupsTabs extends React.Component<{}, {}> {
    render() {
        return (
            <Tab.Container id="groupsTabs" defaultActiveKey="#roommates">
                <Row sm={2}>
                    <Col sm={"auto"} >
                        <ListGroup>
                            <ListGroup.Item action href="#roommates">Roommates</ListGroup.Item>
                            <ListGroup.Item action href="#friends">Friends</ListGroup.Item>
                            <ListGroup.Item action href="#createNewGroup">Create new group...</ListGroup.Item>
                        </ListGroup>
                    </Col>
                    <div style={divStyle}>
                        <Col sm={"auto"} >
                            <Tab.Content>
                                <Tab.Pane eventKey="#roommates">
                                    <GroupTable />
                                </Tab.Pane>
                                <Tab.Pane eventKey="#friends">
                                    <GroupTable />
                                </Tab.Pane>
                                <Tab.Pane eventKey="#createNewGroup">
                                    <CreateGroupForm />
                                </Tab.Pane>
                            </Tab.Content>
                        </Col>
                    </div>
                </Row>
            </Tab.Container>
        );
    }
}