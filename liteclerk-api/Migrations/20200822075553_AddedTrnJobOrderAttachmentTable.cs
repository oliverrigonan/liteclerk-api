using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedTrnJobOrderAttachmentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrnJobOrderAttachment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JOId = table.Column<int>(type: "int", nullable: false),
                    AttachmentCode = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AttachmentType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AttachmentURL = table.Column<string>(type: "nvarchar(max)", maxLength: 255, nullable: false),
                    Particulars = table.Column<string>(type: "nvarchar(max)", maxLength: 255, nullable: false),
                    IsPrinted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrnJobOrderAttachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnJobOrderAttachment_TrnJobOrder_JOId",
                        column: x => x.JOId,
                        principalTable: "TrnJobOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrnJobOrderAttachment_JOId",
                table: "TrnJobOrderAttachment",
                column: "JOId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrnJobOrderAttachment");
        }
    }
}
