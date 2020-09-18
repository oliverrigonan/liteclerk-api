using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddInventoryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrnReceivingReceipt",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    RRNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RRDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ManualNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DocumentReference = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ReceivedByUserId = table.Column<int>(type: "int", nullable: false),
                    MstUser_ReceivedByUserIdId = table.Column<int>(nullable: true),
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
                    table.PrimaryKey("PK_TrnReceivingReceipt", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnReceivingReceipt_MstUser_ApprovedByUserId",
                        column: x => x.ApprovedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnReceivingReceipt_MstCompanyBranch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "MstCompanyBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnReceivingReceipt_MstUser_CheckedByUserId",
                        column: x => x.CheckedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnReceivingReceipt_MstUser_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnReceivingReceipt_MstCurrency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "MstCurrency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnReceivingReceipt_MstUser_MstUser_ReceivedByUserIdId",
                        column: x => x.MstUser_ReceivedByUserIdId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnReceivingReceipt_MstUser_PreparedByUserId",
                        column: x => x.PreparedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnReceivingReceipt_MstUser_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrnStockOut",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    OTNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OTDate = table.Column<DateTime>(type: "datetime", nullable: false),
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
                    table.PrimaryKey("PK_TrnStockOut", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnStockOut_MstUser_ApprovedByUserId",
                        column: x => x.ApprovedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockOut_MstCompanyBranch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "MstCompanyBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockOut_MstUser_CheckedByUserId",
                        column: x => x.CheckedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockOut_MstUser_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockOut_MstCurrency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "MstCurrency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockOut_MstUser_PreparedByUserId",
                        column: x => x.PreparedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockOut_MstUser_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrnStockTransfer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    STNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    STDate = table.Column<DateTime>(type: "datetime", nullable: false),
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
                    table.PrimaryKey("PK_TrnStockTransfer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnStockTransfer_MstUser_ApprovedByUserId",
                        column: x => x.ApprovedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockTransfer_MstCompanyBranch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "MstCompanyBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockTransfer_MstUser_CheckedByUserId",
                        column: x => x.CheckedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockTransfer_MstUser_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockTransfer_MstCurrency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "MstCurrency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockTransfer_MstUser_PreparedByUserId",
                        column: x => x.PreparedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockTransfer_MstUser_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrnStockWithdrawal",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    SWNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SWDate = table.Column<DateTime>(type: "datetime", nullable: false),
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
                    table.PrimaryKey("PK_TrnStockWithdrawal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnStockWithdrawal_MstUser_ApprovedByUserId",
                        column: x => x.ApprovedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockWithdrawal_MstCompanyBranch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "MstCompanyBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockWithdrawal_MstUser_CheckedByUserId",
                        column: x => x.CheckedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockWithdrawal_MstUser_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockWithdrawal_MstCurrency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "MstCurrency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockWithdrawal_MstUser_PreparedByUserId",
                        column: x => x.PreparedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnStockWithdrawal_MstUser_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SysInventory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    IVNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PNDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    ArticleItemInventoryId = table.Column<int>(type: "int", nullable: false),
                    QuantityIn = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    QuantityOut = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    Particulars = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    RRId = table.Column<int>(type: "int", nullable: true),
                    SIId = table.Column<int>(type: "int", nullable: true),
                    INId = table.Column<int>(type: "int", nullable: true),
                    OTId = table.Column<int>(type: "int", nullable: true),
                    STId = table.Column<int>(type: "int", nullable: true),
                    SWId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysInventory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SysInventory_MstAccount_AccountId",
                        column: x => x.AccountId,
                        principalTable: "MstAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SysInventory_MstArticle_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "MstArticle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SysInventory_MstArticleItemInventory_ArticleItemInventoryId",
                        column: x => x.ArticleItemInventoryId,
                        principalTable: "MstArticleItemInventory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SysInventory_MstCompanyBranch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "MstCompanyBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SysInventory_TrnStockIn_INId",
                        column: x => x.INId,
                        principalTable: "TrnStockIn",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SysInventory_TrnStockOut_OTId",
                        column: x => x.OTId,
                        principalTable: "TrnStockOut",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SysInventory_TrnReceivingReceipt_RRId",
                        column: x => x.RRId,
                        principalTable: "TrnReceivingReceipt",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SysInventory_TrnSalesInvoice_SIId",
                        column: x => x.SIId,
                        principalTable: "TrnSalesInvoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SysInventory_TrnStockTransfer_STId",
                        column: x => x.STId,
                        principalTable: "TrnStockTransfer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SysInventory_TrnStockWithdrawal_STId",
                        column: x => x.STId,
                        principalTable: "TrnStockWithdrawal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SysInventory_AccountId",
                table: "SysInventory",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_SysInventory_ArticleId",
                table: "SysInventory",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_SysInventory_ArticleItemInventoryId",
                table: "SysInventory",
                column: "ArticleItemInventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SysInventory_BranchId",
                table: "SysInventory",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_SysInventory_INId",
                table: "SysInventory",
                column: "INId");

            migrationBuilder.CreateIndex(
                name: "IX_SysInventory_OTId",
                table: "SysInventory",
                column: "OTId");

            migrationBuilder.CreateIndex(
                name: "IX_SysInventory_RRId",
                table: "SysInventory",
                column: "RRId");

            migrationBuilder.CreateIndex(
                name: "IX_SysInventory_SIId",
                table: "SysInventory",
                column: "SIId");

            migrationBuilder.CreateIndex(
                name: "IX_SysInventory_STId",
                table: "SysInventory",
                column: "STId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnReceivingReceipt_ApprovedByUserId",
                table: "TrnReceivingReceipt",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnReceivingReceipt_BranchId",
                table: "TrnReceivingReceipt",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnReceivingReceipt_CheckedByUserId",
                table: "TrnReceivingReceipt",
                column: "CheckedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnReceivingReceipt_CreatedByUserId",
                table: "TrnReceivingReceipt",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnReceivingReceipt_CurrencyId",
                table: "TrnReceivingReceipt",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnReceivingReceipt_MstUser_ReceivedByUserIdId",
                table: "TrnReceivingReceipt",
                column: "MstUser_ReceivedByUserIdId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnReceivingReceipt_PreparedByUserId",
                table: "TrnReceivingReceipt",
                column: "PreparedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnReceivingReceipt_UpdatedByUserId",
                table: "TrnReceivingReceipt",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockOut_ApprovedByUserId",
                table: "TrnStockOut",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockOut_BranchId",
                table: "TrnStockOut",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockOut_CheckedByUserId",
                table: "TrnStockOut",
                column: "CheckedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockOut_CreatedByUserId",
                table: "TrnStockOut",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockOut_CurrencyId",
                table: "TrnStockOut",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockOut_PreparedByUserId",
                table: "TrnStockOut",
                column: "PreparedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockOut_UpdatedByUserId",
                table: "TrnStockOut",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockTransfer_ApprovedByUserId",
                table: "TrnStockTransfer",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockTransfer_BranchId",
                table: "TrnStockTransfer",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockTransfer_CheckedByUserId",
                table: "TrnStockTransfer",
                column: "CheckedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockTransfer_CreatedByUserId",
                table: "TrnStockTransfer",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockTransfer_CurrencyId",
                table: "TrnStockTransfer",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockTransfer_PreparedByUserId",
                table: "TrnStockTransfer",
                column: "PreparedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockTransfer_UpdatedByUserId",
                table: "TrnStockTransfer",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockWithdrawal_ApprovedByUserId",
                table: "TrnStockWithdrawal",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockWithdrawal_BranchId",
                table: "TrnStockWithdrawal",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockWithdrawal_CheckedByUserId",
                table: "TrnStockWithdrawal",
                column: "CheckedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockWithdrawal_CreatedByUserId",
                table: "TrnStockWithdrawal",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockWithdrawal_CurrencyId",
                table: "TrnStockWithdrawal",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockWithdrawal_PreparedByUserId",
                table: "TrnStockWithdrawal",
                column: "PreparedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnStockWithdrawal_UpdatedByUserId",
                table: "TrnStockWithdrawal",
                column: "UpdatedByUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SysInventory");

            migrationBuilder.DropTable(
                name: "TrnStockOut");

            migrationBuilder.DropTable(
                name: "TrnReceivingReceipt");

            migrationBuilder.DropTable(
                name: "TrnStockTransfer");

            migrationBuilder.DropTable(
                name: "TrnStockWithdrawal");
        }
    }
}
