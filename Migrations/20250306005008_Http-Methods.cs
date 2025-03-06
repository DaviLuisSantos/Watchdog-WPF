using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Watchdog.Migrations
{
    /// <inheritdoc />
    public partial class HttpMethods : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UrlWatchdog",
                table: "WatchdogTasks",
                newName: "Url");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Url",
                table: "WatchdogTasks",
                newName: "UrlWatchdog");
        }
    }
}
