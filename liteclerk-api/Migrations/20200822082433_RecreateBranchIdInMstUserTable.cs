using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class RecreateBranchIdInMstUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MstUser_MstCompanyBranch_MstCompanyBranch_BranchId",
                table: "MstUser");

            migrationBuilder.DropIndex(
                name: "IX_MstUser_MstCompanyBranch_BranchId",
                table: "MstUser");

            migrationBuilder.DropColumn(
                name: "MstCompanyBranch_BranchId",
                table: "MstUser");

            migrationBuilder.CreateIndex(
                name: "IX_MstUser_BranchId",
                table: "MstUser",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_MstUser_MstCompanyBranch_BranchId",
                table: "MstUser",
                column: "BranchId",
                principalTable: "MstCompanyBranch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MstUser_MstCompanyBranch_BranchId",
                table: "MstUser");

            migrationBuilder.DropIndex(
                name: "IX_MstUser_BranchId",
                table: "MstUser");

            migrationBuilder.AddColumn<int>(
                name: "MstCompanyBranch_BranchId",
                table: "MstUser",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MstUser_MstCompanyBranch_BranchId",
                table: "MstUser",
                column: "MstCompanyBranch_BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_MstUser_MstCompanyBranch_MstCompanyBranch_BranchId",
                table: "MstUser",
                column: "MstCompanyBranch_BranchId",
                principalTable: "MstCompanyBranch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
