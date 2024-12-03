using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BmisApi.Migrations
{
    /// <inheritdoc />
    public partial class SexEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProjectName",
                table: "projects",
                newName: "BrgyProjectName");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "projects",
                newName: "BrgyProjectId");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:sex", "prefer_not_to_say,male,female");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BrgyProjectName",
                table: "projects",
                newName: "ProjectName");

            migrationBuilder.RenameColumn(
                name: "BrgyProjectId",
                table: "projects",
                newName: "ProjectId");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:Enum:sex", "prefer_not_to_say,male,female");
        }
    }
}
