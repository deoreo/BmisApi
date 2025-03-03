using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BmisApi.Migrations
{
    /// <inheritdoc />
    public partial class ChangedVawcBlotterComplainantToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_blotters_residents_ComplainantId",
                table: "blotters");

            migrationBuilder.DropForeignKey(
                name: "FK_vawc_residents_ComplainantId",
                table: "vawc");

            migrationBuilder.DropIndex(
                name: "IX_vawc_ComplainantId",
                table: "vawc");

            migrationBuilder.DropIndex(
                name: "IX_blotters_ComplainantId",
                table: "blotters");

            migrationBuilder.DropColumn(
                name: "ComplainantId",
                table: "vawc");

            migrationBuilder.DropColumn(
                name: "ComplainantId",
                table: "blotters");

            migrationBuilder.AddColumn<string>(
                name: "Complainant",
                table: "vawc",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactInfo",
                table: "vawc",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Complainant",
                table: "blotters",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactInfo",
                table: "blotters",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Complainant",
                table: "vawc");

            migrationBuilder.DropColumn(
                name: "ContactInfo",
                table: "vawc");

            migrationBuilder.DropColumn(
                name: "Complainant",
                table: "blotters");

            migrationBuilder.DropColumn(
                name: "ContactInfo",
                table: "blotters");

            migrationBuilder.AddColumn<int>(
                name: "ComplainantId",
                table: "vawc",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ComplainantId",
                table: "blotters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_vawc_ComplainantId",
                table: "vawc",
                column: "ComplainantId");

            migrationBuilder.CreateIndex(
                name: "IX_blotters_ComplainantId",
                table: "blotters",
                column: "ComplainantId");

            migrationBuilder.AddForeignKey(
                name: "FK_blotters_residents_ComplainantId",
                table: "blotters",
                column: "ComplainantId",
                principalTable: "residents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_vawc_residents_ComplainantId",
                table: "vawc",
                column: "ComplainantId",
                principalTable: "residents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
