using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class CascadeArticleIdInArticleItemInventory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MstArticleItemInventory_MstArticle_ArticleId",
                table: "MstArticleItemInventory");

            migrationBuilder.DropForeignKey(
                name: "FK_SysInventory_MstArticle_ArticleId",
                table: "SysInventory");

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleItemInventory_MstArticle_ArticleId",
                table: "MstArticleItemInventory",
                column: "ArticleId",
                principalTable: "MstArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SysInventory_MstArticle_ArticleId",
                table: "SysInventory",
                column: "ArticleId",
                principalTable: "MstArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MstArticleItemInventory_MstArticle_ArticleId",
                table: "MstArticleItemInventory");

            migrationBuilder.DropForeignKey(
                name: "FK_SysInventory_MstArticle_ArticleId",
                table: "SysInventory");

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleItemInventory_MstArticle_ArticleId",
                table: "MstArticleItemInventory",
                column: "ArticleId",
                principalTable: "MstArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SysInventory_MstArticle_ArticleId",
                table: "SysInventory",
                column: "ArticleId",
                principalTable: "MstArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
