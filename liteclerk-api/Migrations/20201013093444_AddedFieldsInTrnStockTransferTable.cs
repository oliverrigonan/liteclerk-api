using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedFieldsInTrnStockTransferTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "TrnStockTransfer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "TrnStockTransfer",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "ArticleId",
                table: "TrnStockTransfer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "TrnStockTransfer",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ToBranchId",
                table: "TrnStockTransfer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockTransfer_AccountId",
                table: "TrnStockTransfer",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockTransfer_ArticleId",
                table: "TrnStockTransfer",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockTransfer_ToBranchId",
                table: "TrnStockTransfer",
                column: "ToBranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrnStockTransfer_MstAccount_AccountId",
                table: "TrnStockTransfer",
                column: "AccountId",
                principalTable: "MstAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnStockTransfer_MstArticle_ArticleId",
                table: "TrnStockTransfer",
                column: "ArticleId",
                principalTable: "MstArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnStockTransfer_MstCompanyBranch_ToBranchId",
                table: "TrnStockTransfer",
                column: "ToBranchId",
                principalTable: "MstCompanyBranch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrnStockTransfer_MstAccount_AccountId",
                table: "TrnStockTransfer");

            migrationBuilder.DropForeignKey(
                name: "FK_TrnStockTransfer_MstArticle_ArticleId",
                table: "TrnStockTransfer");

            migrationBuilder.DropForeignKey(
                name: "FK_TrnStockTransfer_MstCompanyBranch_ToBranchId",
                table: "TrnStockTransfer");

            migrationBuilder.DropIndex(
                name: "IX_TrnStockTransfer_AccountId",
                table: "TrnStockTransfer");

            migrationBuilder.DropIndex(
                name: "IX_TrnStockTransfer_ArticleId",
                table: "TrnStockTransfer");

            migrationBuilder.DropIndex(
                name: "IX_TrnStockTransfer_ToBranchId",
                table: "TrnStockTransfer");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "TrnStockTransfer");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "TrnStockTransfer");

            migrationBuilder.DropColumn(
                name: "ArticleId",
                table: "TrnStockTransfer");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "TrnStockTransfer");

            migrationBuilder.DropColumn(
                name: "ToBranchId",
                table: "TrnStockTransfer");
        }
    }
}
