using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PTANonCrown.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Stands",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CruiseID = table.Column<int>(type: "INTEGER", nullable: false),
                    Easting = table.Column<float>(type: "REAL", nullable: true),
                    Ecodistrict = table.Column<int>(type: "INTEGER", nullable: false),
                    Location = table.Column<string>(type: "TEXT", nullable: true),
                    Northing = table.Column<float>(type: "REAL", nullable: true),
                    Organization = table.Column<string>(type: "TEXT", nullable: true),
                    PlannerID = table.Column<int>(type: "INTEGER", nullable: false),
                    StandNumber = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stands", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Plots",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AverageSampleTreeAge = table.Column<int>(type: "INTEGER", nullable: false),
                    AverageSampleTreeDBH_cm = table.Column<int>(type: "INTEGER", nullable: false),
                    AverageSampleTreeSpecies = table.Column<int>(type: "INTEGER", nullable: false),
                    Blowdown = table.Column<int>(type: "INTEGER", nullable: false),
                    HorizontalStructure = table.Column<int>(type: "INTEGER", nullable: false),
                    OGFSampleTreeAge = table.Column<int>(type: "INTEGER", nullable: false),
                    OGFSampleTreeDBH_cm = table.Column<int>(type: "INTEGER", nullable: false),
                    OGFSampleTreeSpecies = table.Column<int>(type: "INTEGER", nullable: false),
                    OneCohortSenescent = table.Column<bool>(type: "INTEGER", nullable: false),
                    PlotNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    RegenHeightHWLIT = table.Column<bool>(type: "INTEGER", nullable: false),
                    RegenHeightSWLIT = table.Column<bool>(type: "INTEGER", nullable: false),
                    StandID = table.Column<int>(type: "INTEGER", nullable: false),
                    StockingBeechRegeneration = table.Column<int>(type: "INTEGER", nullable: false),
                    StockingLITSeedTree = table.Column<int>(type: "INTEGER", nullable: false),
                    StockingRegenCommercialSpecies = table.Column<int>(type: "INTEGER", nullable: false),
                    StockingRegenLITSpecies = table.Column<int>(type: "INTEGER", nullable: false),
                    UnderstoryDominated = table.Column<int>(type: "INTEGER", nullable: false),
                    UnderstoryStrata = table.Column<int>(type: "INTEGER", nullable: false),
                    UnevenAged = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plots", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Plots_Stands_StandID",
                        column: x => x.StandID,
                        principalTable: "Stands",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CoarseWoodys",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DBH_end = table.Column<int>(type: "INTEGER", nullable: false),
                    DBH_start = table.Column<int>(type: "INTEGER", nullable: false),
                    PlotID = table.Column<int>(type: "INTEGER", nullable: false),
                    Tally_Cavity = table.Column<int>(type: "INTEGER", nullable: false),
                    Tally_Hardwood = table.Column<int>(type: "INTEGER", nullable: false),
                    Tally_Softwood = table.Column<int>(type: "INTEGER", nullable: false),
                    Tally_Unknown = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoarseWoodys", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CoarseWoodys_Plots_PlotID",
                        column: x => x.PlotID,
                        principalTable: "Plots",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TreesDead",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DBH_end = table.Column<int>(type: "INTEGER", nullable: false),
                    DBH_start = table.Column<int>(type: "INTEGER", nullable: false),
                    PlotID = table.Column<int>(type: "INTEGER", nullable: false),
                    Tally_Cavity = table.Column<int>(type: "INTEGER", nullable: false),
                    Tally_Hardwood = table.Column<int>(type: "INTEGER", nullable: false),
                    Tally_Softwood = table.Column<int>(type: "INTEGER", nullable: false),
                    Tally_Unknown = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreesDead", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TreesDead_Plots_PlotID",
                        column: x => x.PlotID,
                        principalTable: "Plots",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TreesLive",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AGS = table.Column<bool>(type: "INTEGER", nullable: false),
                    Cavity = table.Column<bool>(type: "INTEGER", nullable: false),
                    DBH_cm = table.Column<int>(type: "INTEGER", nullable: false),
                    Diversity = table.Column<bool>(type: "INTEGER", nullable: false),
                    Height_m = table.Column<decimal>(type: "TEXT", nullable: false),
                    Legacy = table.Column<bool>(type: "INTEGER", nullable: false),
                    LIT = table.Column<bool>(type: "INTEGER", nullable: false),
                    Mast = table.Column<bool>(type: "INTEGER", nullable: false),
                    PLExSitu = table.Column<bool>(type: "INTEGER", nullable: false),
                    PLInSitu = table.Column<bool>(type: "INTEGER", nullable: false),
                    PlotID = table.Column<int>(type: "INTEGER", nullable: false),
                    SCanopy = table.Column<bool>(type: "INTEGER", nullable: false),
                    Species = table.Column<int>(type: "INTEGER", nullable: false),
                    TreeNumber = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreesLive", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TreesLive_Plots_PlotID",
                        column: x => x.PlotID,
                        principalTable: "Plots",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoarseWoodys_PlotID",
                table: "CoarseWoodys",
                column: "PlotID");

            migrationBuilder.CreateIndex(
                name: "IX_Plots_StandID",
                table: "Plots",
                column: "StandID");

            migrationBuilder.CreateIndex(
                name: "IX_TreesDead_PlotID",
                table: "TreesDead",
                column: "PlotID");

            migrationBuilder.CreateIndex(
                name: "IX_TreesLive_PlotID",
                table: "TreesLive",
                column: "PlotID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoarseWoodys");

            migrationBuilder.DropTable(
                name: "TreesDead");

            migrationBuilder.DropTable(
                name: "TreesLive");

            migrationBuilder.DropTable(
                name: "Plots");

            migrationBuilder.DropTable(
                name: "Stands");
        }
    }
}
