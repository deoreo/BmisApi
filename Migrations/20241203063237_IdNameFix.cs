using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BmisApi.Migrations
{
    /// <inheritdoc />
    public partial class IdNameFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ResidentId",
                table: "residents",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "HouseholdId",
                table: "households",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "residents",
                newName: "ResidentId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "households",
                newName: "HouseholdId");
        }
    }
}
