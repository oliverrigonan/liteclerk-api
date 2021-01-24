using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedCurrentDepartmentInformationInJobOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrentDepartment",
                table: "TrnJobOrder",
                type: "nvarchar(255)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentDepartmentStatus",
                table: "TrnJobOrder",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentDepartmentUserFullName",
                table: "TrnJobOrder",
                type: "nvarchar(255)",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentDepartment",
                table: "TrnJobOrder");

            migrationBuilder.DropColumn(
                name: "CurrentDepartmentStatus",
                table: "TrnJobOrder");

            migrationBuilder.DropColumn(
                name: "CurrentDepartmentUserFullName",
                table: "TrnJobOrder");
        }
    }
}
