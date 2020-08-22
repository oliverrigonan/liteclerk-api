using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedMstJobTypeAttachmentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MstJobTypeAttachment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobTypeId = table.Column<int>(type: "int", nullable: false),
                    AttachmentCode = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AttachmentType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsPrinted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstJobTypeAttachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MstJobTypeAttachment_MstJobType_JobTypeId",
                        column: x => x.JobTypeId,
                        principalTable: "MstJobType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MstJobTypeAttachment_JobTypeId",
                table: "MstJobTypeAttachment",
                column: "JobTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MstJobTypeAttachment");
        }
    }
}
