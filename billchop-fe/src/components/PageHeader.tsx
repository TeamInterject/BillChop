import * as React from "react";
import { Button, Nav, Navbar } from "react-bootstrap";
import { Link } from "react-router-dom";

export default class PageHeader extends React.Component {
  render(): JSX.Element {
    return (
      <Navbar bg="light">
        <Navbar.Toggle aria-controls="basic-navbar-nav" />
        <Navbar.Collapse id="basic-navbar-nav">
          <Nav className="mr-auto">
            {/* Change to react-avatar */}
            <Link to="/profile">
              <Button variant="link">Profile</Button>
            </Link>
            <Link to="/groups">
              <Button variant="link">Groups</Button>
            </Link>
            <Link to="/createGroup">
              <Button variant="link">Create group</Button>
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
