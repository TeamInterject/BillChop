import * as React from "react";
import { Button, Nav, Navbar } from "react-bootstrap";
import { Link } from "react-router-dom";
import Avatar from "react-avatar";
import ImageButton from "./ImageButton";
import GroupIcon from "../assets/group-icon.svg";
import GroupCreateIcon from "../assets/group-create-icon.svg";
import User from "../backend/models/User";

export interface NavigationBarProps {
  currentUser?: User;
  logout: () => void;
}

export default class NavigationBar extends React.Component<NavigationBarProps> {
  render(): JSX.Element {
    const { currentUser, logout } = this.props;

    if (!currentUser) return <></>;

    return (
      <Navbar className="mainContainer__pageHeader" bg="light">
        <Navbar.Toggle aria-controls="basic-navbar-nav" />
        <Navbar.Collapse id="basic-navbar-nav">
          <Nav className="mr-auto">
            <Link to="/profile">
              <div className="mr-2">
                <Avatar name={currentUser.Name} round size="40" />
              </div>
            </Link>
            <Link to="/groups">
              <ImageButton imageSource={GroupIcon} tooltipText="Groups" />
            </Link>
            <Link to="/createGroup">
              <ImageButton
                imageSource={GroupCreateIcon}
                tooltipText="Create Group"
              />
            </Link>
          </Nav>
          <Link to="/login">
            <Button variant="light" onClick={logout}>
              Logout
            </Button>
          </Link>
        </Navbar.Collapse>
      </Navbar>
    );
  }
}
