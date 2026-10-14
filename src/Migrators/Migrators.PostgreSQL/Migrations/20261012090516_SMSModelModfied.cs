using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitecture.Blazor.Migrators.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class SMSModelModfied : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_sms_messages_direction",
                table: "SmsMessages");

            migrationBuilder.DropIndex(
                name: "ix_sms_messages_gateway_id",
                table: "SmsMessages");

            migrationBuilder.DropIndex(
                name: "ix_sms_messages_status",
                table: "SmsMessages");

            migrationBuilder.DropIndex(
                name: "ix_sms_inbound_parts_assembled_sms_message_id_created_at",
                table: "SmsInboundParts");

            migrationBuilder.DropIndex(
                name: "ix_sms_inbound_parts_from_concat_ref_part_number",
                table: "SmsInboundParts");

            migrationBuilder.RenameColumn(
                name: "from",
                table: "SmsInboundParts",
                newName: "From");

            migrationBuilder.RenameColumn(
                name: "body",
                table: "SmsInboundParts",
                newName: "Body");

            migrationBuilder.RenameColumn(
                name: "timeout_seconds",
                table: "SmsGatewaySettings",
                newName: "TimeoutSeconds");

            migrationBuilder.RenameColumn(
                name: "polling_interval_seconds",
                table: "SmsGatewaySettings",
                newName: "PollingIntervalSeconds");

            migrationBuilder.RenameColumn(
                name: "enable_inbound_polling",
                table: "SmsGatewaySettings",
                newName: "EnableInboundPolling");

            migrationBuilder.RenameColumn(
                name: "WebhookSecret",
                table: "SmsGatewaySettings",
                newName: "webhook_secret");

            migrationBuilder.RenameColumn(
                name: "WebhookPublicUrl",
                table: "SmsGatewaySettings",
                newName: "webhook_public_url");

            migrationBuilder.AddColumn<string>(
                name: "AssemblyKey",
                table: "SmsMessages",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PartsGatewayIds",
                table: "SmsMessages",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "concat_ref",
                table: "SmsMessages",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_multipart",
                table: "SmsMessages",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "total_parts",
                table: "SmsMessages",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BaseUrl",
                table: "SmsGatewaySettings",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<string>(
                name: "ApiKey",
                table: "SmsGatewaySettings",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TimeoutSeconds",
                table: "SmsGatewaySettings",
                type: "integer",
                nullable: false,
                defaultValue: 30,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "PollingIntervalSeconds",
                table: "SmsGatewaySettings",
                type: "integer",
                nullable: false,
                defaultValue: 30,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<bool>(
                name: "EnableInboundPolling",
                table: "SmsGatewaySettings",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "webhook_secret",
                table: "SmsGatewaySettings",
                type: "character varying(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "webhook_public_url",
                table: "SmsGatewaySettings",
                type: "character varying(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AssemblyWindowMinutes",
                table: "SmsGatewaySettings",
                type: "integer",
                nullable: false,
                defaultValue: 10);

            migrationBuilder.AddColumn<int>(
                name: "MaxGroupsPerSweep",
                table: "SmsGatewaySettings",
                type: "integer",
                nullable: false,
                defaultValue: 50);

            migrationBuilder.AddColumn<int>(
                name: "OrphanThresholdMinutes",
                table: "SmsGatewaySettings",
                type: "integer",
                nullable: false,
                defaultValue: 30);

            migrationBuilder.AddColumn<int>(
                name: "RetentionDays",
                table: "SmsGatewaySettings",
                type: "integer",
                nullable: false,
                defaultValue: 30);

            migrationBuilder.AddColumn<int>(
                name: "SweeperIntervalSeconds",
                table: "SmsGatewaySettings",
                type: "integer",
                nullable: false,
                defaultValue: 60);

            migrationBuilder.CreateIndex(
                name: "ix_sms_messages_direction_gateway_id",
                table: "SmsMessages",
                columns: new[] { "Direction", "GatewayId" },
                unique: true,
                filter: "\"GatewayId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UX_SmsMessage_AssemblyKey",
                table: "SmsMessages",
                column: "AssemblyKey",
                unique: true,
                filter: "\"AssemblyKey\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_sms_inbound_parts_assembled_sms_message_id",
                table: "SmsInboundParts",
                column: "assembled_sms_message_id");

            migrationBuilder.CreateIndex(
                name: "IX_SmsInboundPart_Group",
                table: "SmsInboundParts",
                columns: new[] { "From", "concat_ref", "assembled_sms_message_id" });

            migrationBuilder.CreateIndex(
                name: "UX_SmsInboundPart_Exact",
                table: "SmsInboundParts",
                columns: new[] { "From", "concat_ref", "part_number", "total_parts" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_sms_inbound_parts_sms_messages_assembled_sms_message_id",
                table: "SmsInboundParts",
                column: "assembled_sms_message_id",
                principalTable: "SmsMessages",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_sms_inbound_parts_sms_messages_assembled_sms_message_id",
                table: "SmsInboundParts");

            migrationBuilder.DropIndex(
                name: "ix_sms_messages_direction_gateway_id",
                table: "SmsMessages");

            migrationBuilder.DropIndex(
                name: "UX_SmsMessage_AssemblyKey",
                table: "SmsMessages");

            migrationBuilder.DropIndex(
                name: "ix_sms_inbound_parts_assembled_sms_message_id",
                table: "SmsInboundParts");

            migrationBuilder.DropIndex(
                name: "IX_SmsInboundPart_Group",
                table: "SmsInboundParts");

            migrationBuilder.DropIndex(
                name: "UX_SmsInboundPart_Exact",
                table: "SmsInboundParts");

            migrationBuilder.DropColumn(
                name: "AssemblyKey",
                table: "SmsMessages");

            migrationBuilder.DropColumn(
                name: "PartsGatewayIds",
                table: "SmsMessages");

            migrationBuilder.DropColumn(
                name: "concat_ref",
                table: "SmsMessages");

            migrationBuilder.DropColumn(
                name: "is_multipart",
                table: "SmsMessages");

            migrationBuilder.DropColumn(
                name: "total_parts",
                table: "SmsMessages");

            migrationBuilder.DropColumn(
                name: "AssemblyWindowMinutes",
                table: "SmsGatewaySettings");

            migrationBuilder.DropColumn(
                name: "MaxGroupsPerSweep",
                table: "SmsGatewaySettings");

            migrationBuilder.DropColumn(
                name: "OrphanThresholdMinutes",
                table: "SmsGatewaySettings");

            migrationBuilder.DropColumn(
                name: "RetentionDays",
                table: "SmsGatewaySettings");

            migrationBuilder.DropColumn(
                name: "SweeperIntervalSeconds",
                table: "SmsGatewaySettings");

            migrationBuilder.RenameColumn(
                name: "From",
                table: "SmsInboundParts",
                newName: "from");

            migrationBuilder.RenameColumn(
                name: "Body",
                table: "SmsInboundParts",
                newName: "body");

            migrationBuilder.RenameColumn(
                name: "webhook_secret",
                table: "SmsGatewaySettings",
                newName: "WebhookSecret");

            migrationBuilder.RenameColumn(
                name: "webhook_public_url",
                table: "SmsGatewaySettings",
                newName: "WebhookPublicUrl");

            migrationBuilder.RenameColumn(
                name: "TimeoutSeconds",
                table: "SmsGatewaySettings",
                newName: "timeout_seconds");

            migrationBuilder.RenameColumn(
                name: "PollingIntervalSeconds",
                table: "SmsGatewaySettings",
                newName: "polling_interval_seconds");

            migrationBuilder.RenameColumn(
                name: "EnableInboundPolling",
                table: "SmsGatewaySettings",
                newName: "enable_inbound_polling");

            migrationBuilder.AlterColumn<string>(
                name: "BaseUrl",
                table: "SmsGatewaySettings",
                type: "character varying(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "ApiKey",
                table: "SmsGatewaySettings",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "WebhookSecret",
                table: "SmsGatewaySettings",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "WebhookPublicUrl",
                table: "SmsGatewaySettings",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "timeout_seconds",
                table: "SmsGatewaySettings",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 30);

            migrationBuilder.AlterColumn<int>(
                name: "polling_interval_seconds",
                table: "SmsGatewaySettings",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 30);

            migrationBuilder.AlterColumn<bool>(
                name: "enable_inbound_polling",
                table: "SmsGatewaySettings",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.CreateIndex(
                name: "ix_sms_messages_direction",
                table: "SmsMessages",
                column: "Direction");

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

            migrationBuilder.CreateIndex(
                name: "ix_sms_inbound_parts_assembled_sms_message_id_created_at",
                table: "SmsInboundParts",
                columns: new[] { "assembled_sms_message_id", "created_at" });

            migrationBuilder.CreateIndex(
                name: "ix_sms_inbound_parts_from_concat_ref_part_number",
                table: "SmsInboundParts",
                columns: new[] { "from", "concat_ref", "part_number" },
                unique: true);
        }
    }
}
