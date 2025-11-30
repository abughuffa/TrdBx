using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Blazor.Migrators.SqLite.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseUpdate001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActivateGprsTestCases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TrackingUnitId = table.Column<int>(type: "INTEGER", nullable: true),
                    InstallerId = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    SNo = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    TsDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    CaseCode = table.Column<int>(type: "INTEGER", nullable: true),
                    IsSucssed = table.Column<bool>(type: "INTEGER", nullable: true),
                    Message = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivateGprsTestCases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ActivateHostingTestCases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TrackingUnitId = table.Column<int>(type: "INTEGER", nullable: true),
                    InstallerId = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    SNo = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    TsDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    CaseCode = table.Column<int>(type: "INTEGER", nullable: true),
                    IsSucssed = table.Column<bool>(type: "INTEGER", nullable: true),
                    Message = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivateHostingTestCases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ActivateTestCases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TrackingUnitId = table.Column<int>(type: "INTEGER", nullable: true),
                    InstallerId = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    SNo = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    TsDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    CaseCode = table.Column<int>(type: "INTEGER", nullable: true),
                    IsSucssed = table.Column<bool>(type: "INTEGER", nullable: true),
                    Message = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivateTestCases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    Country = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ParentId = table.Column<int>(type: "INTEGER", nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Account = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    BillingPlan = table.Column<int>(type: "INTEGER", nullable: false),
                    IsTaxable = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsRenewable = table.Column<bool>(type: "INTEGER", nullable: false),
                    WUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    WUnitGroupId = table.Column<int>(type: "INTEGER", nullable: true),
                    Address = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    Mobile1 = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    Mobile2 = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    IsAvaliable = table.Column<bool>(type: "INTEGER", nullable: false),
                    OldId = table.Column<int>(type: "INTEGER", nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_Customers_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DataProtectionKeys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FriendlyName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    Xml = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataProtectionKeys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeactivateTestCases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TrackingUnitId = table.Column<int>(type: "INTEGER", nullable: true),
                    InstallerId = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    SNo = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    TsDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    CaseCode = table.Column<int>(type: "INTEGER", nullable: true),
                    IsSucssed = table.Column<bool>(type: "INTEGER", nullable: true),
                    Message = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeactivateTestCases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LibyanaSimCards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SimCardNo = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    SimCardStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    Balance = table.Column<decimal>(type: "TEXT", nullable: true),
                    BExDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    JoinDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Package = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    DExDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DataOffer = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    DOExpired = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LibyanaSimCards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Loggers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Message = table.Column<string>(type: "TEXT", maxLength: 2147483647, nullable: true),
                    MessageTemplate = table.Column<string>(type: "TEXT", maxLength: 2147483647, nullable: true),
                    Level = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Exception = table.Column<string>(type: "TEXT", maxLength: 2147483647, nullable: true),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    ClientIP = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    ClientAgent = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    Properties = table.Column<string>(type: "TEXT", maxLength: 2147483647, nullable: true),
                    LogEvent = table.Column<string>(type: "TEXT", maxLength: 2147483647, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loggers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PicklistSets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    Value = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Text = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PicklistSets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    Brand = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    Unit = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    Pictures = table.Column<string>(type: "TEXT", nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServicePrices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ServiceTask = table.Column<int>(type: "INTEGER", nullable: false),
                    Desc = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicePrices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SPackages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    OldId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SPackages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    Description = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrackedAssets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TrackedAssetNo = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    TrackedAssetCode = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    VinSerNo = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    PlateNo = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    TrackedAssetDesc = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    IsAvaliable = table.Column<bool>(type: "INTEGER", nullable: false),
                    OldId = table.Column<int>(type: "INTEGER", nullable: true),
                    OldVehicleNo = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackedAssets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrackingUnitModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WialonName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    WhwTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    DefualtHost = table.Column<decimal>(type: "TEXT", nullable: false),
                    DefualtGprs = table.Column<decimal>(type: "TEXT", nullable: false),
                    DefualtPrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    PortNo1 = table.Column<int>(type: "INTEGER", nullable: false),
                    PortNo2 = table.Column<int>(type: "INTEGER", nullable: false),
                    OldId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackingUnitModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Image = table.Column<byte>(type: "INTEGER", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Warehouses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Latitude = table.Column<double>(type: "REAL", nullable: false),
                    Longitude = table.Column<double>(type: "REAL", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WialonUnits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UnitName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    Account = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    UnitSNo = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    Deactivation = table.Column<DateTime>(type: "TEXT", nullable: true),
                    SimCardNo = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    StatusOnWialon = table.Column<int>(type: "INTEGER", nullable: false),
                    Note = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WialonUnits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InvNo = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    InvDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    DueDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    InvoiceType = table.Column<int>(type: "INTEGER", nullable: false),
                    IStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: false),
                    InvDesc = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    Total = table.Column<decimal>(type: "TEXT", nullable: false),
                    Taxes = table.Column<decimal>(type: "TEXT", nullable: false),
                    GrangTotal = table.Column<decimal>(type: "TEXT", nullable: false),
                    IsTaxable = table.Column<bool>(type: "INTEGER", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SimCards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SimCardNo = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    ICCID = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    SPackageId = table.Column<int>(type: "INTEGER", nullable: false),
                    SStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    ExDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    OldId = table.Column<int>(type: "INTEGER", nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SimCards_SPackages_SPackageId",
                        column: x => x.SPackageId,
                        principalTable: "SPackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    TenantId = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    Description = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoles_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    DisplayName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    Provider = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    TenantId = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    ProfilePictureDataUrl = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsLive = table.Column<bool>(type: "INTEGER", nullable: false),
                    RefreshToken = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SuperiorId = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    TimeZoneId = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    LanguageCode = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUsers_AspNetUsers_LastModifiedBy",
                        column: x => x.LastModifiedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUsers_AspNetUsers_SuperiorId",
                        column: x => x.SuperiorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CusPrices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AssignedTo = table.Column<int>(type: "INTEGER", nullable: false),
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: false),
                    TrackingUnitModelId = table.Column<int>(type: "INTEGER", nullable: false),
                    Host = table.Column<decimal>(type: "TEXT", nullable: false),
                    Gprs = table.Column<decimal>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CusPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CusPrices_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CusPrices_TrackingUnitModels_TrackingUnitModelId",
                        column: x => x.TrackingUnitModelId,
                        principalTable: "TrackingUnitModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrackingUnits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SNo = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Imei = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    UnitName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    TrackingUnitModelId = table.Column<int>(type: "INTEGER", nullable: false),
                    UStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    InsMode = table.Column<int>(type: "INTEGER", nullable: false),
                    WryDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    TrackedAssetId = table.Column<int>(type: "INTEGER", nullable: true),
                    SimCardId = table.Column<int>(type: "INTEGER", nullable: true),
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: true),
                    IsOnWialon = table.Column<bool>(type: "INTEGER", nullable: false),
                    WStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    WUnitId = table.Column<int>(type: "INTEGER", nullable: true),
                    OldId = table.Column<int>(type: "INTEGER", nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackingUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrackingUnits_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TrackingUnits_SimCards_SimCardId",
                        column: x => x.SimCardId,
                        principalTable: "SimCards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TrackingUnits_TrackedAssets_TrackedAssetId",
                        column: x => x.TrackedAssetId,
                        principalTable: "TrackedAssets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TrackingUnits_TrackingUnitModels_TrackingUnitModelId",
                        column: x => x.TrackingUnitModelId,
                        principalTable: "TrackingUnitModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    Group = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    RoleId = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    UserId = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    UserId = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    RoleId = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    Value = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuditTrails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    AuditType = table.Column<string>(type: "TEXT", nullable: false),
                    TableName = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    DateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    OldValues = table.Column<string>(type: "TEXT", nullable: true),
                    NewValues = table.Column<string>(type: "TEXT", nullable: true),
                    AffectedColumns = table.Column<string>(type: "TEXT", nullable: true),
                    PrimaryKey = table.Column<string>(type: "TEXT", nullable: false),
                    DebugView = table.Column<string>(type: "TEXT", maxLength: 2147483647, nullable: true),
                    ErrorMessage = table.Column<string>(type: "TEXT", maxLength: 2147483647, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditTrails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditTrails_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    Description = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Content = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: true),
                    IsPublic = table.Column<bool>(type: "INTEGER", nullable: false),
                    URL = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    DocumentType = table.Column<string>(type: "TEXT", nullable: false),
                    TenantId = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_AspNetUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Documents_AspNetUsers_LastModifiedBy",
                        column: x => x.LastModifiedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Documents_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ServiceLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ServiceNo = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    ServiceTask = table.Column<int>(type: "INTEGER", nullable: false),
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: false),
                    InstallerId = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    Desc = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    SerDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    IsDeserved = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsBilled = table.Column<bool>(type: "INTEGER", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceLogs_AspNetUsers_InstallerId",
                        column: x => x.InstallerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceLogs_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VehicleNo = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    VehicleTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    DriverId = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehicles_AspNetUsers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Vehicles_VehicleTypes_VehicleTypeId",
                        column: x => x.VehicleTypeId,
                        principalTable: "VehicleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TicketNo = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    ServiceTask = table.Column<int>(type: "INTEGER", nullable: false),
                    Desc = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    TicketStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    TrackingUnitId = table.Column<int>(type: "INTEGER", nullable: false),
                    TcDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    TaDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    InstallerId = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    TeDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    Note = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_AspNetUsers_InstallerId",
                        column: x => x.InstallerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tickets_TrackingUnits_TrackingUnitId",
                        column: x => x.TrackingUnitId,
                        principalTable: "TrackingUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InvoiceId = table.Column<int>(type: "INTEGER", nullable: false),
                    ServiceLogId = table.Column<int>(type: "INTEGER", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceItems_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceItems_ServiceLogs_ServiceLogId",
                        column: x => x.ServiceLogId,
                        principalTable: "ServiceLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ServiceLogId = table.Column<int>(type: "INTEGER", nullable: false),
                    TrackingUnitId = table.Column<int>(type: "INTEGER", nullable: false),
                    CaseCode = table.Column<int>(type: "INTEGER", nullable: false),
                    LastPaidFees = table.Column<int>(type: "INTEGER", nullable: false),
                    Desc = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    SsDate = table.Column<DateOnly>(type: "TEXT", maxLength: 256, nullable: false),
                    SeDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Days = table.Column<int>(type: "INTEGER", nullable: false, computedColumnSql: "julianday(SeDate) - julianday(SsDate)", stored: true),
                    DailyFees = table.Column<decimal>(type: "TEXT", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false, computedColumnSql: "(julianday(SeDate) - julianday(SsDate)) * DailyFees", stored: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscriptions_ServiceLogs_ServiceLogId",
                        column: x => x.ServiceLogId,
                        principalTable: "ServiceLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Subscriptions_TrackingUnits_TrackingUnitId",
                        column: x => x.TrackingUnitId,
                        principalTable: "TrackingUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WialonTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ServiceLogId = table.Column<int>(type: "INTEGER", nullable: false),
                    TrackingUnitId = table.Column<int>(type: "INTEGER", nullable: false),
                    Desc = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    APITask = table.Column<int>(type: "INTEGER", nullable: true),
                    ExcDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    IsExecuted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WialonTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WialonTasks_ServiceLogs_ServiceLogId",
                        column: x => x.ServiceLogId,
                        principalTable: "ServiceLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WialonTasks_TrackingUnits_TrackingUnitId",
                        column: x => x.TrackingUnitId,
                        principalTable: "TrackingUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shipments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ShipmentNo = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    ShipmentStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    StartLocation = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    EndLocation = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    IsBidable = table.Column<bool>(type: "INTEGER", nullable: false),
                    VehicleId = table.Column<int>(type: "INTEGER", nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shipments_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ShipmentVehicleType",
                columns: table => new
                {
                    ShipmentId = table.Column<int>(type: "INTEGER", nullable: false),
                    VehicleTypeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipmentVehicleType", x => new { x.ShipmentId, x.VehicleTypeId });
                    table.ForeignKey(
                        name: "FK_ShipmentVehicleType_Shipments_ShipmentId",
                        column: x => x.ShipmentId,
                        principalTable: "Shipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShipmentVehicleType_VehicleTypes_VehicleTypeId",
                        column: x => x.VehicleTypeId,
                        principalTable: "VehicleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_TenantId_Name",
                table: "AspNetRoles",
                columns: new[] { "TenantId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CreatedBy",
                table: "AspNetUsers",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_LastModifiedBy",
                table: "AspNetUsers",
                column: "LastModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SuperiorId",
                table: "AspNetUsers",
                column: "SuperiorId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TenantId",
                table: "AspNetUsers",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditTrails_UserId",
                table: "AuditTrails",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CusPrices_CustomerId",
                table: "CusPrices",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CusPrices_TrackingUnitModelId",
                table: "CusPrices",
                column: "TrackingUnitModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Name",
                table: "Customers",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_ParentId",
                table: "Customers",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_CreatedBy",
                table: "Documents",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_LastModifiedBy",
                table: "Documents",
                column: "LastModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_TenantId",
                table: "Documents",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItems_InvoiceId",
                table: "InvoiceItems",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItems_ServiceLogId",
                table: "InvoiceItems",
                column: "ServiceLogId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_CustomerId",
                table: "Invoices",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_InvNo",
                table: "Invoices",
                column: "InvNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Loggers_Level",
                table: "Loggers",
                column: "Level");

            migrationBuilder.CreateIndex(
                name: "IX_Loggers_TimeStamp",
                table: "Loggers",
                column: "TimeStamp");

            migrationBuilder.CreateIndex(
                name: "IX_PicklistSets_Name_Value",
                table: "PicklistSets",
                columns: new[] { "Name", "Value" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_Name",
                table: "Products",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceLogs_CustomerId",
                table: "ServiceLogs",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceLogs_InstallerId",
                table: "ServiceLogs",
                column: "InstallerId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceLogs_ServiceNo",
                table: "ServiceLogs",
                column: "ServiceNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_VehicleId",
                table: "Shipments",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_ShipmentVehicleType_VehicleTypeId",
                table: "ShipmentVehicleType",
                column: "VehicleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SimCards_SimCardNo",
                table: "SimCards",
                column: "SimCardNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SimCards_SPackageId",
                table: "SimCards",
                column: "SPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_SPackages_Name",
                table: "SPackages",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_ServiceLogId",
                table: "Subscriptions",
                column: "ServiceLogId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_TrackingUnitId",
                table: "Subscriptions",
                column: "TrackingUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_InstallerId",
                table: "Tickets",
                column: "InstallerId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_TicketNo",
                table: "Tickets",
                column: "TicketNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_TrackingUnitId",
                table: "Tickets",
                column: "TrackingUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackedAssets_TrackedAssetNo",
                table: "TrackedAssets",
                column: "TrackedAssetNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrackingUnitModels_Name",
                table: "TrackingUnitModels",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrackingUnits_CustomerId",
                table: "TrackingUnits",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackingUnits_SimCardId",
                table: "TrackingUnits",
                column: "SimCardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrackingUnits_TrackedAssetId",
                table: "TrackingUnits",
                column: "TrackedAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackingUnits_TrackingUnitModelId",
                table: "TrackingUnits",
                column: "TrackingUnitModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_DriverId",
                table: "Vehicles",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_VehicleTypeId",
                table: "Vehicles",
                column: "VehicleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WialonTasks_ServiceLogId",
                table: "WialonTasks",
                column: "ServiceLogId");

            migrationBuilder.CreateIndex(
                name: "IX_WialonTasks_TrackingUnitId",
                table: "WialonTasks",
                column: "TrackingUnitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivateGprsTestCases");

            migrationBuilder.DropTable(
                name: "ActivateHostingTestCases");

            migrationBuilder.DropTable(
                name: "ActivateTestCases");

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
                name: "AuditTrails");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "CusPrices");

            migrationBuilder.DropTable(
                name: "DataProtectionKeys");

            migrationBuilder.DropTable(
                name: "DeactivateTestCases");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "InvoiceItems");

            migrationBuilder.DropTable(
                name: "LibyanaSimCards");

            migrationBuilder.DropTable(
                name: "Loggers");

            migrationBuilder.DropTable(
                name: "PicklistSets");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "ServicePrices");

            migrationBuilder.DropTable(
                name: "ShipmentVehicleType");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Warehouses");

            migrationBuilder.DropTable(
                name: "WialonTasks");

            migrationBuilder.DropTable(
                name: "WialonUnits");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "Shipments");

            migrationBuilder.DropTable(
                name: "ServiceLogs");

            migrationBuilder.DropTable(
                name: "TrackingUnits");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "SimCards");

            migrationBuilder.DropTable(
                name: "TrackedAssets");

            migrationBuilder.DropTable(
                name: "TrackingUnitModels");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "VehicleTypes");

            migrationBuilder.DropTable(
                name: "SPackages");

            migrationBuilder.DropTable(
                name: "Tenants");
        }
    }
}
