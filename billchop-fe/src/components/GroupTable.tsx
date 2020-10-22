import * as React from "react";
import { BillSplitInput } from "./BillSplitInput";
import Table from "react-bootstrap/Table";

export default class GroupTable extends React.Component<{}, {}> {
    render() {
        return (
            <div>
                <BillSplitInput />
                <div className="m-2">
                    <Table striped bordered hover>
                        <thead>
                            <tr>
                                <th>Name</th>
                                <th>Amount</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>Mark</td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>Jacob</td>
                                <td></td>
                            </tr>
                        </tbody>
                    </Table>
                </div>
            </div>
        );
    }
}