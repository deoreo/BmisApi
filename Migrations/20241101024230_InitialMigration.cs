using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BmisApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "households",
                columns: table => new
                {
                    HouseholdId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HeadId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("household_pkeys", x => x.HouseholdId);
                });

            migrationBuilder.CreateTable(
                name: "residents",
                columns: table => new
                {
                    ResidentId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    Gender = table.Column<int>(type: "integer", nullable: false),
                    Birthday = table.Column<DateOnly>(type: "date", nullable: false),
                    Occupation = table.Column<string>(type: "text", nullable: true),
                    RegisteredVoter = table.Column<bool>(type: "boolean", nullable: false),
                    HouseholdId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("resident_pkeys", x => x.ResidentId);
                    table.ForeignKey(
                        name: "FK_residents_households_HouseholdId",
                        column: x => x.HouseholdId,
                        principalTable: "households",
                        principalColumn: "HouseholdId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_households_HeadId",
                table: "households",
                column: "HeadId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_residents_HouseholdId",
                table: "residents",
                column: "HouseholdId");

            migrationBuilder.AddForeignKey(
                name: "FK_households_residents_HeadId",
                table: "households",
                column: "HeadId",
                principalTable: "residents",
                principalColumn: "ResidentId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_households_residents_HeadId",
                table: "households");

            migrationBuilder.DropTable(
                name: "residents");

            migrationBuilder.DropTable(
                name: "households");
        }
    }
}
