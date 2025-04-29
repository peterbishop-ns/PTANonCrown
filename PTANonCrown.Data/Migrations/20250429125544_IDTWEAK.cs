using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PTANonCrown.Data.Migrations
{
    /// <inheritdoc />
    public partial class IDTWEAK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PlotTreatment",
                table: "PlotTreatment");

            migrationBuilder.DropIndex(
                name: "IX_PlotTreatment_PlotId",
                table: "PlotTreatment");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PlotTreatment");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlotTreatment",
                table: "PlotTreatment",
                columns: new[] { "PlotId", "TreatmentId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PlotTreatment",
                table: "PlotTreatment");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "PlotTreatment",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlotTreatment",
                table: "PlotTreatment",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PlotTreatment_PlotId",
                table: "PlotTreatment",
                column: "PlotId");
        }
    }
}
