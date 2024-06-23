using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPharm.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Pharmacy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Allergies_PharmaCompanies_PharmaCompanyId",
                table: "Allergies");

            migrationBuilder.DropForeignKey(
                name: "FK_DosageForms_PharmaCompanies_PharmaCompanyId",
                table: "DosageForms");

            migrationBuilder.DropForeignKey(
                name: "FK_Indications_PharmaCompanies_PharmaCompanyId",
                table: "Indications");

            migrationBuilder.DropForeignKey(
                name: "FK_RouteOfAdministrations_PharmaCompanies_PharmaCompanyId",
                table: "RouteOfAdministrations");

            migrationBuilder.DropForeignKey(
                name: "FK_SideEffects_PharmaCompanies_PharmaCompanyId",
                table: "SideEffects");

            migrationBuilder.DropForeignKey(
                name: "FK_UsageWarnings_PharmaCompanies_PharmaCompanyId",
                table: "UsageWarnings");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "PharmaCompanyManagers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "PharmaCompanyManagers");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "PharmaCompanyManagers");

            migrationBuilder.RenameColumn(
                name: "PharmaCompanyId",
                table: "UsageWarnings",
                newName: "PharmacyId");

            migrationBuilder.RenameIndex(
                name: "IX_UsageWarnings_PharmaCompanyId",
                table: "UsageWarnings",
                newName: "IX_UsageWarnings_PharmacyId");

            migrationBuilder.RenameColumn(
                name: "PharmaCompanyId",
                table: "SideEffects",
                newName: "PharmacyId");

            migrationBuilder.RenameIndex(
                name: "IX_SideEffects_PharmaCompanyId",
                table: "SideEffects",
                newName: "IX_SideEffects_PharmacyId");

            migrationBuilder.RenameColumn(
                name: "PharmaCompanyId",
                table: "RouteOfAdministrations",
                newName: "PharmacyId");

            migrationBuilder.RenameIndex(
                name: "IX_RouteOfAdministrations_PharmaCompanyId",
                table: "RouteOfAdministrations",
                newName: "IX_RouteOfAdministrations_PharmacyId");

            migrationBuilder.RenameColumn(
                name: "PharmaCompanyId",
                table: "Indications",
                newName: "PharmacyId");

            migrationBuilder.RenameIndex(
                name: "IX_Indications_PharmaCompanyId",
                table: "Indications",
                newName: "IX_Indications_PharmacyId");

            migrationBuilder.RenameColumn(
                name: "PharmaCompanyId",
                table: "DosageForms",
                newName: "PharmacyId");

            migrationBuilder.RenameIndex(
                name: "IX_DosageForms_PharmaCompanyId",
                table: "DosageForms",
                newName: "IX_DosageForms_PharmacyId");

            migrationBuilder.RenameColumn(
                name: "PharmaCompanyId",
                table: "Allergies",
                newName: "PharmacyId");

            migrationBuilder.RenameIndex(
                name: "IX_Allergies_PharmaCompanyId",
                table: "Allergies",
                newName: "IX_Allergies_PharmacyId");

            migrationBuilder.AlterColumn<int>(
                name: "SupplyDuration",
                table: "OrderProducts",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "Frequency",
                table: "OrderProducts",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Allergies_PharmaCompanies_PharmacyId",
                table: "Allergies",
                column: "PharmacyId",
                principalTable: "PharmaCompanies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DosageForms_PharmaCompanies_PharmacyId",
                table: "DosageForms",
                column: "PharmacyId",
                principalTable: "PharmaCompanies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Indications_PharmaCompanies_PharmacyId",
                table: "Indications",
                column: "PharmacyId",
                principalTable: "PharmaCompanies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RouteOfAdministrations_PharmaCompanies_PharmacyId",
                table: "RouteOfAdministrations",
                column: "PharmacyId",
                principalTable: "PharmaCompanies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SideEffects_PharmaCompanies_PharmacyId",
                table: "SideEffects",
                column: "PharmacyId",
                principalTable: "PharmaCompanies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UsageWarnings_PharmaCompanies_PharmacyId",
                table: "UsageWarnings",
                column: "PharmacyId",
                principalTable: "PharmaCompanies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Allergies_PharmaCompanies_PharmacyId",
                table: "Allergies");

            migrationBuilder.DropForeignKey(
                name: "FK_DosageForms_PharmaCompanies_PharmacyId",
                table: "DosageForms");

            migrationBuilder.DropForeignKey(
                name: "FK_Indications_PharmaCompanies_PharmacyId",
                table: "Indications");

            migrationBuilder.DropForeignKey(
                name: "FK_RouteOfAdministrations_PharmaCompanies_PharmacyId",
                table: "RouteOfAdministrations");

            migrationBuilder.DropForeignKey(
                name: "FK_SideEffects_PharmaCompanies_PharmacyId",
                table: "SideEffects");

            migrationBuilder.DropForeignKey(
                name: "FK_UsageWarnings_PharmaCompanies_PharmacyId",
                table: "UsageWarnings");

            migrationBuilder.RenameColumn(
                name: "PharmacyId",
                table: "UsageWarnings",
                newName: "PharmaCompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_UsageWarnings_PharmacyId",
                table: "UsageWarnings",
                newName: "IX_UsageWarnings_PharmaCompanyId");

            migrationBuilder.RenameColumn(
                name: "PharmacyId",
                table: "SideEffects",
                newName: "PharmaCompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_SideEffects_PharmacyId",
                table: "SideEffects",
                newName: "IX_SideEffects_PharmaCompanyId");

            migrationBuilder.RenameColumn(
                name: "PharmacyId",
                table: "RouteOfAdministrations",
                newName: "PharmaCompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_RouteOfAdministrations_PharmacyId",
                table: "RouteOfAdministrations",
                newName: "IX_RouteOfAdministrations_PharmaCompanyId");

            migrationBuilder.RenameColumn(
                name: "PharmacyId",
                table: "Indications",
                newName: "PharmaCompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Indications_PharmacyId",
                table: "Indications",
                newName: "IX_Indications_PharmaCompanyId");

            migrationBuilder.RenameColumn(
                name: "PharmacyId",
                table: "DosageForms",
                newName: "PharmaCompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_DosageForms_PharmacyId",
                table: "DosageForms",
                newName: "IX_DosageForms_PharmaCompanyId");

            migrationBuilder.RenameColumn(
                name: "PharmacyId",
                table: "Allergies",
                newName: "PharmaCompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Allergies_PharmacyId",
                table: "Allergies",
                newName: "IX_Allergies_PharmaCompanyId");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "PharmaCompanyManagers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "PharmaCompanyManagers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "PharmaCompanyManagers",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "SupplyDuration",
                table: "OrderProducts",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Frequency",
                table: "OrderProducts",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Allergies_PharmaCompanies_PharmaCompanyId",
                table: "Allergies",
                column: "PharmaCompanyId",
                principalTable: "PharmaCompanies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DosageForms_PharmaCompanies_PharmaCompanyId",
                table: "DosageForms",
                column: "PharmaCompanyId",
                principalTable: "PharmaCompanies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Indications_PharmaCompanies_PharmaCompanyId",
                table: "Indications",
                column: "PharmaCompanyId",
                principalTable: "PharmaCompanies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RouteOfAdministrations_PharmaCompanies_PharmaCompanyId",
                table: "RouteOfAdministrations",
                column: "PharmaCompanyId",
                principalTable: "PharmaCompanies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SideEffects_PharmaCompanies_PharmaCompanyId",
                table: "SideEffects",
                column: "PharmaCompanyId",
                principalTable: "PharmaCompanies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UsageWarnings_PharmaCompanies_PharmaCompanyId",
                table: "UsageWarnings",
                column: "PharmaCompanyId",
                principalTable: "PharmaCompanies",
                principalColumn: "Id");
        }
    }
}
