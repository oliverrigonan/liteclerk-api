using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedFieldAssignedToUserIdInTrnJobOrderDepartment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssignedToUserId",
                table: "TrnJobOrderDepartment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TrnJobOrderDepartment_AssignedToUserId",
                table: "TrnJobOrderDepartment",
                column: "AssignedToUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrnJobOrderDepartment_MstUser_AssignedToUserId",
                table: "TrnJobOrderDepartment",
                column: "AssignedToUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrnJobOrderDepartment_MstUser_AssignedToUserId",
                table: "TrnJobOrderDepartment");

            migrationBuilder.DropIndex(
                name: "IX_TrnJobOrderDepartment_AssignedToUserId",
                table: "TrnJobOrderDepartment");

            migrationBuilder.DropColumn(
                name: "AssignedToUserId",
                table: "TrnJobOrderDepartment");
        }
    }
}
