﻿// <auto-generated />
using System;
using EPharm.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EPharm.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240229123403_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("EPharm.Infrastructure.Context.Entities.CommonEntities.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("BuildingNumber")
                        .HasColumnType("integer");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("NOW()");

                    b.Property<int>("Floor")
                        .HasColumnType("integer");

                    b.Property<int>("PharmaCompanyId")
                        .HasColumnType("integer");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("Region")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("RoomNumber")
                        .HasColumnType("integer");

                    b.Property<string>("StreetAddress")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.HasKey("Id");

                    b.HasIndex("PharmaCompanyId")
                        .IsUnique();

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("EPharm.Infrastructure.Context.Entities.Junctions.IndicationProduct", b =>
                {
                    b.Property<int>("IndicationId")
                        .HasColumnType("integer");

                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.HasKey("IndicationId", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("IndicationProducts");
                });

            modelBuilder.Entity("EPharm.Infrastructure.Context.Entities.Junctions.ProductActiveIngredient", b =>
                {
                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.Property<int>("ActiveIngredientId")
                        .HasColumnType("integer");

                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<decimal>("Quantity")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("ProductId", "ActiveIngredientId");

                    b.HasIndex("ActiveIngredientId");

                    b.ToTable("ProductActiveIngredients");
                });

            modelBuilder.Entity("EPharm.Infrastructure.Context.Entities.Junctions.ProductAllergy", b =>
                {
                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.Property<int>("AllergyId")
                        .HasColumnType("integer");

                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.HasKey("ProductId", "AllergyId");

                    b.HasIndex("AllergyId");

                    b.ToTable("ProductAllergies");
                });

            modelBuilder.Entity("EPharm.Infrastructure.Context.Entities.Junctions.ProductDosageForm", b =>
                {
                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.Property<int>("DosageFormId")
                        .HasColumnType("integer");

                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.HasKey("ProductId", "DosageFormId");

                    b.HasIndex("DosageFormId");

                    b.ToTable("ProductDosageForms");
                });

            modelBuilder.Entity("EPharm.Infrastructure.Context.Entities.Junctions.ProductRouteOfAdministration", b =>
                {
                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.Property<int>("RouteOfAdministrationId")
                        .HasColumnType("integer");

                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.HasKey("ProductId", "RouteOfAdministrationId");

                    b.HasIndex("RouteOfAdministrationId");

                    b.ToTable("ProductRouteOfAdministrations");
                });

            modelBuilder.Entity("EPharm.Infrastructure.Context.Entities.Junctions.ProductSideEffect", b =>
                {
                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.Property<int>("SideEffectId")
                        .HasColumnType("integer");

                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.HasKey("ProductId", "SideEffectId");

                    b.HasIndex("SideEffectId");

                    b.ToTable("ProductSideEffects");
                });

            modelBuilder.Entity("EPharm.Infrastructure.Context.Entities.Junctions.ProductUsageWarning", b =>
                {
                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.Property<int>("UsageWarningId")
                        .HasColumnType("integer");

                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.HasKey("ProductId", "UsageWarningId");

                    b.HasIndex("UsageWarningId");

                    b.ToTable("ProductUsageWarnings");
                });

            modelBuilder.Entity("EPharm.Infrastructure.Context.Entities.PharmaEntities.PharmaCompany", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AddressId")
                        .HasColumnType("integer");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("ContactEmail")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("ContactPhone")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("NOW()");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<int>("PharmaCompanyOwnerId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("PharmaCompanies");
                });

            modelBuilder.Entity("EPharm.Infrastructure.Context.Entities.PharmaEntities.PharmaCompanyManager", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("NOW()");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("ExternalId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("PharmaCompanyId")
                        .HasColumnType("integer");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.HasKey("Id");

                    b.HasIndex("PharmaCompanyId");

                    b.ToTable("PharmaCompanyManagers");
                });

            modelBuilder.Entity("EPharm.Infrastructure.Context.Entities.ProductEntities.ActiveIngredient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("NOW()");

                    b.Property<string>("IngredientDescription")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("IngredientName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.HasKey("Id");

                    b.ToTable("ActiveIngredients");
                });

            modelBuilder.Entity("EPharm.Infrastructure.Context.Entities.ProductEntities.Allergy", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("NOW()");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.HasKey("Id");

                    b.ToTable("Allergies");
                });

            modelBuilder.Entity("EPharm.Infrastructure.Context.Entities.ProductEntities.DosageForm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("DosageFormName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.HasKey("Id");

                    b.ToTable("DosageForms");
                });

            modelBuilder.Entity("EPharm.Infrastructure.Context.Entities.ProductEntities.Indication", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("NOW()");

                    b.Property<string>("IndicationsDescription")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("IndicationsName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.HasKey("Id");

                    b.ToTable("Indications");
                });

            modelBuilder.Entity("EPharm.Infrastructure.Context.Entities.ProductEntities.Manufacturer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("NOW()");

                    b.Property<string>("ManufacturerLocation")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("ManufacturerName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.HasKey("Id");

                    b.ToTable("Manufacturers");
                });

            modelBuilder.Entity("EPharm.Infrastructure.Context.Entities.ProductEntities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Barcode")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("BatchNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("ContraindicationsDescription")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("NOW()");

                    b.Property<DateTime>("ExpiryDate")
                        .HasColumnType("date");

                    b.Property<int>("ManufacturerId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("ManufacturingDate")
                        .HasColumnType("date");

                    b.Property<decimal>("PackagingHeight")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("PackagingLength")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("PackagingWeight")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("PackagingWidth")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("PharmaCompanyId")
                        .HasColumnType("integer");

                    b.Property<double>("Price")
                        .HasColumnType("double precision");

                    b.Property<string>("ProductDescription")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<int>("RegulatoryInformationId")
                        .HasColumnType("integer");

                    b.Property<int>("SpecialRequirementsId")
                        .HasColumnType("integer");

                    b.Property<int>("Stock")
                        .HasColumnType("integer");

                    b.Property<string>("StorageConditionDescription")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<decimal>("StrengthMg")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("ManufacturerId");

                    b.HasIndex("PharmaCompanyId");

                    b.HasIndex("RegulatoryInformationId")
                        .IsUnique();

                    b.HasIndex("SpecialRequirementsId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("EPharm.Infrastructure.Context.Entities.ProductEntities.ProductImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("NOW()");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductImages");
                });

            modelBuilder.Entity("EPharm.Infrastructure.Context.Entities.ProductEntities.RegulatoryInformation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("ApprovalDate")
                        .HasColumnType("date");

                    b.Property<string>("Certification")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("NOW()");

                    b.Property<string>("RegulatoryStandards")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.HasKey("Id");

                    b.ToTable("RegulatoryInformations");
                });

            modelBuilder.Entity("EPharm.Infrastructure.Context.Entities.ProductEntities.RouteOfAdministration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("NOW()");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.HasKey("Id");

                    b.ToTable("RouteOfAdministrations");
                });

            modelBuilder.Entity("EPharm.Infrastructure.Context.Entities.ProductEntities.SideEffect", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("NOW()");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.HasKey("Id");

                    b.ToTable("SideEffects");
                });

            modelBuilder.Entity("EPharm.Infrastructure.Context.Entities.ProductEntities.SpecialRequirement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("NOW()");

                    b.Property<int>("MaximumAgeInMonthsRequirement")
                        .HasColumnType("integer");

                    b.Property<decimal>("MaximumWeighInKgRequirement")
                        .HasColumnType("numeric");

                    b.Property<string>("MedicalConditionsDescription")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<int>("MinimumAgeInMonthsRequirement")
                        .HasColumnType("integer");

                    b.Property<decimal>("MinimumWeighInKgRequirement")
                        .HasColumnType("numeric");

                    b.Property<string>("OtherRequirementsDescription")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.HasKey("Id");

                    b.ToTable("SpecialRequirements");
                });

            modelBuilder.Entity("EPharm.Infrastructure.Context.Entities.ProductEntities.UsageWarning", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("NOW()");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.HasKey("Id");

                    b.ToTable("UsageWarnings");
                });

            modelBuilder.Entity("EPharm.Infrastructure.Context.Entities.CommonEntities.Address", b =>
                {
                    b.HasOne("EPharm.Infrastructure.Context.Entities.PharmaEntities.PharmaCompany", "PharmaCompany")
                        .WithOne("Address")
                        .HasForeignKey("EPharm.Infrastructure.Context.Entities.CommonEntities.Address", "PharmaCompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PharmaCompany");
                });

            modelBuilder.Entity("EPharm.Infrastructure.Context.Entities.Junctions.IndicationProduct", b =>
                {
                    b.HasOne("EPharm.Infrastructure.Context.Entities.ProductEntities.Indication", "Indication")
                        .WithMany("Products")
                        .HasForeignKey("IndicationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EPharm.Infrastructure.Context.Entities.ProductEntities.Product", "Product")
                        .WithMany("Indications")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Indication");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("EPharm.Infrastructure.Context.Entities.Junctions.ProductActiveIngredient", b =>
                {
                    b.HasOne("EPharm.Infrastructure.Context.Entities.ProductEntities.ActiveIngredient", "ActiveIngredient")
                        .WithMany("Products")
                        .HasForeignKey("ActiveIngredientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EPharm.Infrastructure.Context.Entities.ProductEntities.Product", "Product")
                        .WithMany("ActiveIngredients")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ActiveIngredient");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("EPharm.Infrastructure.Context.Entities.Junctions.ProductAllergy", b =>
                {
                    b.HasOne("EPharm.Infrastructure.Context.Entities.ProductEntities.Allergy", "Allergy")
                        .WithMany("Products")
                        .HasForeignKey("AllergyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EPharm.Infrastructure.Context.Entities.ProductEntities.Product", "Product")
                        .WithMany("Allergies")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Allergy");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("EPharm.Infrastructure.Context.Entities.Junctions.ProductDosageForm", b =>
                {
                    b.HasOne("EPharm.Infrastructure.Context.Entities.ProductEntities.DosageForm", "DosageForm")
                        .WithMany("Products")
                        .HasForeignKey("DosageFormId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EPharm.Infrastructure.Context.Entities.ProductEntities.Product", "Product")
                        .WithMany("DosageForms")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DosageForm");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("EPharm.Infrastructure.Context.Entities.Junctions.ProductRouteOfAdministration", b =>
                {
                    b.HasOne("EPharm.Infrastructure.Context.Entities.ProductEntities.Product", "Product")
                        .WithMany("RouteOfAdministrations")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EPharm.Infrastructure.Context.Entities.ProductEntities.RouteOfAdministration", "RouteOfAdministration")
                        .WithMany("Products")
                        .HasForeignKey("RouteOfAdministrationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("RouteOfAdministration");
                });

            modelBuilder.Entity("EPharm.Infrastructure.Context.Entities.Junctions.ProductSideEffect", b =>
                {
                    b.HasOne("EPharm.Infrastructure.Context.Entities.ProductEntities.Product", "Product")
                        .WithMany("SideEffects")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EPharm.Infrastructure.Context.Entities.ProductEntities.SideEffect", "SideEffect")
                        .WithMany("Products")
                        .HasForeignKey("SideEffectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("SideEffect");
                });

            modelBuilder.Entity("EPharm.Infrastructure.Context.Entities.Junctions.ProductUsageWarning", b =>
                {
                    b.HasOne("EPharm.Infrastructure.Context.Entities.ProductEntities.Product", "Product")
                        .WithMany("UsageWarnings")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EPharm.Infrastructure.Context.Entities.ProductEntities.UsageWarning", "UsageWarning")
                        .WithMany("Products")
                        .HasForeignKey("UsageWarningId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("UsageWarning");
                });

            modelBuilder.Entity("EPharm.Infrastructure.Context.Entities.PharmaEntities.PharmaCompanyManager", b =>
                {
                    b.HasOne("EPharm.Infrastructure.Context.Entities.PharmaEntities.PharmaCompany", "PharmaCompany")
                        .WithMany("PharmaCompanyManagers")
                        .HasForeignKey("PharmaCompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PharmaCompany");
                });

            modelBuilder.Entity("EPharm.Infrastructure.Context.Entities.ProductEntities.Product", b =>
                {
                    b.HasOne("EPharm.Infrastructure.Context.Entities.ProductEntities.Manufacturer", "Manufacturer")
                        .WithMany("Products")
                        .HasForeignKey("ManufacturerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EPharm.Infrastructure.Context.Entities.PharmaEntities.PharmaCompany", "PharmaCompany")
                        .WithMany("Products")
                        .HasForeignKey("PharmaCompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EPharm.Infrastructure.Context.Entities.ProductEntities.RegulatoryInformation", "RegulatoryInformation")
                        .WithOne("Product")
                        .HasForeignKey("EPharm.Infrastructure.Context.Entities.ProductEntities.Product", "RegulatoryInformationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EPharm.Infrastructure.Context.Entities.ProductEntities.SpecialRequirement", "SpecialRequirement")
                        .WithMany("Products")
                        .HasForeignKey("SpecialRequirementsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Manufacturer");

                    b.Navigation("PharmaCompany");

                    b.Navigation("RegulatoryInformation");

                    b.Navigation("SpecialRequirement");
                });

            modelBuilder.Entity("EPharm.Infrastructure.Context.Entities.ProductEntities.ProductImage", b =>
                {
                    b.HasOne("EPharm.Infrastructure.Context.Entities.ProductEntities.Product", "Product")
                        .WithMany("ProductImages")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("EPharm.Infrastructure.Context.Entities.PharmaEntities.PharmaCompany", b =>
                {
                    b.Navigation("Address")
                        .IsRequired();

                    b.Navigation("PharmaCompanyManagers");

                    b.Navigation("Products");
                });

            modelBuilder.Entity("EPharm.Infrastructure.Context.Entities.ProductEntities.ActiveIngredient", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("EPharm.Infrastructure.Context.Entities.ProductEntities.Allergy", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("EPharm.Infrastructure.Context.Entities.ProductEntities.DosageForm", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("EPharm.Infrastructure.Context.Entities.ProductEntities.Indication", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("EPharm.Infrastructure.Context.Entities.ProductEntities.Manufacturer", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("EPharm.Infrastructure.Context.Entities.ProductEntities.Product", b =>
                {
                    b.Navigation("ActiveIngredients");

                    b.Navigation("Allergies");

                    b.Navigation("DosageForms");

                    b.Navigation("Indications");

                    b.Navigation("ProductImages");

                    b.Navigation("RouteOfAdministrations");

                    b.Navigation("SideEffects");

                    b.Navigation("UsageWarnings");
                });

            modelBuilder.Entity("EPharm.Infrastructure.Context.Entities.ProductEntities.RegulatoryInformation", b =>
                {
                    b.Navigation("Product")
                        .IsRequired();
                });

            modelBuilder.Entity("EPharm.Infrastructure.Context.Entities.ProductEntities.RouteOfAdministration", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("EPharm.Infrastructure.Context.Entities.ProductEntities.SideEffect", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("EPharm.Infrastructure.Context.Entities.ProductEntities.SpecialRequirement", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("EPharm.Infrastructure.Context.Entities.ProductEntities.UsageWarning", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
