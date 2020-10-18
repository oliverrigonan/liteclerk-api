using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AdditionalFieldsInReceivingReceiptTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "VATAmount",
                table: "TrnReceivingReceiptItem",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "VATId",
                table: "TrnReceivingReceiptItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "VATRate",
                table: "TrnReceivingReceiptItem",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "WTAXAmount",
                table: "TrnReceivingReceiptItem",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "WTAXId",
                table: "TrnReceivingReceiptItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "WTAXRate",
                table: "TrnReceivingReceiptItem",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_TrnReceivingReceiptItem_VATId",
                table: "TrnReceivingReceiptItem",
                column: "VATId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnReceivingReceiptItem_WTAXId",
                table: "TrnReceivingReceiptItem",
                column: "WTAXId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrnReceivingReceiptItem_MstTax_VATId",
                table: "TrnReceivingReceiptItem",
                column: "VATId",
                principalTable: "MstTax",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnReceivingReceiptItem_MstTax_WTAXId",
                table: "TrnReceivingReceiptItem",
                column: "WTAXId",
                principalTable: "MstTax",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrnReceivingReceiptItem_MstTax_VATId",
                table: "TrnReceivingReceiptItem");

            migrationBuilder.DropForeignKey(
                name: "FK_TrnReceivingReceiptItem_MstTax_WTAXId",
                table: "TrnReceivingReceiptItem");

            migrationBuilder.DropIndex(
                name: "IX_TrnReceivingReceiptItem_VATId",
                table: "TrnReceivingReceiptItem");

            migrationBuilder.DropIndex(
                name: "IX_TrnReceivingReceiptItem_WTAXId",
                table: "TrnReceivingReceiptItem");

            migrationBuilder.DropColumn(
                name: "VATAmount",
                table: "TrnReceivingReceiptItem");

            migrationBuilder.DropColumn(
                name: "VATId",
                table: "TrnReceivingReceiptItem");

            migrationBuilder.DropColumn(
                name: "VATRate",
                table: "TrnReceivingReceiptItem");

            migrationBuilder.DropColumn(
                name: "WTAXAmount",
                table: "TrnReceivingReceiptItem");

            migrationBuilder.DropColumn(
                name: "WTAXId",
                table: "TrnReceivingReceiptItem");

            migrationBuilder.DropColumn(
                name: "WTAXRate",
                table: "TrnReceivingReceiptItem");
        }
    }
}
