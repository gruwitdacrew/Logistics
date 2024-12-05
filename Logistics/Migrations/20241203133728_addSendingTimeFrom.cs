using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Logistics.Migrations
{
    /// <inheritdoc />
    public partial class addSendingTimeFrom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "volumeInCubicMeters",
                table: "Shipments");

            migrationBuilder.RenameColumn(
                name: "desiredDeliveryTime",
                table: "Requests",
                newName: "sendingTimeFrom");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "sendingTimeFrom",
                table: "Requests",
                newName: "desiredDeliveryTime");

            migrationBuilder.AddColumn<float>(
                name: "volumeInCubicMeters",
                table: "Shipments",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
