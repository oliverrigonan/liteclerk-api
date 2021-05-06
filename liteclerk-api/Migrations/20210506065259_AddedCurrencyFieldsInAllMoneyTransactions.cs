using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedCurrencyFieldsInAllMoneyTransactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BaseAmount",
                table: "TrnSalesOrderItem",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "BaseAmount",
                table: "TrnSalesOrder",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "ExchangeCurrencyId",
                table: "TrnSalesOrder",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<decimal>(
                name: "ExchangeRate",
                table: "TrnSalesOrder",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "BaseAmount",
                table: "TrnSalesInvoiceItem",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "BaseAdjustmentAmount",
                table: "TrnSalesInvoice",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "BaseAmount",
                table: "TrnSalesInvoice",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "BaseBalanceAmount",
                table: "TrnSalesInvoice",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "BasePaidAmount",
                table: "TrnSalesInvoice",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "ExchangeCurrencyId",
                table: "TrnSalesInvoice",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<decimal>(
                name: "ExchangeRate",
                table: "TrnSalesInvoice",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "BaseAmount",
                table: "TrnReceivingReceiptItem",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "BaseAdjustmentAmount",
                table: "TrnReceivingReceipt",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "BaseAmount",
                table: "TrnReceivingReceipt",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "BaseBalanceAmount",
                table: "TrnReceivingReceipt",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "BasePaidAmount",
                table: "TrnReceivingReceipt",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "ExchangeCurrencyId",
                table: "TrnReceivingReceipt",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<decimal>(
                name: "ExchangeRate",
                table: "TrnReceivingReceipt",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "BaseAmount",
                table: "TrnReceivableMemoLine",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "BaseAmount",
                table: "TrnReceivableMemo",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "ExchangeCurrencyId",
                table: "TrnReceivableMemo",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<decimal>(
                name: "ExchangeRate",
                table: "TrnReceivableMemo",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "BaseAmount",
                table: "TrnPurchaseRequestItem",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "BaseAmount",
                table: "TrnPurchaseRequest",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "ExchangeCurrencyId",
                table: "TrnPurchaseRequest",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<decimal>(
                name: "ExchangeRate",
                table: "TrnPurchaseRequest",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "BaseAmount",
                table: "TrnPurchaseOrderItem",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "BaseAmount",
                table: "TrnPurchaseOrder",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "ExchangeCurrencyId",
                table: "TrnPurchaseOrder",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<decimal>(
                name: "ExchangeRate",
                table: "TrnPurchaseOrder",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "BaseAmount",
                table: "TrnPayableMemoLine",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "BaseAmount",
                table: "TrnPayableMemo",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "ExchangeCurrencyId",
                table: "TrnPayableMemo",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<decimal>(
                name: "ExchangeRate",
                table: "TrnPayableMemo",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "BaseAmount",
                table: "TrnDisbursementLine",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "BaseAmount",
                table: "TrnDisbursement",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "ExchangeCurrencyId",
                table: "TrnDisbursement",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<decimal>(
                name: "ExchangeRate",
                table: "TrnDisbursement",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "BaseAmount",
                table: "TrnCollectionLine",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "BaseAmount",
                table: "TrnCollection",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "ExchangeCurrencyId",
                table: "TrnCollection",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<decimal>(
                name: "ExchangeRate",
                table: "TrnCollection",
                type: "decimal(18,5)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesOrder_ExchangeCurrencyId",
                table: "TrnSalesOrder",
                column: "ExchangeCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesInvoice_ExchangeCurrencyId",
                table: "TrnSalesInvoice",
                column: "ExchangeCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnReceivingReceipt_ExchangeCurrencyId",
                table: "TrnReceivingReceipt",
                column: "ExchangeCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnReceivableMemo_ExchangeCurrencyId",
                table: "TrnReceivableMemo",
                column: "ExchangeCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnPurchaseRequest_ExchangeCurrencyId",
                table: "TrnPurchaseRequest",
                column: "ExchangeCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnPurchaseOrder_ExchangeCurrencyId",
                table: "TrnPurchaseOrder",
                column: "ExchangeCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnPayableMemo_ExchangeCurrencyId",
                table: "TrnPayableMemo",
                column: "ExchangeCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnDisbursement_ExchangeCurrencyId",
                table: "TrnDisbursement",
                column: "ExchangeCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnCollection_ExchangeCurrencyId",
                table: "TrnCollection",
                column: "ExchangeCurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrnCollection_MstCurrency_ExchangeCurrencyId",
                table: "TrnCollection",
                column: "ExchangeCurrencyId",
                principalTable: "MstCurrency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnDisbursement_MstCurrency_ExchangeCurrencyId",
                table: "TrnDisbursement",
                column: "ExchangeCurrencyId",
                principalTable: "MstCurrency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnPayableMemo_MstCurrency_ExchangeCurrencyId",
                table: "TrnPayableMemo",
                column: "ExchangeCurrencyId",
                principalTable: "MstCurrency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnPurchaseOrder_MstCurrency_ExchangeCurrencyId",
                table: "TrnPurchaseOrder",
                column: "ExchangeCurrencyId",
                principalTable: "MstCurrency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnPurchaseRequest_MstCurrency_ExchangeCurrencyId",
                table: "TrnPurchaseRequest",
                column: "ExchangeCurrencyId",
                principalTable: "MstCurrency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnReceivableMemo_MstCurrency_ExchangeCurrencyId",
                table: "TrnReceivableMemo",
                column: "ExchangeCurrencyId",
                principalTable: "MstCurrency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnReceivingReceipt_MstCurrency_ExchangeCurrencyId",
                table: "TrnReceivingReceipt",
                column: "ExchangeCurrencyId",
                principalTable: "MstCurrency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesInvoice_MstCurrency_ExchangeCurrencyId",
                table: "TrnSalesInvoice",
                column: "ExchangeCurrencyId",
                principalTable: "MstCurrency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesOrder_MstCurrency_ExchangeCurrencyId",
                table: "TrnSalesOrder",
                column: "ExchangeCurrencyId",
                principalTable: "MstCurrency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrnCollection_MstCurrency_ExchangeCurrencyId",
                table: "TrnCollection");

            migrationBuilder.DropForeignKey(
                name: "FK_TrnDisbursement_MstCurrency_ExchangeCurrencyId",
                table: "TrnDisbursement");

            migrationBuilder.DropForeignKey(
                name: "FK_TrnPayableMemo_MstCurrency_ExchangeCurrencyId",
                table: "TrnPayableMemo");

            migrationBuilder.DropForeignKey(
                name: "FK_TrnPurchaseOrder_MstCurrency_ExchangeCurrencyId",
                table: "TrnPurchaseOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_TrnPurchaseRequest_MstCurrency_ExchangeCurrencyId",
                table: "TrnPurchaseRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_TrnReceivableMemo_MstCurrency_ExchangeCurrencyId",
                table: "TrnReceivableMemo");

            migrationBuilder.DropForeignKey(
                name: "FK_TrnReceivingReceipt_MstCurrency_ExchangeCurrencyId",
                table: "TrnReceivingReceipt");

            migrationBuilder.DropForeignKey(
                name: "FK_TrnSalesInvoice_MstCurrency_ExchangeCurrencyId",
                table: "TrnSalesInvoice");

            migrationBuilder.DropForeignKey(
                name: "FK_TrnSalesOrder_MstCurrency_ExchangeCurrencyId",
                table: "TrnSalesOrder");

            migrationBuilder.DropIndex(
                name: "IX_TrnSalesOrder_ExchangeCurrencyId",
                table: "TrnSalesOrder");

            migrationBuilder.DropIndex(
                name: "IX_TrnSalesInvoice_ExchangeCurrencyId",
                table: "TrnSalesInvoice");

            migrationBuilder.DropIndex(
                name: "IX_TrnReceivingReceipt_ExchangeCurrencyId",
                table: "TrnReceivingReceipt");

            migrationBuilder.DropIndex(
                name: "IX_TrnReceivableMemo_ExchangeCurrencyId",
                table: "TrnReceivableMemo");

            migrationBuilder.DropIndex(
                name: "IX_TrnPurchaseRequest_ExchangeCurrencyId",
                table: "TrnPurchaseRequest");

            migrationBuilder.DropIndex(
                name: "IX_TrnPurchaseOrder_ExchangeCurrencyId",
                table: "TrnPurchaseOrder");

            migrationBuilder.DropIndex(
                name: "IX_TrnPayableMemo_ExchangeCurrencyId",
                table: "TrnPayableMemo");

            migrationBuilder.DropIndex(
                name: "IX_TrnDisbursement_ExchangeCurrencyId",
                table: "TrnDisbursement");

            migrationBuilder.DropIndex(
                name: "IX_TrnCollection_ExchangeCurrencyId",
                table: "TrnCollection");

            migrationBuilder.DropColumn(
                name: "BaseAmount",
                table: "TrnSalesOrderItem");

            migrationBuilder.DropColumn(
                name: "BaseAmount",
                table: "TrnSalesOrder");

            migrationBuilder.DropColumn(
                name: "ExchangeCurrencyId",
                table: "TrnSalesOrder");

            migrationBuilder.DropColumn(
                name: "ExchangeRate",
                table: "TrnSalesOrder");

            migrationBuilder.DropColumn(
                name: "BaseAmount",
                table: "TrnSalesInvoiceItem");

            migrationBuilder.DropColumn(
                name: "BaseAdjustmentAmount",
                table: "TrnSalesInvoice");

            migrationBuilder.DropColumn(
                name: "BaseAmount",
                table: "TrnSalesInvoice");

            migrationBuilder.DropColumn(
                name: "BaseBalanceAmount",
                table: "TrnSalesInvoice");

            migrationBuilder.DropColumn(
                name: "BasePaidAmount",
                table: "TrnSalesInvoice");

            migrationBuilder.DropColumn(
                name: "ExchangeCurrencyId",
                table: "TrnSalesInvoice");

            migrationBuilder.DropColumn(
                name: "ExchangeRate",
                table: "TrnSalesInvoice");

            migrationBuilder.DropColumn(
                name: "BaseAmount",
                table: "TrnReceivingReceiptItem");

            migrationBuilder.DropColumn(
                name: "BaseAdjustmentAmount",
                table: "TrnReceivingReceipt");

            migrationBuilder.DropColumn(
                name: "BaseAmount",
                table: "TrnReceivingReceipt");

            migrationBuilder.DropColumn(
                name: "BaseBalanceAmount",
                table: "TrnReceivingReceipt");

            migrationBuilder.DropColumn(
                name: "BasePaidAmount",
                table: "TrnReceivingReceipt");

            migrationBuilder.DropColumn(
                name: "ExchangeCurrencyId",
                table: "TrnReceivingReceipt");

            migrationBuilder.DropColumn(
                name: "ExchangeRate",
                table: "TrnReceivingReceipt");

            migrationBuilder.DropColumn(
                name: "BaseAmount",
                table: "TrnReceivableMemoLine");

            migrationBuilder.DropColumn(
                name: "BaseAmount",
                table: "TrnReceivableMemo");

            migrationBuilder.DropColumn(
                name: "ExchangeCurrencyId",
                table: "TrnReceivableMemo");

            migrationBuilder.DropColumn(
                name: "ExchangeRate",
                table: "TrnReceivableMemo");

            migrationBuilder.DropColumn(
                name: "BaseAmount",
                table: "TrnPurchaseRequestItem");

            migrationBuilder.DropColumn(
                name: "BaseAmount",
                table: "TrnPurchaseRequest");

            migrationBuilder.DropColumn(
                name: "ExchangeCurrencyId",
                table: "TrnPurchaseRequest");

            migrationBuilder.DropColumn(
                name: "ExchangeRate",
                table: "TrnPurchaseRequest");

            migrationBuilder.DropColumn(
                name: "BaseAmount",
                table: "TrnPurchaseOrderItem");

            migrationBuilder.DropColumn(
                name: "BaseAmount",
                table: "TrnPurchaseOrder");

            migrationBuilder.DropColumn(
                name: "ExchangeCurrencyId",
                table: "TrnPurchaseOrder");

            migrationBuilder.DropColumn(
                name: "ExchangeRate",
                table: "TrnPurchaseOrder");

            migrationBuilder.DropColumn(
                name: "BaseAmount",
                table: "TrnPayableMemoLine");

            migrationBuilder.DropColumn(
                name: "BaseAmount",
                table: "TrnPayableMemo");

            migrationBuilder.DropColumn(
                name: "ExchangeCurrencyId",
                table: "TrnPayableMemo");

            migrationBuilder.DropColumn(
                name: "ExchangeRate",
                table: "TrnPayableMemo");

            migrationBuilder.DropColumn(
                name: "BaseAmount",
                table: "TrnDisbursementLine");

            migrationBuilder.DropColumn(
                name: "BaseAmount",
                table: "TrnDisbursement");

            migrationBuilder.DropColumn(
                name: "ExchangeCurrencyId",
                table: "TrnDisbursement");

            migrationBuilder.DropColumn(
                name: "ExchangeRate",
                table: "TrnDisbursement");

            migrationBuilder.DropColumn(
                name: "BaseAmount",
                table: "TrnCollectionLine");

            migrationBuilder.DropColumn(
                name: "BaseAmount",
                table: "TrnCollection");

            migrationBuilder.DropColumn(
                name: "ExchangeCurrencyId",
                table: "TrnCollection");

            migrationBuilder.DropColumn(
                name: "ExchangeRate",
                table: "TrnCollection");
        }
    }
}
