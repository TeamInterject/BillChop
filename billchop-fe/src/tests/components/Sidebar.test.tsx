import { expect } from "chai";
import { render, fireEvent } from "@testing-library/react";
import React from "react";
import sinon from "sinon";
import Sidebar from "../../components/Sidebar";

describe("Sidebar Tests", () => {
  it("Should render single group correctly", async () => {
    // Arrange
    // Act
    const sidebar = render(
      <Sidebar
        sidebarTabs={[{ groupId: "1", groupName: "Test group" }]}
        onTabClick={sinon.stub()}
      />,
    );

    // Assert
    const result = await sidebar.findAllByText("Test group");

    expect(result.length).to.be.equal(1);
    expect(result[0].textContent).to.be.equal("Test group");
  });

  it("Should call onTabClick when sidebar item clicked", async () => {
    // Arrange
    const stub = sinon.stub();
    const expectedGroup = { groupId: "1", groupName: "Test group" };

    // Act
    const sidebar = render(
      <Sidebar
        sidebarTabs={[
          expectedGroup,
          { groupId: "2", groupName: "Group for Testing" },
        ]}
        onTabClick={stub}
      />,
    );

    // Assert
    const result = await sidebar.findAllByText("Test group");
    expect(result.length).to.be.equal(1);

    expect(stub.callCount).to.be.equal(0);

    fireEvent.click(result[0]);

    expect(stub.callCount).to.be.equal(1);
    expect(stub.calledWithExactly(expectedGroup.groupId)).to.be.equal(true);
  });
});
