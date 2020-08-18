using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedMstArticleItemInventoryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MstArticleItemInventory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    InventoryCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,5)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstArticleItemInventory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MstArticleItemInventory_MstArticle_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "MstArticle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstArticleItemInventory_MstCompanyBranch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "MstCompanyBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleItemInventory_ArticleId",
                table: "MstArticleItemInventory",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleItemInventory_BranchId",
                table: "MstArticleItemInventory",
                column: "BranchId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MstArticleItemInventory");
        }
    }
}
