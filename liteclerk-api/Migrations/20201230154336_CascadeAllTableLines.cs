using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class CascadeAllTableLines : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MstJobTypeAttachment_MstJobType_JobTypeId",
                table: "MstJobTypeAttachment");

            migrationBuilder.DropForeignKey(
                name: "FK_MstJobTypeDepartment_MstJobType_JobTypeId",
                table: "MstJobTypeDepartment");

            migrationBuilder.DropForeignKey(
                name: "FK_MstJobTypeInformation_MstJobType_JobTypeId",
                table: "MstJobTypeInformation");

            migrationBuilder.DropForeignKey(
                name: "FK_MstUserBranch_MstUser_UserId",
                table: "MstUserBranch");

            migrationBuilder.DropForeignKey(
                name: "FK_MstUserForm_MstUser_UserId",
                table: "MstUserForm");

            migrationBuilder.DropForeignKey(
                name: "FK_MstUserJobDepartment_MstUser_UserId",
                table: "MstUserJobDepartment");

            migrationBuilder.DropForeignKey(
                name: "FK_TrnDisbursementLine_TrnDisbursement_CVId",
                table: "TrnDisbursementLine");

            migrationBuilder.DropForeignKey(
                name: "FK_TrnPurchaseOrderItem_TrnPurchaseOrder_POId",
                table: "TrnPurchaseOrderItem");

            migrationBuilder.DropForeignKey(
                name: "FK_TrnPurchaseRequestItem_TrnPurchaseRequest_PRId",
                table: "TrnPurchaseRequestItem");

            migrationBuilder.DropForeignKey(
                name: "FK_TrnReceivingReceiptItem_TrnReceivingReceipt_RRId",
                table: "TrnReceivingReceiptItem");

            migrationBuilder.DropForeignKey(
                name: "FK_TrnStockOutItem_TrnStockOut_OTId",
                table: "TrnStockOutItem");

            migrationBuilder.AddForeignKey(
                name: "FK_MstJobTypeAttachment_MstJobType_JobTypeId",
                table: "MstJobTypeAttachment",
                column: "JobTypeId",
                principalTable: "MstJobType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MstJobTypeDepartment_MstJobType_JobTypeId",
                table: "MstJobTypeDepartment",
                column: "JobTypeId",
                principalTable: "MstJobType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MstJobTypeInformation_MstJobType_JobTypeId",
                table: "MstJobTypeInformation",
                column: "JobTypeId",
                principalTable: "MstJobType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MstUserBranch_MstUser_UserId",
                table: "MstUserBranch",
                column: "UserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MstUserForm_MstUser_UserId",
                table: "MstUserForm",
                column: "UserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MstUserJobDepartment_MstUser_UserId",
                table: "MstUserJobDepartment",
                column: "UserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnDisbursementLine_TrnDisbursement_CVId",
                table: "TrnDisbursementLine",
                column: "CVId",
                principalTable: "TrnDisbursement",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnPurchaseOrderItem_TrnPurchaseOrder_POId",
                table: "TrnPurchaseOrderItem",
                column: "POId",
                principalTable: "TrnPurchaseOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnPurchaseRequestItem_TrnPurchaseRequest_PRId",
                table: "TrnPurchaseRequestItem",
                column: "PRId",
                principalTable: "TrnPurchaseRequest",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnReceivingReceiptItem_TrnReceivingReceipt_RRId",
                table: "TrnReceivingReceiptItem",
                column: "RRId",
                principalTable: "TrnReceivingReceipt",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnStockOutItem_TrnStockOut_OTId",
                table: "TrnStockOutItem",
                column: "OTId",
                principalTable: "TrnStockOut",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MstJobTypeAttachment_MstJobType_JobTypeId",
                table: "MstJobTypeAttachment");

            migrationBuilder.DropForeignKey(
                name: "FK_MstJobTypeDepartment_MstJobType_JobTypeId",
                table: "MstJobTypeDepartment");

            migrationBuilder.DropForeignKey(
                name: "FK_MstJobTypeInformation_MstJobType_JobTypeId",
                table: "MstJobTypeInformation");

            migrationBuilder.DropForeignKey(
                name: "FK_MstUserBranch_MstUser_UserId",
                table: "MstUserBranch");

            migrationBuilder.DropForeignKey(
                name: "FK_MstUserForm_MstUser_UserId",
                table: "MstUserForm");

            migrationBuilder.DropForeignKey(
                name: "FK_MstUserJobDepartment_MstUser_UserId",
                table: "MstUserJobDepartment");

            migrationBuilder.DropForeignKey(
                name: "FK_TrnDisbursementLine_TrnDisbursement_CVId",
                table: "TrnDisbursementLine");

            migrationBuilder.DropForeignKey(
                name: "FK_TrnPurchaseOrderItem_TrnPurchaseOrder_POId",
                table: "TrnPurchaseOrderItem");

            migrationBuilder.DropForeignKey(
                name: "FK_TrnPurchaseRequestItem_TrnPurchaseRequest_PRId",
                table: "TrnPurchaseRequestItem");

            migrationBuilder.DropForeignKey(
                name: "FK_TrnReceivingReceiptItem_TrnReceivingReceipt_RRId",
                table: "TrnReceivingReceiptItem");

            migrationBuilder.DropForeignKey(
                name: "FK_TrnStockOutItem_TrnStockOut_OTId",
                table: "TrnStockOutItem");

            migrationBuilder.AddForeignKey(
                name: "FK_MstJobTypeAttachment_MstJobType_JobTypeId",
                table: "MstJobTypeAttachment",
                column: "JobTypeId",
                principalTable: "MstJobType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstJobTypeDepartment_MstJobType_JobTypeId",
                table: "MstJobTypeDepartment",
                column: "JobTypeId",
                principalTable: "MstJobType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstJobTypeInformation_MstJobType_JobTypeId",
                table: "MstJobTypeInformation",
                column: "JobTypeId",
                principalTable: "MstJobType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstUserBranch_MstUser_UserId",
                table: "MstUserBranch",
                column: "UserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstUserForm_MstUser_UserId",
                table: "MstUserForm",
                column: "UserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstUserJobDepartment_MstUser_UserId",
                table: "MstUserJobDepartment",
                column: "UserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnDisbursementLine_TrnDisbursement_CVId",
                table: "TrnDisbursementLine",
                column: "CVId",
                principalTable: "TrnDisbursement",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnPurchaseOrderItem_TrnPurchaseOrder_POId",
                table: "TrnPurchaseOrderItem",
                column: "POId",
                principalTable: "TrnPurchaseOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnPurchaseRequestItem_TrnPurchaseRequest_PRId",
                table: "TrnPurchaseRequestItem",
                column: "PRId",
                principalTable: "TrnPurchaseRequest",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnReceivingReceiptItem_TrnReceivingReceipt_RRId",
                table: "TrnReceivingReceiptItem",
                column: "RRId",
                principalTable: "TrnReceivingReceipt",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnStockOutItem_TrnStockOut_OTId",
                table: "TrnStockOutItem",
                column: "OTId",
                principalTable: "TrnStockOut",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
