using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BmisApi.Migrations
{
    /// <inheritdoc />
    public partial class FixHouseholdResidentConnection2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_households_residents_HeadResidentId",
                table: "households");

            migrationBuilder.DropIndex(
                name: "IX_households_HeadResidentId",
                table: "households");

            migrationBuilder.DropColumn(
                name: "HeadResidentId",
                table: "households");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
