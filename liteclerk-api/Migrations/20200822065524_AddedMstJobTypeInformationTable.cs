using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedMstJobTypeInformationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MstJobTypeInformation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobTypeId = table.Column<int>(type: "int", nullable: false),
                    InformationCode = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    InformationGroup = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsPrinted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstJobTypeInformation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MstJobTypeInformation_MstJobType_JobTypeId",
                        column: x => x.JobTypeId,
                        principalTable: "MstJobType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MstJobTypeInformation_JobTypeId",
                table: "MstJobTypeInformation",
                column: "JobTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MstJobTypeInformation");
        }
    }
}
