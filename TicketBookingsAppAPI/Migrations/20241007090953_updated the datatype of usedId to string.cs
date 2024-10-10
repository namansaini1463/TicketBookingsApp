using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketBookingsAppAPI.Migrations
{
    /// <inheritdoc />
    public partial class updatedthedatatypeofusedIdtostring : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_AspNetUsers_BookingUserId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_BookingUserId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "BookingUserId",
                table: "Bookings");

            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "Bookings",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_UserID",
                table: "Bookings",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_AspNetUsers_UserID",
                table: "Bookings",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_AspNetUsers_UserID",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_UserID",
                table: "Bookings");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserID",
                table: "Bookings",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "BookingUserId",
                table: "Bookings",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_BookingUserId",
                table: "Bookings",
                column: "BookingUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_AspNetUsers_BookingUserId",
                table: "Bookings",
                column: "BookingUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
