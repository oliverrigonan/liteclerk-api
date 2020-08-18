using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedTrnSalesInvoiceItemTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrnSalesInvoiceItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SIId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    ItemInventoryId = table.Column<int>(type: "int", nullable: true),
                    ItemJobTypeId = table.Column<int>(type: "int", nullable: true),
                    Particulars = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    UnitId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    DiscountId = table.Column<int>(type: "int", nullable: false),
                    DiscountRate = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    NetPrice = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    VATId = table.Column<int>(type: "int", nullable: false),
                    WTAXId = table.Column<int>(type: "int", nullable: false),
                    BaseUnitId = table.Column<int>(type: "int", nullable: false),
                    BaseQuantity = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    BaseNetPrice = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    LineTimeStamp = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrnSalesInvoiceItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnSalesInvoiceItem_MstUnit_BaseUnitId",
                        column: x => x.BaseUnitId,
                        principalTable: "MstUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnSalesInvoiceItem_MstDiscount_DiscountId",
                        column: x => x.DiscountId,
                        principalTable: "MstDiscount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnSalesInvoiceItem_MstArticle_ItemId",
                        column: x => x.ItemId,
                        principalTable: "MstArticle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnSalesInvoiceItem_MstArticleItemInventory_ItemInventoryId",
                        column: x => x.ItemInventoryId,
                        principalTable: "MstArticleItemInventory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnSalesInvoiceItem_MstJobType_ItemJobTypeId",
                        column: x => x.ItemJobTypeId,
                        principalTable: "MstJobType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnSalesInvoiceItem_TrnSalesInvoice_SIId",
                        column: x => x.SIId,
                        principalTable: "TrnSalesInvoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnSalesInvoiceItem_MstUnit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "MstUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnSalesInvoiceItem_MstTax_VATId",
                        column: x => x.VATId,
                        principalTable: "MstTax",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnSalesInvoiceItem_MstTax_WTAXId",
                        column: x => x.WTAXId,
                        principalTable: "MstTax",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesInvoiceItem_BaseUnitId",
                table: "TrnSalesInvoiceItem",
                column: "BaseUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesInvoiceItem_DiscountId",
                table: "TrnSalesInvoiceItem",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesInvoiceItem_ItemId",
                table: "TrnSalesInvoiceItem",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesInvoiceItem_ItemInventoryId",
                table: "TrnSalesInvoiceItem",
                column: "ItemInventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesInvoiceItem_ItemJobTypeId",
                table: "TrnSalesInvoiceItem",
                column: "ItemJobTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesInvoiceItem_SIId",
                table: "TrnSalesInvoiceItem",
                column: "SIId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesInvoiceItem_UnitId",
                table: "TrnSalesInvoiceItem",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesInvoiceItem_VATId",
                table: "TrnSalesInvoiceItem",
                column: "VATId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesInvoiceItem_WTAXId",
                table: "TrnSalesInvoiceItem",
                column: "WTAXId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrnSalesInvoiceItem");
        }
    }
}
