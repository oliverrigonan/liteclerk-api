using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedAllMstAccountingTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MstAccountCashFlow",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountCashFlowCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ManualCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AccountCashFlow = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedByDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: false),
                    UpdatedByDateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstAccountCashFlow", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MstAccountCashFlow_MstUser_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstAccountCashFlow_MstUser_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MstAccountCategory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountCategoryCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ManualCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AccountCategory = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedByDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: false),
                    UpdatedByDateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstAccountCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MstAccountCategory_MstUser_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstAccountCategory_MstUser_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MstAccountType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountTypeCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ManualCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AccountType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AccountCategoryId = table.Column<int>(type: "int", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedByDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: false),
                    UpdatedByDateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstAccountType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MstAccountType_MstAccountCategory_AccountCategoryId",
                        column: x => x.AccountCategoryId,
                        principalTable: "MstAccountCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstAccountType_MstUser_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstAccountType_MstUser_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MstAccount",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ManualCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Account = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AccountTypeId = table.Column<int>(type: "int", nullable: false),
                    AccountCashFlowId = table.Column<int>(type: "int", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedByDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: false),
                    UpdatedByDateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstAccount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MstAccount_MstAccountCashFlow_AccountCashFlowId",
                        column: x => x.AccountCashFlowId,
                        principalTable: "MstAccountCashFlow",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstAccount_MstAccountType_AccountTypeId",
                        column: x => x.AccountTypeId,
                        principalTable: "MstAccountType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstAccount_MstUser_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstAccount_MstUser_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MstAccount_AccountCashFlowId",
                table: "MstAccount",
                column: "AccountCashFlowId");

            migrationBuilder.CreateIndex(
                name: "IX_MstAccount_AccountTypeId",
                table: "MstAccount",
                column: "AccountTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MstAccount_CreatedByUserId",
                table: "MstAccount",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MstAccount_UpdatedByUserId",
                table: "MstAccount",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MstAccountCashFlow_CreatedByUserId",
                table: "MstAccountCashFlow",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MstAccountCashFlow_UpdatedByUserId",
                table: "MstAccountCashFlow",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MstAccountCategory_CreatedByUserId",
                table: "MstAccountCategory",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MstAccountCategory_UpdatedByUserId",
                table: "MstAccountCategory",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MstAccountType_AccountCategoryId",
                table: "MstAccountType",
                column: "AccountCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MstAccountType_CreatedByUserId",
                table: "MstAccountType",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MstAccountType_UpdatedByUserId",
                table: "MstAccountType",
                column: "UpdatedByUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MstAccount");

            migrationBuilder.DropTable(
                name: "MstAccountCashFlow");

            migrationBuilder.DropTable(
                name: "MstAccountType");

            migrationBuilder.DropTable(
                name: "MstAccountCategory");
        }
    }
}
