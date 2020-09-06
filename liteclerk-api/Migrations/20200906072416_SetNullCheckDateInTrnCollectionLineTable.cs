using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class SetNullCheckDateInTrnCollectionLineTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CheckBank",
                table: "TrnCollectionLine",
                type: "nvarchar(255)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 50);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CheckBank",
                table: "TrnCollectionLine",
                type: "nvarchar(255)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 50,
                oldNullable: true);
        }
    }
}
