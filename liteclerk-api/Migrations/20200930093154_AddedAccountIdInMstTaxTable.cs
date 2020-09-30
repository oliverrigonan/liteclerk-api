using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedAccountIdInMstTaxTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "MstTax",
                type: "int",
                nullable: false,
                defaultValue: 6);

            migrationBuilder.CreateIndex(
                name: "IX_MstTax_AccountId",
                table: "MstTax",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_MstTax_MstAccount_AccountId",
                table: "MstTax",
                column: "AccountId",
                principalTable: "MstAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MstTax_MstAccount_AccountId",
                table: "MstTax");

            migrationBuilder.DropIndex(
                name: "IX_MstTax_AccountId",
                table: "MstTax");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "MstTax");
        }
    }
}
