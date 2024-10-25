using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketBookingsAppAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBookingModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BookingStatus",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookingStatus",
                table: "Bookings");
        }
    }
}
