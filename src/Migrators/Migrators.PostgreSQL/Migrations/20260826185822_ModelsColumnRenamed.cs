using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Blazor.Migrators.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class ModelsColumnRenamed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "defualt_price",
                table: "tracking_unit_models",
                newName: "default_price");

            migrationBuilder.RenameColumn(
                name: "defualt_host",
                table: "tracking_unit_models",
                newName: "default_host");

            migrationBuilder.RenameColumn(
                name: "defualt_gprs",
                table: "tracking_unit_models",
                newName: "default_gprs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "default_price",
                table: "tracking_unit_models",
                newName: "defualt_price");

            migrationBuilder.RenameColumn(
                name: "default_host",
                table: "tracking_unit_models",
                newName: "defualt_host");

            migrationBuilder.RenameColumn(
                name: "default_gprs",
                table: "tracking_unit_models",
                newName: "defualt_gprs");
        }
    }
}
