using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedMstArticleItemComponentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MstArticleItemComponent",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    MstArticle_ArticleIdId = table.Column<int>(nullable: true),
                    ComponentArticleId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,5)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstArticleItemComponent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MstArticleItemComponent_MstArticle_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "MstArticle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MstArticleItemComponent_MstArticle_MstArticle_ArticleIdId",
                        column: x => x.MstArticle_ArticleIdId,
                        principalTable: "MstArticle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleItemComponent_ArticleId",
                table: "MstArticleItemComponent",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleItemComponent_MstArticle_ArticleIdId",
                table: "MstArticleItemComponent",
                column: "MstArticle_ArticleIdId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MstArticleItemComponent");
        }
    }
}
