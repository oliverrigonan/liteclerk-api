using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class AddedMstUserBranchTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MstUserBranchDBSet",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    MstUser_UserIdId = table.Column<int>(nullable: true),
                    BranchId = table.Column<int>(nullable: false),
                    MstCompanyBranch_BranchIdId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstUserBranchDBSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MstUserBranchDBSet_MstCompanyBranch_MstCompanyBranch_BranchIdId",
                        column: x => x.MstCompanyBranch_BranchIdId,
                        principalTable: "MstCompanyBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MstUserBranchDBSet_MstUser_MstUser_UserIdId",
                        column: x => x.MstUser_UserIdId,
                        principalTable: "MstUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MstUserBranchDBSet_MstCompanyBranch_BranchIdId",
                table: "MstUserBranchDBSet",
                column: "MstCompanyBranch_BranchIdId");

            migrationBuilder.CreateIndex(
                name: "IX_MstUserBranchDBSet_MstUser_UserIdId",
                table: "MstUserBranchDBSet",
                column: "MstUser_UserIdId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MstUserBranchDBSet");
        }
    }
}
