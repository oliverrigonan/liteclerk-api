using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedKittingOnMstArticleItemTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsJob",
                table: "MstArticleItem");

            migrationBuilder.AddColumn<string>(
                name: "Kitting",
                table: "MstArticleItem",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Kitting",
                table: "MstArticleItem");

            migrationBuilder.AddColumn<bool>(
                name: "IsJob",
                table: "MstArticleItem",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
