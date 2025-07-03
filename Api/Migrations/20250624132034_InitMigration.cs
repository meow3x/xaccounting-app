using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class InitMigration : Migration
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
                    name = table.Column<string>(type: "text", nullable: true),
                    createdat = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    updatedat = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_accounttypes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "costcenters",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    createdat = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    updatedat = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_costcenters", x => x.id);
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
                    salaryunit = table.Column<int>(type: "integer", nullable: false),
                    createdat = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    updatedat = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
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
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    createdat = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    updatedat = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_itemcategories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "journaltypes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    createdat = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    updatedat = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_journaltypes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "paymentterms",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    label = table.Column<string>(type: "text", nullable: false),
                    createdat = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    updatedat = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_paymentterms", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "projects",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    number = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    createdat = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    updatedat = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_projects", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "unitofmeasurements",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    createdat = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    updatedat = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
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
                    yearendbudget = table.Column<decimal>(type: "numeric", nullable: false),
                    createdat = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    updatedat = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
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
                name: "journalentries",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    journaltypeid = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    referencenumber1 = table.Column<string>(type: "text", nullable: true),
                    createdat = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    updatedat = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_journalentries", x => x.id);
                    table.ForeignKey(
                        name: "fk_journalentries_journaltypes_journaltypeid",
                        column: x => x.journaltypeid,
                        principalTable: "journaltypes",
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
                    paymenttermid = table.Column<int>(type: "integer", nullable: true),
                    createdat = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    updatedat = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
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
                    paymenttermid = table.Column<int>(type: "integer", nullable: true),
                    createdat = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    updatedat = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
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
                    reorder = table.Column<decimal>(type: "numeric", nullable: false),
                    createdat = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    updatedat = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "journallines",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    journalentryid = table.Column<int>(type: "integer", nullable: false),
                    linenumber = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    accountid = table.Column<int>(type: "integer", nullable: false),
                    debit = table.Column<decimal>(type: "numeric", nullable: true),
                    credit = table.Column<decimal>(type: "numeric", nullable: true),
                    costcenterid = table.Column<int>(type: "integer", nullable: true),
                    referencenumber1 = table.Column<string>(type: "text", nullable: true),
                    createdat = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    updatedat = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_journallines", x => x.id);
                    table.ForeignKey(
                        name: "fk_journallines_accounts_accountid",
                        column: x => x.accountid,
                        principalTable: "accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_journallines_costcenters_costcenterid",
                        column: x => x.costcenterid,
                        principalTable: "costcenters",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_journallines_journalentries_journalentryid",
                        column: x => x.journalentryid,
                        principalTable: "journalentries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "invoices",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    number = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'1000', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    paymentmethod = table.Column<int>(type: "integer", nullable: false),
                    customerid = table.Column<int>(type: "integer", nullable: false),
                    vatableamount = table.Column<decimal>(type: "numeric", nullable: false),
                    vatamount = table.Column<decimal>(type: "numeric", nullable: false),
                    discounted = table.Column<decimal>(type: "numeric", nullable: false),
                    netamount = table.Column<decimal>(type: "numeric", nullable: false),
                    createdat = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    updatedat = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_invoices", x => x.id);
                    table.ForeignKey(
                        name: "fk_invoices_customers_customerid",
                        column: x => x.customerid,
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "accountspayable",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    vouchernumber = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'1000', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    referencenumber = table.Column<string>(type: "text", nullable: false),
                    supplierid = table.Column<int>(type: "integer", nullable: false),
                    journalentryid = table.Column<int>(type: "integer", nullable: false),
                    duedate = table.Column<LocalDate>(type: "date", nullable: true),
                    terms = table.Column<int>(type: "integer", nullable: true),
                    totalamount = table.Column<decimal>(type: "numeric", nullable: false),
                    balance = table.Column<decimal>(type: "numeric", nullable: false),
                    createdat = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    updatedat = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_accountspayable", x => x.id);
                    table.ForeignKey(
                        name: "fk_accountspayable_journalentries_journalentryid",
                        column: x => x.journalentryid,
                        principalTable: "journalentries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_accountspayable_suppliers_supplierid",
                        column: x => x.supplierid,
                        principalTable: "suppliers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                    apvnumber = table.Column<int>(type: "integer", nullable: true),
                    ischeque = table.Column<bool>(type: "boolean", nullable: false),
                    chequestatus = table.Column<int>(type: "integer", nullable: true),
                    totalamount = table.Column<decimal>(type: "numeric", nullable: false),
                    createdat = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    updatedat = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "purchaseorders",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    number = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'1000', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    rrnumber = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    requisitionnumber = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    deliverydate = table.Column<LocalDate>(type: "date", nullable: false),
                    projectid = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    supplierid = table.Column<int>(type: "integer", nullable: false),
                    vatableamount = table.Column<decimal>(type: "numeric", nullable: false),
                    vatamount = table.Column<decimal>(type: "numeric", nullable: false),
                    discounted = table.Column<decimal>(type: "numeric", nullable: false),
                    netamount = table.Column<decimal>(type: "numeric", nullable: false),
                    status = table.Column<byte>(type: "smallint", nullable: false),
                    closedat = table.Column<Instant>(type: "timestamp with time zone", nullable: true),
                    debitto = table.Column<int>(type: "integer", nullable: false),
                    creditto = table.Column<int>(type: "integer", nullable: false),
                    createdat = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    updatedat = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_purchaseorders", x => x.id);
                    table.ForeignKey(
                        name: "fk_purchaseorders_accounts_creditto",
                        column: x => x.creditto,
                        principalTable: "accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_purchaseorders_accounts_debitto",
                        column: x => x.debitto,
                        principalTable: "accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_purchaseorders_projects_projectid",
                        column: x => x.projectid,
                        principalTable: "projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_purchaseorders_suppliers_supplierid",
                        column: x => x.supplierid,
                        principalTable: "suppliers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "inventory",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    itemid = table.Column<int>(type: "integer", nullable: false),
                    stock = table.Column<int>(type: "integer", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                    createdat = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    updatedat = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_inventory", x => x.id);
                    table.ForeignKey(
                        name: "fk_inventory_items_itemid",
                        column: x => x.itemid,
                        principalTable: "items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "invoicelineitems",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    originalitemid = table.Column<int>(type: "integer", nullable: false),
                    itemsnapshot_code = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    itemsnapshot_name = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    itemsnapshot_unitofmeasurementid = table.Column<int>(type: "integer", nullable: false),
                    itemsnapshot_unitofmeasurement = table.Column<string>(type: "text", nullable: false),
                    itemsnapshot_unitprice = table.Column<decimal>(type: "numeric", nullable: false),
                    itemsnapshot_unitcost = table.Column<decimal>(type: "numeric", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    discount = table.Column<decimal>(type: "numeric", nullable: false),
                    linetotal = table.Column<decimal>(type: "numeric", nullable: false),
                    invoiceid = table.Column<int>(type: "integer", nullable: true),
                    createdat = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    updatedat = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_invoicelineitems", x => x.id);
                    table.ForeignKey(
                        name: "fk_invoicelineitems_invoices_invoiceid",
                        column: x => x.invoiceid,
                        principalTable: "invoices",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_invoicelineitems_items_originalitemid",
                        column: x => x.originalitemid,
                        principalTable: "items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "purchaseorderlineitems",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    purchaseorderid = table.Column<int>(type: "integer", nullable: false),
                    originalitemid = table.Column<int>(type: "integer", nullable: false),
                    itemsnapshot_code = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    itemsnapshot_name = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    itemsnapshot_unitofmeasurementid = table.Column<int>(type: "integer", nullable: false),
                    itemsnapshot_unitofmeasurement = table.Column<string>(type: "text", nullable: false),
                    itemsnapshot_unitprice = table.Column<decimal>(type: "numeric", nullable: false),
                    itemsnapshot_unitcost = table.Column<decimal>(type: "numeric", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    discount = table.Column<decimal>(type: "numeric", nullable: false),
                    linetotal = table.Column<decimal>(type: "numeric", nullable: false),
                    createdat = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    updatedat = table.Column<Instant>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_purchaseorderlineitems", x => x.id);
                    table.ForeignKey(
                        name: "fk_purchaseorderlineitems_items_originalitemid",
                        column: x => x.originalitemid,
                        principalTable: "items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_purchaseorderlineitems_purchaseorders_purchaseorderid",
                        column: x => x.purchaseorderid,
                        principalTable: "purchaseorders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "inventorylogs",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    inventoryid = table.Column<int>(type: "integer", nullable: false),
                    lineitemid = table.Column<int>(type: "integer", nullable: true),
                    timestamp = table.Column<Instant>(type: "timestamp with time zone", nullable: false),
                    stockbefore = table.Column<int>(type: "integer", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    stockafter = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_inventorylogs", x => x.id);
                    table.ForeignKey(
                        name: "fk_inventorylogs_inventory_inventoryid",
                        column: x => x.inventoryid,
                        principalTable: "inventory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_inventorylogs_purchaseorderlineitems_lineitemid",
                        column: x => x.lineitemid,
                        principalTable: "purchaseorderlineitems",
                        principalColumn: "id");
                });

            migrationBuilder.InsertData(
                table: "accounttypes",
                columns: new[] { "id", "createdat", "name", "updatedat" },
                values: new object[,]
                {
                    { 1, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "Cash", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 2, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "Bank", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 3, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "Trade Receivable", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 4, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "Non-Trade Receivable", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 5, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "Material", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 6, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "Inventory", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 7, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "Properties", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 8, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "Accumulated Depreciation", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 9, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "Other Current Assets", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 10, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "Other Non-Current Assets", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 11, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "Payable", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 12, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "Other Current Liabilities", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 13, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "Other Non-Current Liabilities", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 14, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "Capital", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 15, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "Sales", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 16, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "Sales Discount", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 17, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "Expenses", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) }
                });

            migrationBuilder.InsertData(
                table: "costcenters",
                columns: new[] { "id", "createdat", "name", "updatedat" },
                values: new object[] { 1, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "Default Cost Center", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) });

            migrationBuilder.InsertData(
                table: "customers",
                columns: new[] { "id", "createdat", "creditlimit", "customerid", "discount", "name", "paymenttermid", "tin", "updatedat", "address_city", "address_landlinenumber", "address_mobilenumber", "address_province", "address_street" },
                values: new object[] { 1, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), null, "4000100", null, "Cash", null, null, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "", "", "", "", "" });

            migrationBuilder.InsertData(
                table: "itemcategories",
                columns: new[] { "id", "createdat", "name", "updatedat" },
                values: new object[,]
                {
                    { 1, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "Electrical", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 2, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "Office Equipment", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 3, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "Tools and Equipment", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 4, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "Motorpool", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 5, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "Plumbing", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 6, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "Finishing & Paintings", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 7, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "Masonry", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 8, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "Metals", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 9, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "Woods & Plastics", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 10, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "Consumables", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 11, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "Doors & Windows", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 12, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "Office Supplies", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 13, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "Furniture & Fixture", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 14, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "Services", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) }
                });

            migrationBuilder.InsertData(
                table: "journaltypes",
                columns: new[] { "id", "createdat", "name", "updatedat" },
                values: new object[,]
                {
                    { 1, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "General", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 2, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "Disbursements", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 3, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "Payables", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 4, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "Receipts", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 5, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "Purchases", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 6, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "Sales", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 7, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "Production", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 8, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "Cancelled", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) }
                });

            migrationBuilder.InsertData(
                table: "paymentterms",
                columns: new[] { "id", "createdat", "label", "updatedat" },
                values: new object[,]
                {
                    { 1, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "Cash", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 2, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "7 days", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 3, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "30 Days", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 4, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "120 Day", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 5, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "COD", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) }
                });

            migrationBuilder.InsertData(
                table: "projects",
                columns: new[] { "id", "createdat", "name", "number", "updatedat" },
                values: new object[] { 1, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "Default Project", "DEFP001", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) });

            migrationBuilder.InsertData(
                table: "unitofmeasurements",
                columns: new[] { "id", "createdat", "name", "updatedat" },
                values: new object[,]
                {
                    { 1, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "bag", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 2, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "bags", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 3, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "bottle", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 4, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "box", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 5, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "can", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 6, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "cu", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 7, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "cubic meter", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 8, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "dumptruck", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 9, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "elf", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 10, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "gallon", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 11, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "half elf", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 12, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "kilo", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 13, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "liter", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 14, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "meter", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 15, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "pad", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 16, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "pail", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 17, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "pair", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 18, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "piece", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 19, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "roll", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 20, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "sack", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 21, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "set", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 22, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "tin", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 23, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "tube", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) },
                    { 24, NodaTime.Instant.FromUnixTimeTicks(17356896000000000L), "unit", NodaTime.Instant.FromUnixTimeTicks(17356896000000000L) }
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
                name: "ix_accountspayable_journalentryid",
                table: "accountspayable",
                column: "journalentryid");

            migrationBuilder.CreateIndex(
                name: "ix_accountspayable_supplierid",
                table: "accountspayable",
                column: "supplierid");

            migrationBuilder.CreateIndex(
                name: "ix_accountspayable_vouchernumber",
                table: "accountspayable",
                column: "vouchernumber",
                unique: true);

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
                name: "ix_inventory_itemid",
                table: "inventory",
                column: "itemid");

            migrationBuilder.CreateIndex(
                name: "ix_inventorylogs_inventoryid",
                table: "inventorylogs",
                column: "inventoryid");

            migrationBuilder.CreateIndex(
                name: "ix_inventorylogs_lineitemid",
                table: "inventorylogs",
                column: "lineitemid");

            migrationBuilder.CreateIndex(
                name: "ix_invoicelineitems_invoiceid",
                table: "invoicelineitems",
                column: "invoiceid");

            migrationBuilder.CreateIndex(
                name: "ix_invoicelineitems_originalitemid",
                table: "invoicelineitems",
                column: "originalitemid");

            migrationBuilder.CreateIndex(
                name: "ix_invoices_customerid",
                table: "invoices",
                column: "customerid");

            migrationBuilder.CreateIndex(
                name: "ix_invoices_number",
                table: "invoices",
                column: "number",
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
                name: "ix_journalentries_journaltypeid",
                table: "journalentries",
                column: "journaltypeid");

            migrationBuilder.CreateIndex(
                name: "ix_journallines_accountid",
                table: "journallines",
                column: "accountid");

            migrationBuilder.CreateIndex(
                name: "ix_journallines_costcenterid",
                table: "journallines",
                column: "costcenterid");

            migrationBuilder.CreateIndex(
                name: "ix_journallines_journalentryid",
                table: "journallines",
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

            migrationBuilder.CreateIndex(
                name: "ix_projects_number",
                table: "projects",
                column: "number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_purchaseorderlineitems_originalitemid",
                table: "purchaseorderlineitems",
                column: "originalitemid");

            migrationBuilder.CreateIndex(
                name: "ix_purchaseorderlineitems_purchaseorderid",
                table: "purchaseorderlineitems",
                column: "purchaseorderid");

            migrationBuilder.CreateIndex(
                name: "ix_purchaseorders_creditto",
                table: "purchaseorders",
                column: "creditto");

            migrationBuilder.CreateIndex(
                name: "ix_purchaseorders_debitto",
                table: "purchaseorders",
                column: "debitto");

            migrationBuilder.CreateIndex(
                name: "ix_purchaseorders_number",
                table: "purchaseorders",
                column: "number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_purchaseorders_projectid",
                table: "purchaseorders",
                column: "projectid");

            migrationBuilder.CreateIndex(
                name: "ix_purchaseorders_supplierid",
                table: "purchaseorders",
                column: "supplierid");

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
                name: "accountspayable");

            migrationBuilder.DropTable(
                name: "employees");

            migrationBuilder.DropTable(
                name: "inventorylogs");

            migrationBuilder.DropTable(
                name: "invoicelineitems");

            migrationBuilder.DropTable(
                name: "journallines");

            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.DropTable(
                name: "inventory");

            migrationBuilder.DropTable(
                name: "purchaseorderlineitems");

            migrationBuilder.DropTable(
                name: "invoices");

            migrationBuilder.DropTable(
                name: "costcenters");

            migrationBuilder.DropTable(
                name: "journalentries");

            migrationBuilder.DropTable(
                name: "items");

            migrationBuilder.DropTable(
                name: "purchaseorders");

            migrationBuilder.DropTable(
                name: "customers");

            migrationBuilder.DropTable(
                name: "journaltypes");

            migrationBuilder.DropTable(
                name: "itemcategories");

            migrationBuilder.DropTable(
                name: "unitofmeasurements");

            migrationBuilder.DropTable(
                name: "accounts");

            migrationBuilder.DropTable(
                name: "projects");

            migrationBuilder.DropTable(
                name: "suppliers");

            migrationBuilder.DropTable(
                name: "accounttypes");

            migrationBuilder.DropTable(
                name: "paymentterms");
        }
    }
}
