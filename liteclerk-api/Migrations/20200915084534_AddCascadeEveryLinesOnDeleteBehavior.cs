using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddCascadeEveryLinesOnDeleteBehavior : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MstArticleItemPrice_MstArticle_ArticleId",
                table: "MstArticleItemPrice");

            migrationBuilder.DropForeignKey(
                name: "FK_MstArticleItemUnit_MstArticle_ArticleId",
                table: "MstArticleItemUnit");

            migrationBuilder.DropForeignKey(
                name: "FK_TrnCollectionLine_TrnCollection_CIId",
                table: "TrnCollectionLine");

            migrationBuilder.DropForeignKey(
                name: "FK_TrnJobOrderAttachment_TrnJobOrder_JOId",
                table: "TrnJobOrderAttachment");

            migrationBuilder.DropForeignKey(
                name: "FK_TrnJobOrderDepartment_TrnJobOrder_JOId",
                table: "TrnJobOrderDepartment");

            migrationBuilder.DropForeignKey(
                name: "FK_TrnJobOrderInformation_TrnJobOrder_JOId",
                table: "TrnJobOrderInformation");

            migrationBuilder.DropForeignKey(
                name: "FK_TrnSalesInvoiceItem_TrnSalesInvoice_SIId",
                table: "TrnSalesInvoiceItem");

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleItemPrice_MstArticle_ArticleId",
                table: "MstArticleItemPrice",
                column: "ArticleId",
                principalTable: "MstArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleItemUnit_MstArticle_ArticleId",
                table: "MstArticleItemUnit",
                column: "ArticleId",
                principalTable: "MstArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnCollectionLine_TrnCollection_CIId",
                table: "TrnCollectionLine",
                column: "CIId",
                principalTable: "TrnCollection",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnJobOrderAttachment_TrnJobOrder_JOId",
                table: "TrnJobOrderAttachment",
                column: "JOId",
                principalTable: "TrnJobOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnJobOrderDepartment_TrnJobOrder_JOId",
                table: "TrnJobOrderDepartment",
                column: "JOId",
                principalTable: "TrnJobOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnJobOrderInformation_TrnJobOrder_JOId",
                table: "TrnJobOrderInformation",
                column: "JOId",
                principalTable: "TrnJobOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesInvoiceItem_TrnSalesInvoice_SIId",
                table: "TrnSalesInvoiceItem",
                column: "SIId",
                principalTable: "TrnSalesInvoice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MstArticleItemPrice_MstArticle_ArticleId",
                table: "MstArticleItemPrice");

            migrationBuilder.DropForeignKey(
                name: "FK_MstArticleItemUnit_MstArticle_ArticleId",
                table: "MstArticleItemUnit");

            migrationBuilder.DropForeignKey(
                name: "FK_TrnCollectionLine_TrnCollection_CIId",
                table: "TrnCollectionLine");

            migrationBuilder.DropForeignKey(
                name: "FK_TrnJobOrderAttachment_TrnJobOrder_JOId",
                table: "TrnJobOrderAttachment");

            migrationBuilder.DropForeignKey(
                name: "FK_TrnJobOrderDepartment_TrnJobOrder_JOId",
                table: "TrnJobOrderDepartment");

            migrationBuilder.DropForeignKey(
                name: "FK_TrnJobOrderInformation_TrnJobOrder_JOId",
                table: "TrnJobOrderInformation");

            migrationBuilder.DropForeignKey(
                name: "FK_TrnSalesInvoiceItem_TrnSalesInvoice_SIId",
                table: "TrnSalesInvoiceItem");

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleItemPrice_MstArticle_ArticleId",
                table: "MstArticleItemPrice",
                column: "ArticleId",
                principalTable: "MstArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleItemUnit_MstArticle_ArticleId",
                table: "MstArticleItemUnit",
                column: "ArticleId",
                principalTable: "MstArticle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnCollectionLine_TrnCollection_CIId",
                table: "TrnCollectionLine",
                column: "CIId",
                principalTable: "TrnCollection",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnJobOrderAttachment_TrnJobOrder_JOId",
                table: "TrnJobOrderAttachment",
                column: "JOId",
                principalTable: "TrnJobOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnJobOrderDepartment_TrnJobOrder_JOId",
                table: "TrnJobOrderDepartment",
                column: "JOId",
                principalTable: "TrnJobOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnJobOrderInformation_TrnJobOrder_JOId",
                table: "TrnJobOrderInformation",
                column: "JOId",
                principalTable: "TrnJobOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesInvoiceItem_TrnSalesInvoice_SIId",
                table: "TrnSalesInvoiceItem",
                column: "SIId",
                principalTable: "TrnSalesInvoice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
