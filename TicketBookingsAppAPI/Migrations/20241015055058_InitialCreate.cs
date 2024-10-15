using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketBookingsAppAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "TicketPrice",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "Events",
                newName: "Venue_State");

            migrationBuilder.RenameColumn(
                name: "Venue",
                table: "Events",
                newName: "Venue_Name");

            migrationBuilder.RenameColumn(
                name: "AvailableTickets",
                table: "Events",
                newName: "Venue_Capacity");

            migrationBuilder.AddColumn<string>(
                name: "Categories",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateAndTime",
                table: "Events",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "Organizer_ContactEmail",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Organizer_Name",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Organizer_PhoneNumber",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Venue_Address",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "EventImages",
                columns: table => new
                {
                    EventImageID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SizeInBytes = table.Column<long>(type: "bigint", nullable: false),
                    EventID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventImages", x => x.EventImageID);
                    table.ForeignKey(
                        name: "FK_EventImages_Events_EventID",
                        column: x => x.EventID,
                        principalTable: "Events",
                        principalColumn: "EventID");
                });

            migrationBuilder.CreateTable(
                name: "TicketTypes",
                columns: table => new
                {
                    TicketTypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    QuantityAvailable = table.Column<int>(type: "int", nullable: false),
                    EventID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketTypes", x => x.TicketTypeID);
                    table.ForeignKey(
                        name: "FK_TicketTypes_Events_EventID",
                        column: x => x.EventID,
                        principalTable: "Events",
                        principalColumn: "EventID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventImages_EventID",
                table: "EventImages",
                column: "EventID");

            migrationBuilder.CreateIndex(
                name: "IX_TicketTypes_EventID",
                table: "TicketTypes",
                column: "EventID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventImages");

            migrationBuilder.DropTable(
                name: "TicketTypes");

            migrationBuilder.DropColumn(
                name: "Categories",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "DateAndTime",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Organizer_ContactEmail",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Organizer_Name",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Organizer_PhoneNumber",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Venue_Address",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "Venue_State",
                table: "Events",
                newName: "State");

            migrationBuilder.RenameColumn(
                name: "Venue_Name",
                table: "Events",
                newName: "Venue");

            migrationBuilder.RenameColumn(
                name: "Venue_Capacity",
                table: "Events",
                newName: "AvailableTickets");

            migrationBuilder.AddColumn<DateOnly>(
                name: "Date",
                table: "Events",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<decimal>(
                name: "TicketPrice",
                table: "Events",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "Time",
                table: "Events",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));
        }
    }
}
