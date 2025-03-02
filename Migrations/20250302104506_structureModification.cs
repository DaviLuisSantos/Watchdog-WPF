using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Watchdog.Migrations
{
    /// <inheritdoc />
    public partial class structureModification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WatchdogTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LastCheckTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastSuccessTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastFailureTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    IsEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsRunning = table.Column<bool>(type: "INTEGER", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    TaskType = table.Column<string>(type: "TEXT", maxLength: 13, nullable: false),
                    UrlWatchdog = table.Column<string>(type: "TEXT", nullable: true),
                    HttpRestMethod = table.Column<int>(type: "INTEGER", nullable: true),
                    Interval = table.Column<int>(type: "INTEGER", nullable: true),
                    Host = table.Column<string>(type: "TEXT", nullable: true),
                    Port = table.Column<int>(type: "INTEGER", nullable: true),
                    SendData = table.Column<string>(type: "TEXT", nullable: true),
                    ExpectedResponse = table.Column<string>(type: "TEXT", nullable: true),
                    Timeout = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WatchdogTasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecoveryActions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WatchdogTaskId = table.Column<int>(type: "INTEGER", nullable: false),
                    ActionType = table.Column<string>(type: "TEXT", maxLength: 21, nullable: false),
                    Command = table.Column<string>(type: "TEXT", nullable: true),
                    ProcessName = table.Column<string>(type: "TEXT", nullable: true),
                    ServiceName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecoveryActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecoveryActions_WatchdogTasks_WatchdogTaskId",
                        column: x => x.WatchdogTaskId,
                        principalTable: "WatchdogTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecoveryActions_WatchdogTaskId",
                table: "RecoveryActions",
                column: "WatchdogTaskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecoveryActions");

            migrationBuilder.DropTable(
                name: "WatchdogTasks");
        }
    }
}
