using Microsoft.EntityFrameworkCore.Migrations;

namespace BillChopBE.Migrations
{
    public partial class AddEntityMapping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Users_PayerId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Users_ReceiverId",
                table: "Payments");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Users_PayerId",
                table: "Payments",
                column: "PayerId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Users_ReceiverId",
                table: "Payments",
                column: "ReceiverId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Users_PayerId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Users_ReceiverId",
                table: "Payments");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Users_PayerId",
                table: "Payments",
                column: "PayerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Users_ReceiverId",
                table: "Payments",
                column: "ReceiverId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
