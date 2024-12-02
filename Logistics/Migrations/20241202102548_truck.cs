using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Logistics.Migrations
{
    /// <inheritdoc />
    public partial class truck : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "truckid",
                table: "Transporters",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<float>(
                name: "additionalCostInRubles",
                table: "Requests",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.CreateTable(
                name: "TransportationStatusChanges",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    transportationid = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportationStatusChanges", x => x.id);
                    table.ForeignKey(
                        name: "FK_TransportationStatusChanges_Transportations_transportationid",
                        column: x => x.transportationid,
                        principalTable: "Transportations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Truck",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    carBrand = table.Column<string>(type: "text", nullable: false),
                    model = table.Column<string>(type: "text", nullable: false),
                    truckType = table.Column<int>(type: "integer", nullable: false),
                    loadCapacityInTons = table.Column<int>(type: "integer", nullable: false),
                    yearOfProduction = table.Column<int>(type: "integer", nullable: false),
                    number = table.Column<string>(type: "text", nullable: false),
                    regionCode = table.Column<int>(type: "integer", nullable: false),
                    lengthInMeters = table.Column<float>(type: "real", nullable: false),
                    widthInMeters = table.Column<float>(type: "real", nullable: false),
                    heightInMeters = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Truck", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transporters_truckid",
                table: "Transporters",
                column: "truckid");

            migrationBuilder.CreateIndex(
                name: "IX_TransportationStatusChanges_transportationid",
                table: "TransportationStatusChanges",
                column: "transportationid");

            migrationBuilder.AddForeignKey(
                name: "FK_Transporters_Truck_truckid",
                table: "Transporters",
                column: "truckid",
                principalTable: "Truck",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transporters_Truck_truckid",
                table: "Transporters");

            migrationBuilder.DropTable(
                name: "TransportationStatusChanges");

            migrationBuilder.DropTable(
                name: "Truck");

            migrationBuilder.DropIndex(
                name: "IX_Transporters_truckid",
                table: "Transporters");

            migrationBuilder.DropColumn(
                name: "truckid",
                table: "Transporters");

            migrationBuilder.DropColumn(
                name: "additionalCostInRubles",
                table: "Requests");
        }
    }
}
