using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveManagementSystemAPI.Migrations
{
    /// <inheritdoc />
    public partial class leaveDays : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LeaveType",
                table: "LeaveApplicationForm",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "DaysAppliedFor",
                table: "LeaveApplicationForm",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DaysAppliedFor",
                table: "LeaveApplicationForm");

            migrationBuilder.AlterColumn<string>(
                name: "LeaveType",
                table: "LeaveApplicationForm",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
