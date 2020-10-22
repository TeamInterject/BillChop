import * as React from "react";
import Form from "react-bootstrap/Form";
import Button from "react-bootstrap/Button";

export class CreateGroupForm extends React.Component<{ onCreateNewGroup: (groupName: string) => void }, { inputValue: string }> {
    render() {
        return (
            <div>
                <div className="m-2">
                    <Form.Label>Group name:</Form.Label>
                    <Form.Control placeholder="Enter the group name" onChange={e => this.setState({ inputValue: e.target.value })} />
                </div>
                <div className="m-2">
                    <Button 
                        variant="outline-primary" 
                        onClick={() => this.props.onCreateNewGroup(this.state.inputValue)}
                    >
                        Create
                    </Button>
                </div>
            </div>
        );
    }
}