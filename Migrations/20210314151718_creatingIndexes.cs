using Microsoft.EntityFrameworkCore.Migrations;

namespace PayrollTool.Migrations
{
    public partial class creatingIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TransacionLog_Date_OperationId_ProductId",
                table: "TransacionLog",
                columns: new[] { "Date", "OperationId", "ProductId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssistanceLog_Date_EmployeeId",
                table: "AssistanceLog",
                columns: new[] { "Date", "EmployeeId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TransacionLog_Date_OperationId_ProductId",
                table: "TransacionLog");

            migrationBuilder.DropIndex(
                name: "IX_AssistanceLog_Date_EmployeeId",
                table: "AssistanceLog");
        }
    }
}
