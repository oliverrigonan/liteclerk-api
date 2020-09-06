using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedTrnCollectionLineTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "TrnCollection",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "MstPayType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PayTypeCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ManualCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PayType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstPayType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MstPayType_MstAccount_AccountIdId",
                        column: x => x.AccountId,
                        principalTable: "MstAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstPayType_MstUser_CreatedByUserIdId",
                        column: x => x.CreatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstPayType_MstUser_UpdatedByUserIdId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrnCollectionLine",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CIId = table.Column<int>(type: "int", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    SIId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    PayTypeId = table.Column<int>(type: "int", nullable: false),
                    Particulars = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CheckNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CheckDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CheckBank = table.Column<string>(type: "nvarchar(255)", maxLength: 50, nullable: false),
                    BankId = table.Column<int>(type: "int", nullable: false),
                    IsClear = table.Column<bool>(type: "bit", nullable: false),
                    WTAXId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrnCollectionLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnCollectionLine_MstAccount_AccountIdId",
                        column: x => x.AccountId,
                        principalTable: "MstAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnCollectionLine_MstArticle_ArticleIdId",
                        column: x => x.ArticleId,
                        principalTable: "MstArticle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnCollectionLine_MstArticle_BankIdId",
                        column: x => x.BankId,
                        principalTable: "MstArticle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnCollectionLine_MstCompanyBranch_BranchIdId",
                        column: x => x.BranchId,
                        principalTable: "MstCompanyBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnCollectionLine_TrnCollection_CIId",
                        column: x => x.CIId,
                        principalTable: "TrnCollection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnCollectionLine_MstPayType_PayTypeIdId",
                        column: x => x.PayTypeId,
                        principalTable: "MstPayType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnCollectionLine_TrnSalesInvoice_SIId",
                        column: x => x.SIId,
                        principalTable: "TrnSalesInvoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnCollectionLine_MstTax_WTAXIdId",
                        column: x => x.WTAXId,
                        principalTable: "MstTax",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MstPayType_AccountId",
                table: "MstPayType",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_MstPayType_CreatedByUserId",
                table: "MstPayType",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MstPayType_UpdatedByUserId",
                table: "MstPayType",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnCollectionLine_AccountId",
                table: "TrnCollectionLine",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnCollectionLine_ArticleId",
                table: "TrnCollectionLine",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnCollectionLine_BankId",
                table: "TrnCollectionLine",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnCollectionLine_BranchId",
                table: "TrnCollectionLine",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnCollectionLine_CIId",
                table: "TrnCollectionLine",
                column: "CIId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnCollectionLine_PayTypeId",
                table: "TrnCollectionLine",
                column: "PayTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnCollectionLine_SIId",
                table: "TrnCollectionLine",
                column: "SIId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnCollectionLine_WTAXId",
                table: "TrnCollectionLine",
                column: "WTAXId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrnCollectionLine");

            migrationBuilder.DropTable(
                name: "MstPayType");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "TrnCollection");
        }
    }
}
