import * as React from "react";
import { Button, Nav, Navbar } from "react-bootstrap";
import { Link } from "react-router-dom";
import Avatar from "react-avatar";
import ImageButton from "./ImageButton";
import GroupIcon from "../assets/group-icon.svg";
import GroupCreateIcon from "../assets/group-create-icon.svg";

export default class NavigationBar extends React.Component {
  render(): JSX.Element {
    return (
      <Navbar bg="light">
        <Navbar.Toggle aria-controls="basic-navbar-nav" />
        <Navbar.Collapse id="basic-navbar-nav">
          <Nav className="mr-auto">
            <Link to="/profile">
              {/* Pass username as props */}
              <div className="mr-2">
                <Avatar name="Username" round size="40" />
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
            <Button variant="light">Logout</Button>
          </Link>
        </Navbar.Collapse>
      </Navbar>
    );
  }
}
