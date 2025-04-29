using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PTANonCrown.Data.Migrations
{
    /// <inheritdoc />
    public partial class IDTWEAK2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlotTreatment_Plots_PlotId",
                table: "PlotTreatment");

            migrationBuilder.DropForeignKey(
                name: "FK_PlotTreatment_TreatmentLookup_TreatmentId",
                table: "PlotTreatment");

            migrationBuilder.DropForeignKey(
                name: "FK_TreeLive_Plots_PlotID",
                table: "TreeLive");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TreeLive",
                table: "TreeLive");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlotTreatment",
                table: "PlotTreatment");

            migrationBuilder.RenameTable(
                name: "TreeLive",
                newName: "TreesLive");

            migrationBuilder.RenameTable(
                name: "PlotTreatment",
                newName: "PlotTreatments");

            migrationBuilder.RenameIndex(
                name: "IX_TreeLive_PlotID",
                table: "TreesLive",
                newName: "IX_TreesLive_PlotID");

            migrationBuilder.RenameIndex(
                name: "IX_PlotTreatment_TreatmentId",
                table: "PlotTreatments",
                newName: "IX_PlotTreatments_TreatmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TreesLive",
                table: "TreesLive",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlotTreatments",
                table: "PlotTreatments",
                columns: new[] { "PlotId", "TreatmentId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PlotTreatments_Plots_PlotId",
                table: "PlotTreatments",
                column: "PlotId",
                principalTable: "Plots",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlotTreatments_TreatmentLookup_TreatmentId",
                table: "PlotTreatments",
                column: "TreatmentId",
                principalTable: "TreatmentLookup",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TreesLive_Plots_PlotID",
                table: "TreesLive",
                column: "PlotID",
                principalTable: "Plots",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlotTreatments_Plots_PlotId",
                table: "PlotTreatments");

            migrationBuilder.DropForeignKey(
                name: "FK_PlotTreatments_TreatmentLookup_TreatmentId",
                table: "PlotTreatments");

            migrationBuilder.DropForeignKey(
                name: "FK_TreesLive_Plots_PlotID",
                table: "TreesLive");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TreesLive",
                table: "TreesLive");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlotTreatments",
                table: "PlotTreatments");

            migrationBuilder.RenameTable(
                name: "TreesLive",
                newName: "TreeLive");

            migrationBuilder.RenameTable(
                name: "PlotTreatments",
                newName: "PlotTreatment");

            migrationBuilder.RenameIndex(
                name: "IX_TreesLive_PlotID",
                table: "TreeLive",
                newName: "IX_TreeLive_PlotID");

            migrationBuilder.RenameIndex(
                name: "IX_PlotTreatments_TreatmentId",
                table: "PlotTreatment",
                newName: "IX_PlotTreatment_TreatmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TreeLive",
                table: "TreeLive",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlotTreatment",
                table: "PlotTreatment",
                columns: new[] { "PlotId", "TreatmentId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PlotTreatment_Plots_PlotId",
                table: "PlotTreatment",
                column: "PlotId",
                principalTable: "Plots",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlotTreatment_TreatmentLookup_TreatmentId",
                table: "PlotTreatment",
                column: "TreatmentId",
                principalTable: "TreatmentLookup",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TreeLive_Plots_PlotID",
                table: "TreeLive",
                column: "PlotID",
                principalTable: "Plots",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
