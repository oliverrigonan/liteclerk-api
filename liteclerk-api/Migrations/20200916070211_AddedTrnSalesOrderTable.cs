using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedTrnSalesOrderTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrnSalesOrder",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    SONumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SODate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ManualNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DocumentReference = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    TermId = table.Column<int>(type: "int", nullable: false),
                    DateNeeded = table.Column<DateTime>(type: "datetime", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoldByUserId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    PreparedByUserId = table.Column<int>(type: "int", nullable: false),
                    CheckedByUserId = table.Column<int>(type: "int", nullable: false),
                    ApprovedByUserId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsCancelled = table.Column<bool>(type: "bit", nullable: false),
                    IsPrinted = table.Column<bool>(type: "bit", nullable: false),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrnSalesOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnSalesOrder_MstUser_ApprovedByUserId",
                        column: x => x.ApprovedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnSalesOrder_MstCompanyBranch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "MstCompanyBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnSalesOrder_MstUser_CheckedByUserId",
                        column: x => x.CheckedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnSalesOrder_MstUser_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnSalesOrder_MstCurrency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "MstCurrency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnSalesOrder_MstArticle_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "MstArticle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnSalesOrder_MstUser_PreparedByUserId",
                        column: x => x.PreparedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnSalesOrder_MstUser_SoldByUserId",
                        column: x => x.SoldByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnSalesOrder_MstTerm_TermId",
                        column: x => x.TermId,
                        principalTable: "MstTerm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnSalesOrder_MstUser_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrnSalesOrderItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SOId = table.Column<int>(type: "int", nullable: false),
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
                    VATRate = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    VATAmount = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    WTAXId = table.Column<int>(type: "int", nullable: false),
                    WTAXRate = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    WTAXAmount = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    BaseQuantity = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    BaseUnitId = table.Column<int>(type: "int", nullable: false),
                    BaseNetPrice = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    LineTimeStamp = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrnSalesOrderItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnSalesOrderItem_MstUnit_BaseUnitId",
                        column: x => x.BaseUnitId,
                        principalTable: "MstUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnSalesOrderItem_MstDiscount_DiscountId",
                        column: x => x.DiscountId,
                        principalTable: "MstDiscount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnSalesOrderItem_MstArticle_ItemId",
                        column: x => x.ItemId,
                        principalTable: "MstArticle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnSalesOrderItem_MstArticleItemInventory_ItemInventoryId",
                        column: x => x.ItemInventoryId,
                        principalTable: "MstArticleItemInventory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnSalesOrderItem_MstJobType_ItemJobTypeId",
                        column: x => x.ItemJobTypeId,
                        principalTable: "MstJobType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnSalesOrderItem_TrnSalesOrder_SOId",
                        column: x => x.SOId,
                        principalTable: "TrnSalesOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrnSalesOrderItem_MstUnit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "MstUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnSalesOrderItem_MstTax_VATId",
                        column: x => x.VATId,
                        principalTable: "MstTax",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnSalesOrderItem_MstTax_WTAXId",
                        column: x => x.WTAXId,
                        principalTable: "MstTax",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesOrder_ApprovedByUserId",
                table: "TrnSalesOrder",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesOrder_BranchId",
                table: "TrnSalesOrder",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesOrder_CheckedByUserId",
                table: "TrnSalesOrder",
                column: "CheckedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesOrder_CreatedByUserId",
                table: "TrnSalesOrder",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesOrder_CurrencyId",
                table: "TrnSalesOrder",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesOrder_CustomerId",
                table: "TrnSalesOrder",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesOrder_PreparedByUserId",
                table: "TrnSalesOrder",
                column: "PreparedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesOrder_SoldByUserId",
                table: "TrnSalesOrder",
                column: "SoldByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesOrder_TermId",
                table: "TrnSalesOrder",
                column: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesOrder_UpdatedByUserId",
                table: "TrnSalesOrder",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesOrderItem_BaseUnitId",
                table: "TrnSalesOrderItem",
                column: "BaseUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesOrderItem_DiscountId",
                table: "TrnSalesOrderItem",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesOrderItem_ItemId",
                table: "TrnSalesOrderItem",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesOrderItem_ItemInventoryId",
                table: "TrnSalesOrderItem",
                column: "ItemInventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesOrderItem_ItemJobTypeId",
                table: "TrnSalesOrderItem",
                column: "ItemJobTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesOrderItem_SOId",
                table: "TrnSalesOrderItem",
                column: "SOId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesOrderItem_UnitId",
                table: "TrnSalesOrderItem",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesOrderItem_VATId",
                table: "TrnSalesOrderItem",
                column: "VATId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesOrderItem_WTAXId",
                table: "TrnSalesOrderItem",
                column: "WTAXId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrnSalesOrderItem");

            migrationBuilder.DropTable(
                name: "TrnSalesOrder");
        }
    }
}
