using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class InvoiceJournal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "instalmentbalance",
                table: "invoices",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "journalentryid",
                table: "invoices",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "collectionpayment",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sequencenumber = table.Column<int>(type: "integer", nullable: false),
                    receiptnumber = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'10100', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    referencenumber = table.Column<string>(type: "text", nullable: true),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    journalentryid = table.Column<int>(type: "integer", nullable: false),
                    invoiceid = table.Column<int>(type: "integer", nullable: true),
                    createdat = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    updatedat = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_collectionpayment", x => x.id);
                    table.ForeignKey(
                        name: "fk_collectionpayment_invoices_invoiceid",
                        column: x => x.invoiceid,
                        principalTable: "invoices",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_collectionpayment_journalentries_journalentryid",
                        column: x => x.journalentryid,
                        principalTable: "journalentries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_invoices_journalentryid",
                table: "invoices",
                column: "journalentryid");

            migrationBuilder.CreateIndex(
                name: "ix_collectionpayment_invoiceid",
                table: "collectionpayment",
                column: "invoiceid");

            migrationBuilder.CreateIndex(
                name: "ix_collectionpayment_journalentryid",
                table: "collectionpayment",
                column: "journalentryid");

            migrationBuilder.CreateIndex(
                name: "ix_collectionpayment_receiptnumber",
                table: "collectionpayment",
                column: "receiptnumber",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_invoices_journalentries_journalentryid",
                table: "invoices",
                column: "journalentryid",
                principalTable: "journalentries",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_invoices_journalentries_journalentryid",
                table: "invoices");

            migrationBuilder.DropTable(
                name: "collectionpayment");

            migrationBuilder.DropIndex(
                name: "ix_invoices_journalentryid",
                table: "invoices");

            migrationBuilder.DropColumn(
                name: "instalmentbalance",
                table: "invoices");

            migrationBuilder.DropColumn(
                name: "journalentryid",
                table: "invoices");
        }
    }
}
