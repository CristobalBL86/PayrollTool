using Microsoft.EntityFrameworkCore.Migrations;

namespace PayrollTool.Migrations
{
    public partial class relatingemployeetoPayroll : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PayrollRelease_EmployeeId",
                table: "PayrollRelease",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PayrollRelease_Employee_EmployeeId",
                table: "PayrollRelease",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PayrollRelease_Employee_EmployeeId",
                table: "PayrollRelease");

            migrationBuilder.DropIndex(
                name: "IX_PayrollRelease_EmployeeId",
                table: "PayrollRelease");
        }
    }
}
