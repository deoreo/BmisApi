using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BmisApi.Migrations
{
    /// <inheritdoc />
    public partial class VawcInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "vawc",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    ComplainantId = table.Column<int>(type: "integer", nullable: false),
                    DefendantId = table.Column<int>(type: "integer", nullable: false),
                    Nature = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    Narrative = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("vawc_pkeys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_vawc_residents_ComplainantId",
                        column: x => x.ComplainantId,
                        principalTable: "residents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_vawc_residents_DefendantId",
                        column: x => x.DefendantId,
                        principalTable: "residents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_vawc_ComplainantId",
                table: "vawc",
                column: "ComplainantId");

            migrationBuilder.CreateIndex(
                name: "IX_vawc_DefendantId",
                table: "vawc",
                column: "DefendantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "vawc");
        }
    }
}
