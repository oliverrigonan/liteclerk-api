using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class SetNotNullCheckDateInTrnCollectionLineTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CheckDate",
                table: "TrnCollectionLine",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CheckDate",
                table: "TrnCollectionLine",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime");
        }
    }
}
