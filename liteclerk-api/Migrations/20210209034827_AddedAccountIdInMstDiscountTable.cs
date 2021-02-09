using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedAccountIdInMstDiscountTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "MstDiscount",
                type: "int",
                nullable: false,
                defaultValue: 13);

            migrationBuilder.CreateIndex(
                name: "IX_MstDiscount_AccountId",
                table: "MstDiscount",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_MstDiscount_MstAccount_AccountId",
                table: "MstDiscount",
                column: "AccountId",
                principalTable: "MstAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MstDiscount_MstAccount_AccountId",
                table: "MstDiscount");

            migrationBuilder.DropIndex(
                name: "IX_MstDiscount_AccountId",
                table: "MstDiscount");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "MstDiscount");
        }
    }
}
