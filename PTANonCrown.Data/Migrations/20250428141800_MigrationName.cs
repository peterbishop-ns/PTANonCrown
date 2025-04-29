using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PTANonCrown.Data.Migrations
{
    /// <inheritdoc />
    public partial class MigrationName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "HeightPredicted_m",
                table: "TreesLive",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HeightPredicted_m",
                table: "TreesLive");
        }
    }
}
