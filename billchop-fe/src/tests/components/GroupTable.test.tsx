import { expect } from "chai";
import { render} from "@testing-library/react";
import React from "react";
import GroupTable from "../../components/GroupTable";

describe("Group Table Tests", () => {
  it("Should render group's table correctly", async () => {
    const grouptable = render(
      <GroupTable
        group = {{
          Id: "10",
          Name: "Test 1",
          Users:[
            {Id:"1", Name:"User1", Email:"user1@gmail.com"},
            {Id: "2", Name: "User2", Email: "user2@yahoo.com"}],
        }}
        expenseAmounts = {{1: 36.49, 2: -12.65}}
        currentUserId = {"1"}
      />,
    );

    const firstUser = await grouptable.findAllByText("You");
    expect(firstUser.length).to.be.equal(1);
    expect(firstUser[0].textContent).to.not.equal("User1");

    const secondUser = await grouptable.findAllByText("User2");
    const amountAssigned = secondUser[0].nextElementSibling;
    expect(amountAssigned).to.not.equal(null);
    expect(amountAssigned?.textContent).to.equal("-12.65€");
  });

  it("Should recognise both non-negative/positive and negative expenses", async () => {
    const grouptable = {
      group:{
        Id: "12",
        Name: "Test 2",
        Users:[
          {Id:"2", Name:"John", Email:"johnny@yahoo.com"},
          {Id: "4", Name: "Tom", Email: "tommy@hotmail.com"},
          {Id: "5", Name: "Mark", Email: "mark@gmail.com"},
        ]},
      expenseAmounts: {2: 27.89, 4: 0, 5: -16.60},
      currentUserId: "2",
    };

    expect(grouptable.group.Users).to.not.equal(null);
    expect(grouptable.group.Users[0].Id).to.be.equal(grouptable.currentUserId);
    expect(grouptable.expenseAmounts[2]).to.be.greaterThan(0);
    expect(grouptable.expenseAmounts[4]).to.be.equal(0);
    expect(grouptable.expenseAmounts[5]).to.be.lessThan(0);
  });

  it("Should color the amounts accordingly depending on their values", async () => {
    const grouptable = render(
      <GroupTable
        group = {{
          Id: "8",
          Name: "Test 3",
          Users:[
            {Id:"6", Name:"Roommate1", Email:"roommate1@gmail.com"},
            {Id: "8", Name: "Roommate2", Email: "roommate2@yahoo.com"},
            {Id: "10", Name: "Roommate3", Email: "roommate3@hotmail.com"}],
        }}
        expenseAmounts = {{6: 36.49, 8:-12.65, 10: 0}}
        currentUserId = {"10"}
        colorCode = {true}
        skipCurrentUserAmount = {true}
      />,
    );

    const positive = await grouptable.findAllByText("36.49€");
    expect(positive[0].getAttribute("style")).to.be.equal("color: green;");

    const negative = await grouptable.findAllByText("-12.65€");
    expect(negative[0].getAttribute("style")).to.be.equal("color: red;");

    const current = await grouptable.findAllByText("-");
    expect(current[0].getAttribute("style")).to.be.equal(null);
  });

  it("Should only return users with expenses", async () => {
    const grouptable = render(
      <GroupTable
        group = {{
          Id: "10",
          Name: "Test 4",
          Users:[
            {Id:"123", Name:"User123", Email:"user.123@gmail.com"},
            {Id: "124", Name: "User124", Email: "user.124@yahoo.com"},
            {Id: "126", Name: "User126", Email: "user.126@hotmail.com"}],
        }}
        expenseAmounts = {{123: 20, 124: -12.65}}
        currentUserId = {"126"}
        showMembersOnlyWithExpenses = {true}
      />,
    );
    const initialValue = await grouptable.findAllByText("20.00€");
    expect(initialValue[0].isConnected).to.be.equal(true);
    const rowPart = initialValue[0].parentElement;
    expect(rowPart?.parentNode).to.exist;
    const hasChildren = rowPart?.parentNode?.childElementCount;
    expect(hasChildren).to.be.equal(2);
  });    
});
