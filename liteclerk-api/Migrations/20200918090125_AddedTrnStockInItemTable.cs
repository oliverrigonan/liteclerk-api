using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedTrnStockInItemTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrnStockInItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    INId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_TrnStockInItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnStockInItem_MstUnit_BaseUnitId",
                        column: x => x.BaseUnitId,
                        principalTable: "MstUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockInItem_TrnStockIn_INId",
                        column: x => x.INId,
                        principalTable: "TrnStockIn",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockInItem_MstArticle_ItemId",
                        column: x => x.ItemId,
                        principalTable: "MstArticle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockInItem_MstUnit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "MstUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockInItem_BaseUnitId",
                table: "TrnStockInItem",
                column: "BaseUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockInItem_INId",
                table: "TrnStockInItem",
                column: "INId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockInItem_ItemId",
                table: "TrnStockInItem",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockInItem_UnitId",
                table: "TrnStockInItem",
                column: "UnitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrnStockInItem");
        }
    }
}
