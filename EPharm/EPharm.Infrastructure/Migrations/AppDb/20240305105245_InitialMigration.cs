using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EPharm.Infrastructure.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PharmaCompanies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CompanyName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Location = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ContactEmail = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ContactPhone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    AddressId = table.Column<int>(type: "integer", nullable: false),
                    PharmaCompanyOwnerId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PharmaCompanies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ActiveIngredients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PharmaCompanyId = table.Column<int>(type: "integer", nullable: false),
                    IngredientName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    IngredientDescription = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiveIngredients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActiveIngredients_PharmaCompanies_PharmaCompanyId",
                        column: x => x.PharmaCompanyId,
                        principalTable: "PharmaCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StreetAddress = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    PostalCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Region = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    BuildingNumber = table.Column<int>(type: "integer", nullable: false),
                    Floor = table.Column<int>(type: "integer", nullable: false),
                    RoomNumber = table.Column<int>(type: "integer", nullable: false),
                    PharmaCompanyId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_PharmaCompanies_PharmaCompanyId",
                        column: x => x.PharmaCompanyId,
                        principalTable: "PharmaCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Allergies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PharmaCompanyId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Allergies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Allergies_PharmaCompanies_PharmaCompanyId",
                        column: x => x.PharmaCompanyId,
                        principalTable: "PharmaCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DosageForms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PharmaCompanyId = table.Column<int>(type: "integer", nullable: false),
                    DosageFormName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DosageForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DosageForms_PharmaCompanies_PharmaCompanyId",
                        column: x => x.PharmaCompanyId,
                        principalTable: "PharmaCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Indications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PharmaCompanyId = table.Column<int>(type: "integer", nullable: false),
                    IndicationsName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    IndicationsDescription = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Indications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Indications_PharmaCompanies_PharmaCompanyId",
                        column: x => x.PharmaCompanyId,
                        principalTable: "PharmaCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Manufacturers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PharmaCompanyId = table.Column<int>(type: "integer", nullable: false),
                    ManufacturerName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ManufacturerLocation = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manufacturers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Manufacturers_PharmaCompanies_PharmaCompanyId",
                        column: x => x.PharmaCompanyId,
                        principalTable: "PharmaCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PharmaCompanyManagers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExternalId = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    PharmaCompanyId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PharmaCompanyManagers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PharmaCompanyManagers_PharmaCompanies_PharmaCompanyId",
                        column: x => x.PharmaCompanyId,
                        principalTable: "PharmaCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegulatoryInformations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PharmaCompanyId = table.Column<int>(type: "integer", nullable: false),
                    RegulatoryStandards = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ApprovalDate = table.Column<DateTime>(type: "date", nullable: false),
                    Certification = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegulatoryInformations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegulatoryInformations_PharmaCompanies_PharmaCompanyId",
                        column: x => x.PharmaCompanyId,
                        principalTable: "PharmaCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RouteOfAdministrations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PharmaCompanyId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteOfAdministrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RouteOfAdministrations_PharmaCompanies_PharmaCompanyId",
                        column: x => x.PharmaCompanyId,
                        principalTable: "PharmaCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SideEffects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PharmaCompanyId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SideEffects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SideEffects_PharmaCompanies_PharmaCompanyId",
                        column: x => x.PharmaCompanyId,
                        principalTable: "PharmaCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpecialRequirements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PharmaCompanyId = table.Column<int>(type: "integer", nullable: false),
                    MinimumAgeInMonthsRequirement = table.Column<int>(type: "integer", nullable: false),
                    MaximumAgeInMonthsRequirement = table.Column<int>(type: "integer", nullable: false),
                    MinimumWeighInKgRequirement = table.Column<decimal>(type: "numeric", nullable: false),
                    MaximumWeighInKgRequirement = table.Column<decimal>(type: "numeric", nullable: false),
                    MedicalConditionsDescription = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    OtherRequirementsDescription = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialRequirements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecialRequirements_PharmaCompanies_PharmaCompanyId",
                        column: x => x.PharmaCompanyId,
                        principalTable: "PharmaCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsageWarnings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PharmaCompanyId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsageWarnings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsageWarnings_PharmaCompanies_PharmaCompanyId",
                        column: x => x.PharmaCompanyId,
                        principalTable: "PharmaCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PharmaCompanyId = table.Column<int>(type: "integer", nullable: false),
                    ProductName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ProductDescription = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    StrengthMg = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ContraindicationsDescription = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    StorageConditionDescription = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    SpecialRequirementsId = table.Column<int>(type: "integer", nullable: false),
                    ManufacturerId = table.Column<int>(type: "integer", nullable: false),
                    ManufacturingDate = table.Column<DateTime>(type: "date", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "date", nullable: false),
                    RegulatoryInformationId = table.Column<int>(type: "integer", nullable: false),
                    Stock = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<double>(type: "double precision", nullable: false),
                    BatchNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Barcode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PackagingWidth = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    PackagingHeight = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    PackagingLength = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    PackagingWeight = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Manufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_PharmaCompanies_PharmaCompanyId",
                        column: x => x.PharmaCompanyId,
                        principalTable: "PharmaCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_RegulatoryInformations_RegulatoryInformationId",
                        column: x => x.RegulatoryInformationId,
                        principalTable: "RegulatoryInformations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_SpecialRequirements_SpecialRequirementsId",
                        column: x => x.SpecialRequirementsId,
                        principalTable: "SpecialRequirements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IndicationProducts",
                columns: table => new
                {
                    IndicationId = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndicationProducts", x => new { x.IndicationId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_IndicationProducts_Indications_IndicationId",
                        column: x => x.IndicationId,
                        principalTable: "Indications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IndicationProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductActiveIngredients",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    ActiveIngredientId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductActiveIngredients", x => new { x.ProductId, x.ActiveIngredientId });
                    table.ForeignKey(
                        name: "FK_ProductActiveIngredients_ActiveIngredients_ActiveIngredient~",
                        column: x => x.ActiveIngredientId,
                        principalTable: "ActiveIngredients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductActiveIngredients_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductAllergies",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    AllergyId = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAllergies", x => new { x.ProductId, x.AllergyId });
                    table.ForeignKey(
                        name: "FK_ProductAllergies_Allergies_AllergyId",
                        column: x => x.AllergyId,
                        principalTable: "Allergies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductAllergies_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductDosageForms",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    DosageFormId = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDosageForms", x => new { x.ProductId, x.DosageFormId });
                    table.ForeignKey(
                        name: "FK_ProductDosageForms_DosageForms_DosageFormId",
                        column: x => x.DosageFormId,
                        principalTable: "DosageForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductDosageForms_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImages_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductRouteOfAdministrations",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    RouteOfAdministrationId = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductRouteOfAdministrations", x => new { x.ProductId, x.RouteOfAdministrationId });
                    table.ForeignKey(
                        name: "FK_ProductRouteOfAdministrations_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductRouteOfAdministrations_RouteOfAdministrations_RouteO~",
                        column: x => x.RouteOfAdministrationId,
                        principalTable: "RouteOfAdministrations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductSideEffects",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    SideEffectId = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSideEffects", x => new { x.ProductId, x.SideEffectId });
                    table.ForeignKey(
                        name: "FK_ProductSideEffects_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductSideEffects_SideEffects_SideEffectId",
                        column: x => x.SideEffectId,
                        principalTable: "SideEffects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductUsageWarnings",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    UsageWarningId = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductUsageWarnings", x => new { x.ProductId, x.UsageWarningId });
                    table.ForeignKey(
                        name: "FK_ProductUsageWarnings_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductUsageWarnings_UsageWarnings_UsageWarningId",
                        column: x => x.UsageWarningId,
                        principalTable: "UsageWarnings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActiveIngredients_PharmaCompanyId",
                table: "ActiveIngredients",
                column: "PharmaCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_PharmaCompanyId",
                table: "Addresses",
                column: "PharmaCompanyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Allergies_PharmaCompanyId",
                table: "Allergies",
                column: "PharmaCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_DosageForms_PharmaCompanyId",
                table: "DosageForms",
                column: "PharmaCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_IndicationProducts_ProductId",
                table: "IndicationProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Indications_PharmaCompanyId",
                table: "Indications",
                column: "PharmaCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Manufacturers_PharmaCompanyId",
                table: "Manufacturers",
                column: "PharmaCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_PharmaCompanyManagers_PharmaCompanyId",
                table: "PharmaCompanyManagers",
                column: "PharmaCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductActiveIngredients_ActiveIngredientId",
                table: "ProductActiveIngredients",
                column: "ActiveIngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAllergies_AllergyId",
                table: "ProductAllergies",
                column: "AllergyId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDosageForms_DosageFormId",
                table: "ProductDosageForms",
                column: "DosageFormId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImages",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRouteOfAdministrations_RouteOfAdministrationId",
                table: "ProductRouteOfAdministrations",
                column: "RouteOfAdministrationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSideEffects_SideEffectId",
                table: "ProductSideEffects",
                column: "SideEffectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductUsageWarnings_UsageWarningId",
                table: "ProductUsageWarnings",
                column: "UsageWarningId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ManufacturerId",
                table: "Products",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_PharmaCompanyId",
                table: "Products",
                column: "PharmaCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_RegulatoryInformationId",
                table: "Products",
                column: "RegulatoryInformationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_SpecialRequirementsId",
                table: "Products",
                column: "SpecialRequirementsId");

            migrationBuilder.CreateIndex(
                name: "IX_RegulatoryInformations_PharmaCompanyId",
                table: "RegulatoryInformations",
                column: "PharmaCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteOfAdministrations_PharmaCompanyId",
                table: "RouteOfAdministrations",
                column: "PharmaCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_SideEffects_PharmaCompanyId",
                table: "SideEffects",
                column: "PharmaCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialRequirements_PharmaCompanyId",
                table: "SpecialRequirements",
                column: "PharmaCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_UsageWarnings_PharmaCompanyId",
                table: "UsageWarnings",
                column: "PharmaCompanyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "IndicationProducts");

            migrationBuilder.DropTable(
                name: "PharmaCompanyManagers");

            migrationBuilder.DropTable(
                name: "ProductActiveIngredients");

            migrationBuilder.DropTable(
                name: "ProductAllergies");

            migrationBuilder.DropTable(
                name: "ProductDosageForms");

            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.DropTable(
                name: "ProductRouteOfAdministrations");

            migrationBuilder.DropTable(
                name: "ProductSideEffects");

            migrationBuilder.DropTable(
                name: "ProductUsageWarnings");

            migrationBuilder.DropTable(
                name: "Indications");

            migrationBuilder.DropTable(
                name: "ActiveIngredients");

            migrationBuilder.DropTable(
                name: "Allergies");

            migrationBuilder.DropTable(
                name: "DosageForms");

            migrationBuilder.DropTable(
                name: "RouteOfAdministrations");

            migrationBuilder.DropTable(
                name: "SideEffects");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "UsageWarnings");

            migrationBuilder.DropTable(
                name: "Manufacturers");

            migrationBuilder.DropTable(
                name: "RegulatoryInformations");

            migrationBuilder.DropTable(
                name: "SpecialRequirements");

            migrationBuilder.DropTable(
                name: "PharmaCompanies");
        }
    }
}
