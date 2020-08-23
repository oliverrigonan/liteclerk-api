using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedTrnJobOrderDepartmentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrnJobOrderDepartment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JOId = table.Column<int>(type: "int", nullable: false),
                    JobDepartmentId = table.Column<int>(type: "int", nullable: false),
                    Particulars = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Value = table.Column<string>(nullable: true),
                    StatusByUserId = table.Column<int>(type: "int", nullable: false),
                    StatusUpdatedDateTime = table.Column<DateTime>(nullable: false),
                    MstJobDepartmentDBsetId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrnJobOrderDepartment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnJobOrderDepartment_MstJobDepartment_JOId",
                        column: x => x.JOId,
                        principalTable: "MstJobDepartment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnJobOrderDepartment_TrnJobOrder_JOId",
                        column: x => x.JOId,
                        principalTable: "TrnJobOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnJobOrderDepartment_MstJobDepartment_MstJobDepartmentDBsetId",
                        column: x => x.MstJobDepartmentDBsetId,
                        principalTable: "MstJobDepartment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnJobOrderDepartment_MstUser_StatusByUserId",
                        column: x => x.StatusByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrnJobOrderDepartment_JOId",
                table: "TrnJobOrderDepartment",
                column: "JOId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnJobOrderDepartment_MstJobDepartmentDBsetId",
                table: "TrnJobOrderDepartment",
                column: "MstJobDepartmentDBsetId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnJobOrderDepartment_StatusByUserId",
                table: "TrnJobOrderDepartment",
                column: "StatusByUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrnJobOrderDepartment");
        }
    }
}
