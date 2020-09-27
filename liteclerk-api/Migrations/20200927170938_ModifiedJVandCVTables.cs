using Microsoft.EntityFrameworkCore.Migrations;

namespace liteclerk_api.Migrations
{
    public partial class ModifiedJVandCVTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrnDisbursementLineDBSet");

            migrationBuilder.DropTable(
                name: "TrnJournalVoucherLineDBSet");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrnDisbursementLineDBSet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CVId = table.Column<int>(type: "int", nullable: false),
                    Particulars = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrnDisbursement_CVIdId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrnDisbursementLineDBSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnDisbursementLineDBSet_TrnDisbursement_TrnDisbursement_CVIdId",
                        column: x => x.TrnDisbursement_CVIdId,
                        principalTable: "TrnDisbursement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrnJournalVoucherLineDBSet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JVId = table.Column<int>(type: "int", nullable: false),
                    Particulars = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrnJournalVoucher_JVIdId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrnJournalVoucherLineDBSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrnJournalVoucherLineDBSet_TrnJournalVoucher_TrnJournalVoucher_JVIdId",
                        column: x => x.TrnJournalVoucher_JVIdId,
                        principalTable: "TrnJournalVoucher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrnDisbursementLineDBSet_TrnDisbursement_CVIdId",
                table: "TrnDisbursementLineDBSet",
                column: "TrnDisbursement_CVIdId");

            migrationBuilder.CreateIndex(
                name: "IX_TrnJournalVoucherLineDBSet_TrnJournalVoucher_JVIdId",
                table: "TrnJournalVoucherLineDBSet",
                column: "TrnJournalVoucher_JVIdId");
        }
    }
}
