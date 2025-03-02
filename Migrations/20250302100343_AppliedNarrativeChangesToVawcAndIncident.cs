using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BmisApi.Migrations
{
    /// <inheritdoc />
    public partial class AppliedNarrativeChangesToVawcAndIncident : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_narratives_blotters_ReportId",
                table: "narratives");

            migrationBuilder.DropIndex(
                name: "IX_narratives_ReportId",
                table: "narratives");

            migrationBuilder.DropColumn(
                name: "Narrative",
                table: "vawc");

            migrationBuilder.DropColumn(
                name: "ReportId",
                table: "narratives");

            migrationBuilder.DropColumn(
                name: "Narrative",
                table: "incidents");

            migrationBuilder.AddColumn<int>(
                name: "BlotterId",
                table: "narratives",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IncidentId",
                table: "narratives",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VawcId",
                table: "narratives",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_narratives_BlotterId",
                table: "narratives",
                column: "BlotterId");

            migrationBuilder.CreateIndex(
                name: "IX_narratives_IncidentId",
                table: "narratives",
                column: "IncidentId");

            migrationBuilder.CreateIndex(
                name: "IX_narratives_VawcId",
                table: "narratives",
                column: "VawcId");

            migrationBuilder.AddForeignKey(
                name: "FK_narratives_blotters_BlotterId",
                table: "narratives",
                column: "BlotterId",
                principalTable: "blotters",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_narratives_incidents_IncidentId",
                table: "narratives",
                column: "IncidentId",
                principalTable: "incidents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_narratives_vawc_VawcId",
                table: "narratives",
                column: "VawcId",
                principalTable: "vawc",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_narratives_blotters_BlotterId",
                table: "narratives");

            migrationBuilder.DropForeignKey(
                name: "FK_narratives_incidents_IncidentId",
                table: "narratives");

            migrationBuilder.DropForeignKey(
                name: "FK_narratives_vawc_VawcId",
                table: "narratives");

            migrationBuilder.DropIndex(
                name: "IX_narratives_BlotterId",
                table: "narratives");

            migrationBuilder.DropIndex(
                name: "IX_narratives_IncidentId",
                table: "narratives");

            migrationBuilder.DropIndex(
                name: "IX_narratives_VawcId",
                table: "narratives");

            migrationBuilder.DropColumn(
                name: "BlotterId",
                table: "narratives");

            migrationBuilder.DropColumn(
                name: "IncidentId",
                table: "narratives");

            migrationBuilder.DropColumn(
                name: "VawcId",
                table: "narratives");

            migrationBuilder.AddColumn<string>(
                name: "Narrative",
                table: "vawc",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ReportId",
                table: "narratives",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Narrative",
                table: "incidents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_narratives_ReportId",
                table: "narratives",
                column: "ReportId");

            migrationBuilder.AddForeignKey(
                name: "FK_narratives_blotters_ReportId",
                table: "narratives",
                column: "ReportId",
                principalTable: "blotters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
