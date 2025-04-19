using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class Journal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "status",
                table: "purchaseorders",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<string>(
                name: "referencenumber1",
                table: "journallines",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "referencenumber1",
                table: "journalentries",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "purchaseorders");

            migrationBuilder.DropColumn(
                name: "referencenumber1",
                table: "journallines");

            migrationBuilder.DropColumn(
                name: "referencenumber1",
                table: "journalentries");
        }
    }
}
