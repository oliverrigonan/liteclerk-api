using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class CascadeArtileItemInventoryArticleId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SysInventory_MstArticle_ArticleId",
                table: "SysInventory");

            migrationBuilder.DropForeignKey(
                name: "FK_SysInventory_MstArticleItemInventory_ArticleItemInventoryId",
                table: "SysInventory");

            migrationBuilder.RenameColumn(
                name: "PNDate",
                table: "SysInventory",
                newName: "IVDate");

            migrationBuilder.AddForeignKey(
                name: "FK_SysInventory_MstArticle_ArticleId",
                table: "SysInventory",
                column: "ArticleId",
                principalTable: "MstArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SysInventory_MstArticleItemInventory_ArticleItemInventoryId",
                table: "SysInventory",
                column: "ArticleItemInventoryId",
                principalTable: "MstArticleItemInventory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SysInventory_MstArticle_ArticleId",
                table: "SysInventory");

            migrationBuilder.DropForeignKey(
                name: "FK_SysInventory_MstArticleItemInventory_ArticleItemInventoryId",
                table: "SysInventory");

            migrationBuilder.RenameColumn(
                name: "IVDate",
                table: "SysInventory",
                newName: "PNDate");

            migrationBuilder.AddForeignKey(
                name: "FK_SysInventory_MstArticle_ArticleId",
                table: "SysInventory",
                column: "ArticleId",
                principalTable: "MstArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SysInventory_MstArticleItemInventory_ArticleItemInventoryId",
                table: "SysInventory",
                column: "ArticleItemInventoryId",
                principalTable: "MstArticleItemInventory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
