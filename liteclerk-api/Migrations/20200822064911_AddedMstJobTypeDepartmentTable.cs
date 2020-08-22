using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedMstJobTypeDepartmentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MstJobTypeDepartment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobTypeId = table.Column<int>(type: "int", nullable: false),
                    JobDepartmentId = table.Column<int>(type: "int", nullable: false),
                    NumberOfDays = table.Column<decimal>(type: "decimal(18,5)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstJobTypeDepartment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MstJobTypeDepartment_MstJobDepartment_JobDepartmentId",
                        column: x => x.JobDepartmentId,
                        principalTable: "MstJobDepartment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstJobTypeDepartment_MstJobType_JobTypeId",
                        column: x => x.JobTypeId,
                        principalTable: "MstJobType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MstJobTypeDepartment_JobDepartmentId",
                table: "MstJobTypeDepartment",
                column: "JobDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_MstJobTypeDepartment_JobTypeId",
                table: "MstJobTypeDepartment",
                column: "JobTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MstJobTypeDepartment");
        }
    }
}
