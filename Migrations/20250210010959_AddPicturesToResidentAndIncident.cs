using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BmisApi.Migrations
{
    /// <inheritdoc />
    public partial class AddPicturesToResidentAndIncident : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PicturePath",
                table: "residents",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PicturePath",
                table: "incidents",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PicturePath",
                table: "residents");

            migrationBuilder.DropColumn(
                name: "PicturePath",
                table: "incidents");
        }
    }
}
