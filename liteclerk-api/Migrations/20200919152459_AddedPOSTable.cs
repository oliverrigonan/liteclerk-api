using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedPOSTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrnPointOfSale",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    TerminalCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    POSDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    POSNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OrderNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CustomerCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    ItemCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: true),
                    Particulars = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    NetPrice = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    TaxCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TaxId = table.Column<int>(type: "int", nullable: true),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    CashierUserCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CashierUserId = table.Column<int>(type: "int", nullable: true),
                    TimeStamp = table.Column<DateTime>(type: "datetime", nullable: false),
                    PostCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrnPointOfSale", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnPointOfSale_MstCompanyBranch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "MstCompanyBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnPointOfSale_MstUser_CashierUserId",
                        column: x => x.CashierUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnPointOfSale_MstArticle_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "MstArticle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnPointOfSale_MstArticle_ItemId",
                        column: x => x.ItemId,
                        principalTable: "MstArticle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnPointOfSale_MstTax_TaxId",
                        column: x => x.TaxId,
                        principalTable: "MstTax",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrnPointOfSale_BranchId",
                table: "TrnPointOfSale",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnPointOfSale_CashierUserId",
                table: "TrnPointOfSale",
                column: "CashierUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnPointOfSale_CustomerId",
                table: "TrnPointOfSale",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnPointOfSale_ItemId",
                table: "TrnPointOfSale",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnPointOfSale_TaxId",
                table: "TrnPointOfSale",
                column: "TaxId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrnPointOfSale");
        }
    }
}
