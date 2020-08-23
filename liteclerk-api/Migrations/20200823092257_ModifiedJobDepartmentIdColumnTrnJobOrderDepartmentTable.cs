using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class ModifiedJobDepartmentIdColumnTrnJobOrderDepartmentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrnJobOrderDepartment_MstJobDepartment_JOId",
                table: "TrnJobOrderDepartment");

            migrationBuilder.CreateIndex(
                name: "IX_TrnJobOrderDepartment_JobDepartmentId",
                table: "TrnJobOrderDepartment",
                column: "JobDepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrnJobOrderDepartment_MstJobDepartment_JobDepartmentId",
                table: "TrnJobOrderDepartment",
                column: "JobDepartmentId",
                principalTable: "MstJobDepartment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrnJobOrderDepartment_MstJobDepartment_JobDepartmentId",
                table: "TrnJobOrderDepartment");

            migrationBuilder.DropIndex(
                name: "IX_TrnJobOrderDepartment_JobDepartmentId",
                table: "TrnJobOrderDepartment");

            migrationBuilder.AddForeignKey(
                name: "FK_TrnJobOrderDepartment_MstJobDepartment_JOId",
                table: "TrnJobOrderDepartment",
                column: "JOId",
                principalTable: "MstJobDepartment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
