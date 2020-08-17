using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class ModifiedCompanyIdAndBranchIdInMstUserTableSetToRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MstArticleItem_MstArticleAccountGroup_ArticleAccountGroupId",
                table: "MstArticleItem");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "MstUser",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BranchId",
                table: "MstUser",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleItem_MstArticleAccountGroup_ArticleAccountGroupId",
                table: "MstArticleItem",
                column: "ArticleAccountGroupId",
                principalTable: "MstArticleAccountGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MstArticleItem_MstArticleAccountGroup_ArticleAccountGroupId",
                table: "MstArticleItem");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "MstUser",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "BranchId",
                table: "MstUser",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_MstArticleItem_MstArticleAccountGroup_ArticleAccountGroupId",
                table: "MstArticleItem",
                column: "ArticleAccountGroupId",
                principalTable: "MstArticleAccountGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
