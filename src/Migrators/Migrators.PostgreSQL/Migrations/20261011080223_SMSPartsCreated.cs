using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CleanArchitecture.Blazor.Migrators.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class SMSPartsCreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SmsInboundParts",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    from = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    concat_ref = table.Column<int>(type: "integer", nullable: false),
                    part_number = table.Column<int>(type: "integer", nullable: false),
                    total_parts = table.Column<int>(type: "integer", nullable: false),
                    body = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    gateway_id = table.Column<long>(type: "bigint", nullable: false),
                    ts = table.Column<long>(type: "bigint", nullable: false),
                    sim_slot = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    assembled_sms_message_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sms_inbound_parts", x => x.id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SmsInboundParts");
        }
    }
}
