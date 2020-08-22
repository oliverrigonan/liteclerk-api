using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedMstJobDepartmentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MstJobDepartment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobDepartmentCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ManualCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    JobDepartment = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: false),
                    UpdatedDateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstJobDepartment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MstJobDepartment_MstUser_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstJobDepartment_MstUser_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MstJobDepartment_CreatedByUserId",
                table: "MstJobDepartment",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MstJobDepartment_UpdatedByUserId",
                table: "MstJobDepartment",
                column: "UpdatedByUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MstJobDepartment");
        }
    }
}
