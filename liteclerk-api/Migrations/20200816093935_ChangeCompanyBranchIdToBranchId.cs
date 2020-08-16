using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class ChangeCompanyBranchIdToBranchId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MstUser_MstCompanyBranch_CompanyBranchId",
                table: "MstUser");

            migrationBuilder.RenameColumn(
                name: "CompanyBranchId",
                table: "MstUser",
                newName: "BranchId");

            migrationBuilder.RenameIndex(
                name: "IX_MstUser_CompanyBranchId",
                table: "MstUser",
                newName: "IX_MstUser_BranchId");

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

            migrationBuilder.RenameColumn(
                name: "BranchId",
                table: "MstUser",
                newName: "CompanyBranchId");

            migrationBuilder.RenameIndex(
                name: "IX_MstUser_BranchId",
                table: "MstUser",
                newName: "IX_MstUser_CompanyBranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_MstUser_MstCompanyBranch_CompanyBranchId",
                table: "MstUser",
                column: "CompanyBranchId",
                principalTable: "MstCompanyBranch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
