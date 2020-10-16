using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedDisbursementTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "TrnDisbursement",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "BankId",
                table: "TrnDisbursement",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CheckBank",
                table: "TrnDisbursement",
                type: "nvarchar(255)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckDate",
                table: "TrnDisbursement",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CheckNumber",
                table: "TrnDisbursement",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsClear",
                table: "TrnDisbursement",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsCrossCheck",
                table: "TrnDisbursement",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PayTypeId",
                table: "TrnDisbursement",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Payee",
                table: "TrnDisbursement",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "TrnDisbursement",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SupplierId",
                table: "TrnDisbursement",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TrnDisbursementLine",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CVId = table.Column<int>(type: "int", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    RRId = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    Particulars = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WTAXId = table.Column<int>(type: "int", nullable: false),
                    WTAXRate = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    WTAXAmount = table.Column<decimal>(type: "decimal(18,5)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrnDisbursementLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnDisbursementLine_MstAccount_AccountId",
                        column: x => x.AccountId,
                        principalTable: "MstAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnDisbursementLine_MstArticle_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "MstArticle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnDisbursementLine_MstCompanyBranch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "MstCompanyBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnDisbursementLine_TrnDisbursement_CVId",
                        column: x => x.CVId,
                        principalTable: "TrnDisbursement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnDisbursementLine_TrnReceivingReceipt_RRId",
                        column: x => x.RRId,
                        principalTable: "TrnReceivingReceipt",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnDisbursementLine_MstTax_WTAXId",
                        column: x => x.WTAXId,
                        principalTable: "MstTax",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrnDisbursement_BankId",
                table: "TrnDisbursement",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnDisbursement_PayTypeId",
                table: "TrnDisbursement",
                column: "PayTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnDisbursement_SupplierId",
                table: "TrnDisbursement",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnDisbursementLine_AccountId",
                table: "TrnDisbursementLine",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnDisbursementLine_ArticleId",
                table: "TrnDisbursementLine",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnDisbursementLine_BranchId",
                table: "TrnDisbursementLine",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnDisbursementLine_CVId",
                table: "TrnDisbursementLine",
                column: "CVId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnDisbursementLine_RRId",
                table: "TrnDisbursementLine",
                column: "RRId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnDisbursementLine_WTAXId",
                table: "TrnDisbursementLine",
                column: "WTAXId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrnDisbursement_MstArticle_BankId",
                table: "TrnDisbursement",
                column: "BankId",
                principalTable: "MstArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnDisbursement_MstPayType_PayTypeId",
                table: "TrnDisbursement",
                column: "PayTypeId",
                principalTable: "MstPayType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnDisbursement_MstArticle_SupplierId",
                table: "TrnDisbursement",
                column: "SupplierId",
                principalTable: "MstArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrnDisbursement_MstArticle_BankId",
                table: "TrnDisbursement");

            migrationBuilder.DropForeignKey(
                name: "FK_TrnDisbursement_MstPayType_PayTypeId",
                table: "TrnDisbursement");

            migrationBuilder.DropForeignKey(
                name: "FK_TrnDisbursement_MstArticle_SupplierId",
                table: "TrnDisbursement");

            migrationBuilder.DropTable(
                name: "TrnDisbursementLine");

            migrationBuilder.DropIndex(
                name: "IX_TrnDisbursement_BankId",
                table: "TrnDisbursement");

            migrationBuilder.DropIndex(
                name: "IX_TrnDisbursement_PayTypeId",
                table: "TrnDisbursement");

            migrationBuilder.DropIndex(
                name: "IX_TrnDisbursement_SupplierId",
                table: "TrnDisbursement");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "TrnDisbursement");

            migrationBuilder.DropColumn(
                name: "BankId",
                table: "TrnDisbursement");

            migrationBuilder.DropColumn(
                name: "CheckBank",
                table: "TrnDisbursement");

            migrationBuilder.DropColumn(
                name: "CheckDate",
                table: "TrnDisbursement");

            migrationBuilder.DropColumn(
                name: "CheckNumber",
                table: "TrnDisbursement");

            migrationBuilder.DropColumn(
                name: "IsClear",
                table: "TrnDisbursement");

            migrationBuilder.DropColumn(
                name: "IsCrossCheck",
                table: "TrnDisbursement");

            migrationBuilder.DropColumn(
                name: "PayTypeId",
                table: "TrnDisbursement");

            migrationBuilder.DropColumn(
                name: "Payee",
                table: "TrnDisbursement");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "TrnDisbursement");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "TrnDisbursement");
        }
    }
}
