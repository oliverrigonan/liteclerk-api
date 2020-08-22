using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedTrnJobOrderTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrnJobOrder",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    JONumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    JODate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ManualNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DocumentReference = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DateScheduled = table.Column<DateTime>(type: "datetime", nullable: false),
                    DateNeeded = table.Column<DateTime>(type: "datetime", nullable: false),
                    SIItemId = table.Column<int>(type: "int", nullable: true),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    ItemJobTypeId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
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
                    table.PrimaryKey("PK_TrnJobOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnJobOrder_MstUser_ApprovedByUserId",
                        column: x => x.ApprovedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnJobOrder_MstCompanyBranch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "MstCompanyBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnJobOrder_MstUser_CheckedByUserId",
                        column: x => x.CheckedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnJobOrder_MstUser_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnJobOrder_MstCurrency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "MstCurrency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnJobOrder_MstArticle_ItemId",
                        column: x => x.ItemId,
                        principalTable: "MstArticle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnJobOrder_MstJobType_ItemJobTypeId",
                        column: x => x.ItemJobTypeId,
                        principalTable: "MstJobType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnJobOrder_MstUser_PreparedByUserId",
                        column: x => x.PreparedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnJobOrder_TrnSalesInvoiceItem_SIItemId",
                        column: x => x.SIItemId,
                        principalTable: "TrnSalesInvoiceItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnJobOrder_MstUser_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrnJobOrder_ApprovedByUserId",
                table: "TrnJobOrder",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnJobOrder_BranchId",
                table: "TrnJobOrder",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnJobOrder_CheckedByUserId",
                table: "TrnJobOrder",
                column: "CheckedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnJobOrder_CreatedByUserId",
                table: "TrnJobOrder",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnJobOrder_CurrencyId",
                table: "TrnJobOrder",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnJobOrder_ItemId",
                table: "TrnJobOrder",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnJobOrder_ItemJobTypeId",
                table: "TrnJobOrder",
                column: "ItemJobTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnJobOrder_PreparedByUserId",
                table: "TrnJobOrder",
                column: "PreparedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnJobOrder_SIItemId",
                table: "TrnJobOrder",
                column: "SIItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnJobOrder_UpdatedByUserId",
                table: "TrnJobOrder",
                column: "UpdatedByUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrnJobOrder");
        }
    }
}
