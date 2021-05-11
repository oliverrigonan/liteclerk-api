using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedForexGainAndLossAccountIdsInCompanyTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ForexGainAccountId",
                table: "MstCompany",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ForexLossAccountId",
                table: "MstCompany",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MstCompany_ForexGainAccountId",
                table: "MstCompany",
                column: "ForexGainAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_MstCompany_ForexLossAccountId",
                table: "MstCompany",
                column: "ForexLossAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_MstCompany_MstAccount_ForexGainAccountId",
                table: "MstCompany",
                column: "ForexGainAccountId",
                principalTable: "MstAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstCompany_MstAccount_ForexLossAccountId",
                table: "MstCompany",
                column: "ForexLossAccountId",
                principalTable: "MstAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MstCompany_MstAccount_ForexGainAccountId",
                table: "MstCompany");

            migrationBuilder.DropForeignKey(
                name: "FK_MstCompany_MstAccount_ForexLossAccountId",
                table: "MstCompany");

            migrationBuilder.DropIndex(
                name: "IX_MstCompany_ForexGainAccountId",
                table: "MstCompany");

            migrationBuilder.DropIndex(
                name: "IX_MstCompany_ForexLossAccountId",
                table: "MstCompany");

            migrationBuilder.DropColumn(
                name: "ForexGainAccountId",
                table: "MstCompany");

            migrationBuilder.DropColumn(
                name: "ForexLossAccountId",
                table: "MstCompany");
        }
    }
}
