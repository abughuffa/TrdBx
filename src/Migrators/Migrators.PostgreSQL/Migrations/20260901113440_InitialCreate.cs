using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CleanArchitecture.Blazor.Migrators.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    description = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    last_modified_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    concurrency_stamp = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ParentId = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Account = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UserName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    BillingPlan = table.Column<int>(type: "integer", nullable: false),
                    IsTaxable = table.Column<bool>(type: "boolean", nullable: false),
                    IsRenewable = table.Column<bool>(type: "boolean", nullable: false),
                    WUserId = table.Column<int>(type: "integer", nullable: true),
                    WUnitGroupId = table.Column<int>(type: "integer", nullable: true),
                    Address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Mobile1 = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Mobile2 = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsAvailable = table.Column<bool>(type: "boolean", nullable: false),
                    OldId = table.Column<int>(type: "integer", nullable: true),
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
                        column: x => x.ParentId,
                        principalTable: "Customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "LibyanaSimCards",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SimCardNo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    SimCardStatus = table.Column<int>(type: "integer", nullable: true),
                    Balance = table.Column<decimal>(type: "numeric(9,3)", precision: 9, scale: 3, nullable: true),
                    BExDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    JoinDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Package = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    DExDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DataOffer = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    DOExpired = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_libyana_sim_cards", x => x.id);
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
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by_id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    last_modified_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    last_modified_by_id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_picklist_sets", x => x.id);
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
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by_id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    last_modified_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    last_modified_by_id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_products", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ServicePrices",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ServiceTask = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(7,3)", precision: 7, scale: 3, nullable: false),
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
                name: "SProviders",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_s_providers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "system_logs",
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
                    table.PrimaryKey("pk_system_logs", x => x.id);
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
                name: "TrackedAssets",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TrackedAssetNo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TrackedAssetCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    VinSerNo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    PlateNo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    TrackedAssetDesc = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    IsAvailable = table.Column<bool>(type: "boolean", nullable: false),
                    OldId = table.Column<int>(type: "integer", nullable: true),
                    OldVehicleNo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
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
                name: "TrackingUnitModels",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WialonName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    WhwTypeId = table.Column<int>(type: "integer", nullable: false),
                    DefaultHost = table.Column<decimal>(type: "numeric(7,3)", precision: 7, scale: 3, nullable: false),
                    DefaultGprs = table.Column<decimal>(type: "numeric(7,3)", precision: 7, scale: 3, nullable: false),
                    DefaultPrice = table.Column<decimal>(type: "numeric(7,3)", precision: 7, scale: 3, nullable: false),
                    PortNo1 = table.Column<int>(type: "integer", nullable: false),
                    PortNo2 = table.Column<int>(type: "integer", nullable: false),
                    OldId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tracking_unit_models", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "WialonUnits",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UnitName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Account = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    UnitSNo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Deactivation = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    SimCardNo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    StatusOnWialon = table.Column<int>(type: "integer", nullable: true),
                    Note = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_wialon_units", x => x.id);
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
                name: "Invoices",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InvoiceNo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    InvoiceDate = table.Column<DateOnly>(type: "date", nullable: false),
                    DueDate = table.Column<DateOnly>(type: "date", nullable: false),
                    PaymentDate = table.Column<DateOnly>(type: "date", nullable: true),
                    PaidAmount = table.Column<decimal>(type: "numeric(9,3)", precision: 9, scale: 3, nullable: false),
                    InvoiceType = table.Column<int>(type: "integer", nullable: false),
                    IStatus = table.Column<int>(type: "integer", nullable: false),
                    DisplayCusName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CustomerId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    IsTaxable = table.Column<bool>(type: "boolean", nullable: false),
                    IsTaxIgnored = table.Column<bool>(type: "boolean", nullable: false),
                    Total = table.Column<decimal>(type: "numeric(9,3)", precision: 9, scale: 3, nullable: false),
                    DiscountRate = table.Column<decimal>(type: "numeric(9,3)", precision: 9, scale: 3, nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "numeric(9,3)", precision: 9, scale: 3, nullable: false),
                    TaxRate = table.Column<decimal>(type: "numeric(9,3)", precision: 9, scale: 3, nullable: false),
                    TaxAmount = table.Column<decimal>(type: "numeric(9,3)", precision: 9, scale: 3, nullable: false),
                    TaxableAmount = table.Column<decimal>(type: "numeric(9,3)", precision: 9, scale: 3, nullable: false),
                    GrandTotal = table.Column<decimal>(type: "numeric(9,3)", precision: 9, scale: 3, nullable: false),
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
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SPackages",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SProviderId = table.Column<int>(type: "integer", nullable: false),
                    old_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_s_packages", x => x.id);
                    table.ForeignKey(
                        name: "fk_s_packages_s_providers_s_provider_id",
                        column: x => x.SProviderId,
                        principalTable: "SProviders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
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
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    last_modified_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
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
                name: "CusPrices",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CustomerId = table.Column<int>(type: "integer", nullable: false),
                    TrackingUnitModelId = table.Column<int>(type: "integer", nullable: false),
                    Host = table.Column<decimal>(type: "numeric(7,3)", precision: 7, scale: 3, nullable: false),
                    Gprs = table.Column<decimal>(type: "numeric(7,3)", precision: 7, scale: 3, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(7,3)", precision: 7, scale: 3, nullable: false),
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
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_cus_prices_tracking_unit_models_tracking_unit_model_id",
                        column: x => x.TrackingUnitModelId,
                        principalTable: "TrackingUnitModels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SimCards",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SimCardNo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ICCID = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    SPackageId = table.Column<int>(type: "integer", nullable: false),
                    SStatus = table.Column<int>(type: "integer", nullable: false),
                    IsOwned = table.Column<bool>(type: "boolean", nullable: false),
                    ExDate = table.Column<DateOnly>(type: "date", nullable: true),
                    OldId = table.Column<int>(type: "integer", nullable: true),
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
                        column: x => x.SPackageId,
                        principalTable: "SPackages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "AspNetUserPasskeys",
                columns: table => new
                {
                    credential_id = table.Column<byte[]>(type: "bytea", maxLength: 1024, nullable: false),
                    user_id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    data = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_user_passkeys", x => x.credential_id);
                    table.ForeignKey(
                        name: "fk_asp_net_user_passkeys_asp_net_users_user_id",
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
                    changes = table.Column<string>(type: "text", nullable: true),
                    affected_columns = table.Column<List<string>>(type: "text[]", nullable: true),
                    primary_key = table.Column<string>(type: "text", nullable: false)
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
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by_id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    last_modified_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    last_modified_by_id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_contacts", x => x.id);
                    table.ForeignKey(
                        name: "fk_contacts_users_created_by_id",
                        column: x => x.created_by_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_contacts_users_last_modified_by_id",
                        column: x => x.last_modified_by_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id");
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
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_by_id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    last_modified_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    last_modified_by_id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_documents", x => x.id);
                    table.ForeignKey(
                        name: "fk_documents_tenants_tenant_id",
                        column: x => x.tenant_id,
                        principalTable: "tenants",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_documents_users_created_by_id",
                        column: x => x.created_by_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_documents_users_last_modified_by_id",
                        column: x => x.last_modified_by_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ServiceLogs",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ServiceNo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ServiceTask = table.Column<int>(type: "integer", nullable: false),
                    CustomerId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    SerDate = table.Column<DateOnly>(type: "date", nullable: false),
                    IsDeserved = table.Column<bool>(type: "boolean", nullable: false),
                    IsBilled = table.Column<bool>(type: "boolean", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(7,3)", precision: 7, scale: 3, nullable: false),
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
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tenant_users",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    tenant_id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    user_id = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tenant_users", x => x.id);
                    table.ForeignKey(
                        name: "fk_tenant_users_tenants_tenant_id",
                        column: x => x.tenant_id,
                        principalTable: "tenants",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_tenant_users_users_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrackingUnits",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SNo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Imei = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    UnitName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    TrackingUnitModelId = table.Column<int>(type: "integer", nullable: false),
                    UStatus = table.Column<int>(type: "integer", nullable: false),
                    InsMode = table.Column<int>(type: "integer", nullable: false),
                    WryDate = table.Column<DateOnly>(type: "date", nullable: true),
                    TrackedAssetId = table.Column<int>(type: "integer", nullable: true),
                    SimCardId = table.Column<int>(type: "integer", nullable: true),
                    CustomerId = table.Column<int>(type: "integer", nullable: true),
                    IsOnWialon = table.Column<bool>(type: "boolean", nullable: false),
                    WStatus = table.Column<int>(type: "integer", nullable: true),
                    WUnitId = table.Column<int>(type: "integer", nullable: true),
                    OldId = table.Column<int>(type: "integer", nullable: true),
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
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_tracking_units_sim_cards_sim_card_id",
                        column: x => x.SimCardId,
                        principalTable: "SimCards",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_tracking_units_tracked_assets_tracked_asset_id",
                        column: x => x.TrackedAssetId,
                        principalTable: "TrackedAssets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_tracking_units_tracking_unit_models_tracking_unit_model_id",
                        column: x => x.TrackingUnitModelId,
                        principalTable: "TrackingUnitModels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceItemGroups",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SerialIndex = table.Column<int>(type: "integer", nullable: false),
                    InvoiceId = table.Column<int>(type: "integer", nullable: false),
                    ServiceLogId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(9,3)", precision: 9, scale: 3, nullable: false),
                    SubTotal = table.Column<decimal>(type: "numeric(9,3)", precision: 9, scale: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_invoice_item_groups", x => x.id);
                    table.ForeignKey(
                        name: "fk_invoice_item_groups_invoices_invoice_id",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_invoice_item_groups_service_logs_service_log_id",
                        column: x => x.ServiceLogId,
                        principalTable: "ServiceLogs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ServiceLogId = table.Column<int>(type: "integer", nullable: false),
                    TrackingUnitId = table.Column<int>(type: "integer", nullable: false),
                    CaseCode = table.Column<int>(type: "integer", nullable: false),
                    LastPaidFees = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    SsDate = table.Column<DateOnly>(type: "date", nullable: false),
                    SeDate = table.Column<DateOnly>(type: "date", nullable: false),
                    DailyFees = table.Column<decimal>(type: "numeric(7,3)", precision: 7, scale: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_subscriptions", x => x.id);
                    table.ForeignKey(
                        name: "fk_subscriptions_service_logs_service_log_id",
                        column: x => x.ServiceLogId,
                        principalTable: "ServiceLogs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_subscriptions_tracking_units_tracking_unit_id",
                        column: x => x.TrackingUnitId,
                        principalTable: "TrackingUnits",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TicketNo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ServiceTask = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    TicketStatus = table.Column<int>(type: "integer", nullable: false),
                    TrackingUnitId = table.Column<int>(type: "integer", nullable: false),
                    TcDate = table.Column<DateOnly>(type: "date", nullable: false),
                    TaDate = table.Column<DateOnly>(type: "date", nullable: true),
                    TeDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Note = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
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
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_tickets_asp_net_users_last_modified_by_id",
                        column: x => x.last_modified_by_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_tickets_tracking_units_tracking_unit_id",
                        column: x => x.TrackingUnitId,
                        principalTable: "TrackingUnits",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WialonTasks",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ServiceLogId = table.Column<int>(type: "integer", nullable: false),
                    TrackingUnitId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    WialonAPIAction = table.Column<int>(type: "integer", nullable: true),
                    ExcDate = table.Column<DateOnly>(type: "date", nullable: false),
                    IsExecuted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_wialon_tasks", x => x.id);
                    table.ForeignKey(
                        name: "fk_wialon_tasks_service_logs_service_log_id",
                        column: x => x.ServiceLogId,
                        principalTable: "ServiceLogs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_wialon_tasks_tracking_units_tracking_unit_id",
                        column: x => x.TrackingUnitId,
                        principalTable: "TrackingUnits",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceItems",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SubSerialIndex = table.Column<int>(type: "integer", nullable: false),
                    InvoiceItemGroupId = table.Column<int>(type: "integer", nullable: false),
                    SubscriptionId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Amount = table.Column<decimal>(type: "numeric(9,3)", precision: 9, scale: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_invoice_items", x => x.id);
                    table.ForeignKey(
                        name: "fk_invoice_items_invoice_item_groups_invoice_item_group_id",
                        column: x => x.InvoiceItemGroupId,
                        principalTable: "InvoiceItemGroups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_invoice_items_subscriptions_subscription_id",
                        column: x => x.SubscriptionId,
                        principalTable: "Subscriptions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_role_claims_role_id",
                table: "AspNetRoleClaims",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "normalized_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_user_claims_user_id",
                table: "AspNetUserClaims",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_user_logins_user_id",
                table: "AspNetUserLogins",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_user_passkeys_user_id",
                table: "AspNetUserPasskeys",
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
                name: "ix_contacts_created_by_id",
                table: "contacts",
                column: "created_by_id");

            migrationBuilder.CreateIndex(
                name: "ix_contacts_last_modified_by_id",
                table: "contacts",
                column: "last_modified_by_id");

            migrationBuilder.CreateIndex(
                name: "IX_CusPrice_CustomerId",
                table: "CusPrices",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CusPrice_TrackingUnitModelId",
                table: "CusPrices",
                column: "TrackingUnitModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_Account",
                table: "Customers",
                column: "Account");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_Email",
                table: "Customers",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_ParentId",
                table: "Customers",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_UserName",
                table: "Customers",
                column: "UserName");

            migrationBuilder.CreateIndex(
                name: "ix_documents_created_by_id",
                table: "documents",
                column: "created_by_id");

            migrationBuilder.CreateIndex(
                name: "ix_documents_last_modified_by_id",
                table: "documents",
                column: "last_modified_by_id");

            migrationBuilder.CreateIndex(
                name: "ix_documents_tenant_id",
                table: "documents",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItemGroup_InvoiceId",
                table: "InvoiceItemGroups",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItemGroup_ServiceLogId",
                table: "InvoiceItemGroups",
                column: "ServiceLogId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItem_InvoiceItemGroupId",
                table: "InvoiceItems",
                column: "InvoiceItemGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItem_SubscriptionId",
                table: "InvoiceItems",
                column: "SubscriptionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_CustomerId_IStatus_InvoiceDate",
                table: "Invoices",
                columns: new[] { "CustomerId", "IStatus", "InvoiceDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_InvoiceDate",
                table: "Invoices",
                column: "InvoiceDate");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_InvoiceNo",
                table: "Invoices",
                column: "InvoiceNo");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_IStatus",
                table: "Invoices",
                column: "IStatus");

            migrationBuilder.CreateIndex(
                name: "IX_LibyanaSimCard_SimCardNo",
                table: "LibyanaSimCards",
                column: "SimCardNo");

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
                name: "ix_service_logs_created_by_id",
                table: "ServiceLogs",
                column: "created_by_id");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceLog_CustomerId_SerDate",
                table: "ServiceLogs",
                columns: new[] { "CustomerId", "SerDate" });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceLog_ServiceNo",
                table: "ServiceLogs",
                column: "ServiceNo");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceLog_ServiceTask",
                table: "ServiceLogs",
                column: "ServiceTask");

            migrationBuilder.CreateIndex(
                name: "IX_ServicePrice_ServiceTask",
                table: "ServicePrices",
                column: "ServiceTask");

            migrationBuilder.CreateIndex(
                name: "IX_SimCard_ICCID",
                table: "SimCards",
                column: "ICCID");

            migrationBuilder.CreateIndex(
                name: "IX_SimCard_SimCardNo",
                table: "SimCards",
                column: "SimCardNo");

            migrationBuilder.CreateIndex(
                name: "IX_SimCard_SPackageId",
                table: "SimCards",
                column: "SPackageId");

            migrationBuilder.CreateIndex(
                name: "ix_s_packages_s_provider_id",
                table: "SPackages",
                column: "SProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_SPackage_Name",
                table: "SPackages",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_SProvider_Name",
                table: "SProviders",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_ServiceLogId",
                table: "Subscriptions",
                column: "ServiceLogId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_TrackingUnitId_Dates",
                table: "Subscriptions",
                columns: new[] { "TrackingUnitId", "SsDate", "SeDate" });

            migrationBuilder.CreateIndex(
                name: "ix_system_logs_level",
                table: "system_logs",
                column: "level");

            migrationBuilder.CreateIndex(
                name: "ix_system_logs_time_stamp",
                table: "system_logs",
                column: "time_stamp");

            migrationBuilder.CreateIndex(
                name: "ix_tenant_users_tenant_id",
                table: "tenant_users",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "ix_tenant_users_user_id",
                table: "tenant_users",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_tenants_name",
                table: "tenants",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_TicketNo",
                table: "Tickets",
                column: "TicketNo");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_TicketStatus",
                table: "Tickets",
                column: "TicketStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_TrackingUnitId_TicketStatus",
                table: "Tickets",
                columns: new[] { "TrackingUnitId", "TicketStatus" });

            migrationBuilder.CreateIndex(
                name: "ix_tickets_created_by_id",
                table: "Tickets",
                column: "created_by_id");

            migrationBuilder.CreateIndex(
                name: "ix_tickets_last_modified_by_id",
                table: "Tickets",
                column: "last_modified_by_id");

            migrationBuilder.CreateIndex(
                name: "IX_TrackedAsset_PlateNo",
                table: "TrackedAssets",
                column: "PlateNo");

            migrationBuilder.CreateIndex(
                name: "IX_TrackedAsset_TrackedAssetCode",
                table: "TrackedAssets",
                column: "TrackedAssetCode");

            migrationBuilder.CreateIndex(
                name: "IX_TrackedAsset_TrackedAssetNo",
                table: "TrackedAssets",
                column: "TrackedAssetNo");

            migrationBuilder.CreateIndex(
                name: "IX_TrackedAsset_VinSerNo",
                table: "TrackedAssets",
                column: "VinSerNo");

            migrationBuilder.CreateIndex(
                name: "IX_TrackingUnitModel_Name",
                table: "TrackingUnitModels",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_TrackingUnitModel_WialonName",
                table: "TrackingUnitModels",
                column: "WialonName");

            migrationBuilder.CreateIndex(
                name: "ix_tracking_units_sim_card_id",
                table: "TrackingUnits",
                column: "SimCardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_tracking_units_tracked_asset_id",
                table: "TrackingUnits",
                column: "TrackedAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackingUnit_CustomerId_UStatus",
                table: "TrackingUnits",
                columns: new[] { "CustomerId", "UStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_TrackingUnit_Imei",
                table: "TrackingUnits",
                column: "Imei",
                filter: "\"Imei\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TrackingUnit_SNo",
                table: "TrackingUnits",
                column: "SNo");

            migrationBuilder.CreateIndex(
                name: "IX_TrackingUnit_TrackingUnitModelId",
                table: "TrackingUnits",
                column: "TrackingUnitModelId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackingUnit_UStatus_IsOnWialon",
                table: "TrackingUnits",
                columns: new[] { "UStatus", "IsOnWialon" });

            migrationBuilder.CreateIndex(
                name: "IX_WialonTask_ServiceLogId",
                table: "WialonTasks",
                column: "ServiceLogId");

            migrationBuilder.CreateIndex(
                name: "IX_WialonTask_TrackingUnitId_IsExecuted",
                table: "WialonTasks",
                columns: new[] { "TrackingUnitId", "IsExecuted" });

            migrationBuilder.CreateIndex(
                name: "IX_WialonUnit_SimCardNo",
                table: "WialonUnits",
                column: "SimCardNo");

            migrationBuilder.CreateIndex(
                name: "IX_WialonUnit_UnitSNo",
                table: "WialonUnits",
                column: "UnitSNo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserPasskeys");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "audit_trails");

            migrationBuilder.DropTable(
                name: "contacts");

            migrationBuilder.DropTable(
                name: "CusPrices");

            migrationBuilder.DropTable(
                name: "data_protection_keys");

            migrationBuilder.DropTable(
                name: "documents");

            migrationBuilder.DropTable(
                name: "InvoiceItems");

            migrationBuilder.DropTable(
                name: "LibyanaSimCards");

            migrationBuilder.DropTable(
                name: "picklist_sets");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "ServicePrices");

            migrationBuilder.DropTable(
                name: "system_logs");

            migrationBuilder.DropTable(
                name: "tenant_users");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "WialonTasks");

            migrationBuilder.DropTable(
                name: "WialonUnits");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "InvoiceItemGroups");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "ServiceLogs");

            migrationBuilder.DropTable(
                name: "TrackingUnits");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "SimCards");

            migrationBuilder.DropTable(
                name: "TrackedAssets");

            migrationBuilder.DropTable(
                name: "TrackingUnitModels");

            migrationBuilder.DropTable(
                name: "tenants");

            migrationBuilder.DropTable(
                name: "SPackages");

            migrationBuilder.DropTable(
                name: "SProviders");
        }
    }
}
