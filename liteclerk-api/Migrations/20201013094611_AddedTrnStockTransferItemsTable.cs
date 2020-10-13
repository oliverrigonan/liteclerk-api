using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedTrnStockTransferItemsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrnStockTransferItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    STId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    ItemInventoryId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_TrnStockTransferItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnStockTransferItem_MstUnit_BaseUnitId",
                        column: x => x.BaseUnitId,
                        principalTable: "MstUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockTransferItem_MstArticle_ItemId",
                        column: x => x.ItemId,
                        principalTable: "MstArticle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockTransferItem_MstArticleItemInventory_ItemInventoryId",
                        column: x => x.ItemInventoryId,
                        principalTable: "MstArticleItemInventory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockTransferItem_TrnStockTransfer_STId",
                        column: x => x.STId,
                        principalTable: "TrnStockTransfer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockTransferItem_MstUnit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "MstUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockTransferItem_BaseUnitId",
                table: "TrnStockTransferItem",
                column: "BaseUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockTransferItem_ItemId",
                table: "TrnStockTransferItem",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockTransferItem_ItemInventoryId",
                table: "TrnStockTransferItem",
                column: "ItemInventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockTransferItem_STId",
                table: "TrnStockTransferItem",
                column: "STId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockTransferItem_UnitId",
                table: "TrnStockTransferItem",
                column: "UnitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrnStockTransferItem");
        }
    }
}
