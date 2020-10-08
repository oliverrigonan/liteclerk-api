using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedStockOutTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "TrnStockOut",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "TrnStockOut",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "ArticleId",
                table: "TrnStockOut",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "TrnStockOut",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "TrnStockOutItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OTId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    ItemInventoryId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_TrnStockOutItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnStockOutItem_MstUnit_BaseUnitId",
                        column: x => x.BaseUnitId,
                        principalTable: "MstUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockOutItem_MstArticle_ItemId",
                        column: x => x.ItemId,
                        principalTable: "MstArticle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockOutItem_MstArticleItemInventory_ItemInventoryId",
                        column: x => x.ItemInventoryId,
                        principalTable: "MstArticleItemInventory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockOutItem_TrnStockOut_OTId",
                        column: x => x.OTId,
                        principalTable: "TrnStockOut",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockOutItem_MstUnit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "MstUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockOut_AccountId",
                table: "TrnStockOut",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockOut_ArticleId",
                table: "TrnStockOut",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockOutItem_BaseUnitId",
                table: "TrnStockOutItem",
                column: "BaseUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockOutItem_ItemId",
                table: "TrnStockOutItem",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockOutItem_ItemInventoryId",
                table: "TrnStockOutItem",
                column: "ItemInventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockOutItem_OTId",
                table: "TrnStockOutItem",
                column: "OTId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockOutItem_UnitId",
                table: "TrnStockOutItem",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrnStockOut_MstAccount_AccountId",
                table: "TrnStockOut",
                column: "AccountId",
                principalTable: "MstAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnStockOut_MstArticle_ArticleId",
                table: "TrnStockOut",
                column: "ArticleId",
                principalTable: "MstArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrnStockOut_MstAccount_AccountId",
                table: "TrnStockOut");

            migrationBuilder.DropForeignKey(
                name: "FK_TrnStockOut_MstArticle_ArticleId",
                table: "TrnStockOut");

            migrationBuilder.DropTable(
                name: "TrnStockOutItem");

            migrationBuilder.DropIndex(
                name: "IX_TrnStockOut_AccountId",
                table: "TrnStockOut");

            migrationBuilder.DropIndex(
                name: "IX_TrnStockOut_ArticleId",
                table: "TrnStockOut");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "TrnStockOut");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "TrnStockOut");

            migrationBuilder.DropColumn(
                name: "ArticleId",
                table: "TrnStockOut");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "TrnStockOut");
        }
    }
}
