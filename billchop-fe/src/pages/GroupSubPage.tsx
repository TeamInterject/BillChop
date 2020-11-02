import * as React from "react";
import Group from "../api/Group";
import GroupTable from "../components/GroupTable";

interface IProps {
  group: Group;
}

export default class GroupSubPage extends React.Component<IProps> {
  render(): JSX.Element {
    const { group } = this.props;
    return (
      <div className="group-page__sub-page-container">
        <GroupTable group={group} />
      </div>
    );
  }
}
