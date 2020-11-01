using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedStockWithdrawalTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "TrnStockWithdrawal",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "TrnStockWithdrawal",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ContactNumber",
                table: "TrnStockWithdrawal",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactPerson",
                table: "TrnStockWithdrawal",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "TrnStockWithdrawal",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FromBranchId",
                table: "TrnStockWithdrawal",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReceivedByUserId",
                table: "TrnStockWithdrawal",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "TrnStockWithdrawal",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SIId",
                table: "TrnStockWithdrawal",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TrnStockWithdrawalItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SWId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_TrnStockWithdrawalItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnStockWithdrawalItem_MstUnit_BaseUnitId",
                        column: x => x.BaseUnitId,
                        principalTable: "MstUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockWithdrawalItem_MstArticle_ItemId",
                        column: x => x.ItemId,
                        principalTable: "MstArticle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockWithdrawalItem_MstArticleItemInventory_ItemInventoryId",
                        column: x => x.ItemInventoryId,
                        principalTable: "MstArticleItemInventory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockWithdrawalItem_TrnStockWithdrawal_SWId",
                        column: x => x.SWId,
                        principalTable: "TrnStockWithdrawal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockWithdrawalItem_MstUnit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "MstUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockWithdrawal_CustomerId",
                table: "TrnStockWithdrawal",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockWithdrawal_FromBranchId",
                table: "TrnStockWithdrawal",
                column: "FromBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockWithdrawal_ReceivedByUserId",
                table: "TrnStockWithdrawal",
                column: "ReceivedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockWithdrawal_SIId",
                table: "TrnStockWithdrawal",
                column: "SIId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockWithdrawalItem_BaseUnitId",
                table: "TrnStockWithdrawalItem",
                column: "BaseUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockWithdrawalItem_ItemId",
                table: "TrnStockWithdrawalItem",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockWithdrawalItem_ItemInventoryId",
                table: "TrnStockWithdrawalItem",
                column: "ItemInventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockWithdrawalItem_SWId",
                table: "TrnStockWithdrawalItem",
                column: "SWId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockWithdrawalItem_UnitId",
                table: "TrnStockWithdrawalItem",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrnStockWithdrawal_MstArticle_CustomerId",
                table: "TrnStockWithdrawal",
                column: "CustomerId",
                principalTable: "MstArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnStockWithdrawal_MstCompanyBranch_FromBranchId",
                table: "TrnStockWithdrawal",
                column: "FromBranchId",
                principalTable: "MstCompanyBranch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnStockWithdrawal_MstUser_ReceivedByUserId",
                table: "TrnStockWithdrawal",
                column: "ReceivedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnStockWithdrawal_TrnSalesInvoice_SIId",
                table: "TrnStockWithdrawal",
                column: "SIId",
                principalTable: "TrnSalesInvoice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrnStockWithdrawal_MstArticle_CustomerId",
                table: "TrnStockWithdrawal");

            migrationBuilder.DropForeignKey(
                name: "FK_TrnStockWithdrawal_MstCompanyBranch_FromBranchId",
                table: "TrnStockWithdrawal");

            migrationBuilder.DropForeignKey(
                name: "FK_TrnStockWithdrawal_MstUser_ReceivedByUserId",
                table: "TrnStockWithdrawal");

            migrationBuilder.DropForeignKey(
                name: "FK_TrnStockWithdrawal_TrnSalesInvoice_SIId",
                table: "TrnStockWithdrawal");

            migrationBuilder.DropTable(
                name: "TrnStockWithdrawalItem");

            migrationBuilder.DropIndex(
                name: "IX_TrnStockWithdrawal_CustomerId",
                table: "TrnStockWithdrawal");

            migrationBuilder.DropIndex(
                name: "IX_TrnStockWithdrawal_FromBranchId",
                table: "TrnStockWithdrawal");

            migrationBuilder.DropIndex(
                name: "IX_TrnStockWithdrawal_ReceivedByUserId",
                table: "TrnStockWithdrawal");

            migrationBuilder.DropIndex(
                name: "IX_TrnStockWithdrawal_SIId",
                table: "TrnStockWithdrawal");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "TrnStockWithdrawal");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "TrnStockWithdrawal");

            migrationBuilder.DropColumn(
                name: "ContactNumber",
                table: "TrnStockWithdrawal");

            migrationBuilder.DropColumn(
                name: "ContactPerson",
                table: "TrnStockWithdrawal");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "TrnStockWithdrawal");

            migrationBuilder.DropColumn(
                name: "FromBranchId",
                table: "TrnStockWithdrawal");

            migrationBuilder.DropColumn(
                name: "ReceivedByUserId",
                table: "TrnStockWithdrawal");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "TrnStockWithdrawal");

            migrationBuilder.DropColumn(
                name: "SIId",
                table: "TrnStockWithdrawal");
        }
    }
}
