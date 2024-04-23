using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPharm.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RequireProductApprove : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApprovedByAdminId",
                table: "Products",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Products",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovedByAdminId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Products");
        }
    }
}
