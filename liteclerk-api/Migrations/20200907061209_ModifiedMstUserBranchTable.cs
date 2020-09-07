using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class ModifiedMstUserBranchTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MstUserBranchDBSet_MstCompanyBranch_MstCompanyBranch_BranchIdId",
                table: "MstUserBranchDBSet");

            migrationBuilder.DropForeignKey(
                name: "FK_MstUserBranchDBSet_MstUser_MstUser_UserIdId",
                table: "MstUserBranchDBSet");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MstUserBranchDBSet",
                table: "MstUserBranchDBSet");

            migrationBuilder.DropIndex(
                name: "IX_MstUserBranchDBSet_MstCompanyBranch_BranchIdId",
                table: "MstUserBranchDBSet");

            migrationBuilder.DropIndex(
                name: "IX_MstUserBranchDBSet_MstUser_UserIdId",
                table: "MstUserBranchDBSet");

            migrationBuilder.DropColumn(
                name: "MstCompanyBranch_BranchIdId",
                table: "MstUserBranchDBSet");

            migrationBuilder.DropColumn(
                name: "MstUser_UserIdId",
                table: "MstUserBranchDBSet");

            migrationBuilder.RenameTable(
                name: "MstUserBranchDBSet",
                newName: "MstUserBranch");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MstUserBranch",
                table: "MstUserBranch",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_MstUserBranch_BranchId",
                table: "MstUserBranch",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_MstUserBranch_UserId",
                table: "MstUserBranch",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MstUserBranch_MstCompanyBranch_BranchId",
                table: "MstUserBranch",
                column: "BranchId",
                principalTable: "MstCompanyBranch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstUserBranch_MstUser_UserId",
                table: "MstUserBranch",
                column: "UserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MstUserBranch_MstCompanyBranch_BranchId",
                table: "MstUserBranch");

            migrationBuilder.DropForeignKey(
                name: "FK_MstUserBranch_MstUser_UserId",
                table: "MstUserBranch");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MstUserBranch",
                table: "MstUserBranch");

            migrationBuilder.DropIndex(
                name: "IX_MstUserBranch_BranchId",
                table: "MstUserBranch");

            migrationBuilder.DropIndex(
                name: "IX_MstUserBranch_UserId",
                table: "MstUserBranch");

            migrationBuilder.RenameTable(
                name: "MstUserBranch",
                newName: "MstUserBranchDBSet");

            migrationBuilder.AddColumn<int>(
                name: "MstCompanyBranch_BranchIdId",
                table: "MstUserBranchDBSet",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MstUser_UserIdId",
                table: "MstUserBranchDBSet",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MstUserBranchDBSet",
                table: "MstUserBranchDBSet",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_MstUserBranchDBSet_MstCompanyBranch_BranchIdId",
                table: "MstUserBranchDBSet",
                column: "MstCompanyBranch_BranchIdId");

            migrationBuilder.CreateIndex(
                name: "IX_MstUserBranchDBSet_MstUser_UserIdId",
                table: "MstUserBranchDBSet",
                column: "MstUser_UserIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_MstUserBranchDBSet_MstCompanyBranch_MstCompanyBranch_BranchIdId",
                table: "MstUserBranchDBSet",
                column: "MstCompanyBranch_BranchIdId",
                principalTable: "MstCompanyBranch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstUserBranchDBSet_MstUser_MstUser_UserIdId",
                table: "MstUserBranchDBSet",
                column: "MstUser_UserIdId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
