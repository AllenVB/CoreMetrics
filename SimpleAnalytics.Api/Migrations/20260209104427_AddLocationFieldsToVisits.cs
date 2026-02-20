using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleAnalytics.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddLocationFieldsToVisits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Visits",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Visits",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                table: "Visits",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Visits");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Visits");

            migrationBuilder.DropColumn(
                name: "IpAddress",
                table: "Visits");
        }
    }
}
