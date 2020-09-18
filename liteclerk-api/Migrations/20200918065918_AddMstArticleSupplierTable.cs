using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddMstArticleSupplierTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MstArticleSupplier",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    Supplier = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactPerson = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PayableAccountId = table.Column<int>(type: "int", nullable: false),
                    TermId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstArticleSupplier", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MstArticleSupplier_MstArticle_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "MstArticle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MstArticleSupplier_MstAccount_PayableAccountId",
                        column: x => x.PayableAccountId,
                        principalTable: "MstAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstArticleSupplier_MstTerm_TermId",
                        column: x => x.TermId,
                        principalTable: "MstTerm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleSupplier_ArticleId",
                table: "MstArticleSupplier",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleSupplier_PayableAccountId",
                table: "MstArticleSupplier",
                column: "PayableAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleSupplier_TermId",
                table: "MstArticleSupplier",
                column: "TermId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MstArticleSupplier");
        }
    }
}
