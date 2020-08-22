using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedTrnJobOrderInformationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrnJobOrderInformation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JOId = table.Column<int>(type: "int", nullable: false),
                    InformationCode = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    InformationGroup = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Particulars = table.Column<string>(type: "nvarchar(max)", maxLength: 255, nullable: false),
                    IsPrinted = table.Column<bool>(type: "bit", nullable: false),
                    InformationByUserId = table.Column<int>(type: "int", nullable: false),
                    InformationUpdatedDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrnJobOrderInformation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnJobOrderInformation_MstUser_InformationByUserId",
                        column: x => x.InformationByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrnJobOrderInformation_TrnJobOrder_JOId",
                        column: x => x.JOId,
                        principalTable: "TrnJobOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrnJobOrderInformation_InformationByUserId",
                table: "TrnJobOrderInformation",
                column: "InformationByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnJobOrderInformation_JOId",
                table: "TrnJobOrderInformation",
                column: "JOId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrnJobOrderInformation");
        }
    }
}
