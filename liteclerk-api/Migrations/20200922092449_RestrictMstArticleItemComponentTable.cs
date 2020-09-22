using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class RestrictMstArticleItemComponentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MstArticleItemComponent_MstArticle_MstArticle_ArticleIdId",
                table: "MstArticleItemComponent");

            migrationBuilder.DropIndex(
                name: "IX_MstArticleItemComponent_MstArticle_ArticleIdId",
                table: "MstArticleItemComponent");

            migrationBuilder.DropColumn(
                name: "MstArticle_ArticleIdId",
                table: "MstArticleItemComponent");

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleItemComponent_ComponentArticleId",
                table: "MstArticleItemComponent",
                column: "ComponentArticleId");

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleItemComponent_MstArticle_ComponentArticleId",
                table: "MstArticleItemComponent",
                column: "ComponentArticleId",
                principalTable: "MstArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MstArticleItemComponent_MstArticle_ComponentArticleId",
                table: "MstArticleItemComponent");

            migrationBuilder.DropIndex(
                name: "IX_MstArticleItemComponent_ComponentArticleId",
                table: "MstArticleItemComponent");

            migrationBuilder.AddColumn<int>(
                name: "MstArticle_ArticleIdId",
                table: "MstArticleItemComponent",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleItemComponent_MstArticle_ArticleIdId",
                table: "MstArticleItemComponent",
                column: "MstArticle_ArticleIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleItemComponent_MstArticle_MstArticle_ArticleIdId",
                table: "MstArticleItemComponent",
                column: "MstArticle_ArticleIdId",
                principalTable: "MstArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
