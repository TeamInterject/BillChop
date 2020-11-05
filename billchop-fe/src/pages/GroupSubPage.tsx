import * as React from "react";
import Group from "../backend/models/Group";
import GroupTable from "../components/GroupTable";

interface IGroupSubPageProps {
  group: Group;
}

export default class GroupSubPage extends React.Component<IGroupSubPageProps> {
  render(): JSX.Element {
    const { group } = this.props;
    return (
      <div className="group-page__sub-page-container">
        <GroupTable group={group} />
      </div>
    );
  }
}
