using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedReceivingReceiptTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AdjustmentAmount",
                table: "TrnReceivingReceipt",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "TrnReceivingReceipt",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "BalanceAmount",
                table: "TrnReceivingReceipt",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PaidAmount",
                table: "TrnReceivingReceipt",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "TrnReceivingReceipt",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SupplierId",
                table: "TrnReceivingReceipt",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TermId",
                table: "TrnReceivingReceipt",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TrnReceivingReceiptItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RRId = table.Column<int>(type: "int", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    POId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Particulars = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    UnitId = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    BaseQuantity = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    BaseUnitId = table.Column<int>(type: "int", nullable: false),
                    BaseCost = table.Column<decimal>(type: "decimal(18,5)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrnReceivingReceiptItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnReceivingReceiptItem_MstUnit_BaseUnitId",
                        column: x => x.BaseUnitId,
                        principalTable: "MstUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnReceivingReceiptItem_MstCompanyBranch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "MstCompanyBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnReceivingReceiptItem_MstArticle_ItemId",
                        column: x => x.ItemId,
                        principalTable: "MstArticle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnReceivingReceiptItem_TrnPurchaseOrder_POId",
                        column: x => x.POId,
                        principalTable: "TrnPurchaseOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnReceivingReceiptItem_TrnReceivingReceipt_RRId",
                        column: x => x.RRId,
                        principalTable: "TrnReceivingReceipt",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnReceivingReceiptItem_MstUnit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "MstUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrnReceivingReceipt_SupplierId",
                table: "TrnReceivingReceipt",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnReceivingReceipt_TermId",
                table: "TrnReceivingReceipt",
                column: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnReceivingReceiptItem_BaseUnitId",
                table: "TrnReceivingReceiptItem",
                column: "BaseUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnReceivingReceiptItem_BranchId",
                table: "TrnReceivingReceiptItem",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnReceivingReceiptItem_ItemId",
                table: "TrnReceivingReceiptItem",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnReceivingReceiptItem_POId",
                table: "TrnReceivingReceiptItem",
                column: "POId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnReceivingReceiptItem_RRId",
                table: "TrnReceivingReceiptItem",
                column: "RRId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnReceivingReceiptItem_UnitId",
                table: "TrnReceivingReceiptItem",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrnReceivingReceipt_MstArticle_SupplierId",
                table: "TrnReceivingReceipt",
                column: "SupplierId",
                principalTable: "MstArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnReceivingReceipt_MstTerm_TermId",
                table: "TrnReceivingReceipt",
                column: "TermId",
                principalTable: "MstTerm",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrnReceivingReceipt_MstArticle_SupplierId",
                table: "TrnReceivingReceipt");

            migrationBuilder.DropForeignKey(
                name: "FK_TrnReceivingReceipt_MstTerm_TermId",
                table: "TrnReceivingReceipt");

            migrationBuilder.DropTable(
                name: "TrnReceivingReceiptItem");

            migrationBuilder.DropIndex(
                name: "IX_TrnReceivingReceipt_SupplierId",
                table: "TrnReceivingReceipt");

            migrationBuilder.DropIndex(
                name: "IX_TrnReceivingReceipt_TermId",
                table: "TrnReceivingReceipt");

            migrationBuilder.DropColumn(
                name: "AdjustmentAmount",
                table: "TrnReceivingReceipt");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "TrnReceivingReceipt");

            migrationBuilder.DropColumn(
                name: "BalanceAmount",
                table: "TrnReceivingReceipt");

            migrationBuilder.DropColumn(
                name: "PaidAmount",
                table: "TrnReceivingReceipt");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "TrnReceivingReceipt");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "TrnReceivingReceipt");

            migrationBuilder.DropColumn(
                name: "TermId",
                table: "TrnReceivingReceipt");
        }
    }
}
