using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedMstUserJobDepartmentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MstUserJobDepartment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    JobDepartmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstUserJobDepartment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MstUserJobDepartment_MstJobDepartment_JobDepartmentId",
                        column: x => x.JobDepartmentId,
                        principalTable: "MstJobDepartment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstUserJobDepartment_MstUser_UserId",
                        column: x => x.UserId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MstUserJobDepartment_JobDepartmentId",
                table: "MstUserJobDepartment",
                column: "JobDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_MstUserJobDepartment_UserId",
                table: "MstUserJobDepartment",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MstUserJobDepartment");
        }
    }
}
