using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedIsRequiredFieldInMstJobTypeDepartment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SequenceNumber",
                table: "TrnJobOrderDepartment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsRequired",
                table: "MstJobTypeDepartment",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SequenceNumber",
                table: "MstJobTypeDepartment",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SequenceNumber",
                table: "TrnJobOrderDepartment");

            migrationBuilder.DropColumn(
                name: "IsRequired",
                table: "MstJobTypeDepartment");

            migrationBuilder.DropColumn(
                name: "SequenceNumber",
                table: "MstJobTypeDepartment");
        }
    }
}
