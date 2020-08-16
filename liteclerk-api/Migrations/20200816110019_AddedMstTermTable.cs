﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedMstTermTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MstTerm",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TermCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ManualCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Term = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    NumberOfDays = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedByDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: false),
                    UpdatedByDateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstTerm", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MstTerm_MstUser_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstTerm_MstUser_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MstTerm_CreatedByUserId",
                table: "MstTerm",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MstTerm_UpdatedByUserId",
                table: "MstTerm",
                column: "UpdatedByUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MstTerm");
        }
    }
}
