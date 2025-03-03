using System;
using System.Collections.Generic;
using BmisApi.Models;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BmisApi.Migrations
{
    /// <inheritdoc />
    public partial class ChangedIncidentComplainantsToJsonb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "incidentcomplainants");

            migrationBuilder.AddColumn<List<ComplainantInfo>>(
                name: "Complainants",
                table: "incidents",
                type: "jsonb",
                nullable: false,
                defaultValue: new List<ComplainantInfo>());
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Complainants",
                table: "incidents");

            migrationBuilder.CreateTable(
                name: "incidentcomplainants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ContactInfo = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IncidentId = table.Column<int>(type: "integer", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false)
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
    }
}
