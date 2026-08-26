using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Blazor.Migrators.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class SomeColumnRenamed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "is_avaliable",
                table: "tracked_assets",
                newName: "is_available");

            migrationBuilder.RenameColumn(
                name: "is_owen",
                table: "sim_cards",
                newName: "is_owned");

            migrationBuilder.RenameColumn(
                name: "is_avaliable",
                table: "customers",
                newName: "is_available");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "is_available",
                table: "tracked_assets",
                newName: "is_avaliable");

            migrationBuilder.RenameColumn(
                name: "is_owned",
                table: "sim_cards",
                newName: "is_owen");

            migrationBuilder.RenameColumn(
                name: "is_available",
                table: "customers",
                newName: "is_avaliable");
        }
    }
}
