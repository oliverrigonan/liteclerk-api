using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class SetNullSIIdInTrnCollectionLineTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SIId",
                table: "TrnCollectionLine",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SIId",
                table: "TrnCollectionLine",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
