using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BmisApi.Migrations
{
    /// <inheritdoc />
    public partial class AddedJustice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JusticeId",
                table: "narratives",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "justice",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CaseId = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Complainant = table.Column<string>(type: "text", nullable: false),
                    ContactInfo = table.Column<string>(type: "text", nullable: true),
                    DefendantId = table.Column<int>(type: "integer", nullable: false),
                    Nature = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("justice_pkeys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_justice_residents_DefendantId",
                        column: x => x.DefendantId,
                        principalTable: "residents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_narratives_JusticeId",
                table: "narratives",
                column: "JusticeId");

            migrationBuilder.CreateIndex(
                name: "IX_justice_DefendantId",
                table: "justice",
                column: "DefendantId");

            migrationBuilder.AddForeignKey(
                name: "FK_narratives_justice_JusticeId",
                table: "narratives",
                column: "JusticeId",
                principalTable: "justice",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_narratives_justice_JusticeId",
                table: "narratives");

            migrationBuilder.DropTable(
                name: "justice");

            migrationBuilder.DropIndex(
                name: "IX_narratives_JusticeId",
                table: "narratives");

            migrationBuilder.DropColumn(
                name: "JusticeId",
                table: "narratives");
        }
    }
}
