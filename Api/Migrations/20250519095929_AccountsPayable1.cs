using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class AccountsPayable1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "costcenterid",
                table: "journallines",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "accountspayableid",
                table: "journalentries",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "accountspayable",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    vouchernumber = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'1000', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    supplierid = table.Column<int>(type: "integer", nullable: false),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updatedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_accountspayable", x => x.id);
                    table.ForeignKey(
                        name: "fk_accountspayable_suppliers_supplierid",
                        column: x => x.supplierid,
                        principalTable: "suppliers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "costcenters",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updatedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_costcenters", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "costcenters",
                columns: new[] { "id", "createdat", "name", "updatedat" },
                values: new object[] { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Local), "Default Cost Center", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.CreateIndex(
                name: "ix_journallines_costcenterid",
                table: "journallines",
                column: "costcenterid");

            migrationBuilder.CreateIndex(
                name: "ix_journalentries_accountspayableid",
                table: "journalentries",
                column: "accountspayableid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_accountspayable_supplierid",
                table: "accountspayable",
                column: "supplierid");

            migrationBuilder.CreateIndex(
                name: "ix_accountspayable_vouchernumber",
                table: "accountspayable",
                column: "vouchernumber",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_journalentries_accountspayable_accountspayableid",
                table: "journalentries",
                column: "accountspayableid",
                principalTable: "accountspayable",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_journallines_costcenters_costcenterid",
                table: "journallines",
                column: "costcenterid",
                principalTable: "costcenters",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_journalentries_accountspayable_accountspayableid",
                table: "journalentries");

            migrationBuilder.DropForeignKey(
                name: "fk_journallines_costcenters_costcenterid",
                table: "journallines");

            migrationBuilder.DropTable(
                name: "accountspayable");

            migrationBuilder.DropTable(
                name: "costcenters");

            migrationBuilder.DropIndex(
                name: "ix_journallines_costcenterid",
                table: "journallines");

            migrationBuilder.DropIndex(
                name: "ix_journalentries_accountspayableid",
                table: "journalentries");

            migrationBuilder.DropColumn(
                name: "costcenterid",
                table: "journallines");

            migrationBuilder.DropColumn(
                name: "accountspayableid",
                table: "journalentries");
        }
    }
}
