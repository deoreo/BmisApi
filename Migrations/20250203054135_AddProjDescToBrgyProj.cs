using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BmisApi.Migrations
{
    /// <inheritdoc />
    public partial class AddProjDescToBrgyProj : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProjectDescription",
                table: "projects",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectDescription",
                table: "projects");
        }
    }
}
