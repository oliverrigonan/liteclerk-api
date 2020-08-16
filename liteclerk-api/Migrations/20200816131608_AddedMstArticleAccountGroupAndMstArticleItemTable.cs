using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedMstArticleAccountGroupAndMstArticleItemTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MstArticleAccountGroup",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArticleAccountGroupCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ManualCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ArticleAccountGroup = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AssetAccountId = table.Column<int>(type: "int", nullable: false),
                    SalesAccountId = table.Column<int>(type: "int", nullable: false),
                    CostAccountId = table.Column<int>(type: "int", nullable: false),
                    ExpenseAccountId = table.Column<int>(type: "int", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedByDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: false),
                    UpdatedByDateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstArticleAccountGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MstArticleAccountGroup_MstAccount_AssetAccountId",
                        column: x => x.AssetAccountId,
                        principalTable: "MstAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstArticleAccountGroup_MstAccount_CostAccountId",
                        column: x => x.CostAccountId,
                        principalTable: "MstAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstArticleAccountGroup_MstUser_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstArticleAccountGroup_MstAccount_ExpenseAccountId",
                        column: x => x.ExpenseAccountId,
                        principalTable: "MstAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstArticleAccountGroup_MstAccount_SalesAccountId",
                        column: x => x.SalesAccountId,
                        principalTable: "MstAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstArticleAccountGroup_MstUser_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MstArticleItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    SKUCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BarCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    UnitId = table.Column<int>(type: "int", nullable: false),
                    IsJob = table.Column<bool>(type: "bit", nullable: false),
                    IsInventory = table.Column<bool>(type: "bit", nullable: false),
                    ArticleAccountGroupId = table.Column<int>(nullable: false),
                    AssetAccountId = table.Column<int>(type: "int", nullable: false),
                    SalesAccountId = table.Column<int>(type: "int", nullable: false),
                    CostAccountId = table.Column<int>(type: "int", nullable: false),
                    ExpenseAccountId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstArticleItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MstArticleItem_MstArticleAccountGroup_ArticleAccountGroupId",
                        column: x => x.ArticleAccountGroupId,
                        principalTable: "MstArticleAccountGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MstArticleItem_MstArticle_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "MstArticle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstArticleItem_MstAccount_AssetAccountId",
                        column: x => x.AssetAccountId,
                        principalTable: "MstAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstArticleItem_MstAccount_CostAccountId",
                        column: x => x.CostAccountId,
                        principalTable: "MstAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstArticleItem_MstAccount_ExpenseAccountId",
                        column: x => x.ExpenseAccountId,
                        principalTable: "MstAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstArticleItem_MstAccount_SalesAccountId",
                        column: x => x.SalesAccountId,
                        principalTable: "MstAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstArticleItem_MstUnit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "MstUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleAccountGroup_AssetAccountId",
                table: "MstArticleAccountGroup",
                column: "AssetAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleAccountGroup_CostAccountId",
                table: "MstArticleAccountGroup",
                column: "CostAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleAccountGroup_CreatedByUserId",
                table: "MstArticleAccountGroup",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleAccountGroup_ExpenseAccountId",
                table: "MstArticleAccountGroup",
                column: "ExpenseAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleAccountGroup_SalesAccountId",
                table: "MstArticleAccountGroup",
                column: "SalesAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleAccountGroup_UpdatedByUserId",
                table: "MstArticleAccountGroup",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleItem_ArticleAccountGroupId",
                table: "MstArticleItem",
                column: "ArticleAccountGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleItem_ArticleId",
                table: "MstArticleItem",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleItem_AssetAccountId",
                table: "MstArticleItem",
                column: "AssetAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleItem_CostAccountId",
                table: "MstArticleItem",
                column: "CostAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleItem_ExpenseAccountId",
                table: "MstArticleItem",
                column: "ExpenseAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleItem_SalesAccountId",
                table: "MstArticleItem",
                column: "SalesAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_MstArticleItem_UnitId",
                table: "MstArticleItem",
                column: "UnitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MstArticleItem");

            migrationBuilder.DropTable(
                name: "MstArticleAccountGroup");
        }
    }
}
