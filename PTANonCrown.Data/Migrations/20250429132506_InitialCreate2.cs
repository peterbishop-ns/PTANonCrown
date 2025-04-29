using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PTANonCrown.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlotTreatments_TreatmentLookup_TreatmentId",
                table: "PlotTreatments");

            migrationBuilder.DropTable(
                name: "TreatmentLookup");

            migrationBuilder.CreateTable(
                name: "Treatments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ShortCode = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Treatments", x => x.ID);
                });

            migrationBuilder.InsertData(
                table: "Treatments",
                columns: new[] { "ID", "Name", "ShortCode" },
                values: new object[,]
                {
                    { 1, "Planting", null },
                    { 2, "Pre-commercial thinning", null },
                    { 3, "Commercial thinning", null },
                    { 4, "Strip shelterwood", null },
                    { 5, "Patch shelterwood", null },
                    { 6, "Uniform shelterwood", null },
                    { 7, "Gap irregular shelterwood", null },
                    { 8, "Continuous cover irregular shelterwood", null },
                    { 9, "Single tree selection", null },
                    { 10, "Group selection", null },
                    { 11, "Partial harvest (unknown)", null }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_PlotTreatments_Treatments_TreatmentId",
                table: "PlotTreatments",
                column: "TreatmentId",
                principalTable: "Treatments",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlotTreatments_Treatments_TreatmentId",
                table: "PlotTreatments");

            migrationBuilder.DropTable(
                name: "Treatments");

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

            migrationBuilder.AddForeignKey(
                name: "FK_PlotTreatments_TreatmentLookup_TreatmentId",
                table: "PlotTreatments",
                column: "TreatmentId",
                principalTable: "TreatmentLookup",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
