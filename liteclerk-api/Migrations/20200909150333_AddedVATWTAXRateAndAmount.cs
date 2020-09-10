using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedVATWTAXRateAndAmount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "VATAmount",
                table: "TrnSalesInvoiceItem",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "VATRate",
                table: "TrnSalesInvoiceItem",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "WTAXAmount",
                table: "TrnSalesInvoiceItem",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "WTAXRate",
                table: "TrnSalesInvoiceItem",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "WTAXAmount",
                table: "TrnCollectionLine",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "WTAXRate",
                table: "TrnCollectionLine",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VATAmount",
                table: "TrnSalesInvoiceItem");

            migrationBuilder.DropColumn(
                name: "VATRate",
                table: "TrnSalesInvoiceItem");

            migrationBuilder.DropColumn(
                name: "WTAXAmount",
                table: "TrnSalesInvoiceItem");

            migrationBuilder.DropColumn(
                name: "WTAXRate",
                table: "TrnSalesInvoiceItem");

            migrationBuilder.DropColumn(
                name: "WTAXAmount",
                table: "TrnCollectionLine");

            migrationBuilder.DropColumn(
                name: "WTAXRate",
                table: "TrnCollectionLine");
        }
    }
}
