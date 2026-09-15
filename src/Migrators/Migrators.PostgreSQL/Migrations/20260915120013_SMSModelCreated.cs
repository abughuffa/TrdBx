using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Blazor.Migrators.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class SMSModelCreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "sent_at",
                table: "SmsMessages",
                newName: "sms_date");

            migrationBuilder.AddColumn<int>(
                name: "Direction",
                table: "SmsMessages",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "ix_sms_messages_direction",
                table: "SmsMessages",
                column: "Direction");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_sms_messages_direction",
                table: "SmsMessages");

            migrationBuilder.DropColumn(
                name: "Direction",
                table: "SmsMessages");

            migrationBuilder.RenameColumn(
                name: "sms_date",
                table: "SmsMessages",
                newName: "sent_at");
        }
    }
}
