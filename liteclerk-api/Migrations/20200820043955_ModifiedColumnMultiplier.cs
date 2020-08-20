using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class ModifiedColumnMultiplier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Mutliplier",
                table: "MstArticleItemUnit",
                newName: "Multiplier");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Multiplier",
                table: "MstArticleItemUnit",
                newName: "Mutliplier");
        }
    }
}
