using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Blazor.Migrators.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class ToBeInitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.RenameTable(
            //     name: "sms_cursors",
            //     newName: "SmsCursors");

            // migrationBuilder.RenameColumn(
            //     name: "value",
            //     table: "SmsCursors",
            //     newName: "Value");

            // migrationBuilder.RenameColumn(
            //     name: "key",
            //     table: "SmsCursors",
            //     newName: "Key");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.RenameTable(
            //     name: "SmsCursors",
            //     newName: "sms_cursors");

            // migrationBuilder.RenameColumn(
            //     name: "Value",
            //     table: "SmsCursors",
            //     newName: "value");

            // migrationBuilder.RenameColumn(
            //     name: "Key",
            //     table: "SmsCursors",
            //     newName: "key");
        }
    }
}
