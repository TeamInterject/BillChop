import * as React from "react";
import Form from "react-bootstrap/Form";
import Button from "react-bootstrap/Button";
import Container from "react-bootstrap/Container";
import Row from "react-bootstrap/Row";

export class BillSplitInput extends React.Component<{}, {}> {
    render() {
        return (
            <Container>
                <Row>
                    <Form.Label>Expense amount:</Form.Label>
                    <Form.Control placeholder="Enter the amount" />
                </Row>
                <Row>
                    <Button variant="outline-primary">Split</Button>
                </Row>
            </Container>
        );
    }
}