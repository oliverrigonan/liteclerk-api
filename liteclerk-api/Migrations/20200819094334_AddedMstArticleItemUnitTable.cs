using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedMstArticleItemUnitTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MstArticleItemUnit",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    UnitId = table.Column<int>(type: "int", nullable: false),
                    Mutliplier = table.Column<decimal>(type: "decimal(18,5)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstArticleItemUnit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MstArticleItemUnit_MstArticle_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "MstArticle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstArticleItemUnit_MstUnit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "MstUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleItemUnit_ArticleId",
                table: "MstArticleItemUnit",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleItemUnit_UnitId",
                table: "MstArticleItemUnit",
                column: "UnitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MstArticleItemUnit");
        }
    }
}
