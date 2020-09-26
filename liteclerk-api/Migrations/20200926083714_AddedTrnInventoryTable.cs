using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedTrnInventoryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrnInventory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    ILNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ILDate = table.Column<DateTime>(type: "datetime", nullable: false),
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
                    table.PrimaryKey("PK_TrnInventory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnInventory_MstUser_ApprovedByUserId",
                        column: x => x.ApprovedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnInventory_MstCompanyBranch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "MstCompanyBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnInventory_MstUser_CheckedByUserId",
                        column: x => x.CheckedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnInventory_MstUser_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnInventory_MstCurrency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "MstCurrency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnInventory_MstUser_PreparedByUserId",
                        column: x => x.PreparedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnInventory_MstUser_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrnInventory_ApprovedByUserId",
                table: "TrnInventory",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnInventory_BranchId",
                table: "TrnInventory",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnInventory_CheckedByUserId",
                table: "TrnInventory",
                column: "CheckedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnInventory_CreatedByUserId",
                table: "TrnInventory",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnInventory_CurrencyId",
                table: "TrnInventory",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnInventory_PreparedByUserId",
                table: "TrnInventory",
                column: "PreparedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnInventory_UpdatedByUserId",
                table: "TrnInventory",
                column: "UpdatedByUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrnInventory");
        }
    }
}
