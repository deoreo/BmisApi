using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BmisApi.Migrations
{
    /// <inheritdoc />
    public partial class FixHouseholdResidentConnection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_households_residents_HeadId",
                table: "households");

            migrationBuilder.DropIndex(
                name: "IX_households_HeadId",
                table: "households");

            migrationBuilder.AddColumn<bool>(
                name: "HouseholdHead",
                table: "residents",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "HeadResidentId",
                table: "households",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_households_HeadResidentId",
                table: "households",
                column: "HeadResidentId");

            migrationBuilder.AddForeignKey(
                name: "FK_households_residents_HeadResidentId",
                table: "households",
                column: "HeadResidentId",
                principalTable: "residents",
                principalColumn: "ResidentId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_households_residents_HeadResidentId",
                table: "households");

            migrationBuilder.DropIndex(
                name: "IX_households_HeadResidentId",
                table: "households");

            migrationBuilder.DropColumn(
                name: "HouseholdHead",
                table: "residents");

            migrationBuilder.DropColumn(
                name: "HeadResidentId",
                table: "households");

            migrationBuilder.CreateIndex(
                name: "IX_households_HeadId",
                table: "households",
                column: "HeadId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_households_residents_HeadId",
                table: "households",
                column: "HeadId",
                principalTable: "residents",
                principalColumn: "ResidentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
