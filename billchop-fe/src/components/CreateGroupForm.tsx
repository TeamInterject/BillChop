import * as React from "react";
import Form from "react-bootstrap/Form";
import Button from "react-bootstrap/Button";

export class CreateGroupForm extends React.Component<{}, {}> {
    render() {
        return (
            <div>
                <div className="m-2">
                    <Form.Label>Group name:</Form.Label>
                    <Form.Control placeholder="Enter the group name" />
                </div>
                <div className="m-2">
                    <Button variant="outline-primary">Create</Button>
                </div>
            </div>
        );
    }
}