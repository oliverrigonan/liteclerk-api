using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedTrnStockCountTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrnStockCount",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    SCNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SCDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ManualNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DocumentReference = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreparedByUserId = table.Column<int>(type: "int", nullable: false),
                    CheckedByUserId = table.Column<int>(type: "int", nullable: false),
                    ApprovedByUserId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsCancelled = table.Column<bool>(type: "bit", nullable: false),
                    IsPrinted = table.Column<bool>(type: "bit", nullable: false),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrnStockCount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnStockCount_MstUser_ApprovedByUserId",
                        column: x => x.ApprovedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockCount_MstCompanyBranch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "MstCompanyBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockCount_MstUser_CheckedByUserId",
                        column: x => x.CheckedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockCount_MstUser_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockCount_MstCurrency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "MstCurrency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockCount_MstUser_PreparedByUserId",
                        column: x => x.PreparedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockCount_MstUser_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrnStockCountItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SCId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Particulars = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,5)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrnStockCountItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnStockCountItem_MstArticle_ItemId",
                        column: x => x.ItemId,
                        principalTable: "MstArticle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockCountItem_TrnStockCount_SCId",
                        column: x => x.SCId,
                        principalTable: "TrnStockCount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockCount_ApprovedByUserId",
                table: "TrnStockCount",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockCount_BranchId",
                table: "TrnStockCount",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockCount_CheckedByUserId",
                table: "TrnStockCount",
                column: "CheckedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockCount_CreatedByUserId",
                table: "TrnStockCount",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockCount_CurrencyId",
                table: "TrnStockCount",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockCount_PreparedByUserId",
                table: "TrnStockCount",
                column: "PreparedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockCount_UpdatedByUserId",
                table: "TrnStockCount",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockCountItem_ItemId",
                table: "TrnStockCountItem",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockCountItem_SCId",
                table: "TrnStockCountItem",
                column: "SCId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrnStockCountItem");

            migrationBuilder.DropTable(
                name: "TrnStockCount");
        }
    }
}
