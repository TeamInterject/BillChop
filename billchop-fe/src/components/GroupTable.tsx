import * as React from "react";
import { BillSplitInput } from "./BillSplitInput";
import Table from "react-bootstrap/Table";

export default class GroupTable extends React.Component<{}, {}> {
    render() {
        return (
            <div>
                <BillSplitInput />
                <Table striped bordered hover>
                    <thead>
                        <tr>
                            <th>First Name</th>
                            <th>Last Name</th>
                            <th>Amount</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>Mark</td>
                            <td>Otto</td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>Jacob</td>
                            <td>Thornton</td>
                            <td></td>
                        </tr>
                    </tbody>
                </Table>
            </div>
        );
    }
}