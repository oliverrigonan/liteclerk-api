using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedReceivableMemoAndPayableMemoTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "TrnReceivableMemo",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "TrnReceivableMemo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "TrnReceivableMemo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "TrnPayableMemo",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "TrnPayableMemo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SupplierId",
                table: "TrnPayableMemo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TrnPayableMemoLine",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PMId = table.Column<int>(type: "int", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    RRId = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    Particulars = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrnPayableMemoLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnPayableMemoLine_MstAccount_AccountId",
                        column: x => x.AccountId,
                        principalTable: "MstAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnPayableMemoLine_MstArticle_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "MstArticle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnPayableMemoLine_MstCompanyBranch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "MstCompanyBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnPayableMemoLine_TrnPayableMemo_PMId",
                        column: x => x.PMId,
                        principalTable: "TrnPayableMemo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrnPayableMemoLine_TrnReceivingReceipt_RRId",
                        column: x => x.RRId,
                        principalTable: "TrnReceivingReceipt",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrnReceivableMemoLine",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RMId = table.Column<int>(type: "int", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    SIId = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    Particulars = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrnReceivableMemoLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnReceivableMemoLine_MstAccount_AccountId",
                        column: x => x.AccountId,
                        principalTable: "MstAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnReceivableMemoLine_MstArticle_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "MstArticle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnReceivableMemoLine_MstCompanyBranch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "MstCompanyBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnReceivableMemoLine_TrnReceivableMemo_RMId",
                        column: x => x.RMId,
                        principalTable: "TrnReceivableMemo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrnReceivableMemoLine_TrnSalesInvoice_SIId",
                        column: x => x.SIId,
                        principalTable: "TrnSalesInvoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrnReceivableMemo_CustomerId",
                table: "TrnReceivableMemo",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnPayableMemo_SupplierId",
                table: "TrnPayableMemo",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnPayableMemoLine_AccountId",
                table: "TrnPayableMemoLine",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnPayableMemoLine_ArticleId",
                table: "TrnPayableMemoLine",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnPayableMemoLine_BranchId",
                table: "TrnPayableMemoLine",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnPayableMemoLine_PMId",
                table: "TrnPayableMemoLine",
                column: "PMId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnPayableMemoLine_RRId",
                table: "TrnPayableMemoLine",
                column: "RRId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnReceivableMemoLine_AccountId",
                table: "TrnReceivableMemoLine",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnReceivableMemoLine_ArticleId",
                table: "TrnReceivableMemoLine",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnReceivableMemoLine_BranchId",
                table: "TrnReceivableMemoLine",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnReceivableMemoLine_RMId",
                table: "TrnReceivableMemoLine",
                column: "RMId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnReceivableMemoLine_SIId",
                table: "TrnReceivableMemoLine",
                column: "SIId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrnPayableMemo_MstArticle_SupplierId",
                table: "TrnPayableMemo",
                column: "SupplierId",
                principalTable: "MstArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnReceivableMemo_MstArticle_CustomerId",
                table: "TrnReceivableMemo",
                column: "CustomerId",
                principalTable: "MstArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrnPayableMemo_MstArticle_SupplierId",
                table: "TrnPayableMemo");

            migrationBuilder.DropForeignKey(
                name: "FK_TrnReceivableMemo_MstArticle_CustomerId",
                table: "TrnReceivableMemo");

            migrationBuilder.DropTable(
                name: "TrnPayableMemoLine");

            migrationBuilder.DropTable(
                name: "TrnReceivableMemoLine");

            migrationBuilder.DropIndex(
                name: "IX_TrnReceivableMemo_CustomerId",
                table: "TrnReceivableMemo");

            migrationBuilder.DropIndex(
                name: "IX_TrnPayableMemo_SupplierId",
                table: "TrnPayableMemo");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "TrnReceivableMemo");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "TrnReceivableMemo");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "TrnReceivableMemo");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "TrnPayableMemo");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "TrnPayableMemo");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "TrnPayableMemo");
        }
    }
}
