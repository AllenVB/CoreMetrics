using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleAnalytics.Api.Migrations
{
    /// <inheritdoc />
    public partial class CreateTracksTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Browser",
                table: "Visits",
                newName: "UserAgent");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserAgent",
                table: "Visits",
                newName: "Browser");
        }
    }
}
