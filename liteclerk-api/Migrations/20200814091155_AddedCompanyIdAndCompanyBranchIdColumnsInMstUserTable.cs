using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedCompanyIdAndCompanyBranchIdColumnsInMstUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyBranchId",
                table: "MstUser",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "MstUser",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MstUser_CompanyBranchId",
                table: "MstUser",
                column: "CompanyBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_MstUser_CompanyId",
                table: "MstUser",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_MstUser_MstCompanyBranch_CompanyBranchId",
                table: "MstUser",
                column: "CompanyBranchId",
                principalTable: "MstCompanyBranch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstUser_MstCompany_CompanyId",
                table: "MstUser",
                column: "CompanyId",
                principalTable: "MstCompany",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MstUser_MstCompanyBranch_CompanyBranchId",
                table: "MstUser");

            migrationBuilder.DropForeignKey(
                name: "FK_MstUser_MstCompany_CompanyId",
                table: "MstUser");

            migrationBuilder.DropIndex(
                name: "IX_MstUser_CompanyBranchId",
                table: "MstUser");

            migrationBuilder.DropIndex(
                name: "IX_MstUser_CompanyId",
                table: "MstUser");

            migrationBuilder.DropColumn(
                name: "CompanyBranchId",
                table: "MstUser");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "MstUser");
        }
    }
}
