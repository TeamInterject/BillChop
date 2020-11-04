import { assert } from "chai";
import { render } from "@testing-library/react";
import Sidebar from "../../components/Sidebar";
import React from "react";
import sinon from "sinon";

describe("Sidebar Tests", () => {
    it("Should render correctly", async () => {
        //Arrange
        const spy = sinon.spy();
        
        //Act
        const sidebar = render(<Sidebar sidebarTabs={[{groupId: "1", groupName: "Test group"}]} onTabClick={spy}/>);

        //Assert
        const result = await sidebar.findAllByText("Test group");

        assert.equal(result.length, 1);
        assert.equal(result[0].textContent, "Test group");
    });
});