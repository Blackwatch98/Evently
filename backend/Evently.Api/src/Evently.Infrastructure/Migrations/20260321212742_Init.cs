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
            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                schema: "Main",
                table: "Users",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiresAtUtc",
                schema: "Main",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ScheduledAt",
                schema: "Main",
                table: "Events",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2026, 3, 21, 21, 27, 42, 504, DateTimeKind.Utc).AddTicks(1786),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2026, 3, 21, 15, 53, 34, 312, DateTimeKind.Utc).AddTicks(1251));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                schema: "Main",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiresAtUtc",
                schema: "Main",
                table: "Users");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ScheduledAt",
                schema: "Main",
                table: "Events",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2026, 3, 21, 15, 53, 34, 312, DateTimeKind.Utc).AddTicks(1251),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2026, 3, 21, 21, 27, 42, 504, DateTimeKind.Utc).AddTicks(1786));
        }
    }
}
