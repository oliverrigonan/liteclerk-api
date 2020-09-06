using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedBaseQuantityAndBaseUnitInTrnJobOrderTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BaseQuantity",
                table: "TrnJobOrder",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "BaseUnitId",
                table: "TrnJobOrder",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TrnJobOrder_BaseUnitId",
                table: "TrnJobOrder",
                column: "BaseUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrnJobOrder_MstUnit_BaseUnitIdId",
                table: "TrnJobOrder",
                column: "BaseUnitId",
                principalTable: "MstUnit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrnJobOrder_MstUnit_BaseUnitIdId",
                table: "TrnJobOrder");

            migrationBuilder.DropIndex(
                name: "IX_TrnJobOrder_BaseUnitId",
                table: "TrnJobOrder");

            migrationBuilder.DropColumn(
                name: "BaseQuantity",
                table: "TrnJobOrder");

            migrationBuilder.DropColumn(
                name: "BaseUnitId",
                table: "TrnJobOrder");
        }
    }
}
