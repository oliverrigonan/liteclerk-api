using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class RemoveJobTypeInSalesOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrnSalesOrderItem_MstJobType_ItemJobTypeId",
                table: "TrnSalesOrderItem");

            migrationBuilder.DropIndex(
                name: "IX_TrnSalesOrderItem_ItemJobTypeId",
                table: "TrnSalesOrderItem");

            migrationBuilder.DropColumn(
                name: "ItemJobTypeId",
                table: "TrnSalesOrderItem");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ItemJobTypeId",
                table: "TrnSalesOrderItem",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesOrderItem_ItemJobTypeId",
                table: "TrnSalesOrderItem",
                column: "ItemJobTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrnSalesOrderItem_MstJobType_ItemJobTypeId",
                table: "TrnSalesOrderItem",
                column: "ItemJobTypeId",
                principalTable: "MstJobType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
