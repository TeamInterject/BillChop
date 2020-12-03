using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BillChopBE.Migrations
{
    public partial class AddGroupContextToPayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GroupContextId",
                table: "Payments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Payments_GroupContextId",
                table: "Payments",
                column: "GroupContextId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Groups_GroupContextId",
                table: "Payments",
                column: "GroupContextId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Groups_GroupContextId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_GroupContextId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "GroupContextId",
                table: "Payments");
        }
    }
}
