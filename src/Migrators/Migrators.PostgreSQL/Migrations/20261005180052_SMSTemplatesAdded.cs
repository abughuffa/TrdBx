using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CleanArchitecture.Blazor.Migrators.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class SMSTemplatesAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SmsConfigCommands",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SmsTemplate = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    TrackingUnitModelId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sms_config_commands", x => x.id);
                    table.ForeignKey(
                        name: "fk_sms_config_commands_tracking_unit_models_tracking_unit_model_",
                        column: x => x.TrackingUnitModelId,
                        principalTable: "TrackingUnitModels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_sms_config_commands_tracking_unit_model_id",
                table: "SmsConfigCommands",
                column: "TrackingUnitModelId");

            migrationBuilder.CreateIndex(
                name: "IX_SmsConfigCommand_Name",
                table: "SmsConfigCommands",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SmsConfigCommands");
        }
    }
}
