using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedUnitIdInTrnJobOrderTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "TrnJobOrder",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TrnJobOrder_UnitId",
                table: "TrnJobOrder",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrnJobOrder_MstUnit_UnitId",
                table: "TrnJobOrder",
                column: "UnitId",
                principalTable: "MstUnit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrnJobOrder_MstUnit_UnitId",
                table: "TrnJobOrder");

            migrationBuilder.DropIndex(
                name: "IX_TrnJobOrder_UnitId",
                table: "TrnJobOrder");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "TrnJobOrder");
        }
    }
}
