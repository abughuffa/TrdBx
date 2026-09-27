using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CleanArchitecture.Blazor.Migrators.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class SMSModelEnhanced : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_sms_messages_phone_number",
                table: "SmsMessages");

            migrationBuilder.DropIndex(
                name: "ix_sms_messages_provider_message_id",
                table: "SmsMessages");

            migrationBuilder.DropIndex(
                name: "ix_sms_messages_sms_provider",
                table: "SmsMessages");

            migrationBuilder.DropIndex(
                name: "ix_sms_messages_sms_status",
                table: "SmsMessages");

            migrationBuilder.DropColumn(
                name: "ErrorMessage",
                table: "SmsMessages");

            migrationBuilder.DropColumn(
                name: "ProviderMessageId",
                table: "SmsMessages");

            migrationBuilder.DropColumn(
                name: "SmsProvider",
                table: "SmsMessages");

            migrationBuilder.DropColumn(
                name: "SmsStatus",
                table: "SmsMessages");

            migrationBuilder.RenameColumn(
                name: "delivered_at",
                table: "SmsMessages",
                newName: "DeliveredAt");

            migrationBuilder.RenameColumn(
                name: "retry_count",
                table: "SmsMessages",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "parts_count",
                table: "SmsMessages",
                newName: "SimSlot");

            migrationBuilder.RenameColumn(
                name: "SMSDate",
                table: "SmsMessages",
                newName: "SentAt");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "SmsMessages",
                newName: "To");

            migrationBuilder.RenameColumn(
                name: "Message",
                table: "SmsMessages",
                newName: "Body");

            migrationBuilder.RenameColumn(
                name: "Encoding",
                table: "SmsMessages",
                newName: "From");

            migrationBuilder.AddColumn<string>(
                name: "Error",
                table: "SmsMessages",
                type: "character varying(1024)",
                maxLength: 1024,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GatewayId",
                table: "SmsMessages",
                type: "character varying(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReceivedAt",
                table: "SmsMessages",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reference",
                table: "SmsMessages",
                type: "character varying(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "sms_cursors",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    key = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    value = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sms_cursors", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_sms_messages_gateway_id",
                table: "SmsMessages",
                column: "GatewayId",
                unique: true,
                filter: "\"GatewayId\" IS NOT NULL AND \"Direction\" = 1");

            migrationBuilder.CreateIndex(
                name: "ix_sms_messages_status",
                table: "SmsMessages",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sms_cursors");

            migrationBuilder.DropIndex(
                name: "ix_sms_messages_gateway_id",
                table: "SmsMessages");

            migrationBuilder.DropIndex(
                name: "ix_sms_messages_status",
                table: "SmsMessages");

            migrationBuilder.DropColumn(
                name: "Error",
                table: "SmsMessages");

            migrationBuilder.DropColumn(
                name: "GatewayId",
                table: "SmsMessages");

            migrationBuilder.DropColumn(
                name: "ReceivedAt",
                table: "SmsMessages");

            migrationBuilder.DropColumn(
                name: "Reference",
                table: "SmsMessages");

            migrationBuilder.RenameColumn(
                name: "DeliveredAt",
                table: "SmsMessages",
                newName: "delivered_at");

            migrationBuilder.RenameColumn(
                name: "To",
                table: "SmsMessages",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "SmsMessages",
                newName: "retry_count");

            migrationBuilder.RenameColumn(
                name: "SimSlot",
                table: "SmsMessages",
                newName: "parts_count");

            migrationBuilder.RenameColumn(
                name: "SentAt",
                table: "SmsMessages",
                newName: "SMSDate");

            migrationBuilder.RenameColumn(
                name: "From",
                table: "SmsMessages",
                newName: "Encoding");

            migrationBuilder.RenameColumn(
                name: "Body",
                table: "SmsMessages",
                newName: "Message");

            migrationBuilder.AddColumn<string>(
                name: "ErrorMessage",
                table: "SmsMessages",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProviderMessageId",
                table: "SmsMessages",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SmsProvider",
                table: "SmsMessages",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SmsStatus",
                table: "SmsMessages",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "ix_sms_messages_phone_number",
                table: "SmsMessages",
                column: "PhoneNumber");

            migrationBuilder.CreateIndex(
                name: "ix_sms_messages_provider_message_id",
                table: "SmsMessages",
                column: "ProviderMessageId");

            migrationBuilder.CreateIndex(
                name: "ix_sms_messages_sms_provider",
                table: "SmsMessages",
                column: "SmsProvider");

            migrationBuilder.CreateIndex(
                name: "ix_sms_messages_sms_status",
                table: "SmsMessages",
                column: "SmsStatus");
        }
    }
}
