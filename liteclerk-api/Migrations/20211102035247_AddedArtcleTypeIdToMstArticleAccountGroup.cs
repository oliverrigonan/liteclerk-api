using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedArtcleTypeIdToMstArticleAccountGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ArticleTypeId",
                table: "MstArticleAccountGroup",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleAccountGroup_ArticleTypeId",
                table: "MstArticleAccountGroup",
                column: "ArticleTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleAccountGroup_MstArticleType_ArticleTypeId",
                table: "MstArticleAccountGroup",
                column: "ArticleTypeId",
                principalTable: "MstArticleType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MstArticleAccountGroup_MstArticleType_ArticleTypeId",
                table: "MstArticleAccountGroup");

            migrationBuilder.DropIndex(
                name: "IX_MstArticleAccountGroup_ArticleTypeId",
                table: "MstArticleAccountGroup");

            migrationBuilder.DropColumn(
                name: "ArticleTypeId",
                table: "MstArticleAccountGroup");
        }
    }
}
