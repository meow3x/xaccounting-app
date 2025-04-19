using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "accounttypes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    label = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_accounttypes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "employees",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    employeeid = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    firstname = table.Column<string>(type: "text", nullable: false),
                    lastname = table.Column<string>(type: "text", nullable: false),
                    middlename = table.Column<string>(type: "text", nullable: true),
                    address_street = table.Column<string>(type: "text", nullable: true),
                    address_city = table.Column<string>(type: "text", nullable: true),
                    address_province = table.Column<string>(type: "text", nullable: true),
                    address_landlinenumber = table.Column<string>(type: "text", nullable: true),
                    address_mobilenumber = table.Column<string>(type: "text", nullable: true),
                    tin = table.Column<string>(type: "text", nullable: true),
                    pagibigid = table.Column<string>(type: "text", nullable: true),
                    philhealthid = table.Column<string>(type: "text", nullable: true),
                    rate = table.Column<decimal>(type: "numeric", nullable: true),
                    salaryunit = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_employees", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "itemcategories",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_itemcategories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "paymentterms",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    label = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_paymentterms", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "unitofmeasurements",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_unitofmeasurements", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "accounts",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    accountid = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    accounttypeid = table.Column<int>(type: "integer", nullable: false),
                    debit = table.Column<decimal>(type: "numeric", nullable: false),
                    credit = table.Column<decimal>(type: "numeric", nullable: false),
                    balance = table.Column<decimal>(type: "numeric", nullable: false),
                    yearendbudget = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_accounts", x => x.id);
                    table.ForeignKey(
                        name: "fk_accounts_accounttypes_accounttypeid",
                        column: x => x.accounttypeid,
                        principalTable: "accounttypes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "customers",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    customerid = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    address_street = table.Column<string>(type: "text", nullable: true),
                    address_city = table.Column<string>(type: "text", nullable: true),
                    address_province = table.Column<string>(type: "text", nullable: true),
                    address_landlinenumber = table.Column<string>(type: "text", nullable: true),
                    address_mobilenumber = table.Column<string>(type: "text", nullable: true),
                    tin = table.Column<string>(type: "text", nullable: true),
                    discount = table.Column<decimal>(type: "numeric", nullable: true),
                    creditlimit = table.Column<decimal>(type: "numeric", nullable: true),
                    paymenttermid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_customers", x => x.id);
                    table.ForeignKey(
                        name: "fk_customers_paymentterms_paymenttermid",
                        column: x => x.paymenttermid,
                        principalTable: "paymentterms",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "suppliers",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    supplierid = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    address_street = table.Column<string>(type: "text", nullable: true),
                    address_city = table.Column<string>(type: "text", nullable: true),
                    address_province = table.Column<string>(type: "text", nullable: true),
                    address_landlinenumber = table.Column<string>(type: "text", nullable: true),
                    address_mobilenumber = table.Column<string>(type: "text", nullable: true),
                    tin = table.Column<string>(type: "text", nullable: true),
                    discount = table.Column<decimal>(type: "numeric", nullable: true),
                    creditlimit = table.Column<decimal>(type: "numeric", nullable: true),
                    paymenttermid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_suppliers", x => x.id);
                    table.ForeignKey(
                        name: "fk_suppliers_paymentterms_paymenttermid",
                        column: x => x.paymenttermid,
                        principalTable: "paymentterms",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "items",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    name = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    uomid = table.Column<int>(type: "integer", nullable: false),
                    unitprice = table.Column<decimal>(type: "numeric", nullable: false),
                    unitcost = table.Column<decimal>(type: "numeric", nullable: false),
                    categoryid = table.Column<int>(type: "integer", nullable: false),
                    wholesale = table.Column<decimal>(type: "numeric", nullable: false),
                    reorder = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_items", x => x.id);
                    table.ForeignKey(
                        name: "fk_items_itemcategories_categoryid",
                        column: x => x.categoryid,
                        principalTable: "itemcategories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_items_unitofmeasurements_uomid",
                        column: x => x.uomid,
                        principalTable: "unitofmeasurements",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "accounttypes",
                columns: new[] { "id", "label" },
                values: new object[,]
                {
                    { 1, "Cash" },
                    { 2, "Bank" },
                    { 3, "Trade Receivable" },
                    { 4, "Non-Trade Receivable" },
                    { 5, "Material" },
                    { 6, "Inventory" },
                    { 7, "Properties" },
                    { 8, "Accumulated Depreciation" },
                    { 9, "Other Current Assets" },
                    { 10, "Other Non-Current Assets" },
                    { 11, "Payable" },
                    { 12, "Other Current Liabilities" },
                    { 13, "Other Non-Current Liabilities" },
                    { 14, "Capital" },
                    { 15, "Sales" },
                    { 16, "Sales Discount" },
                    { 17, "Expenses" }
                });

            migrationBuilder.InsertData(
                table: "itemcategories",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "Electrical" },
                    { 2, "Office Equipment" },
                    { 3, "Tools and Equipment" },
                    { 4, "Motorpool" },
                    { 5, "Plumbing" },
                    { 6, "Finishing & Paintings" },
                    { 7, "Masonry" },
                    { 8, "Metals" },
                    { 9, "Woods & Plastics" },
                    { 10, "Consumables" },
                    { 11, "Doors & Windows" },
                    { 12, "Office Supplies" },
                    { 13, "Furniture & Fixture" },
                    { 14, "Services" }
                });

            migrationBuilder.InsertData(
                table: "paymentterms",
                columns: new[] { "id", "label" },
                values: new object[,]
                {
                    { 1, "Cash" },
                    { 2, "7 days" },
                    { 3, "30 Days" },
                    { 4, "120 Day" },
                    { 5, "COD" }
                });

            migrationBuilder.InsertData(
                table: "unitofmeasurements",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "bag" },
                    { 2, "bags" },
                    { 3, "bottle" },
                    { 4, "box" },
                    { 5, "can" },
                    { 6, "cu" },
                    { 7, "cubic meter" },
                    { 8, "dumptruck" },
                    { 9, "elf" },
                    { 10, "gallon" },
                    { 11, "half elf" },
                    { 12, "kilo" },
                    { 13, "liter" },
                    { 14, "meter" },
                    { 15, "pad" },
                    { 16, "pail" },
                    { 17, "pair" },
                    { 18, "piece" },
                    { 19, "roll" },
                    { 20, "sack" },
                    { 21, "set" },
                    { 22, "tin" },
                    { 23, "tube" },
                    { 24, "unit" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_accounts_accountid",
                table: "accounts",
                column: "accountid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_accounts_accounttypeid",
                table: "accounts",
                column: "accounttypeid");

            migrationBuilder.CreateIndex(
                name: "ix_customers_customerid",
                table: "customers",
                column: "customerid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_customers_paymenttermid",
                table: "customers",
                column: "paymenttermid");

            migrationBuilder.CreateIndex(
                name: "ix_employees_employeeid",
                table: "employees",
                column: "employeeid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_items_categoryid",
                table: "items",
                column: "categoryid");

            migrationBuilder.CreateIndex(
                name: "ix_items_code",
                table: "items",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_items_uomid",
                table: "items",
                column: "uomid");

            migrationBuilder.CreateIndex(
                name: "ix_suppliers_paymenttermid",
                table: "suppliers",
                column: "paymenttermid");

            migrationBuilder.CreateIndex(
                name: "ix_suppliers_supplierid",
                table: "suppliers",
                column: "supplierid",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "accounts");

            migrationBuilder.DropTable(
                name: "customers");

            migrationBuilder.DropTable(
                name: "employees");

            migrationBuilder.DropTable(
                name: "items");

            migrationBuilder.DropTable(
                name: "suppliers");

            migrationBuilder.DropTable(
                name: "accounttypes");

            migrationBuilder.DropTable(
                name: "itemcategories");

            migrationBuilder.DropTable(
                name: "unitofmeasurements");

            migrationBuilder.DropTable(
                name: "paymentterms");
        }
    }
}
