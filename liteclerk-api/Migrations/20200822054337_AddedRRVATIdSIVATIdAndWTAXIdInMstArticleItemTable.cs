using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedRRVATIdSIVATIdAndWTAXIdInMstArticleItemTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RRVATId",
                table: "MstArticleItem",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "SIVATId",
                table: "MstArticleItem",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "WTAXId",
                table: "MstArticleItem",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleItem_RRVATId",
                table: "MstArticleItem",
                column: "RRVATId");

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleItem_SIVATId",
                table: "MstArticleItem",
                column: "SIVATId");

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleItem_WTAXId",
                table: "MstArticleItem",
                column: "WTAXId");

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleItem_MstTax_RRVATId",
                table: "MstArticleItem",
                column: "RRVATId",
                principalTable: "MstTax",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleItem_MstTax_SIVATId",
                table: "MstArticleItem",
                column: "SIVATId",
                principalTable: "MstTax",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleItem_MstTax_WTAXId",
                table: "MstArticleItem",
                column: "WTAXId",
                principalTable: "MstTax",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MstArticleItem_MstTax_RRVATId",
                table: "MstArticleItem");

            migrationBuilder.DropForeignKey(
                name: "FK_MstArticleItem_MstTax_SIVATId",
                table: "MstArticleItem");

            migrationBuilder.DropForeignKey(
                name: "FK_MstArticleItem_MstTax_WTAXId",
                table: "MstArticleItem");

            migrationBuilder.DropIndex(
                name: "IX_MstArticleItem_RRVATId",
                table: "MstArticleItem");

            migrationBuilder.DropIndex(
                name: "IX_MstArticleItem_SIVATId",
                table: "MstArticleItem");

            migrationBuilder.DropIndex(
                name: "IX_MstArticleItem_WTAXId",
                table: "MstArticleItem");

            migrationBuilder.DropColumn(
                name: "RRVATId",
                table: "MstArticleItem");

            migrationBuilder.DropColumn(
                name: "SIVATId",
                table: "MstArticleItem");

            migrationBuilder.DropColumn(
                name: "WTAXId",
                table: "MstArticleItem");
        }
    }
}
