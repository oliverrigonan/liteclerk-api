using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedPurchaseOrderAndPurchaseRequestTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrnPurchaseRequest",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    PRNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PRDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ManualNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DocumentReference = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    TermId = table.Column<int>(type: "int", nullable: false),
                    DateNeeded = table.Column<DateTime>(type: "datetime", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestedByUserId = table.Column<int>(type: "int", nullable: false),
                    PreparedByUserId = table.Column<int>(type: "int", nullable: false),
                    CheckedByUserId = table.Column<int>(type: "int", nullable: false),
                    ApprovedByUserId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
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
                    table.PrimaryKey("PK_TrnPurchaseRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnPurchaseRequest_MstUser_ApprovedByUserId",
                        column: x => x.ApprovedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnPurchaseRequest_MstCompanyBranch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "MstCompanyBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnPurchaseRequest_MstUser_CheckedByUserId",
                        column: x => x.CheckedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnPurchaseRequest_MstUser_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnPurchaseRequest_MstCurrency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "MstCurrency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnPurchaseRequest_MstUser_PreparedByUserId",
                        column: x => x.PreparedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnPurchaseRequest_MstUser_RequestedByUserId",
                        column: x => x.RequestedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnPurchaseRequest_MstArticle_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "MstArticle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnPurchaseRequest_MstTerm_TermId",
                        column: x => x.TermId,
                        principalTable: "MstTerm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnPurchaseRequest_MstUser_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrnPurchaseOrder",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    PONumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PODate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ManualNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DocumentReference = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    TermId = table.Column<int>(type: "int", nullable: false),
                    DateNeeded = table.Column<DateTime>(type: "datetime", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PRId = table.Column<int>(type: "int", nullable: true),
                    RequestedByUserId = table.Column<int>(type: "int", nullable: false),
                    PreparedByUserId = table.Column<int>(type: "int", nullable: false),
                    CheckedByUserId = table.Column<int>(type: "int", nullable: false),
                    ApprovedByUserId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
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
                    table.PrimaryKey("PK_TrnPurchaseOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnPurchaseOrder_MstUser_ApprovedByUserId",
                        column: x => x.ApprovedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnPurchaseOrder_MstCompanyBranch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "MstCompanyBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnPurchaseOrder_MstUser_CheckedByUserId",
                        column: x => x.CheckedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnPurchaseOrder_MstUser_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnPurchaseOrder_MstCurrency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "MstCurrency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnPurchaseOrder_TrnPurchaseRequest_PRId",
                        column: x => x.PRId,
                        principalTable: "TrnPurchaseRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnPurchaseOrder_MstUser_PreparedByUserId",
                        column: x => x.PreparedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnPurchaseOrder_MstUser_RequestedByUserId",
                        column: x => x.RequestedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnPurchaseOrder_MstArticle_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "MstArticle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnPurchaseOrder_MstTerm_TermId",
                        column: x => x.TermId,
                        principalTable: "MstTerm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnPurchaseOrder_MstUser_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrnPurchaseRequestItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PRId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Particulars = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    UnitId = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    BaseQuantity = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    BaseUnitId = table.Column<int>(type: "int", nullable: false),
                    BaseCost = table.Column<decimal>(type: "decimal(18,5)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrnPurchaseRequestItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnPurchaseRequestItem_MstUnit_BaseUnitId",
                        column: x => x.BaseUnitId,
                        principalTable: "MstUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnPurchaseRequestItem_MstArticle_ItemId",
                        column: x => x.ItemId,
                        principalTable: "MstArticle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnPurchaseRequestItem_TrnPurchaseRequest_PRId",
                        column: x => x.PRId,
                        principalTable: "TrnPurchaseRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnPurchaseRequestItem_MstUnit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "MstUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrnPurchaseOrderItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    POId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Particulars = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    UnitId = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    BaseQuantity = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    BaseUnitId = table.Column<int>(type: "int", nullable: false),
                    BaseCost = table.Column<decimal>(type: "decimal(18,5)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrnPurchaseOrderItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnPurchaseOrderItem_MstUnit_BaseUnitId",
                        column: x => x.BaseUnitId,
                        principalTable: "MstUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnPurchaseOrderItem_MstArticle_ItemId",
                        column: x => x.ItemId,
                        principalTable: "MstArticle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnPurchaseOrderItem_TrnPurchaseOrder_POId",
                        column: x => x.POId,
                        principalTable: "TrnPurchaseOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnPurchaseOrderItem_MstUnit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "MstUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrnPurchaseOrder_ApprovedByUserId",
                table: "TrnPurchaseOrder",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnPurchaseOrder_BranchId",
                table: "TrnPurchaseOrder",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnPurchaseOrder_CheckedByUserId",
                table: "TrnPurchaseOrder",
                column: "CheckedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnPurchaseOrder_CreatedByUserId",
                table: "TrnPurchaseOrder",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnPurchaseOrder_CurrencyId",
                table: "TrnPurchaseOrder",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnPurchaseOrder_PRId",
                table: "TrnPurchaseOrder",
                column: "PRId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnPurchaseOrder_PreparedByUserId",
                table: "TrnPurchaseOrder",
                column: "PreparedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnPurchaseOrder_RequestedByUserId",
                table: "TrnPurchaseOrder",
                column: "RequestedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnPurchaseOrder_SupplierId",
                table: "TrnPurchaseOrder",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnPurchaseOrder_TermId",
                table: "TrnPurchaseOrder",
                column: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnPurchaseOrder_UpdatedByUserId",
                table: "TrnPurchaseOrder",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnPurchaseOrderItem_BaseUnitId",
                table: "TrnPurchaseOrderItem",
                column: "BaseUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnPurchaseOrderItem_ItemId",
                table: "TrnPurchaseOrderItem",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnPurchaseOrderItem_POId",
                table: "TrnPurchaseOrderItem",
                column: "POId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnPurchaseOrderItem_UnitId",
                table: "TrnPurchaseOrderItem",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnPurchaseRequest_ApprovedByUserId",
                table: "TrnPurchaseRequest",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnPurchaseRequest_BranchId",
                table: "TrnPurchaseRequest",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnPurchaseRequest_CheckedByUserId",
                table: "TrnPurchaseRequest",
                column: "CheckedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnPurchaseRequest_CreatedByUserId",
                table: "TrnPurchaseRequest",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnPurchaseRequest_CurrencyId",
                table: "TrnPurchaseRequest",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnPurchaseRequest_PreparedByUserId",
                table: "TrnPurchaseRequest",
                column: "PreparedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnPurchaseRequest_RequestedByUserId",
                table: "TrnPurchaseRequest",
                column: "RequestedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnPurchaseRequest_SupplierId",
                table: "TrnPurchaseRequest",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnPurchaseRequest_TermId",
                table: "TrnPurchaseRequest",
                column: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnPurchaseRequest_UpdatedByUserId",
                table: "TrnPurchaseRequest",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnPurchaseRequestItem_BaseUnitId",
                table: "TrnPurchaseRequestItem",
                column: "BaseUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnPurchaseRequestItem_ItemId",
                table: "TrnPurchaseRequestItem",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnPurchaseRequestItem_PRId",
                table: "TrnPurchaseRequestItem",
                column: "PRId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnPurchaseRequestItem_UnitId",
                table: "TrnPurchaseRequestItem",
                column: "UnitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrnPurchaseOrderItem");

            migrationBuilder.DropTable(
                name: "TrnPurchaseRequestItem");

            migrationBuilder.DropTable(
                name: "TrnPurchaseOrder");

            migrationBuilder.DropTable(
                name: "TrnPurchaseRequest");
        }
    }
}
