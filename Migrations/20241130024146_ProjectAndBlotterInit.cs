using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BmisApi.Migrations
{
    /// <inheritdoc />
    public partial class ProjectAndBlotterInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Sex",
                table: "residents",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateTable(
                name: "blotters",
                columns: table => new
                {
                    BlotterId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    ComplainantId = table.Column<int>(type: "integer", nullable: false),
                    DefendantId = table.Column<int>(type: "integer", nullable: false),
                    Nature = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("blotter_pkeys", x => x.BlotterId);
                    table.ForeignKey(
                        name: "FK_blotters_residents_ComplainantId",
                        column: x => x.ComplainantId,
                        principalTable: "residents",
                        principalColumn: "ResidentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_blotters_residents_DefendantId",
                        column: x => x.DefendantId,
                        principalTable: "residents",
                        principalColumn: "ResidentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "projects",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProjectName = table.Column<string>(type: "text", nullable: false),
                    ImplementingAgency = table.Column<string>(type: "text", nullable: false),
                    StartingDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CompletionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpectedOutput = table.Column<string>(type: "text", nullable: false),
                    FundingSource = table.Column<string>(type: "text", nullable: false),
                    PS = table.Column<string>(type: "text", nullable: false),
                    MOE = table.Column<string>(type: "text", nullable: false),
                    CO = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("project_pkeys", x => x.ProjectId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_blotters_ComplainantId",
                table: "blotters",
                column: "ComplainantId");

            migrationBuilder.CreateIndex(
                name: "IX_blotters_DefendantId",
                table: "blotters",
                column: "DefendantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "blotters");

            migrationBuilder.DropTable(
                name: "projects");

            migrationBuilder.AlterColumn<int>(
                name: "Sex",
                table: "residents",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
