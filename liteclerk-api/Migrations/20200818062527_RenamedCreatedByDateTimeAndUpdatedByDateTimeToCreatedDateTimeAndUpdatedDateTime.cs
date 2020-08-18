using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class RenamedCreatedByDateTimeAndUpdatedByDateTimeToCreatedDateTimeAndUpdatedDateTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedByDateTime",
                table: "TrnSalesInvoice",
                newName: "UpdatedDateTime");

            migrationBuilder.RenameColumn(
                name: "CreatedByDateTime",
                table: "TrnSalesInvoice",
                newName: "CreatedDateTime");

            migrationBuilder.RenameColumn(
                name: "UpdatedByDateTime",
                table: "MstUnit",
                newName: "UpdatedDateTime");

            migrationBuilder.RenameColumn(
                name: "CreatedByDateTime",
                table: "MstUnit",
                newName: "CreatedDateTime");

            migrationBuilder.RenameColumn(
                name: "UpdatedByDateTime",
                table: "MstTerm",
                newName: "UpdatedDateTime");

            migrationBuilder.RenameColumn(
                name: "CreatedByDateTime",
                table: "MstTerm",
                newName: "CreatedDateTime");

            migrationBuilder.RenameColumn(
                name: "UpdatedByDateTime",
                table: "MstTax",
                newName: "UpdatedDateTime");

            migrationBuilder.RenameColumn(
                name: "CreatedByDateTime",
                table: "MstTax",
                newName: "CreatedDateTime");

            migrationBuilder.RenameColumn(
                name: "UpdatedByDateTime",
                table: "MstJobType",
                newName: "UpdatedDateTime");

            migrationBuilder.RenameColumn(
                name: "CreatedByDateTime",
                table: "MstJobType",
                newName: "CreatedDateTime");

            migrationBuilder.RenameColumn(
                name: "UpdatedByDateTime",
                table: "MstDiscount",
                newName: "UpdatedDateTime");

            migrationBuilder.RenameColumn(
                name: "CreatedByDateTime",
                table: "MstDiscount",
                newName: "CreatedDateTime");

            migrationBuilder.RenameColumn(
                name: "UpdatedByDateTime",
                table: "MstCurrency",
                newName: "UpdatedDateTime");

            migrationBuilder.RenameColumn(
                name: "CreatedByDateTime",
                table: "MstCurrency",
                newName: "CreatedDateTime");

            migrationBuilder.RenameColumn(
                name: "UpdatedByDateTime",
                table: "MstCompany",
                newName: "UpdatedDateTime");

            migrationBuilder.RenameColumn(
                name: "CreatedByDateTime",
                table: "MstCompany",
                newName: "CreatedDateTime");

            migrationBuilder.RenameColumn(
                name: "UpdatedByDateTime",
                table: "MstArticleAccountGroup",
                newName: "UpdatedDateTime");

            migrationBuilder.RenameColumn(
                name: "CreatedByDateTime",
                table: "MstArticleAccountGroup",
                newName: "CreatedDateTime");

            migrationBuilder.RenameColumn(
                name: "UpdatedByDateTime",
                table: "MstArticle",
                newName: "UpdatedDateTime");

            migrationBuilder.RenameColumn(
                name: "CreatedByDateTime",
                table: "MstArticle",
                newName: "CreatedDateTime");

            migrationBuilder.RenameColumn(
                name: "UpdatedByDateTime",
                table: "MstAccountType",
                newName: "UpdatedDateTime");

            migrationBuilder.RenameColumn(
                name: "CreatedByDateTime",
                table: "MstAccountType",
                newName: "CreatedDateTime");

            migrationBuilder.RenameColumn(
                name: "UpdatedByDateTime",
                table: "MstAccountCategory",
                newName: "UpdatedDateTime");

            migrationBuilder.RenameColumn(
                name: "CreatedByDateTime",
                table: "MstAccountCategory",
                newName: "CreatedDateTime");

            migrationBuilder.RenameColumn(
                name: "UpdatedByDateTime",
                table: "MstAccountCashFlow",
                newName: "UpdatedDateTime");

            migrationBuilder.RenameColumn(
                name: "CreatedByDateTime",
                table: "MstAccountCashFlow",
                newName: "CreatedDateTime");

            migrationBuilder.RenameColumn(
                name: "UpdatedByDateTime",
                table: "MstAccount",
                newName: "UpdatedDateTime");

            migrationBuilder.RenameColumn(
                name: "CreatedByDateTime",
                table: "MstAccount",
                newName: "CreatedDateTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedDateTime",
                table: "TrnSalesInvoice",
                newName: "UpdatedByDateTime");

            migrationBuilder.RenameColumn(
                name: "CreatedDateTime",
                table: "TrnSalesInvoice",
                newName: "CreatedByDateTime");

            migrationBuilder.RenameColumn(
                name: "UpdatedDateTime",
                table: "MstUnit",
                newName: "UpdatedByDateTime");

            migrationBuilder.RenameColumn(
                name: "CreatedDateTime",
                table: "MstUnit",
                newName: "CreatedByDateTime");

            migrationBuilder.RenameColumn(
                name: "UpdatedDateTime",
                table: "MstTerm",
                newName: "UpdatedByDateTime");

            migrationBuilder.RenameColumn(
                name: "CreatedDateTime",
                table: "MstTerm",
                newName: "CreatedByDateTime");

            migrationBuilder.RenameColumn(
                name: "UpdatedDateTime",
                table: "MstTax",
                newName: "UpdatedByDateTime");

            migrationBuilder.RenameColumn(
                name: "CreatedDateTime",
                table: "MstTax",
                newName: "CreatedByDateTime");

            migrationBuilder.RenameColumn(
                name: "UpdatedDateTime",
                table: "MstJobType",
                newName: "UpdatedByDateTime");

            migrationBuilder.RenameColumn(
                name: "CreatedDateTime",
                table: "MstJobType",
                newName: "CreatedByDateTime");

            migrationBuilder.RenameColumn(
                name: "UpdatedDateTime",
                table: "MstDiscount",
                newName: "UpdatedByDateTime");

            migrationBuilder.RenameColumn(
                name: "CreatedDateTime",
                table: "MstDiscount",
                newName: "CreatedByDateTime");

            migrationBuilder.RenameColumn(
                name: "UpdatedDateTime",
                table: "MstCurrency",
                newName: "UpdatedByDateTime");

            migrationBuilder.RenameColumn(
                name: "CreatedDateTime",
                table: "MstCurrency",
                newName: "CreatedByDateTime");

            migrationBuilder.RenameColumn(
                name: "UpdatedDateTime",
                table: "MstCompany",
                newName: "UpdatedByDateTime");

            migrationBuilder.RenameColumn(
                name: "CreatedDateTime",
                table: "MstCompany",
                newName: "CreatedByDateTime");

            migrationBuilder.RenameColumn(
                name: "UpdatedDateTime",
                table: "MstArticleAccountGroup",
                newName: "UpdatedByDateTime");

            migrationBuilder.RenameColumn(
                name: "CreatedDateTime",
                table: "MstArticleAccountGroup",
                newName: "CreatedByDateTime");

            migrationBuilder.RenameColumn(
                name: "UpdatedDateTime",
                table: "MstArticle",
                newName: "UpdatedByDateTime");

            migrationBuilder.RenameColumn(
                name: "CreatedDateTime",
                table: "MstArticle",
                newName: "CreatedByDateTime");

            migrationBuilder.RenameColumn(
                name: "UpdatedDateTime",
                table: "MstAccountType",
                newName: "UpdatedByDateTime");

            migrationBuilder.RenameColumn(
                name: "CreatedDateTime",
                table: "MstAccountType",
                newName: "CreatedByDateTime");

            migrationBuilder.RenameColumn(
                name: "UpdatedDateTime",
                table: "MstAccountCategory",
                newName: "UpdatedByDateTime");

            migrationBuilder.RenameColumn(
                name: "CreatedDateTime",
                table: "MstAccountCategory",
                newName: "CreatedByDateTime");

            migrationBuilder.RenameColumn(
                name: "UpdatedDateTime",
                table: "MstAccountCashFlow",
                newName: "UpdatedByDateTime");

            migrationBuilder.RenameColumn(
                name: "CreatedDateTime",
                table: "MstAccountCashFlow",
                newName: "CreatedByDateTime");

            migrationBuilder.RenameColumn(
                name: "UpdatedDateTime",
                table: "MstAccount",
                newName: "UpdatedByDateTime");

            migrationBuilder.RenameColumn(
                name: "CreatedDateTime",
                table: "MstAccount",
                newName: "CreatedByDateTime");
        }
    }
}
