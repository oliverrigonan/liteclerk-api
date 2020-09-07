using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedMstAccountArticleTypeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MstAccountArticleType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    ArticleTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstAccountArticleType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MstAccountArticleType_MstAccount_AccountId",
                        column: x => x.AccountId,
                        principalTable: "MstAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstAccountArticleType_MstArticleType_ArticleTypeId",
                        column: x => x.ArticleTypeId,
                        principalTable: "MstArticleType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MstAccountArticleType_AccountId",
                table: "MstAccountArticleType",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_MstAccountArticleType_ArticleTypeId",
                table: "MstAccountArticleType",
                column: "ArticleTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MstAccountArticleType");
        }
    }
}
