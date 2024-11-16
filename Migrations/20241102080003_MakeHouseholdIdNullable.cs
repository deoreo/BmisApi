using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BmisApi.Migrations
{
    /// <inheritdoc />
    public partial class MakeHouseholdIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_residents_households_HouseholdId",
                table: "residents");

            migrationBuilder.AlterColumn<int>(
                name: "HouseholdId",
                table: "residents",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_residents_households_HouseholdId",
                table: "residents",
                column: "HouseholdId",
                principalTable: "households",
                principalColumn: "HouseholdId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_residents_households_HouseholdId",
                table: "residents");

            migrationBuilder.AlterColumn<int>(
                name: "HouseholdId",
                table: "residents",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_residents_households_HouseholdId",
                table: "residents",
                column: "HouseholdId",
                principalTable: "households",
                principalColumn: "HouseholdId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
