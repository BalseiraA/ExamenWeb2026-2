using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketSystem.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Venue = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketZones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EventId = table.Column<int>(type: "INTEGER", nullable: false),
                    ZoneType = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalCapacity = table.Column<int>(type: "INTEGER", nullable: false),
                    AvailableCapacity = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketZones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketZones_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketPurchases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EventId = table.Column<int>(type: "INTEGER", nullable: false),
                    ZoneId = table.Column<int>(type: "INTEGER", nullable: false),
                    BuyerName = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    BuyerEmail = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PurchasedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketPurchases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketPurchases_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketPurchases_TicketZones_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "TicketZones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TicketPurchases_EventId",
                table: "TicketPurchases",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketPurchases_ZoneId",
                table: "TicketPurchases",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketZones_EventId",
                table: "TicketZones",
                column: "EventId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TicketPurchases");

            migrationBuilder.DropTable(
                name: "TicketZones");

            migrationBuilder.DropTable(
                name: "Events");
        }
    }
}
