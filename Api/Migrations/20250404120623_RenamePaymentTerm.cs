using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class RenamePaymentTerm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_customers_paymentterms_termid",
                table: "customers");

            migrationBuilder.DropForeignKey(
                name: "fk_suppliers_paymentterms_termid",
                table: "suppliers");

            migrationBuilder.RenameColumn(
                name: "termid",
                table: "suppliers",
                newName: "paymenttermid");

            migrationBuilder.RenameIndex(
                name: "ix_suppliers_termid",
                table: "suppliers",
                newName: "ix_suppliers_paymenttermid");

            migrationBuilder.RenameColumn(
                name: "termid",
                table: "customers",
                newName: "paymenttermid");

            migrationBuilder.RenameIndex(
                name: "ix_customers_termid",
                table: "customers",
                newName: "ix_customers_paymenttermid");

            migrationBuilder.AddForeignKey(
                name: "fk_customers_paymentterms_paymenttermid",
                table: "customers",
                column: "paymenttermid",
                principalTable: "paymentterms",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_suppliers_paymentterms_paymenttermid",
                table: "suppliers",
                column: "paymenttermid",
                principalTable: "paymentterms",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_customers_paymentterms_paymenttermid",
                table: "customers");

            migrationBuilder.DropForeignKey(
                name: "fk_suppliers_paymentterms_paymenttermid",
                table: "suppliers");

            migrationBuilder.RenameColumn(
                name: "paymenttermid",
                table: "suppliers",
                newName: "termid");

            migrationBuilder.RenameIndex(
                name: "ix_suppliers_paymenttermid",
                table: "suppliers",
                newName: "ix_suppliers_termid");

            migrationBuilder.RenameColumn(
                name: "paymenttermid",
                table: "customers",
                newName: "termid");

            migrationBuilder.RenameIndex(
                name: "ix_customers_paymenttermid",
                table: "customers",
                newName: "ix_customers_termid");

            migrationBuilder.AddForeignKey(
                name: "fk_customers_paymentterms_termid",
                table: "customers",
                column: "termid",
                principalTable: "paymentterms",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_suppliers_paymentterms_termid",
                table: "suppliers",
                column: "termid",
                principalTable: "paymentterms",
                principalColumn: "id");
        }
    }
}
