using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Logistics.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Shipments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    lengthInMeters = table.Column<float>(type: "real", nullable: false),
                    widthInMeters = table.Column<float>(type: "real", nullable: false),
                    heightInMeters = table.Column<float>(type: "real", nullable: false),
                    weightInTons = table.Column<float>(type: "real", nullable: false),
                    volumeInCubicMeters = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipments", x => x.id);
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

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    fullName = table.Column<string>(type: "text", nullable: false),
                    phone = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: true),
                    organizationalForm = table.Column<int>(type: "integer", nullable: true),
                    companyName = table.Column<string>(type: "text", nullable: true),
                    INN = table.Column<string>(type: "text", nullable: true),
                    role = table.Column<int>(type: "integer", nullable: false),
                    password = table.Column<string>(type: "text", nullable: true),
                    token = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Passports",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    series = table.Column<string>(type: "text", nullable: false),
                    number = table.Column<string>(type: "text", nullable: false),
                    issuedBy = table.Column<string>(type: "text", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false),
                    dateOfIssue = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    scan = table.Column<byte[]>(type: "bytea", nullable: true),
                    userid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passports", x => x.id);
                    table.ForeignKey(
                        name: "FK_Passports_Users_userid",
                        column: x => x.userid,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PendingEmails",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    userid = table.Column<Guid>(type: "uuid", nullable: false),
                    value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PendingEmails", x => x.id);
                    table.ForeignKey(
                        name: "FK_PendingEmails_Users_userid",
                        column: x => x.userid,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shippers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shippers", x => x.id);
                    table.ForeignKey(
                        name: "FK_Shippers_Users_id",
                        column: x => x.id,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transporters",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    permanentResidence = table.Column<string>(type: "text", nullable: true),
                    truckid = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transporters", x => x.id);
                    table.ForeignKey(
                        name: "FK_Transporters_Truck_truckid",
                        column: x => x.truckid,
                        principalTable: "Truck",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Transporters_Users_id",
                        column: x => x.id,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Licenses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    series = table.Column<string>(type: "text", nullable: false),
                    number = table.Column<string>(type: "text", nullable: false),
                    scan = table.Column<byte[]>(type: "bytea", nullable: true),
                    transporterid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Licenses", x => x.id);
                    table.ForeignKey(
                        name: "FK_Licenses_Transporters_transporterid",
                        column: x => x.transporterid,
                        principalTable: "Transporters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transportations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    transporterid = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transportations", x => x.id);
                    table.ForeignKey(
                        name: "FK_Transportations_Transporters_transporterid",
                        column: x => x.transporterid,
                        principalTable: "Transporters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    shipperid = table.Column<Guid>(type: "uuid", nullable: false),
                    shipmentid = table.Column<Guid>(type: "uuid", nullable: false),
                    transportationid = table.Column<Guid>(type: "uuid", nullable: true),
                    creationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    loadCity = table.Column<string>(type: "text", nullable: false),
                    loadAddress = table.Column<string>(type: "text", nullable: false),
                    unloadCity = table.Column<string>(type: "text", nullable: false),
                    unloadAddress = table.Column<string>(type: "text", nullable: false),
                    receiverFullName = table.Column<string>(type: "text", nullable: true),
                    receiverContacts = table.Column<string>(type: "text", nullable: true),
                    sendingTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    truckType = table.Column<int>(type: "integer", nullable: false),
                    desiredDeliveryTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    costInRubles = table.Column<float>(type: "real", nullable: false),
                    additionalCostInRubles = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.id);
                    table.ForeignKey(
                        name: "FK_Requests_Shipments_shipmentid",
                        column: x => x.shipmentid,
                        principalTable: "Shipments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Requests_Shippers_shipperid",
                        column: x => x.shipperid,
                        principalTable: "Shippers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Requests_Transportations_transportationid",
                        column: x => x.transportationid,
                        principalTable: "Transportations",
                        principalColumn: "id");
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Licenses_transporterid",
                table: "Licenses",
                column: "transporterid");

            migrationBuilder.CreateIndex(
                name: "IX_Passports_userid",
                table: "Passports",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_PendingEmails_userid",
                table: "PendingEmails",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_shipmentid",
                table: "Requests",
                column: "shipmentid");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_shipperid",
                table: "Requests",
                column: "shipperid");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_transportationid",
                table: "Requests",
                column: "transportationid");

            migrationBuilder.CreateIndex(
                name: "IX_Transportations_transporterid",
                table: "Transportations",
                column: "transporterid");

            migrationBuilder.CreateIndex(
                name: "IX_TransportationStatusChanges_transportationid",
                table: "TransportationStatusChanges",
                column: "transportationid");

            migrationBuilder.CreateIndex(
                name: "IX_Transporters_truckid",
                table: "Transporters",
                column: "truckid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Licenses");

            migrationBuilder.DropTable(
                name: "Passports");

            migrationBuilder.DropTable(
                name: "PendingEmails");

            migrationBuilder.DropTable(
                name: "Requests");

            migrationBuilder.DropTable(
                name: "TransportationStatusChanges");

            migrationBuilder.DropTable(
                name: "Shipments");

            migrationBuilder.DropTable(
                name: "Shippers");

            migrationBuilder.DropTable(
                name: "Transportations");

            migrationBuilder.DropTable(
                name: "Transporters");

            migrationBuilder.DropTable(
                name: "Truck");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
