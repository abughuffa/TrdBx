using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CleanArchitecture.Blazor.Migrators.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateOfTrdBx : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "customers",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    parent_id = table.Column<int>(type: "integer", nullable: true),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    account = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    user_name = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    billing_plan = table.Column<int>(type: "integer", nullable: false),
                    is_taxable = table.Column<bool>(type: "boolean", nullable: false),
                    is_renewable = table.Column<bool>(type: "boolean", nullable: false),
                    w_user_id = table.Column<int>(type: "integer", nullable: true),
                    w_unit_group_id = table.Column<int>(type: "integer", nullable: true),
                    address = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    mobile1 = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    mobile2 = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    email = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    is_avaliable = table.Column<bool>(type: "boolean", nullable: false),
                    old_id = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by_id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    last_modified_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    last_modified_by_id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_customers", x => x.id);
                    table.ForeignKey(
                        name: "fk_customers_customers_parent_id",
                        column: x => x.parent_id,
                        principalTable: "customers",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "libyana_sim_cards",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sim_card_no = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    sim_card_status = table.Column<int>(type: "integer", nullable: true),
                    balance = table.Column<decimal>(type: "numeric", nullable: true),
                    b_ex_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    join_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    package = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    d_ex_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    data_offer = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    do_expired = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_libyana_sim_cards", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "s_providers",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_s_providers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "service_prices",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    service_task = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    price = table.Column<decimal>(type: "numeric", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by_id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    last_modified_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    last_modified_by_id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_service_prices", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tracked_assets",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tracked_asset_no = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    tracked_asset_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    vin_ser_no = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    plate_no = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    tracked_asset_desc = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    is_avaliable = table.Column<bool>(type: "boolean", nullable: false),
                    old_id = table.Column<int>(type: "integer", nullable: true),
                    old_vehicle_no = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by_id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    last_modified_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    last_modified_by_id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tracked_assets", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tracking_unit_models",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    wialon_name = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    whw_type_id = table.Column<int>(type: "integer", nullable: false),
                    defualt_host = table.Column<decimal>(type: "numeric", nullable: false),
                    defualt_gprs = table.Column<decimal>(type: "numeric", nullable: false),
                    defualt_price = table.Column<decimal>(type: "numeric", nullable: false),
                    port_no1 = table.Column<int>(type: "integer", nullable: false),
                    port_no2 = table.Column<int>(type: "integer", nullable: false),
                    old_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tracking_unit_models", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "wialon_units",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    unit_name = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    account = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    unit_s_no = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    deactivation = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    sim_card_no = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    status_on_wialon = table.Column<int>(type: "integer", nullable: true),
                    note = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_wialon_units", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "invoices",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    invoice_no = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    invoice_date = table.Column<DateOnly>(type: "date", nullable: false),
                    due_date = table.Column<DateOnly>(type: "date", nullable: false),
                    payment_date = table.Column<DateOnly>(type: "date", nullable: true),
                    paid_amount = table.Column<decimal>(type: "numeric", nullable: false),
                    invoice_type = table.Column<int>(type: "integer", nullable: false),
                    i_status = table.Column<int>(type: "integer", nullable: false),
                    display_cus_name = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    customer_id = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    is_taxable = table.Column<bool>(type: "boolean", nullable: false),
                    is_tax_ignored = table.Column<bool>(type: "boolean", nullable: false),
                    total = table.Column<decimal>(type: "numeric", nullable: false),
                    discount_rate = table.Column<decimal>(type: "numeric", nullable: false),
                    discount_amount = table.Column<decimal>(type: "numeric", nullable: false),
                    tax_rate = table.Column<decimal>(type: "numeric", nullable: false),
                    tax_amount = table.Column<decimal>(type: "numeric", nullable: false),
                    taxable_amount = table.Column<decimal>(type: "numeric", nullable: false),
                    grand_total = table.Column<decimal>(type: "numeric", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by_id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    last_modified_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    last_modified_by_id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_invoices", x => x.id);
                    table.ForeignKey(
                        name: "fk_invoices_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "s_packages",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    s_provider_id = table.Column<int>(type: "integer", nullable: false),
                    old_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_s_packages", x => x.id);
                    table.ForeignKey(
                        name: "fk_s_packages_s_providers_s_provider_id",
                        column: x => x.s_provider_id,
                        principalTable: "s_providers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cus_prices",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    customer_id = table.Column<int>(type: "integer", nullable: false),
                    tracking_unit_model_id = table.Column<int>(type: "integer", nullable: false),
                    host = table.Column<decimal>(type: "numeric", nullable: false),
                    gprs = table.Column<decimal>(type: "numeric", nullable: false),
                    price = table.Column<decimal>(type: "numeric", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by_id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    last_modified_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    last_modified_by_id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_cus_prices", x => x.id);
                    table.ForeignKey(
                        name: "fk_cus_prices_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_cus_prices_tracking_unit_models_tracking_unit_model_id",
                        column: x => x.tracking_unit_model_id,
                        principalTable: "tracking_unit_models",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sim_cards",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sim_card_no = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    iccid = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    s_package_id = table.Column<int>(type: "integer", nullable: false),
                    s_status = table.Column<int>(type: "integer", nullable: false),
                    is_owen = table.Column<bool>(type: "boolean", nullable: false),
                    ex_date = table.Column<DateOnly>(type: "date", nullable: true),
                    old_id = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by_id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    last_modified_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    last_modified_by_id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sim_cards", x => x.id);
                    table.ForeignKey(
                        name: "fk_sim_cards_s_packages_s_package_id",
                        column: x => x.s_package_id,
                        principalTable: "s_packages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tracking_units",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    s_no = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    imei = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    unit_name = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    tracking_unit_model_id = table.Column<int>(type: "integer", nullable: false),
                    u_status = table.Column<int>(type: "integer", nullable: false),
                    ins_mode = table.Column<int>(type: "integer", nullable: false),
                    wry_date = table.Column<DateOnly>(type: "date", nullable: true),
                    tracked_asset_id = table.Column<int>(type: "integer", nullable: true),
                    sim_card_id = table.Column<int>(type: "integer", nullable: true),
                    customer_id = table.Column<int>(type: "integer", nullable: true),
                    is_on_wialon = table.Column<bool>(type: "boolean", nullable: false),
                    w_status = table.Column<int>(type: "integer", nullable: true),
                    w_unit_id = table.Column<int>(type: "integer", nullable: true),
                    old_id = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by_id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    last_modified_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    last_modified_by_id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tracking_units", x => x.id);
                    table.ForeignKey(
                        name: "fk_tracking_units_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_tracking_units_sim_cards_sim_card_id",
                        column: x => x.sim_card_id,
                        principalTable: "sim_cards",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_tracking_units_tracked_assets_tracked_asset_id",
                        column: x => x.tracked_asset_id,
                        principalTable: "tracked_assets",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_tracking_units_tracking_unit_models_tracking_unit_model_id",
                        column: x => x.tracking_unit_model_id,
                        principalTable: "tracking_unit_models",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tickets",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ticket_no = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    service_task = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    ticket_status = table.Column<int>(type: "integer", nullable: false),
                    tracking_unit_id = table.Column<int>(type: "integer", nullable: false),
                    tc_date = table.Column<DateOnly>(type: "date", nullable: false),
                    ta_date = table.Column<DateOnly>(type: "date", nullable: true),
                    te_date = table.Column<DateOnly>(type: "date", nullable: true),
                    note = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by_id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    last_modified_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    last_modified_by_id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tickets", x => x.id);
                    table.ForeignKey(
                        name: "fk_tickets_asp_net_users_created_by_id",
                        column: x => x.created_by_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_tickets_asp_net_users_last_modified_by_id",
                        column: x => x.last_modified_by_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_tickets_tracking_units_tracking_unit_id",
                        column: x => x.tracking_unit_id,
                        principalTable: "tracking_units",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "invoice_item_groups",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    serial_index = table.Column<int>(type: "integer", nullable: false),
                    invoice_id = table.Column<int>(type: "integer", nullable: false),
                    service_log_id = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    sub_total = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_invoice_item_groups", x => x.id);
                    table.ForeignKey(
                        name: "fk_invoice_item_groups_invoices_invoice_id",
                        column: x => x.invoice_id,
                        principalTable: "invoices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "invoice_items",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sub_serial_index = table.Column<int>(type: "integer", nullable: false),
                    invoice_item_group_id = table.Column<int>(type: "integer", nullable: false),
                    subscription_id = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    amount = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_invoice_items", x => x.id);
                    table.ForeignKey(
                        name: "fk_invoice_items_invoice_item_groups_invoice_item_group_id",
                        column: x => x.invoice_item_group_id,
                        principalTable: "invoice_item_groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "service_logs",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    service_no = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    service_task = table.Column<int>(type: "integer", nullable: false),
                    customer_id = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    ser_date = table.Column<DateOnly>(type: "date", nullable: false),
                    is_deserved = table.Column<bool>(type: "boolean", nullable: false),
                    is_billed = table.Column<bool>(type: "boolean", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    invoice_item_id = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by_id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    last_modified_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    last_modified_by_id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_service_logs", x => x.id);
                    table.ForeignKey(
                        name: "fk_service_logs_asp_net_users_created_by_id",
                        column: x => x.created_by_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_service_logs_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_service_logs_invoice_items_invoice_item_id",
                        column: x => x.invoice_item_id,
                        principalTable: "invoice_items",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "subscriptions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    service_log_id = table.Column<int>(type: "integer", nullable: false),
                    tracking_unit_id = table.Column<int>(type: "integer", nullable: false),
                    case_code = table.Column<int>(type: "integer", nullable: false),
                    last_paid_fees = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    ss_date = table.Column<DateOnly>(type: "date", nullable: false),
                    se_date = table.Column<DateOnly>(type: "date", nullable: false),
                    days = table.Column<int>(type: "integer", nullable: false, computedColumnSql: "\"se_date\" - \"ss_date\"", stored: true),
                    daily_fees = table.Column<decimal>(type: "numeric", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false, computedColumnSql: "(\"se_date\" - \"ss_date\") * daily_fees", stored: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_subscriptions", x => x.id);
                    table.ForeignKey(
                        name: "fk_subscriptions_service_logs_service_log_id",
                        column: x => x.service_log_id,
                        principalTable: "service_logs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_subscriptions_tracking_units_tracking_unit_id",
                        column: x => x.tracking_unit_id,
                        principalTable: "tracking_units",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "wialon_tasks",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    service_log_id = table.Column<int>(type: "integer", nullable: false),
                    tracking_unit_id = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    api_task = table.Column<int>(type: "integer", nullable: true),
                    exc_date = table.Column<DateOnly>(type: "date", nullable: false),
                    is_executed = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_wialon_tasks", x => x.id);
                    table.ForeignKey(
                        name: "fk_wialon_tasks_service_logs_service_log_id",
                        column: x => x.service_log_id,
                        principalTable: "service_logs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_wialon_tasks_tracking_units_tracking_unit_id",
                        column: x => x.tracking_unit_id,
                        principalTable: "tracking_units",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_cus_prices_customer_id",
                table: "cus_prices",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "ix_cus_prices_tracking_unit_model_id",
                table: "cus_prices",
                column: "tracking_unit_model_id");

            migrationBuilder.CreateIndex(
                name: "ix_customers_name",
                table: "customers",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_customers_parent_id",
                table: "customers",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "ix_invoice_item_groups_invoice_id",
                table: "invoice_item_groups",
                column: "invoice_id");

            migrationBuilder.CreateIndex(
                name: "ix_invoice_item_groups_service_log_id",
                table: "invoice_item_groups",
                column: "service_log_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_invoice_items_invoice_item_group_id",
                table: "invoice_items",
                column: "invoice_item_group_id");

            migrationBuilder.CreateIndex(
                name: "ix_invoice_items_subscription_id",
                table: "invoice_items",
                column: "subscription_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_invoices_customer_id",
                table: "invoices",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "ix_invoices_invoice_no",
                table: "invoices",
                column: "invoice_no",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_s_packages_name",
                table: "s_packages",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_s_packages_s_provider_id",
                table: "s_packages",
                column: "s_provider_id");

            migrationBuilder.CreateIndex(
                name: "ix_s_providers_name",
                table: "s_providers",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_service_logs_created_by_id",
                table: "service_logs",
                column: "created_by_id");

            migrationBuilder.CreateIndex(
                name: "ix_service_logs_customer_id",
                table: "service_logs",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "ix_service_logs_invoice_item_id",
                table: "service_logs",
                column: "invoice_item_id");

            migrationBuilder.CreateIndex(
                name: "ix_service_logs_service_no",
                table: "service_logs",
                column: "service_no",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_sim_cards_s_package_id",
                table: "sim_cards",
                column: "s_package_id");

            migrationBuilder.CreateIndex(
                name: "ix_sim_cards_sim_card_no",
                table: "sim_cards",
                column: "sim_card_no",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_subscriptions_service_log_id",
                table: "subscriptions",
                column: "service_log_id");

            migrationBuilder.CreateIndex(
                name: "ix_subscriptions_tracking_unit_id",
                table: "subscriptions",
                column: "tracking_unit_id");

            migrationBuilder.CreateIndex(
                name: "ix_tickets_created_by_id",
                table: "tickets",
                column: "created_by_id");

            migrationBuilder.CreateIndex(
                name: "ix_tickets_last_modified_by_id",
                table: "tickets",
                column: "last_modified_by_id");

            migrationBuilder.CreateIndex(
                name: "ix_tickets_ticket_no",
                table: "tickets",
                column: "ticket_no",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_tickets_tracking_unit_id",
                table: "tickets",
                column: "tracking_unit_id");

            migrationBuilder.CreateIndex(
                name: "ix_tracked_assets_tracked_asset_no",
                table: "tracked_assets",
                column: "tracked_asset_no",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_tracking_unit_models_name",
                table: "tracking_unit_models",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_tracking_units_customer_id",
                table: "tracking_units",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "ix_tracking_units_sim_card_id",
                table: "tracking_units",
                column: "sim_card_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_tracking_units_tracked_asset_id",
                table: "tracking_units",
                column: "tracked_asset_id");

            migrationBuilder.CreateIndex(
                name: "ix_tracking_units_tracking_unit_model_id",
                table: "tracking_units",
                column: "tracking_unit_model_id");

            migrationBuilder.CreateIndex(
                name: "ix_wialon_tasks_service_log_id",
                table: "wialon_tasks",
                column: "service_log_id");

            migrationBuilder.CreateIndex(
                name: "ix_wialon_tasks_tracking_unit_id",
                table: "wialon_tasks",
                column: "tracking_unit_id");

            migrationBuilder.AddForeignKey(
                name: "fk_invoice_item_groups_service_logs_service_log_id",
                table: "invoice_item_groups",
                column: "service_log_id",
                principalTable: "service_logs",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_invoice_items_subscriptions_subscription_id",
                table: "invoice_items",
                column: "subscription_id",
                principalTable: "subscriptions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_invoices_customers_customer_id",
                table: "invoices");

            migrationBuilder.DropForeignKey(
                name: "fk_service_logs_customers_customer_id",
                table: "service_logs");

            migrationBuilder.DropForeignKey(
                name: "fk_tracking_units_customers_customer_id",
                table: "tracking_units");

            migrationBuilder.DropForeignKey(
                name: "fk_tracking_units_tracking_unit_models_tracking_unit_model_id",
                table: "tracking_units");

            migrationBuilder.DropForeignKey(
                name: "fk_invoice_item_groups_invoices_invoice_id",
                table: "invoice_item_groups");

            migrationBuilder.DropForeignKey(
                name: "fk_invoice_item_groups_service_logs_service_log_id",
                table: "invoice_item_groups");

            migrationBuilder.DropForeignKey(
                name: "fk_subscriptions_service_logs_service_log_id",
                table: "subscriptions");

            migrationBuilder.DropTable(
                name: "cus_prices");

            migrationBuilder.DropTable(
                name: "libyana_sim_cards");

            migrationBuilder.DropTable(
                name: "service_prices");

            migrationBuilder.DropTable(
                name: "tickets");

            migrationBuilder.DropTable(
                name: "wialon_tasks");

            migrationBuilder.DropTable(
                name: "wialon_units");

            migrationBuilder.DropTable(
                name: "customers");

            migrationBuilder.DropTable(
                name: "tracking_unit_models");

            migrationBuilder.DropTable(
                name: "invoices");

            migrationBuilder.DropTable(
                name: "service_logs");

            migrationBuilder.DropTable(
                name: "invoice_items");

            migrationBuilder.DropTable(
                name: "invoice_item_groups");

            migrationBuilder.DropTable(
                name: "subscriptions");

            migrationBuilder.DropTable(
                name: "tracking_units");

            migrationBuilder.DropTable(
                name: "sim_cards");

            migrationBuilder.DropTable(
                name: "tracked_assets");

            migrationBuilder.DropTable(
                name: "s_packages");

            migrationBuilder.DropTable(
                name: "s_providers");
        }
    }
}
