using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BmisApi.Migrations
{
    /// <inheritdoc />
    public partial class RefactorResidentFullName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "residents",
                newName: "LastName");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "residents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "residents",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Suffix",
                table: "residents",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "residents");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "residents");

            migrationBuilder.DropColumn(
                name: "Suffix",
                table: "residents");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "residents",
                newName: "FullName");
        }
    }
}
