using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PTANonCrown.Data.Migrations
{
    /// <inheritdoc />
    public partial class MissingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TreesLive_Plots_PlotID",
                table: "TreesLive");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TreesLive",
                table: "TreesLive");

            migrationBuilder.RenameTable(
                name: "TreesLive",
                newName: "TreeLive");

            migrationBuilder.RenameIndex(
                name: "IX_TreesLive_PlotID",
                table: "TreeLive",
                newName: "IX_TreeLive_PlotID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TreeLive",
                table: "TreeLive",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_TreeLive_Plots_PlotID",
                table: "TreeLive",
                column: "PlotID",
                principalTable: "Plots",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TreeLive_Plots_PlotID",
                table: "TreeLive");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TreeLive",
                table: "TreeLive");

            migrationBuilder.RenameTable(
                name: "TreeLive",
                newName: "TreesLive");

            migrationBuilder.RenameIndex(
                name: "IX_TreeLive_PlotID",
                table: "TreesLive",
                newName: "IX_TreesLive_PlotID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TreesLive",
                table: "TreesLive",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_TreesLive_Plots_PlotID",
                table: "TreesLive",
                column: "PlotID",
                principalTable: "Plots",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
