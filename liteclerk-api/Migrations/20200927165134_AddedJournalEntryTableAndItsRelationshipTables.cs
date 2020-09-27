using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedJournalEntryTableAndItsRelationshipTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrnDisbursement",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    CVNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CVDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ManualNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DocumentReference = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
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
                    table.PrimaryKey("PK_TrnDisbursement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnDisbursement_MstUser_ApprovedByUserId",
                        column: x => x.ApprovedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnDisbursement_MstCompanyBranch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "MstCompanyBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnDisbursement_MstUser_CheckedByUserId",
                        column: x => x.CheckedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnDisbursement_MstUser_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnDisbursement_MstCurrency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "MstCurrency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnDisbursement_MstUser_PreparedByUserId",
                        column: x => x.PreparedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnDisbursement_MstUser_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrnJournalVoucher",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    CRNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CRDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ManualNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DocumentReference = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
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
                    table.PrimaryKey("PK_TrnJournalVoucher", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnJournalVoucher_MstUser_ApprovedByUserId",
                        column: x => x.ApprovedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnJournalVoucher_MstCompanyBranch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "MstCompanyBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnJournalVoucher_MstUser_CheckedByUserId",
                        column: x => x.CheckedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnJournalVoucher_MstUser_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnJournalVoucher_MstCurrency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "MstCurrency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnJournalVoucher_MstUser_PreparedByUserId",
                        column: x => x.PreparedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnJournalVoucher_MstUser_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrnPayableMemo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    PMNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PMDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ManualNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DocumentReference = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
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
                    table.PrimaryKey("PK_TrnPayableMemo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnPayableMemo_MstUser_ApprovedByUserId",
                        column: x => x.ApprovedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnPayableMemo_MstCompanyBranch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "MstCompanyBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnPayableMemo_MstUser_CheckedByUserId",
                        column: x => x.CheckedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnPayableMemo_MstUser_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnPayableMemo_MstCurrency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "MstCurrency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnPayableMemo_MstUser_PreparedByUserId",
                        column: x => x.PreparedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnPayableMemo_MstUser_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrnReceivableMemo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    RMNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RMDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ManualNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DocumentReference = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
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
                    table.PrimaryKey("PK_TrnReceivableMemo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnReceivableMemo_MstUser_ApprovedByUserId",
                        column: x => x.ApprovedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnReceivableMemo_MstCompanyBranch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "MstCompanyBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnReceivableMemo_MstUser_CheckedByUserId",
                        column: x => x.CheckedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnReceivableMemo_MstUser_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnReceivableMemo_MstCurrency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "MstCurrency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnReceivableMemo_MstUser_PreparedByUserId",
                        column: x => x.PreparedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnReceivableMemo_MstUser_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrnDisbursementLineDBSet",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CVId = table.Column<int>(nullable: false),
                    TrnDisbursement_CVIdId = table.Column<int>(nullable: true),
                    Particulars = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrnDisbursementLineDBSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnDisbursementLineDBSet_TrnDisbursement_TrnDisbursement_CVIdId",
                        column: x => x.TrnDisbursement_CVIdId,
                        principalTable: "TrnDisbursement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrnJournalVoucherLineDBSet",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JVId = table.Column<int>(nullable: false),
                    TrnJournalVoucher_JVIdId = table.Column<int>(nullable: true),
                    Particulars = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrnJournalVoucherLineDBSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnJournalVoucherLineDBSet_TrnJournalVoucher_TrnJournalVoucher_JVIdId",
                        column: x => x.TrnJournalVoucher_JVIdId,
                        principalTable: "TrnJournalVoucher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SysJournalEntry",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    JournalEntryDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    DebitAmount = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    CreditAmount = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    Particulars = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RRId = table.Column<int>(type: "int", nullable: true),
                    SIId = table.Column<int>(type: "int", nullable: true),
                    CIId = table.Column<int>(type: "int", nullable: true),
                    CVId = table.Column<int>(type: "int", nullable: true),
                    PMId = table.Column<int>(type: "int", nullable: true),
                    RMId = table.Column<int>(type: "int", nullable: true),
                    JVId = table.Column<int>(type: "int", nullable: true),
                    ILId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysJournalEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SysJournalEntry_MstAccount_AccountId",
                        column: x => x.AccountId,
                        principalTable: "MstAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SysJournalEntry_MstArticle_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "MstArticle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SysJournalEntry_MstCompanyBranch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "MstCompanyBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SysJournalEntry_TrnCollection_CIId",
                        column: x => x.CIId,
                        principalTable: "TrnCollection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SysJournalEntry_TrnDisbursement_CVId",
                        column: x => x.CVId,
                        principalTable: "TrnDisbursement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SysJournalEntry_TrnInventory_ILId",
                        column: x => x.ILId,
                        principalTable: "TrnInventory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SysJournalEntry_TrnJournalVoucher_JVId",
                        column: x => x.JVId,
                        principalTable: "TrnJournalVoucher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SysJournalEntry_TrnPayableMemo_PMId",
                        column: x => x.PMId,
                        principalTable: "TrnPayableMemo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SysJournalEntry_TrnReceivableMemo_RMId",
                        column: x => x.RMId,
                        principalTable: "TrnReceivableMemo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SysJournalEntry_TrnReceivingReceipt_RRId",
                        column: x => x.RRId,
                        principalTable: "TrnReceivingReceipt",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SysJournalEntry_TrnSalesInvoice_SIId",
                        column: x => x.SIId,
                        principalTable: "TrnSalesInvoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SysJournalEntry_AccountId",
                table: "SysJournalEntry",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_SysJournalEntry_ArticleId",
                table: "SysJournalEntry",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_SysJournalEntry_BranchId",
                table: "SysJournalEntry",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_SysJournalEntry_CIId",
                table: "SysJournalEntry",
                column: "CIId");

            migrationBuilder.CreateIndex(
                name: "IX_SysJournalEntry_CVId",
                table: "SysJournalEntry",
                column: "CVId");

            migrationBuilder.CreateIndex(
                name: "IX_SysJournalEntry_ILId",
                table: "SysJournalEntry",
                column: "ILId");

            migrationBuilder.CreateIndex(
                name: "IX_SysJournalEntry_JVId",
                table: "SysJournalEntry",
                column: "JVId");

            migrationBuilder.CreateIndex(
                name: "IX_SysJournalEntry_PMId",
                table: "SysJournalEntry",
                column: "PMId");

            migrationBuilder.CreateIndex(
                name: "IX_SysJournalEntry_RMId",
                table: "SysJournalEntry",
                column: "RMId");

            migrationBuilder.CreateIndex(
                name: "IX_SysJournalEntry_RRId",
                table: "SysJournalEntry",
                column: "RRId");

            migrationBuilder.CreateIndex(
                name: "IX_SysJournalEntry_SIId",
                table: "SysJournalEntry",
                column: "SIId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnDisbursement_ApprovedByUserId",
                table: "TrnDisbursement",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnDisbursement_BranchId",
                table: "TrnDisbursement",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnDisbursement_CheckedByUserId",
                table: "TrnDisbursement",
                column: "CheckedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnDisbursement_CreatedByUserId",
                table: "TrnDisbursement",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnDisbursement_CurrencyId",
                table: "TrnDisbursement",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnDisbursement_PreparedByUserId",
                table: "TrnDisbursement",
                column: "PreparedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnDisbursement_UpdatedByUserId",
                table: "TrnDisbursement",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnDisbursementLineDBSet_TrnDisbursement_CVIdId",
                table: "TrnDisbursementLineDBSet",
                column: "TrnDisbursement_CVIdId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnJournalVoucher_ApprovedByUserId",
                table: "TrnJournalVoucher",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnJournalVoucher_BranchId",
                table: "TrnJournalVoucher",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnJournalVoucher_CheckedByUserId",
                table: "TrnJournalVoucher",
                column: "CheckedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnJournalVoucher_CreatedByUserId",
                table: "TrnJournalVoucher",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnJournalVoucher_CurrencyId",
                table: "TrnJournalVoucher",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnJournalVoucher_PreparedByUserId",
                table: "TrnJournalVoucher",
                column: "PreparedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnJournalVoucher_UpdatedByUserId",
                table: "TrnJournalVoucher",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnJournalVoucherLineDBSet_TrnJournalVoucher_JVIdId",
                table: "TrnJournalVoucherLineDBSet",
                column: "TrnJournalVoucher_JVIdId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnPayableMemo_ApprovedByUserId",
                table: "TrnPayableMemo",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnPayableMemo_BranchId",
                table: "TrnPayableMemo",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnPayableMemo_CheckedByUserId",
                table: "TrnPayableMemo",
                column: "CheckedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnPayableMemo_CreatedByUserId",
                table: "TrnPayableMemo",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnPayableMemo_CurrencyId",
                table: "TrnPayableMemo",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnPayableMemo_PreparedByUserId",
                table: "TrnPayableMemo",
                column: "PreparedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnPayableMemo_UpdatedByUserId",
                table: "TrnPayableMemo",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnReceivableMemo_ApprovedByUserId",
                table: "TrnReceivableMemo",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnReceivableMemo_BranchId",
                table: "TrnReceivableMemo",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnReceivableMemo_CheckedByUserId",
                table: "TrnReceivableMemo",
                column: "CheckedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnReceivableMemo_CreatedByUserId",
                table: "TrnReceivableMemo",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnReceivableMemo_CurrencyId",
                table: "TrnReceivableMemo",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnReceivableMemo_PreparedByUserId",
                table: "TrnReceivableMemo",
                column: "PreparedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnReceivableMemo_UpdatedByUserId",
                table: "TrnReceivableMemo",
                column: "UpdatedByUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SysJournalEntry");

            migrationBuilder.DropTable(
                name: "TrnDisbursementLineDBSet");

            migrationBuilder.DropTable(
                name: "TrnJournalVoucherLineDBSet");

            migrationBuilder.DropTable(
                name: "TrnPayableMemo");

            migrationBuilder.DropTable(
                name: "TrnReceivableMemo");

            migrationBuilder.DropTable(
                name: "TrnDisbursement");

            migrationBuilder.DropTable(
                name: "TrnJournalVoucher");
        }
    }
}
