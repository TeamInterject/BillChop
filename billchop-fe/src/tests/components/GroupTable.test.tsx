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
          Name: "Test",
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
    expect(secondUser[0].nextSibling?.textContent).to.equal("-12.65€");
  });

  it("Should recognise both non-negative/positive and negative expenses", async () => {
    const grouptable = {
      group:{
        Id: "10",
        Name: "Test",
        Users:[
          {Id:"1", Name:"User1", Email:"user1@gmail.com"},
          {Id: "2", Name: "User2", Email: "user2@yahoo.com"},
          {Id: "3", Name: "User3", Email: "user3@hotmail.com"},
        ]},
      expenseAmounts: {1: 36.49, 2: 0, 3: -12.65},
      currentUserId: "1",
    };

    expect(grouptable.expenseAmounts[1]).to.be.greaterThan(0);
    expect(grouptable.expenseAmounts[2]).to.be.equal(0);
    expect(grouptable.expenseAmounts[3]).to.be.lessThan(0);
  });

  it("Should color the amounts accordingly depending on their values", async () => {
    const grouptable = render(
      <GroupTable
        group = {{
          Id: "10",
          Name: "Test",
          Users:[
            {Id:"1", Name:"User1", Email:"user1@gmail.com"},
            {Id: "2", Name: "User2", Email: "user2@yahoo.com"},
            {Id: "3", Name: "User3", Email: "user3@hotmail.com"}],
        }}
        expenseAmounts = {{1: 36.49, 2:-12.65, 3: 0}}
        currentUserId = {"3"}
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
});
