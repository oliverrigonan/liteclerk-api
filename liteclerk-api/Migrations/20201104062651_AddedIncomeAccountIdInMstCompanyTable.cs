using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedIncomeAccountIdInMstCompanyTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IncomeAccountId",
                table: "MstCompany",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MstCompany_IncomeAccountId",
                table: "MstCompany",
                column: "IncomeAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_MstCompany_MstAccount_IncomeAccountId",
                table: "MstCompany",
                column: "IncomeAccountId",
                principalTable: "MstAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MstCompany_MstAccount_IncomeAccountId",
                table: "MstCompany");

            migrationBuilder.DropIndex(
                name: "IX_MstCompany_IncomeAccountId",
                table: "MstCompany");

            migrationBuilder.DropColumn(
                name: "IncomeAccountId",
                table: "MstCompany");
        }
    }
}
