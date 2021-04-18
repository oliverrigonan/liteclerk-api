using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedSysFormTableAndSysFormTableColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SysFormTable",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormId = table.Column<int>(type: "int", nullable: false),
                    Table = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysFormTable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SysFormTable_SysForm_FormId",
                        column: x => x.FormId,
                        principalTable: "SysForm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SysFormTableColumn",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TableId = table.Column<int>(type: "int", nullable: false),
                    Column = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsDisplayed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysFormTableColumn", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SysFormTableColumn_SysFormTable_TableId",
                        column: x => x.TableId,
                        principalTable: "SysFormTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SysFormTable_FormId",
                table: "SysFormTable",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_SysFormTableColumn_TableId",
                table: "SysFormTableColumn",
                column: "TableId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SysFormTableColumn");

            migrationBuilder.DropTable(
                name: "SysFormTable");
        }
    }
}
