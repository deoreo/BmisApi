using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BmisApi.Migrations
{
    /// <inheritdoc />
    public partial class ChangedIncidentNarrativeToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_narratives_incidents_IncidentId",
                table: "narratives");

            migrationBuilder.DropIndex(
                name: "IX_narratives_IncidentId",
                table: "narratives");

            migrationBuilder.DropColumn(
                name: "IncidentId",
                table: "narratives");

            migrationBuilder.AddColumn<string>(
                name: "NarrativeReport",
                table: "incidents",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NarrativeReport",
                table: "incidents");

            migrationBuilder.AddColumn<int>(
                name: "IncidentId",
                table: "narratives",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_narratives_IncidentId",
                table: "narratives",
                column: "IncidentId");

            migrationBuilder.AddForeignKey(
                name: "FK_narratives_incidents_IncidentId",
                table: "narratives",
                column: "IncidentId",
                principalTable: "incidents",
                principalColumn: "Id");
        }
    }
}
