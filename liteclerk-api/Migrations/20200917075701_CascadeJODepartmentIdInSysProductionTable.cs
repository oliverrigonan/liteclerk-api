using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class CascadeJODepartmentIdInSysProductionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SysProduction_TrnJobOrderDepartment_JODepartmentId",
                table: "SysProduction");

            migrationBuilder.AddForeignKey(
                name: "FK_SysProduction_TrnJobOrderDepartment_JODepartmentId",
                table: "SysProduction",
                column: "JODepartmentId",
                principalTable: "TrnJobOrderDepartment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SysProduction_TrnJobOrderDepartment_JODepartmentId",
                table: "SysProduction");

            migrationBuilder.AddForeignKey(
                name: "FK_SysProduction_TrnJobOrderDepartment_JODepartmentId",
                table: "SysProduction",
                column: "JODepartmentId",
                principalTable: "TrnJobOrderDepartment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
