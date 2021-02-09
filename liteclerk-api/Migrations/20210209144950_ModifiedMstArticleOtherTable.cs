using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class ModifiedMstArticleOtherTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Particulars",
                table: "MstArticleOther");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Particulars",
                table: "MstArticleOther",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
