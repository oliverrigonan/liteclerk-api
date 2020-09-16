using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedSysProductionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SysProduction",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    PNNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PNDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Particulars = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductionTimeStamp = table.Column<DateTime>(type: "datetime", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    JODepartmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysProduction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SysProduction_MstCompanyBranch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "MstCompanyBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SysProduction_TrnJobOrderDepartment_JODepartmentId",
                        column: x => x.JODepartmentId,
                        principalTable: "TrnJobOrderDepartment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SysProduction_MstUser_UserId",
                        column: x => x.UserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SysProduction_BranchId",
                table: "SysProduction",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_SysProduction_JODepartmentId",
                table: "SysProduction",
                column: "JODepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_SysProduction_UserId",
                table: "SysProduction",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SysProduction");
        }
    }
}
