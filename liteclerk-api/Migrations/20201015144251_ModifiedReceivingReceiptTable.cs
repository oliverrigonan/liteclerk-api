using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class ModifiedReceivingReceiptTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrnReceivingReceipt_MstUser_MstUser_ReceivedByUserIdId",
                table: "TrnReceivingReceipt");

            migrationBuilder.DropIndex(
                name: "IX_TrnReceivingReceipt_MstUser_ReceivedByUserIdId",
                table: "TrnReceivingReceipt");

            migrationBuilder.DropColumn(
                name: "MstUser_ReceivedByUserIdId",
                table: "TrnReceivingReceipt");

            migrationBuilder.CreateIndex(
                name: "IX_TrnReceivingReceipt_ReceivedByUserId",
                table: "TrnReceivingReceipt",
                column: "ReceivedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrnReceivingReceipt_MstUser_ReceivedByUserId",
                table: "TrnReceivingReceipt",
                column: "ReceivedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrnReceivingReceipt_MstUser_ReceivedByUserId",
                table: "TrnReceivingReceipt");

            migrationBuilder.DropIndex(
                name: "IX_TrnReceivingReceipt_ReceivedByUserId",
                table: "TrnReceivingReceipt");

            migrationBuilder.AddColumn<int>(
                name: "MstUser_ReceivedByUserIdId",
                table: "TrnReceivingReceipt",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrnReceivingReceipt_MstUser_ReceivedByUserIdId",
                table: "TrnReceivingReceipt",
                column: "MstUser_ReceivedByUserIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrnReceivingReceipt_MstUser_MstUser_ReceivedByUserIdId",
                table: "TrnReceivingReceipt",
                column: "MstUser_ReceivedByUserIdId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
