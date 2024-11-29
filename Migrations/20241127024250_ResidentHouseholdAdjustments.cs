using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BmisApi.Migrations
{
    /// <inheritdoc />
    public partial class ResidentHouseholdAdjustments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HouseholdHead",
                table: "residents",
                newName: "IsHouseholdHead");

            migrationBuilder.AlterColumn<int>(
                name: "HeadId",
                table: "households",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "households",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsHouseholdHead",
                table: "residents",
                newName: "HouseholdHead");

            migrationBuilder.AlterColumn<int>(
                name: "HeadId",
                table: "households",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "households",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
