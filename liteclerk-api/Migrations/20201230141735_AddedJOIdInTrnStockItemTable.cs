using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedJOIdInTrnStockItemTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JOId",
                table: "TrnStockInItem",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockInItem_JOId",
                table: "TrnStockInItem",
                column: "JOId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrnStockInItem_TrnJobOrder_JOId",
                table: "TrnStockInItem",
                column: "JOId",
                principalTable: "TrnJobOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrnStockInItem_TrnJobOrder_JOId",
                table: "TrnStockInItem");

            migrationBuilder.DropIndex(
                name: "IX_TrnStockInItem_JOId",
                table: "TrnStockInItem");

            migrationBuilder.DropColumn(
                name: "JOId",
                table: "TrnStockInItem");
        }
    }
}
