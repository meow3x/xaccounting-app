using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class Payment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_journalentries_accountspayable_accountspayableid",
                table: "journalentries");

            migrationBuilder.DropIndex(
                name: "ix_journalentries_accountspayableid",
                table: "journalentries");

            migrationBuilder.DropColumn(
                name: "accountspayableid",
                table: "journalentries");

            migrationBuilder.AddColumn<int>(
                name: "journalentryid",
                table: "accountspayable",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    vouchernumber = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'10000', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    journalentryid = table.Column<int>(type: "integer", nullable: false),
                    referencenumber = table.Column<string>(type: "text", nullable: false),
                    payeeid = table.Column<int>(type: "integer", nullable: false),
                    ischeque = table.Column<bool>(type: "boolean", nullable: false),
                    chequestatus = table.Column<int>(type: "integer", nullable: true),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updatedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_payments", x => x.id);
                    table.ForeignKey(
                        name: "fk_payments_journalentries_journalentryid",
                        column: x => x.journalentryid,
                        principalTable: "journalentries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_payments_suppliers_payeeid",
                        column: x => x.payeeid,
                        principalTable: "suppliers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_accountspayable_journalentryid",
                table: "accountspayable",
                column: "journalentryid");

            migrationBuilder.CreateIndex(
                name: "ix_payments_journalentryid",
                table: "payments",
                column: "journalentryid");

            migrationBuilder.CreateIndex(
                name: "ix_payments_payeeid",
                table: "payments",
                column: "payeeid");

            migrationBuilder.CreateIndex(
                name: "ix_payments_vouchernumber",
                table: "payments",
                column: "vouchernumber",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_accountspayable_journalentries_journalentryid",
                table: "accountspayable",
                column: "journalentryid",
                principalTable: "journalentries",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_accountspayable_journalentries_journalentryid",
                table: "accountspayable");

            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.DropIndex(
                name: "ix_accountspayable_journalentryid",
                table: "accountspayable");

            migrationBuilder.DropColumn(
                name: "journalentryid",
                table: "accountspayable");

            migrationBuilder.AddColumn<int>(
                name: "accountspayableid",
                table: "journalentries",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_journalentries_accountspayableid",
                table: "journalentries",
                column: "accountspayableid",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_journalentries_accountspayable_accountspayableid",
                table: "journalentries",
                column: "accountspayableid",
                principalTable: "accountspayable",
                principalColumn: "id");
        }
    }
}
