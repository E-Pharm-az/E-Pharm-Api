using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPharm.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PharmacyChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PharmacyStaff_Pharmacy_PharmacyId",
                table: "PharmacyStaff");

            migrationBuilder.AlterColumn<int>(
                name: "PharmacyId",
                table: "PharmacyStaff",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_PharmacyStaff_Pharmacy_PharmacyId",
                table: "PharmacyStaff",
                column: "PharmacyId",
                principalTable: "Pharmacy",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PharmacyStaff_Pharmacy_PharmacyId",
                table: "PharmacyStaff");

            migrationBuilder.AlterColumn<int>(
                name: "PharmacyId",
                table: "PharmacyStaff",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PharmacyStaff_Pharmacy_PharmacyId",
                table: "PharmacyStaff",
                column: "PharmacyId",
                principalTable: "Pharmacy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
