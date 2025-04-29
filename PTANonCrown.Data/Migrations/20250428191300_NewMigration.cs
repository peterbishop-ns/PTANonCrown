using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PTANonCrown.Data.Migrations
{
    /// <inheritdoc />
    public partial class NewMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TreatmentLookup",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ShortCode = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreatmentLookup", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PlotTreatment",
                columns: table => new
                {
                    PlotId = table.Column<int>(type: "INTEGER", nullable: false),
                    TreatmentId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlotTreatment", x => new { x.PlotId, x.TreatmentId });
                    table.ForeignKey(
                        name: "FK_PlotTreatment_Plots_PlotId",
                        column: x => x.PlotId,
                        principalTable: "Plots",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlotTreatment_TreatmentLookup_TreatmentId",
                        column: x => x.TreatmentId,
                        principalTable: "TreatmentLookup",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlotTreatment_TreatmentId",
                table: "PlotTreatment",
                column: "TreatmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlotTreatment");

            migrationBuilder.DropTable(
                name: "TreatmentLookup");
        }
    }
}
