using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class ChangedColumnNameBranchCodeToCurrencyCodeInMstCurrencyTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BranchCode",
                table: "MstCurrency",
                newName: "CurrencyCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CurrencyCode",
                table: "MstCurrency",
                newName: "BranchCode");
        }
    }
}
