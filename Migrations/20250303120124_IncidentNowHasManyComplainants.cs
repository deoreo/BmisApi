using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BmisApi.Migrations
{
    /// <inheritdoc />
    public partial class IncidentNowHasManyComplainants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_incidents_residents_ComplainantId",
                table: "incidents");

            migrationBuilder.DropIndex(
                name: "IX_incidents_ComplainantId",
                table: "incidents");

            migrationBuilder.DropColumn(
                name: "ComplainantId",
                table: "incidents");

            migrationBuilder.CreateTable(
                name: "incidentcomplainants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IncidentId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ContactInfo = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("incidentcomplainant_pkeys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_incidentcomplainants_incidents_IncidentId",
                        column: x => x.IncidentId,
                        principalTable: "incidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_incidentcomplainants_IncidentId",
                table: "incidentcomplainants",
                column: "IncidentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "incidentcomplainants");

            migrationBuilder.AddColumn<int>(
                name: "ComplainantId",
                table: "incidents",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_incidents_ComplainantId",
                table: "incidents",
                column: "ComplainantId");

            migrationBuilder.AddForeignKey(
                name: "FK_incidents_residents_ComplainantId",
                table: "incidents",
                column: "ComplainantId",
                principalTable: "residents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
