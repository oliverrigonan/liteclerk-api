using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedMstArticleBankTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MstArticleBank",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    Bank = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TypeOfAccount = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactPerson = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CashInBankAccountId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstArticleBank", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MstArticleBank_MstArticle_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "MstArticle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MstArticleBank_MstAccount_CashInBankAccountId",
                        column: x => x.CashInBankAccountId,
                        principalTable: "MstAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleBank_ArticleId",
                table: "MstArticleBank",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleBank_CashInBankAccountId",
                table: "MstArticleBank",
                column: "CashInBankAccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MstArticleBank");
        }
    }
}
