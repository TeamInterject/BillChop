using Microsoft.EntityFrameworkCore.Migrations;

namespace BillChopBE.Migrations
{
    public partial class RenamedBillLoaner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bills_Users_PayerId",
                table: "Bills");

            migrationBuilder.RenameColumn(
                name: "PayerId",
                table: "Bills",
                newName: "LoanerId");

            migrationBuilder.RenameIndex(
                name: "IX_Bills_PayerId",
                table: "Bills",
                newName: "IX_Bills_LoanerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_Users_LoanerId",
                table: "Bills",
                column: "LoanerId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bills_Users_LoanerId",
                table: "Bills");

            migrationBuilder.RenameColumn(
                name: "LoanerId",
                table: "Bills",
                newName: "PayerId");

            migrationBuilder.RenameIndex(
                name: "IX_Bills_LoanerId",
                table: "Bills",
                newName: "IX_Bills_PayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_Users_PayerId",
                table: "Bills",
                column: "PayerId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
