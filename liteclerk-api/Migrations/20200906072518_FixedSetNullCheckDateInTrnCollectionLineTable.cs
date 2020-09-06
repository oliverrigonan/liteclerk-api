using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class FixedSetNullCheckDateInTrnCollectionLineTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CheckDate",
                table: "TrnCollectionLine",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CheckDate",
                table: "TrnCollectionLine",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

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
    }
}
