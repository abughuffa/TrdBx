using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Blazor.Migrators.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "desc",
                table: "wialon_tasks",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "desc",
                table: "tickets",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "desc",
                table: "subscriptions",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "desc",
                table: "service_prices",
                newName: "description");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "description",
                table: "wialon_tasks",
                newName: "desc");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "tickets",
                newName: "desc");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "subscriptions",
                newName: "desc");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "service_prices",
                newName: "desc");
        }
    }
}
