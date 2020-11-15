import * as React from "react";
import { Col, Row } from "react-bootstrap";
import ImageButton from "./ImageButton";
import AddBillIcon from "../assets/add-bill-icon.svg";
import AddPersonIcon from "../assets/add-person-icon.svg";
import AddBillModal from "./AddBillModal";
import SearchBox from "./SearchBox";
import UserClient from "../backend/clients/UserClient";
import User from "../backend/models/User";

export interface IGroupPageHeaderProps {
  groupId: string;
  onAddNewBill: (name: string, total: number) => void;
  onAddNewMember: (userId: string) => void;
}

export interface IGroupPageHeaderState {
  showAddBillModal: boolean;
  showSearchBox: boolean;
  foundUsers: User[];
}

export default class GroupPageHeader extends React.Component<
  IGroupPageHeaderProps,
  IGroupPageHeaderState
  > {
  constructor(props: IGroupPageHeaderProps) {
    super(props);
    this.state = {
      showAddBillModal: false,
      showSearchBox: false,
      foundUsers: [],
    };
  }

  private userClient = new UserClient();

  toggleAddBillModal = (): void => {
    const { showAddBillModal } = this.state;
    this.setState({ showAddBillModal: !showAddBillModal });
  };

  handleAddNewBill = (name: string, total: number): void => {
    const { onAddNewBill } = this.props;
    onAddNewBill(name, total);
    this.toggleAddBillModal();
  };

  toggleSearchBox = (): void => {
    const { showSearchBox } = this.state;
    this.setState({ showSearchBox: !showSearchBox });
  };

  handleSearchInputChange = async (keyword: string): Promise<void> => {
    const { groupId } = this.props;
    const result = await this.userClient.searchUserByKeyword({ keyword, exclusionGroupId: groupId });
    this.setState({ foundUsers: result });
  };

  handleAddNewMember = (selectedItemId: string): void => {
    const { onAddNewMember } = this.props;

    onAddNewMember(selectedItemId);
    this.toggleSearchBox();
  };

  generateSearchResultsMap = (): Map<string, string> => {
    const { foundUsers } = this.state;
    const searchResults = new Map<string, string>();
    foundUsers.forEach((user) => {
      searchResults.set(user.Id, `${user.Name} (${user.Email})`);
    });
    return searchResults;
  };

  render(): JSX.Element {
    const { showAddBillModal, showSearchBox } = this.state;
    return (
      <div className="m-2">
        {
          showSearchBox ?
            <Row>
              <Col className="mb-2">
                <SearchBox
                  placeholder="Enter email or name"
                  searchResults={this.generateSearchResultsMap()}
                  onChange={this.handleSearchInputChange}
                  onActionButtonClick={this.handleAddNewMember}
                  actionButtonText="Add"
                  onHide={this.toggleSearchBox}
                />
              </Col>
            </Row>
            :
            <Row className="justify-content-start">
              <Col xs={1}>
                <ImageButton
                  imageSource={AddBillIcon}
                  tooltipText="Add Bill"
                  onClick={this.toggleAddBillModal}
                />
              </Col>
              <Col xs={1}>
                <ImageButton
                  imageSource={AddPersonIcon}
                  tooltipText="Add new member"
                  onClick={this.toggleSearchBox}
                />
              </Col>
            </Row>
        }
        <AddBillModal
          showModal={showAddBillModal}
          onHide={this.toggleAddBillModal}
          onAdd={this.handleAddNewBill}
        />
      </div>
    );
  }
}
