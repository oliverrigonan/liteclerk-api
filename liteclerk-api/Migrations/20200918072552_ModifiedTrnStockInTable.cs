using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class ModifiedTrnStockInTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApprovedByUserId",
                table: "TrnStockIn",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CheckedByUserId",
                table: "TrnStockIn",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PreparedByUserId",
                table: "TrnStockIn",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockIn_ApprovedByUserId",
                table: "TrnStockIn",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockIn_CheckedByUserId",
                table: "TrnStockIn",
                column: "CheckedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockIn_PreparedByUserId",
                table: "TrnStockIn",
                column: "PreparedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrnStockIn_MstUser_ApprovedByUserId",
                table: "TrnStockIn",
                column: "ApprovedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnStockIn_MstUser_CheckedByUserId",
                table: "TrnStockIn",
                column: "CheckedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrnStockIn_MstUser_PreparedByUserId",
                table: "TrnStockIn",
                column: "PreparedByUserId",
                principalTable: "MstUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrnStockIn_MstUser_ApprovedByUserId",
                table: "TrnStockIn");

            migrationBuilder.DropForeignKey(
                name: "FK_TrnStockIn_MstUser_CheckedByUserId",
                table: "TrnStockIn");

            migrationBuilder.DropForeignKey(
                name: "FK_TrnStockIn_MstUser_PreparedByUserId",
                table: "TrnStockIn");

            migrationBuilder.DropIndex(
                name: "IX_TrnStockIn_ApprovedByUserId",
                table: "TrnStockIn");

            migrationBuilder.DropIndex(
                name: "IX_TrnStockIn_CheckedByUserId",
                table: "TrnStockIn");

            migrationBuilder.DropIndex(
                name: "IX_TrnStockIn_PreparedByUserId",
                table: "TrnStockIn");

            migrationBuilder.DropColumn(
                name: "ApprovedByUserId",
                table: "TrnStockIn");

            migrationBuilder.DropColumn(
                name: "CheckedByUserId",
                table: "TrnStockIn");

            migrationBuilder.DropColumn(
                name: "PreparedByUserId",
                table: "TrnStockIn");
        }
    }
}
