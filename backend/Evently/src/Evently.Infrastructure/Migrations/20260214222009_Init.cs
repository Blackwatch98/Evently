using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evently.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Main");

            migrationBuilder.CreateTable(
                name: "EntityReadModels",
                schema: "Main",
                columns: table => new
                {
                    IdReadModel = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ScheduledAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityReadModels", x => x.IdReadModel);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                schema: "Main",
                columns: table => new
                {
                    IdEvent = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ScheduledAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2026, 2, 14, 22, 20, 7, 525, DateTimeKind.Utc).AddTicks(7640)),
                    Capacity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.IdEvent);
                    table.CheckConstraint("CK_Event_Capacity_Positive", "[Capacity] >= 0");
                });

            migrationBuilder.CreateTable(
                name: "OutboxMessages",
                schema: "Main",
                columns: table => new
                {
                    IdOutboxMessage = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OccurredOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Payload = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProcessedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Error = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessages", x => x.IdOutboxMessage);
                });

            migrationBuilder.CreateTable(
                name: "Registrations",
                schema: "Main",
                columns: table => new
                {
                    IdRegistration = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    RegisteredAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2026, 2, 14, 22, 20, 7, 526, DateTimeKind.Utc).AddTicks(4234))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registrations", x => x.IdRegistration);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_Title",
                schema: "Main",
                table: "Events",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_EventId_Email",
                schema: "Main",
                table: "Registrations",
                columns: new[] { "EventId", "Email" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntityReadModels",
                schema: "Main");

            migrationBuilder.DropTable(
                name: "Events",
                schema: "Main");

            migrationBuilder.DropTable(
                name: "OutboxMessages",
                schema: "Main");

            migrationBuilder.DropTable(
                name: "Registrations",
                schema: "Main");
        }
    }
}
