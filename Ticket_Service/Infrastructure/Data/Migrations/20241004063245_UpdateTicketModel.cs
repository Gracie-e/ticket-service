using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ticket_Service.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTicketModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int[]>(
                name: "ReceiverUserIds",
                table: "Tickets",
                type: "integer[]",
                nullable: false,
                defaultValue: new int[0]);

            migrationBuilder.AddColumn<int>(
                name: "SenderUserId",
                table: "Tickets",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiverUserIds",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "SenderUserId",
                table: "Tickets");
        }
    }
}
