using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BmisApi.Migrations
{
    /// <inheritdoc />
    public partial class BrgyProjectDecimalFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BrgyProjectId",
                table: "projects",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "BlotterId",
                table: "blotters",
                newName: "Id");

            //migrationBuilder.Sql("ALTER TABLE projects ALTER COLUMN \"PS\" TYPE numeric USING \"PS\"::numeric;");
            //migrationBuilder.Sql("ALTER TABLE projects ALTER COLUMN \"MOE\" TYPE numeric USING \"MOE\"::numeric;");
            //migrationBuilder.Sql("ALTER TABLE projects ALTER COLUMN \"CO\" TYPE numeric USING \"CO\"::numeric;");

            migrationBuilder.AlterColumn<decimal>(
                name: "PS",
                table: "projects",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<decimal>(
                name: "MOE",
                table: "projects",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "CompletionDate",
                table: "projects",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<decimal>(
                name: "CO",
                table: "projects",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "projects",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Total",
                table: "projects");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "projects",
                newName: "BrgyProjectId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "blotters",
                newName: "BlotterId");

            migrationBuilder.AlterColumn<string>(
                name: "PS",
                table: "projects",
                type: "text",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "MOE",
                table: "projects",
                type: "text",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CompletionDate",
                table: "projects",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "CO",
                table: "projects",
                type: "text",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");
        }
    }
}
