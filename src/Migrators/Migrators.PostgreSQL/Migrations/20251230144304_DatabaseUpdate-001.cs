using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CleanArchitecture.Blazor.Migrators.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseUpdate001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "activate_gprs_test_cases",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tracking_unit_id = table.Column<int>(type: "integer", nullable: true),
                    s_no = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ts_date = table.Column<DateOnly>(type: "date", nullable: false),
                    case_code = table.Column<int>(type: "integer", nullable: true),
                    is_sucssed = table.Column<bool>(type: "boolean", nullable: true),
                    message = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_activate_gprs_test_cases", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "activate_hosting_test_cases",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tracking_unit_id = table.Column<int>(type: "integer", nullable: true),
                    s_no = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ts_date = table.Column<DateOnly>(type: "date", nullable: false),
                    case_code = table.Column<int>(type: "integer", nullable: true),
                    is_sucssed = table.Column<bool>(type: "boolean", nullable: true),
                    message = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_activate_hosting_test_cases", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "activate_test_cases",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tracking_unit_id = table.Column<int>(type: "integer", nullable: true),
                    s_no = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ts_date = table.Column<DateOnly>(type: "date", nullable: false),
                    case_code = table.Column<int>(type: "integer", nullable: true),
                    is_sucssed = table.Column<bool>(type: "boolean", nullable: true),
                    message = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_activate_test_cases", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "contacts",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    email = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    phone_number = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    country = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    last_modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    last_modified_by = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_contacts", x => x.id);
                });

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
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    last_modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    last_modified_by = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
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
                name: "data_protection_keys",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    friendly_name = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    xml = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_data_protection_keys", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "deactivate_test_cases",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tracking_unit_id = table.Column<int>(type: "integer", nullable: true),
                    s_no = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ts_date = table.Column<DateOnly>(type: "date", nullable: false),
                    case_code = table.Column<int>(type: "integer", nullable: true),
                    is_sucssed = table.Column<bool>(type: "boolean", nullable: true),
                    message = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_deactivate_test_cases", x => x.id);
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
                name: "loggers",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    message = table.Column<string>(type: "text", maxLength: 2147483647, nullable: true),
                    message_template = table.Column<string>(type: "text", maxLength: 2147483647, nullable: true),
                    level = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    time_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    exception = table.Column<string>(type: "text", maxLength: 2147483647, nullable: true),
                    user_name = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    client_ip = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    client_agent = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    properties = table.Column<string>(type: "text", maxLength: 2147483647, nullable: true),
                    log_event = table.Column<string>(type: "text", maxLength: 2147483647, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_loggers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "picklist_sets",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    value = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    text = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    last_modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    last_modified_by = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_picklist_sets", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "po_is",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    latitude = table.Column<double>(type: "double precision", nullable: false),
                    longitude = table.Column<double>(type: "double precision", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    last_modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    last_modified_by = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_po_is", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    description = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    brand = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    unit = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    price = table.Column<decimal>(type: "numeric", nullable: false),
                    pictures = table.Column<string>(type: "text", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    last_modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    last_modified_by = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_products", x => x.id);
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
                    desc = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    price = table.Column<decimal>(type: "numeric", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    last_modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    last_modified_by = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_service_prices", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tenants",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    name = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    description = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tenants", x => x.id);
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
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    last_modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    last_modified_by = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
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
                name: "vehicle_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    image = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vehicle_types", x => x.id);
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
                    inv_no = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    inv_date = table.Column<DateOnly>(type: "date", nullable: false),
                    due_date = table.Column<DateOnly>(type: "date", nullable: false),
                    invoice_type = table.Column<int>(type: "integer", nullable: false),
                    i_status = table.Column<int>(type: "integer", nullable: false),
                    customer_id = table.Column<int>(type: "integer", nullable: false),
                    inv_desc = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    total = table.Column<decimal>(type: "numeric", nullable: false),
                    taxes = table.Column<decimal>(type: "numeric", nullable: false),
                    grang_total = table.Column<decimal>(type: "numeric", nullable: false),
                    is_taxable = table.Column<bool>(type: "boolean", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    last_modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    last_modified_by = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
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
                name: "AspNetRoles",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    tenant_id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    description = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    last_modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    last_modified_by = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    concurrency_stamp = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_roles", x => x.id);
                    table.ForeignKey(
                        name: "fk_asp_net_roles_tenants_tenant_id",
                        column: x => x.tenant_id,
                        principalTable: "tenants",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    display_name = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    provider = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    tenant_id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    profile_picture_data_url = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_live = table.Column<bool>(type: "boolean", nullable: false),
                    refresh_token = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    refresh_token_expiry_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    superior_id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    last_modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    last_modified_by = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    time_zone_id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    language_code = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    email_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    password_hash = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    security_stamp = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    concurrency_stamp = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    phone_number = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    phone_number_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    two_factor_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    lockout_end = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    lockout_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    access_failed_count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_users", x => x.id);
                    table.ForeignKey(
                        name: "fk_asp_net_users_asp_net_users_created_by",
                        column: x => x.created_by,
                        principalTable: "AspNetUsers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_asp_net_users_asp_net_users_last_modified_by",
                        column: x => x.last_modified_by,
                        principalTable: "AspNetUsers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_asp_net_users_asp_net_users_superior_id",
                        column: x => x.superior_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_asp_net_users_tenants_tenant_id",
                        column: x => x.tenant_id,
                        principalTable: "tenants",
                        principalColumn: "id");
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
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    last_modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    last_modified_by = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
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
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    last_modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    last_modified_by = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
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
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    group = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    role_id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    claim_type = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    claim_value = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_role_claims", x => x.id);
                    table.ForeignKey(
                        name: "fk_asp_net_role_claims_asp_net_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "AspNetRoles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    user_id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    claim_type = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    claim_value = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_user_claims", x => x.id);
                    table.ForeignKey(
                        name: "fk_asp_net_user_claims_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    login_provider = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    provider_key = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    provider_display_name = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    user_id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_user_logins", x => new { x.login_provider, x.provider_key });
                    table.ForeignKey(
                        name: "fk_asp_net_user_logins_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    role_id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_user_roles", x => new { x.user_id, x.role_id });
                    table.ForeignKey(
                        name: "fk_asp_net_user_roles_asp_net_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "AspNetRoles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_asp_net_user_roles_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    login_provider = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    name = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    value = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_user_tokens", x => new { x.user_id, x.login_provider, x.name });
                    table.ForeignKey(
                        name: "fk_asp_net_user_tokens_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "audit_trails",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    audit_type = table.Column<string>(type: "text", nullable: false),
                    table_name = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    date_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    old_values = table.Column<string>(type: "text", nullable: true),
                    new_values = table.Column<string>(type: "text", nullable: true),
                    affected_columns = table.Column<string>(type: "text", nullable: true),
                    primary_key = table.Column<string>(type: "text", nullable: false),
                    debug_view = table.Column<string>(type: "text", maxLength: 2147483647, nullable: true),
                    error_message = table.Column<string>(type: "text", maxLength: 2147483647, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_audit_trails", x => x.id);
                    table.ForeignKey(
                        name: "fk_audit_trails_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "documents",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    description = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false),
                    content = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    is_public = table.Column<bool>(type: "boolean", nullable: false),
                    url = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    document_type = table.Column<string>(type: "text", nullable: false),
                    tenant_id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    last_modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    last_modified_by = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_documents", x => x.id);
                    table.ForeignKey(
                        name: "fk_documents_asp_net_users_created_by",
                        column: x => x.created_by,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_documents_asp_net_users_last_modified_by",
                        column: x => x.last_modified_by,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_documents_tenants_tenant_id",
                        column: x => x.tenant_id,
                        principalTable: "tenants",
                        principalColumn: "id");
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
                    desc = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    ser_date = table.Column<DateOnly>(type: "date", nullable: false),
                    is_deserved = table.Column<bool>(type: "boolean", nullable: false),
                    is_billed = table.Column<bool>(type: "boolean", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    last_modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    last_modified_by = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_service_logs", x => x.id);
                    table.ForeignKey(
                        name: "fk_service_logs_asp_net_users_created_by",
                        column: x => x.created_by,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_service_logs_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vehicles",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    vehicle_no = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    vehicle_type_id = table.Column<int>(type: "integer", nullable: false),
                    driver_id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    last_modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    last_modified_by = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vehicles", x => x.id);
                    table.ForeignKey(
                        name: "fk_vehicles_users_driver_id",
                        column: x => x.driver_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_vehicles_vehicle_types_vehicle_type_id",
                        column: x => x.vehicle_type_id,
                        principalTable: "vehicle_types",
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
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    last_modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    last_modified_by = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
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
                name: "invoice_items",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    invoice_id = table.Column<int>(type: "integer", nullable: false),
                    service_log_id = table.Column<int>(type: "integer", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_invoice_items", x => x.id);
                    table.ForeignKey(
                        name: "fk_invoice_items_invoices_invoice_id",
                        column: x => x.invoice_id,
                        principalTable: "invoices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_invoice_items_service_logs_service_log_id",
                        column: x => x.service_log_id,
                        principalTable: "service_logs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "shipments",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    shipment_no = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    shipment_status = table.Column<int>(type: "integer", nullable: false),
                    price = table.Column<decimal>(type: "numeric", nullable: false),
                    is_bidable = table.Column<bool>(type: "boolean", nullable: false),
                    vehicle_id = table.Column<int>(type: "integer", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    last_modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    last_modified_by = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_shipments", x => x.id);
                    table.ForeignKey(
                        name: "fk_shipments_vehicles_vehicle_id",
                        column: x => x.vehicle_id,
                        principalTable: "vehicles",
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
                    desc = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    ss_date = table.Column<DateOnly>(type: "date", nullable: false),
                    se_date = table.Column<DateOnly>(type: "date", nullable: false),
                    days = table.Column<int>(type: "integer", nullable: false, computedColumnSql: "(Se_Date - Ss_Date) / 86400", stored: true),
                    daily_fees = table.Column<decimal>(type: "numeric", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false, computedColumnSql: "((Se_Date - Ss_Date) / 86400) * daily_fees", stored: true)
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
                name: "tickets",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ticket_no = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    service_task = table.Column<int>(type: "integer", nullable: false),
                    desc = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    ticket_status = table.Column<int>(type: "integer", nullable: false),
                    tracking_unit_id = table.Column<int>(type: "integer", nullable: false),
                    tc_date = table.Column<DateOnly>(type: "date", nullable: false),
                    ta_date = table.Column<DateOnly>(type: "date", nullable: true),
                    te_date = table.Column<DateOnly>(type: "date", nullable: true),
                    note = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    last_modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    last_modified_by = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tickets", x => x.id);
                    table.ForeignKey(
                        name: "fk_tickets_asp_net_users_created_by",
                        column: x => x.created_by,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_tickets_asp_net_users_last_modified_by",
                        column: x => x.last_modified_by,
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
                name: "wialon_tasks",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    service_log_id = table.Column<int>(type: "integer", nullable: false),
                    tracking_unit_id = table.Column<int>(type: "integer", nullable: false),
                    desc = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
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

            migrationBuilder.CreateTable(
                name: "bid_records",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    shipment_id = table.Column<int>(type: "integer", nullable: false),
                    transporter_id = table.Column<int>(type: "integer", nullable: false),
                    price = table.Column<decimal>(type: "numeric", nullable: false),
                    transporter_id1 = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    last_modified = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    last_modified_by = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_bid_records", x => x.id);
                    table.ForeignKey(
                        name: "fk_bid_records_shipments_shipment_id",
                        column: x => x.shipment_id,
                        principalTable: "shipments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_bid_records_users_transporter_id1",
                        column: x => x.transporter_id1,
                        principalTable: "AspNetUsers",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "shipment_vehicle_type",
                columns: table => new
                {
                    shipment_id = table.Column<int>(type: "integer", nullable: false),
                    vehicle_type_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_shipment_vehicle_type", x => new { x.shipment_id, x.vehicle_type_id });
                    table.ForeignKey(
                        name: "fk_shipment_vehicle_type_shipments_shipment_id",
                        column: x => x.shipment_id,
                        principalTable: "shipments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_shipment_vehicle_type_vehicle_types_vehicle_type_id",
                        column: x => x.vehicle_type_id,
                        principalTable: "vehicle_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "way_points",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    shipment_id = table.Column<int>(type: "integer", nullable: false),
                    latitude = table.Column<double>(type: "double precision", nullable: false),
                    longitude = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_way_points", x => x.id);
                    table.ForeignKey(
                        name: "fk_way_points_shipments_shipment_id",
                        column: x => x.shipment_id,
                        principalTable: "shipments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_role_claims_role_id",
                table: "AspNetRoleClaims",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_roles_tenant_id_name",
                table: "AspNetRoles",
                columns: new[] { "tenant_id", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "normalized_name");

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_user_claims_user_id",
                table: "AspNetUserClaims",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_user_logins_user_id",
                table: "AspNetUserLogins",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_user_roles_role_id",
                table: "AspNetUserRoles",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "normalized_email");

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_users_created_by",
                table: "AspNetUsers",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_users_last_modified_by",
                table: "AspNetUsers",
                column: "last_modified_by");

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_users_superior_id",
                table: "AspNetUsers",
                column: "superior_id");

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_users_tenant_id",
                table: "AspNetUsers",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "normalized_user_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_audit_trails_user_id",
                table: "audit_trails",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_bid_records_shipment_id",
                table: "bid_records",
                column: "shipment_id");

            migrationBuilder.CreateIndex(
                name: "ix_bid_records_transporter_id1",
                table: "bid_records",
                column: "transporter_id1");

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
                name: "ix_documents_created_by",
                table: "documents",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "ix_documents_last_modified_by",
                table: "documents",
                column: "last_modified_by");

            migrationBuilder.CreateIndex(
                name: "ix_documents_tenant_id",
                table: "documents",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "ix_invoice_items_invoice_id",
                table: "invoice_items",
                column: "invoice_id");

            migrationBuilder.CreateIndex(
                name: "ix_invoice_items_service_log_id",
                table: "invoice_items",
                column: "service_log_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_invoices_customer_id",
                table: "invoices",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "ix_invoices_inv_no",
                table: "invoices",
                column: "inv_no",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_loggers_level",
                table: "loggers",
                column: "level");

            migrationBuilder.CreateIndex(
                name: "ix_loggers_time_stamp",
                table: "loggers",
                column: "time_stamp");

            migrationBuilder.CreateIndex(
                name: "ix_picklist_sets_name_value",
                table: "picklist_sets",
                columns: new[] { "name", "value" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_products_name",
                table: "products",
                column: "name",
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
                name: "ix_service_logs_created_by",
                table: "service_logs",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "ix_service_logs_customer_id",
                table: "service_logs",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "ix_service_logs_service_no",
                table: "service_logs",
                column: "service_no",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_shipment_vehicle_type_vehicle_type_id",
                table: "shipment_vehicle_type",
                column: "vehicle_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_shipments_vehicle_id",
                table: "shipments",
                column: "vehicle_id");

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
                name: "ix_tickets_created_by",
                table: "tickets",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "ix_tickets_last_modified_by",
                table: "tickets",
                column: "last_modified_by");

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
                name: "ix_vehicles_driver_id",
                table: "vehicles",
                column: "driver_id");

            migrationBuilder.CreateIndex(
                name: "ix_vehicles_vehicle_type_id",
                table: "vehicles",
                column: "vehicle_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_way_points_shipment_id",
                table: "way_points",
                column: "shipment_id");

            migrationBuilder.CreateIndex(
                name: "ix_wialon_tasks_service_log_id",
                table: "wialon_tasks",
                column: "service_log_id");

            migrationBuilder.CreateIndex(
                name: "ix_wialon_tasks_tracking_unit_id",
                table: "wialon_tasks",
                column: "tracking_unit_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "activate_gprs_test_cases");

            migrationBuilder.DropTable(
                name: "activate_hosting_test_cases");

            migrationBuilder.DropTable(
                name: "activate_test_cases");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "audit_trails");

            migrationBuilder.DropTable(
                name: "bid_records");

            migrationBuilder.DropTable(
                name: "contacts");

            migrationBuilder.DropTable(
                name: "cus_prices");

            migrationBuilder.DropTable(
                name: "data_protection_keys");

            migrationBuilder.DropTable(
                name: "deactivate_test_cases");

            migrationBuilder.DropTable(
                name: "documents");

            migrationBuilder.DropTable(
                name: "invoice_items");

            migrationBuilder.DropTable(
                name: "libyana_sim_cards");

            migrationBuilder.DropTable(
                name: "loggers");

            migrationBuilder.DropTable(
                name: "picklist_sets");

            migrationBuilder.DropTable(
                name: "po_is");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "service_prices");

            migrationBuilder.DropTable(
                name: "shipment_vehicle_type");

            migrationBuilder.DropTable(
                name: "subscriptions");

            migrationBuilder.DropTable(
                name: "tickets");

            migrationBuilder.DropTable(
                name: "way_points");

            migrationBuilder.DropTable(
                name: "wialon_tasks");

            migrationBuilder.DropTable(
                name: "wialon_units");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "invoices");

            migrationBuilder.DropTable(
                name: "shipments");

            migrationBuilder.DropTable(
                name: "service_logs");

            migrationBuilder.DropTable(
                name: "tracking_units");

            migrationBuilder.DropTable(
                name: "vehicles");

            migrationBuilder.DropTable(
                name: "customers");

            migrationBuilder.DropTable(
                name: "sim_cards");

            migrationBuilder.DropTable(
                name: "tracked_assets");

            migrationBuilder.DropTable(
                name: "tracking_unit_models");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "vehicle_types");

            migrationBuilder.DropTable(
                name: "s_packages");

            migrationBuilder.DropTable(
                name: "tenants");

            migrationBuilder.DropTable(
                name: "s_providers");
        }
    }
}
