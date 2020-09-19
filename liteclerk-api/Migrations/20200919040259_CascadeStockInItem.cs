using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class CascadeStockInItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MstArticleItem_MstArticle_ArticleId",
                table: "MstArticleItem");

            migrationBuilder.DropForeignKey(
                name: "FK_TrnStockInItem_TrnStockIn_INId",
                table: "TrnStockInItem");

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleItem_MstArticle_ArticleId",
                table: "MstArticleItem",
                column: "ArticleId",
                principalTable: "MstArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnStockInItem_TrnStockIn_INId",
                table: "TrnStockInItem",
                column: "INId",
                principalTable: "TrnStockIn",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MstArticleItem_MstArticle_ArticleId",
                table: "MstArticleItem");

            migrationBuilder.DropForeignKey(
                name: "FK_TrnStockInItem_TrnStockIn_INId",
                table: "TrnStockInItem");

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleItem_MstArticle_ArticleId",
                table: "MstArticleItem",
                column: "ArticleId",
                principalTable: "MstArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnStockInItem_TrnStockIn_INId",
                table: "TrnStockInItem",
                column: "INId",
                principalTable: "TrnStockIn",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
