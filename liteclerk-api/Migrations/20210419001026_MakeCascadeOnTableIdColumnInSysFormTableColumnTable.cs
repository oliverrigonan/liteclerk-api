using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class MakeCascadeOnTableIdColumnInSysFormTableColumnTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SysFormTableColumn_SysFormTable_TableId",
                table: "SysFormTableColumn");

            migrationBuilder.AddForeignKey(
                name: "FK_SysFormTableColumn_SysFormTable_TableId",
                table: "SysFormTableColumn",
                column: "TableId",
                principalTable: "SysFormTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SysFormTableColumn_SysFormTable_TableId",
                table: "SysFormTableColumn");

            migrationBuilder.AddForeignKey(
                name: "FK_SysFormTableColumn_SysFormTable_TableId",
                table: "SysFormTableColumn",
                column: "TableId",
                principalTable: "SysFormTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
