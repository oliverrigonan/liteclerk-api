using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedTrnCollectionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrnCollection",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    CINumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CIDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ManualNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DocumentReference = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_TrnCollection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnCollection_MstUser_ApprovedByUserId",
                        column: x => x.ApprovedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnCollection_MstCompanyBranch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "MstCompanyBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnCollection_MstUser_CheckedByUserId",
                        column: x => x.CheckedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnCollection_MstUser_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnCollection_MstCurrency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "MstCurrency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnCollection_MstArticle_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "MstArticle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnCollection_MstUser_PreparedByUserId",
                        column: x => x.PreparedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnCollection_MstUser_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrnCollection_ApprovedByUserId",
                table: "TrnCollection",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnCollection_BranchId",
                table: "TrnCollection",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnCollection_CheckedByUserId",
                table: "TrnCollection",
                column: "CheckedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnCollection_CreatedByUserId",
                table: "TrnCollection",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnCollection_CurrencyId",
                table: "TrnCollection",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnCollection_CustomerId",
                table: "TrnCollection",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnCollection_PreparedByUserId",
                table: "TrnCollection",
                column: "PreparedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnCollection_UpdatedByUserId",
                table: "TrnCollection",
                column: "UpdatedByUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrnCollection");
        }
    }
}
