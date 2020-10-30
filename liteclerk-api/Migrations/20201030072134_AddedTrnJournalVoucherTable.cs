using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedTrnJournalVoucherTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CRNumber",
                table: "TrnJournalVoucher",
                newName: "JVNumber");

            migrationBuilder.RenameColumn(
                name: "CRDate",
                table: "TrnJournalVoucher",
                newName: "JVDate");

            migrationBuilder.AddColumn<decimal>(
                name: "CreditAmount",
                table: "TrnJournalVoucher",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DebitAmount",
                table: "TrnJournalVoucher",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "TrnJournalVoucher",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "TrnJournalVoucherLine",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JVId = table.Column<int>(type: "int", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    DebitAmount = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    CreditAmount = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    Particulars = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrnJournalVoucherLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnJournalVoucherLine_MstAccount_AccountId",
                        column: x => x.AccountId,
                        principalTable: "MstAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnJournalVoucherLine_MstArticle_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "MstArticle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnJournalVoucherLine_MstCompanyBranch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "MstCompanyBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnJournalVoucherLine_TrnJournalVoucher_JVId",
                        column: x => x.JVId,
                        principalTable: "TrnJournalVoucher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrnJournalVoucherLine_AccountId",
                table: "TrnJournalVoucherLine",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnJournalVoucherLine_ArticleId",
                table: "TrnJournalVoucherLine",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnJournalVoucherLine_BranchId",
                table: "TrnJournalVoucherLine",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnJournalVoucherLine_JVId",
                table: "TrnJournalVoucherLine",
                column: "JVId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrnJournalVoucherLine");

            migrationBuilder.DropColumn(
                name: "CreditAmount",
                table: "TrnJournalVoucher");

            migrationBuilder.DropColumn(
                name: "DebitAmount",
                table: "TrnJournalVoucher");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "TrnJournalVoucher");

            migrationBuilder.RenameColumn(
                name: "JVNumber",
                table: "TrnJournalVoucher",
                newName: "CRNumber");

            migrationBuilder.RenameColumn(
                name: "JVDate",
                table: "TrnJournalVoucher",
                newName: "CRDate");
        }
    }
}
