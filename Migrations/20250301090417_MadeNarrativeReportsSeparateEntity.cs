using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BmisApi.Migrations
{
    /// <inheritdoc />
    public partial class MadeNarrativeReportsSeparateEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Narrative",
                table: "blotters");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:sex", "prefer_not_to_say,male,female")
                .OldAnnotation("Npgsql:Enum:blotter_status", "first_confrontation,second_confrontation,third_confrontation,settled,justice")
                .OldAnnotation("Npgsql:Enum:sex", "prefer_not_to_say,male,female")
                .OldAnnotation("Npgsql:Enum:vawc_status", "settled,unsettled");

            migrationBuilder.CreateTable(
                name: "narratives",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReportId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    NarrativeReport = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_narratives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_narratives_blotters_ReportId",
                        column: x => x.ReportId,
                        principalTable: "blotters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_narratives_ReportId",
                table: "narratives",
                column: "ReportId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "narratives");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:blotter_status", "first_confrontation,second_confrontation,third_confrontation,settled,justice")
                .Annotation("Npgsql:Enum:sex", "prefer_not_to_say,male,female")
                .Annotation("Npgsql:Enum:vawc_status", "settled,unsettled")
                .OldAnnotation("Npgsql:Enum:sex", "prefer_not_to_say,male,female");

            migrationBuilder.AddColumn<string>(
                name: "Narrative",
                table: "blotters",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
