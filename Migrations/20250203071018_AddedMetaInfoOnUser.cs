using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BmisApi.Migrations
{
    /// <inheritdoc />
    public partial class AddedMetaInfoOnUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:blotter_status", "first_confrontation,second_confrontation,third_confrontation,settled,justice")
                .Annotation("Npgsql:Enum:sex", "prefer_not_to_say,male,female")
                .Annotation("Npgsql:Enum:vawc_status", "settled,unsettled")
                .OldAnnotation("Npgsql:Enum:sex", "prefer_not_to_say,male,female");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserName",
                table: "AspNetUsers",
                column: "UserName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastUpdatedAt",
                table: "AspNetUsers");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:sex", "prefer_not_to_say,male,female")
                .OldAnnotation("Npgsql:Enum:blotter_status", "first_confrontation,second_confrontation,third_confrontation,settled,justice")
                .OldAnnotation("Npgsql:Enum:sex", "prefer_not_to_say,male,female")
                .OldAnnotation("Npgsql:Enum:vawc_status", "settled,unsettled");
        }
    }
}
