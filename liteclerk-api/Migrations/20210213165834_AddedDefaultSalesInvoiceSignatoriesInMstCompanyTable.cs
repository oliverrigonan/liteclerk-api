using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedDefaultSalesInvoiceSignatoriesInMstCompanyTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SalesInvoiceApprovedByUserId",
                table: "MstCompany",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SalesInvoiceCheckedByUserId",
                table: "MstCompany",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MstCompany_SalesInvoiceApprovedByUserId",
                table: "MstCompany",
                column: "SalesInvoiceApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MstCompany_SalesInvoiceCheckedByUserId",
                table: "MstCompany",
                column: "SalesInvoiceCheckedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MstCompany_MstUser_SalesInvoiceApprovedByUserId",
                table: "MstCompany",
                column: "SalesInvoiceApprovedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstCompany_MstUser_SalesInvoiceCheckedByUserId",
                table: "MstCompany",
                column: "SalesInvoiceCheckedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MstCompany_MstUser_SalesInvoiceApprovedByUserId",
                table: "MstCompany");

            migrationBuilder.DropForeignKey(
                name: "FK_MstCompany_MstUser_SalesInvoiceCheckedByUserId",
                table: "MstCompany");

            migrationBuilder.DropIndex(
                name: "IX_MstCompany_SalesInvoiceApprovedByUserId",
                table: "MstCompany");

            migrationBuilder.DropIndex(
                name: "IX_MstCompany_SalesInvoiceCheckedByUserId",
                table: "MstCompany");

            migrationBuilder.DropColumn(
                name: "SalesInvoiceApprovedByUserId",
                table: "MstCompany");

            migrationBuilder.DropColumn(
                name: "SalesInvoiceCheckedByUserId",
                table: "MstCompany");
        }
    }
}
