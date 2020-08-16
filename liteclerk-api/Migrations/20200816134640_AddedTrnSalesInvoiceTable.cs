using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedTrnSalesInvoiceTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrnSalesInvoice",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    SINumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SIDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ManualNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DocumentReference = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    TermId = table.Column<int>(type: "int", nullable: false),
                    DateNeeded = table.Column<DateTime>(type: "datetime", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoldByUserId = table.Column<int>(type: "int", nullable: false),
                    PreparedByUserId = table.Column<int>(type: "int", nullable: false),
                    CheckedByUserId = table.Column<int>(type: "int", nullable: false),
                    ApprovedByUserId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    AdjustmentAmount = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    BalanceAmount = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsCancelled = table.Column<bool>(type: "bit", nullable: false),
                    IsPrinted = table.Column<bool>(type: "bit", nullable: false),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedByDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: false),
                    UpdatedByDateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrnSalesInvoice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnSalesInvoice_MstUser_ApprovedByUserId",
                        column: x => x.ApprovedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnSalesInvoice_MstCompanyBranch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "MstCompanyBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnSalesInvoice_MstUser_CheckedByUserId",
                        column: x => x.CheckedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnSalesInvoice_MstUser_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnSalesInvoice_MstCurrency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "MstCurrency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnSalesInvoice_MstArticle_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "MstArticle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnSalesInvoice_MstUser_PreparedByUserId",
                        column: x => x.PreparedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnSalesInvoice_MstUser_SoldByUserId",
                        column: x => x.SoldByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnSalesInvoice_MstTerm_TermId",
                        column: x => x.TermId,
                        principalTable: "MstTerm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnSalesInvoice_MstUser_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesInvoice_ApprovedByUserId",
                table: "TrnSalesInvoice",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesInvoice_BranchId",
                table: "TrnSalesInvoice",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesInvoice_CheckedByUserId",
                table: "TrnSalesInvoice",
                column: "CheckedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesInvoice_CreatedByUserId",
                table: "TrnSalesInvoice",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesInvoice_CurrencyId",
                table: "TrnSalesInvoice",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesInvoice_CustomerId",
                table: "TrnSalesInvoice",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesInvoice_PreparedByUserId",
                table: "TrnSalesInvoice",
                column: "PreparedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesInvoice_SoldByUserId",
                table: "TrnSalesInvoice",
                column: "SoldByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesInvoice_TermId",
                table: "TrnSalesInvoice",
                column: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesInvoice_UpdatedByUserId",
                table: "TrnSalesInvoice",
                column: "UpdatedByUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrnSalesInvoice");
        }
    }
}
