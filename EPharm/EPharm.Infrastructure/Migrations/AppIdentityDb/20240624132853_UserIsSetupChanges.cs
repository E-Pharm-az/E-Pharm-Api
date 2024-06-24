using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPharm.Infrastructure.Migrations.AppIdentityDb
{
    /// <inheritdoc />
    public partial class UserIsSetupChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAAccountSetup",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<bool>(
                name: "IsAccountSetup",
                table: "AspNetUsers",
                type: "boolean",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAccountSetup",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<bool>(
                name: "IsAAccountSetup",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
