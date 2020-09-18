using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class ModifiedInventoryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SysInventory_TrnStockWithdrawal_STId",
                table: "SysInventory");

            migrationBuilder.CreateIndex(
                name: "IX_SysInventory_SWId",
                table: "SysInventory",
                column: "SWId");

            migrationBuilder.AddForeignKey(
                name: "FK_SysInventory_TrnStockWithdrawal_SWId",
                table: "SysInventory",
                column: "SWId",
                principalTable: "TrnStockWithdrawal",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SysInventory_TrnStockWithdrawal_SWId",
                table: "SysInventory");

            migrationBuilder.DropIndex(
                name: "IX_SysInventory_SWId",
                table: "SysInventory");

            migrationBuilder.AddForeignKey(
                name: "FK_SysInventory_TrnStockWithdrawal_STId",
                table: "SysInventory",
                column: "STId",
                principalTable: "TrnStockWithdrawal",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
