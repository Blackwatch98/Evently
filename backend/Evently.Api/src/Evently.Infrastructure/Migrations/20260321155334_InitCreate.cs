using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Evently.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Main");

            migrationBuilder.EnsureSchema(
                name: "Dict");

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
                    ScheduledAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2026, 3, 21, 15, 53, 34, 312, DateTimeKind.Utc).AddTicks(1251)),
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
                name: "Roles",
                schema: "Dict",
                columns: table => new
                {
                    IdRole = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.IdRole);
                    table.CheckConstraint("CK_Roles_IdRole_Positive", "IdRole > 0");
                    table.CheckConstraint("CK_Roles_Priority", "Priority > 0");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "Main",
                columns: table => new
                {
                    IdUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    IsBlocked = table.Column<bool>(type: "bit", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.IdUser);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Dict",
                        principalTable: "Roles",
                        principalColumn: "IdRole",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Registrations",
                schema: "Main",
                columns: table => new
                {
                    IdRegistration = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RegisteredAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registrations", x => x.IdRegistration);
                    table.ForeignKey(
                        name: "FK_Registrations_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Main",
                        principalTable: "Users",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "Dict",
                table: "Roles",
                columns: new[] { "IdRole", "Description", "Name", "Priority" },
                values: new object[,]
                {
                    { 1, "Administrator with full access to the system.", "Admin", 1 },
                    { 2, "Regular application user with limited access.", "User", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_Title",
                schema: "Main",
                table: "Events",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_EventId_UserId",
                schema: "Main",
                table: "Registrations",
                columns: new[] { "EventId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_UserId",
                schema: "Main",
                table: "Registrations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                schema: "Dict",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                schema: "Main",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                schema: "Main",
                table: "Users",
                column: "RoleId");
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

            migrationBuilder.DropTable(
                name: "Users",
                schema: "Main");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "Dict");
        }
    }
}
