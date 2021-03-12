using Microsoft.EntityFrameworkCore.Migrations;

namespace PayrollTool.Migrations
{
    public partial class ChangingPayrollName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Payroll",
                table: "Payroll");

            migrationBuilder.RenameTable(
                name: "Payroll",
                newName: "PayrollRelease");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PayrollRelease",
                table: "PayrollRelease",
                column: "PayrollReleaseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PayrollRelease",
                table: "PayrollRelease");

            migrationBuilder.RenameTable(
                name: "PayrollRelease",
                newName: "Payroll");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payroll",
                table: "Payroll",
                column: "PayrollReleaseId");
        }
    }
}
