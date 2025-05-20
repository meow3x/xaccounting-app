using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "journallines",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<DateOnly>(
                name: "duedate",
                table: "accountspayable",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "referencenumber",
                table: "accountspayable",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "terms",
                table: "accountspayable",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "duedate",
                table: "accountspayable");

            migrationBuilder.DropColumn(
                name: "referencenumber",
                table: "accountspayable");

            migrationBuilder.DropColumn(
                name: "terms",
                table: "accountspayable");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "journallines",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
