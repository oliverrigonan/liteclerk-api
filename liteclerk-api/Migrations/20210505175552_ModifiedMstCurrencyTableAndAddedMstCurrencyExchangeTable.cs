using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class ModifiedMstCurrencyTableAndAddedMstCurrencyExchangeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "MstCurrency",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "MstCurrency",
                type: "nvarchar(max)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "MstCurrencyExchange",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    ExchangeCurrencyId = table.Column<int>(type: "int", nullable: false),
                    ExchangeDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ExchangeRate = table.Column<decimal>(type: "decimal(18,5)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstCurrencyExchange", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MstCurrencyExchange_MstCurrency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "MstCurrency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstCurrencyExchange_MstCurrency_ExchangeCurrencyId",
                        column: x => x.ExchangeCurrencyId,
                        principalTable: "MstCurrency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MstCurrencyExchange_CurrencyId",
                table: "MstCurrencyExchange",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_MstCurrencyExchange_ExchangeCurrencyId",
                table: "MstCurrencyExchange",
                column: "ExchangeCurrencyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MstCurrencyExchange");

            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "MstCurrency");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "MstCurrency");
        }
    }
}
