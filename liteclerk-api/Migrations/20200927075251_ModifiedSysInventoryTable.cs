using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class ModifiedSysInventoryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IVNumber",
                table: "SysInventory");

            migrationBuilder.RenameColumn(
                name: "IVDate",
                table: "SysInventory",
                newName: "InventoryDate");

            migrationBuilder.AddColumn<int>(
                name: "ILId",
                table: "SysInventory",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SysInventory_ILId",
                table: "SysInventory",
                column: "ILId");

            migrationBuilder.AddForeignKey(
                name: "FK_SysInventory_TrnInventory_ILId",
                table: "SysInventory",
                column: "ILId",
                principalTable: "TrnInventory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SysInventory_TrnInventory_ILId",
                table: "SysInventory");

            migrationBuilder.DropIndex(
                name: "IX_SysInventory_ILId",
                table: "SysInventory");

            migrationBuilder.DropColumn(
                name: "ILId",
                table: "SysInventory");

            migrationBuilder.RenameColumn(
                name: "InventoryDate",
                table: "SysInventory",
                newName: "IVDate");

            migrationBuilder.AddColumn<string>(
                name: "IVNumber",
                table: "SysInventory",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
