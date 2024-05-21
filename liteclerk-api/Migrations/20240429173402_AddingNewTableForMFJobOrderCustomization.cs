using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddingNewTableForMFJobOrderCustomization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrnMFJobOrder",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    JONumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    JODate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ManualNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DocumentReference = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DateScheduled = table.Column<DateTime>(type: "datetime", nullable: false),
                    DateNeeded = table.Column<DateTime>(type: "datetime", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    Engineer = table.Column<string>(nullable: true),
                    Complaint = table.Column<string>(nullable: true),
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
                    table.PrimaryKey("PK_TrnMFJobOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnMFJobOrder_MstUser_ApprovedByUserId",
                        column: x => x.ApprovedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnMFJobOrder_MstCompanyBranch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "MstCompanyBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnMFJobOrder_MstUser_CheckedByUserId",
                        column: x => x.CheckedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnMFJobOrder_MstUser_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnMFJobOrder_MstArticle_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "MstArticle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnMFJobOrder_MstUser_PreparedByUserId",
                        column: x => x.PreparedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnMFJobOrder_MstUser_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrnMFJobOrderLine",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MFJOId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Serial = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Particulars = table.Column<string>(type: "nvarchar(max)", maxLength: 255, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,5)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrnMFJobOrderLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnMFJobOrderLine_TrnMFJobOrder_MFJOId",
                        column: x => x.MFJOId,
                        principalTable: "TrnMFJobOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrnSalesInvoiceMFJOItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SIId = table.Column<int>(type: "int", nullable: false),
                    MFJOItemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrnSalesInvoiceMFJOItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnSalesInvoiceMFJOItem_TrnMFJobOrder_MFJOItemId",
                        column: x => x.MFJOItemId,
                        principalTable: "TrnMFJobOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnSalesInvoiceMFJOItem_TrnSalesInvoice_SIId",
                        column: x => x.SIId,
                        principalTable: "TrnSalesInvoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrnMFJobOrder_ApprovedByUserId",
                table: "TrnMFJobOrder",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnMFJobOrder_BranchId",
                table: "TrnMFJobOrder",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnMFJobOrder_CheckedByUserId",
                table: "TrnMFJobOrder",
                column: "CheckedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnMFJobOrder_CreatedByUserId",
                table: "TrnMFJobOrder",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnMFJobOrder_CustomerId",
                table: "TrnMFJobOrder",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnMFJobOrder_PreparedByUserId",
                table: "TrnMFJobOrder",
                column: "PreparedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnMFJobOrder_UpdatedByUserId",
                table: "TrnMFJobOrder",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnMFJobOrderLine_MFJOId",
                table: "TrnMFJobOrderLine",
                column: "MFJOId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesInvoiceMFJOItem_MFJOItemId",
                table: "TrnSalesInvoiceMFJOItem",
                column: "MFJOItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnSalesInvoiceMFJOItem_SIId",
                table: "TrnSalesInvoiceMFJOItem",
                column: "SIId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrnMFJobOrderLine");

            migrationBuilder.DropTable(
                name: "TrnSalesInvoiceMFJOItem");

            migrationBuilder.DropTable(
                name: "TrnMFJobOrder");
        }
    }
}
