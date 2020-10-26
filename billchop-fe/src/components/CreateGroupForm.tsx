import * as React from "react";
import Form from "react-bootstrap/Form";
import Button from "react-bootstrap/Button";

export interface CreateGroupFormProps {
  onCreateNewGroup: (groupName: string) => void;
}

export interface CreateGroupFormState {
  inputValue: string;
}

export class CreateGroupForm extends React.Component<
  CreateGroupFormProps,
  CreateGroupFormState
> {
  constructor(props: CreateGroupFormProps) {
    super(props);
    this.state = {
      inputValue: "",
    };
  }

  render(): React.ReactNode {
    const { onCreateNewGroup } = this.props;
    const { inputValue } = this.state;

    return (
      <div>
        <div className="m-2">
          <Form.Label>Group name:</Form.Label>
          <Form.Control
            placeholder="Enter the group name"
            onChange={(e) => this.setState({ inputValue: e.target.value })}
          />
        </div>
        <div className="m-2">
          <Button
            variant="outline-primary"
            onClick={() => onCreateNewGroup(inputValue)}
          >
            Create
          </Button>
        </div>
      </div>
    );
  }
}
