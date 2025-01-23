using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BmisApi.Migrations
{
    /// <inheritdoc />
    public partial class BlotterAddNarrativeAndStatusEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Total",
                table: "projects");

            migrationBuilder.AddColumn<string>(
                name: "Narrative",
                table: "blotters",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Narrative",
                table: "blotters");

            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "projects",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
