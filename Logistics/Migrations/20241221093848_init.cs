using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Logistics.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    fullName = table.Column<string>(type: "text", nullable: false),
                    phone = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: true),
                    photo = table.Column<byte[]>(type: "bytea", nullable: true),
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
                    userId = table.Column<Guid>(type: "uuid", nullable: false),
                    series = table.Column<string>(type: "text", nullable: false),
                    number = table.Column<string>(type: "text", nullable: false),
                    issuedBy = table.Column<string>(type: "text", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false),
                    dateOfIssue = table.Column<string>(type: "text", nullable: false),
                    scan_fileName = table.Column<string>(type: "text", nullable: true),
                    scan_data = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passports", x => x.id);
                    table.ForeignKey(
                        name: "FK_Passports_Users_userId",
                        column: x => x.userId,
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
                    permanentResidence = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transporters", x => x.id);
                    table.ForeignKey(
                        name: "FK_Transporters_Users_id",
                        column: x => x.id,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    shipperId = table.Column<Guid>(type: "uuid", nullable: false),
                    creationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    loadCity = table.Column<int>(type: "integer", nullable: false),
                    loadAddress = table.Column<string>(type: "text", nullable: false),
                    unloadCity = table.Column<int>(type: "integer", nullable: false),
                    unloadAddress = table.Column<string>(type: "text", nullable: false),
                    receiverFullName = table.Column<string>(type: "text", nullable: true),
                    receiverContacts = table.Column<string>(type: "text", nullable: true),
                    sendingTimeFrom = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    sendingTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    arrivalTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    truckType = table.Column<int>(type: "integer", nullable: false),
                    costInRubles = table.Column<float>(type: "real", nullable: false),
                    additionalCostInRubles = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.id);
                    table.ForeignKey(
                        name: "FK_Requests_Shippers_shipperId",
                        column: x => x.shipperId,
                        principalTable: "Shippers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Licenses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    transporterId = table.Column<Guid>(type: "uuid", nullable: false),
                    series = table.Column<string>(type: "text", nullable: false),
                    number = table.Column<string>(type: "text", nullable: false),
                    scan_fileName = table.Column<string>(type: "text", nullable: true),
                    scan_data = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Licenses", x => x.id);
                    table.ForeignKey(
                        name: "FK_Licenses_Transporters_transporterId",
                        column: x => x.transporterId,
                        principalTable: "Transporters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Trucks",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    transporterId = table.Column<Guid>(type: "uuid", nullable: false),
                    truckBrand = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_Trucks", x => x.id);
                    table.ForeignKey(
                        name: "FK_Trucks_Transporters_transporterId",
                        column: x => x.transporterId,
                        principalTable: "Transporters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RejectedRequests",
                columns: table => new
                {
                    transporterId = table.Column<Guid>(type: "uuid", nullable: false),
                    requestId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RejectedRequests", x => new { x.transporterId, x.requestId });
                    table.ForeignKey(
                        name: "FK_RejectedRequests_Requests_requestId",
                        column: x => x.requestId,
                        principalTable: "Requests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RejectedRequests_Transporters_transporterId",
                        column: x => x.transporterId,
                        principalTable: "Transporters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shipments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    requestId = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    lengthInMeters = table.Column<float>(type: "real", nullable: false),
                    widthInMeters = table.Column<float>(type: "real", nullable: false),
                    heightInMeters = table.Column<float>(type: "real", nullable: false),
                    weightInTons = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipments", x => x.id);
                    table.ForeignKey(
                        name: "FK_Shipments_Requests_requestId",
                        column: x => x.requestId,
                        principalTable: "Requests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transportations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    requestId = table.Column<Guid>(type: "uuid", nullable: false),
                    transporterId = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transportations", x => x.id);
                    table.ForeignKey(
                        name: "FK_Transportations_Requests_requestId",
                        column: x => x.requestId,
                        principalTable: "Requests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transportations_Transporters_transporterId",
                        column: x => x.transporterId,
                        principalTable: "Transporters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    transportationId = table.Column<Guid>(type: "uuid", nullable: false),
                    reviewerId = table.Column<Guid>(type: "uuid", nullable: false),
                    userId = table.Column<Guid>(type: "uuid", nullable: false),
                    value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => new { x.transportationId, x.reviewerId, x.userId });
                    table.ForeignKey(
                        name: "FK_Reviews_Transportations_transportationId",
                        column: x => x.transportationId,
                        principalTable: "Transportations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransportationStatusChanges",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    transportationId = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportationStatusChanges", x => x.id);
                    table.ForeignKey(
                        name: "FK_TransportationStatusChanges_Transportations_transportationId",
                        column: x => x.transportationId,
                        principalTable: "Transportations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "id", "email", "fullName", "password", "phone", "photo", "role", "token", "INN", "companyName", "organizationalForm" },
                values: new object[,]
                {
                    { new Guid("1bd32de7-2d90-42f4-9742-39da75455127"), "shipper@gmail.com", "Семенов Александр Никитич", "240BE518FABD2724DDB6F04EEB1DA5967448D7E831C08C8FA822809F74C720A9", "+7 931 555 35 35", null, 0, null, "3450550943", "Herriot-Watt", 0 },
                    { new Guid("514b897c-c35f-4457-a64f-7d4f5dd97042"), "transporter@gmail.com", "Петров Анатолий Степанович", "BDD0E4C431DEF2F5CF7549034C0CB76B0F992F3991F0E6357B542F4B67DBE859", "+7 932 812 96 69", null, 1, null, "345055094345", null, 2 }
                });

            migrationBuilder.InsertData(
                table: "Passports",
                columns: new[] { "id", "code", "dateOfIssue", "issuedBy", "number", "series", "userId" },
                values: new object[,]
                {
                    { new Guid("d6cbd007-1b53-4edd-b661-1550c7c00b22"), "540-345", "21.08.2000", "УМВД РОССИИ ПО ТОМСКОЙ ОБЛАСТИ", "540964", "5305", new Guid("514b897c-c35f-4457-a64f-7d4f5dd97042") },
                    { new Guid("e38790d3-86ee-48f8-b316-1f08910ba7a6"), "540-666", "30.10.1991", "УМВД РОССИИ ПО ТОМСКОЙ ОБЛАСТИ", "952812", "9997", new Guid("1bd32de7-2d90-42f4-9742-39da75455127") }
                });

            migrationBuilder.InsertData(
                table: "Shippers",
                column: "id",
                value: new Guid("1bd32de7-2d90-42f4-9742-39da75455127"));

            migrationBuilder.InsertData(
                table: "Transporters",
                columns: new[] { "id", "permanentResidence" },
                values: new object[] { new Guid("514b897c-c35f-4457-a64f-7d4f5dd97042"), 0 });

            migrationBuilder.InsertData(
                table: "Licenses",
                columns: new[] { "id", "number", "series", "transporterId" },
                values: new object[] { new Guid("457fb177-a7f0-4cdb-9165-f8a3009a69d8"), "540964", "5305", new Guid("514b897c-c35f-4457-a64f-7d4f5dd97042") });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "id", "additionalCostInRubles", "arrivalTime", "costInRubles", "creationTime", "loadAddress", "loadCity", "receiverContacts", "receiverFullName", "sendingTime", "sendingTimeFrom", "shipperId", "status", "truckType", "unloadAddress", "unloadCity" },
                values: new object[,]
                {
                    { new Guid("130846cd-494e-4cd1-b0ad-b9577ecef6d3"), 0f, null, 196000f, new DateTime(2024, 12, 14, 9, 38, 48, 706, DateTimeKind.Utc).AddTicks(2107), "ул. Комсомольская, д. 33", 0, null, null, new DateTime(2024, 12, 24, 10, 18, 48, 706, DateTimeKind.Utc).AddTicks(2108), null, new Guid("1bd32de7-2d90-42f4-9742-39da75455127"), 0, 4, "ул. Ленина, д. 55", 2 },
                    { new Guid("2cb17ac5-dc63-4d34-b24f-f38bccb382d4"), 0f, new DateTime(2024, 12, 20, 9, 38, 48, 706, DateTimeKind.Utc).AddTicks(2094), 293750f, new DateTime(2024, 12, 14, 9, 38, 48, 706, DateTimeKind.Utc).AddTicks(2092), "ул. Нахимова, д. 8", 0, null, null, new DateTime(2024, 12, 14, 12, 38, 48, 706, DateTimeKind.Utc).AddTicks(2093), new DateTime(2024, 12, 14, 9, 38, 48, 706, DateTimeKind.Utc).AddTicks(2094), new Guid("1bd32de7-2d90-42f4-9742-39da75455127"), 5, 1, "ул. Советская, д. 76", 1 },
                    { new Guid("40e1beaa-5130-430a-9f1e-00543bb0ea94"), 0f, null, 196000f, new DateTime(2024, 12, 14, 9, 38, 48, 706, DateTimeKind.Utc).AddTicks(2102), "ул. Комсомольская, д. 33", 0, null, null, new DateTime(2024, 12, 24, 10, 18, 48, 706, DateTimeKind.Utc).AddTicks(2103), new DateTime(2024, 12, 24, 9, 38, 48, 706, DateTimeKind.Utc).AddTicks(2104), new Guid("1bd32de7-2d90-42f4-9742-39da75455127"), 0, 4, "ул. Ленина, д. 55", 2 },
                    { new Guid("644baa92-de01-49fa-8cb1-29d3497fef99"), 0f, null, 293750f, new DateTime(2024, 12, 21, 8, 58, 48, 706, DateTimeKind.Utc).AddTicks(2075), "ул. Комсомольская, д. 33", 0, null, null, new DateTime(2024, 12, 23, 12, 38, 48, 706, DateTimeKind.Utc).AddTicks(2083), new DateTime(2024, 12, 23, 9, 38, 48, 706, DateTimeKind.Utc).AddTicks(2084), new Guid("1bd32de7-2d90-42f4-9742-39da75455127"), 2, 1, "ул. Ленина, д. 55", 2 },
                    { new Guid("6db8dd93-54a8-497b-bb63-b9f67de9ed2f"), 0f, null, 293750f, new DateTime(2024, 12, 14, 9, 38, 48, 706, DateTimeKind.Utc).AddTicks(2097), "ул. Комсомольская, д. 33", 0, null, null, new DateTime(2024, 12, 24, 9, 38, 48, 706, DateTimeKind.Utc).AddTicks(2098), null, new Guid("1bd32de7-2d90-42f4-9742-39da75455127"), 0, 4, "ул. Ленина, д. 55", 2 },
                    { new Guid("e54d1c38-333c-40fe-88b3-f6b0757de3fa"), 0f, null, 196000f, new DateTime(2024, 12, 14, 9, 38, 48, 706, DateTimeKind.Utc).AddTicks(2110), "ул. Комсомольская, д. 33", 0, null, null, new DateTime(2024, 12, 24, 10, 18, 48, 706, DateTimeKind.Utc).AddTicks(2111), null, new Guid("1bd32de7-2d90-42f4-9742-39da75455127"), 1, 4, "ул. Ленина, д. 55", 2 }
                });

            migrationBuilder.InsertData(
                table: "Trucks",
                columns: new[] { "id", "heightInMeters", "lengthInMeters", "loadCapacityInTons", "model", "number", "regionCode", "transporterId", "truckBrand", "truckType", "widthInMeters", "yearOfProduction" },
                values: new object[] { new Guid("4e1102ef-33df-470e-ad7a-1e47c72da866"), 3f, 10f, 20, "5Sjp", "A000AA", 70, new Guid("514b897c-c35f-4457-a64f-7d4f5dd97042"), 0, 1, 2.5f, 1999 });

            migrationBuilder.InsertData(
                table: "RejectedRequests",
                columns: new[] { "requestId", "transporterId" },
                values: new object[] { new Guid("6db8dd93-54a8-497b-bb63-b9f67de9ed2f"), new Guid("514b897c-c35f-4457-a64f-7d4f5dd97042") });

            migrationBuilder.InsertData(
                table: "Shipments",
                columns: new[] { "id", "heightInMeters", "lengthInMeters", "requestId", "type", "weightInTons", "widthInMeters" },
                values: new object[,]
                {
                    { new Guid("01a75707-137d-4d39-a1d3-01fe3b917d83"), 2f, 5f, new Guid("2cb17ac5-dc63-4d34-b24f-f38bccb382d4"), 6, 5f, 2f },
                    { new Guid("1744dd03-f3f0-4597-84c3-7e4ce51ee5e2"), 2f, 5f, new Guid("644baa92-de01-49fa-8cb1-29d3497fef99"), 1, 5f, 2f },
                    { new Guid("724c71d5-8cbf-4e13-88f4-61a72380f056"), 2f, 5f, new Guid("130846cd-494e-4cd1-b0ad-b9577ecef6d3"), 6, 5f, 2f },
                    { new Guid("ed30d2e3-8cdc-4147-bbe6-26f7618277a9"), 2f, 5f, new Guid("6db8dd93-54a8-497b-bb63-b9f67de9ed2f"), 1, 5f, 2f },
                    { new Guid("f82476e6-6d47-4a74-8d33-4aca27263783"), 2f, 5f, new Guid("e54d1c38-333c-40fe-88b3-f6b0757de3fa"), 6, 5f, 2f },
                    { new Guid("fd54b173-d029-4ea7-ad10-4e4df73293cf"), 2f, 5f, new Guid("40e1beaa-5130-430a-9f1e-00543bb0ea94"), 6, 5f, 2f }
                });

            migrationBuilder.InsertData(
                table: "Transportations",
                columns: new[] { "id", "requestId", "status", "transporterId" },
                values: new object[,]
                {
                    { new Guid("92b9b9d9-35a1-4228-aa08-b64dae8e3af8"), new Guid("2cb17ac5-dc63-4d34-b24f-f38bccb382d4"), 6, new Guid("514b897c-c35f-4457-a64f-7d4f5dd97042") },
                    { new Guid("b4e4ed8a-7313-442f-bc38-9cde832fcd5e"), new Guid("644baa92-de01-49fa-8cb1-29d3497fef99"), 2, new Guid("514b897c-c35f-4457-a64f-7d4f5dd97042") }
                });

            migrationBuilder.InsertData(
                table: "TransportationStatusChanges",
                columns: new[] { "id", "status", "time", "transportationId" },
                values: new object[,]
                {
                    { new Guid("112a8d6d-1407-4ec5-89f2-936d239d3782"), 4, new DateTime(2024, 12, 19, 9, 38, 48, 706, DateTimeKind.Utc).AddTicks(2234), new Guid("92b9b9d9-35a1-4228-aa08-b64dae8e3af8") },
                    { new Guid("14c61851-9651-496a-9cb5-abac10cf186f"), 1, new DateTime(2024, 12, 21, 5, 38, 48, 706, DateTimeKind.Utc).AddTicks(2224), new Guid("b4e4ed8a-7313-442f-bc38-9cde832fcd5e") },
                    { new Guid("3208483a-1f65-40f1-bd3d-e15d73253c92"), 6, new DateTime(2024, 12, 21, 8, 58, 48, 706, DateTimeKind.Utc).AddTicks(2239), new Guid("92b9b9d9-35a1-4228-aa08-b64dae8e3af8") },
                    { new Guid("5f124dad-2f47-4caf-839b-725808db4d1e"), 0, new DateTime(2024, 12, 20, 9, 38, 48, 706, DateTimeKind.Utc).AddTicks(2220), new Guid("b4e4ed8a-7313-442f-bc38-9cde832fcd5e") },
                    { new Guid("772746d7-ebf5-486e-8951-7dc062e4e8e9"), 3, new DateTime(2024, 12, 18, 9, 38, 48, 706, DateTimeKind.Utc).AddTicks(2233), new Guid("92b9b9d9-35a1-4228-aa08-b64dae8e3af8") },
                    { new Guid("85895507-2efa-4753-9e8d-085b212fa13a"), 2, new DateTime(2024, 12, 17, 9, 38, 48, 706, DateTimeKind.Utc).AddTicks(2231), new Guid("92b9b9d9-35a1-4228-aa08-b64dae8e3af8") },
                    { new Guid("8dc1b823-b7f1-4d42-931b-158833ed8b4a"), 0, new DateTime(2024, 12, 15, 9, 38, 48, 706, DateTimeKind.Utc).AddTicks(2228), new Guid("92b9b9d9-35a1-4228-aa08-b64dae8e3af8") },
                    { new Guid("8dcef863-2ed9-4469-bda1-16a05f8c6949"), 1, new DateTime(2024, 12, 16, 9, 38, 48, 706, DateTimeKind.Utc).AddTicks(2230), new Guid("92b9b9d9-35a1-4228-aa08-b64dae8e3af8") },
                    { new Guid("ab49b6c2-893f-412e-8adc-bfacae445da3"), 5, new DateTime(2024, 12, 20, 9, 38, 48, 706, DateTimeKind.Utc).AddTicks(2236), new Guid("92b9b9d9-35a1-4228-aa08-b64dae8e3af8") },
                    { new Guid("ca806edf-74d8-4bd0-bde0-26c3ab3ad04d"), 2, new DateTime(2024, 12, 21, 8, 58, 48, 706, DateTimeKind.Utc).AddTicks(2226), new Guid("b4e4ed8a-7313-442f-bc38-9cde832fcd5e") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Licenses_transporterId",
                table: "Licenses",
                column: "transporterId");

            migrationBuilder.CreateIndex(
                name: "IX_Passports_userId",
                table: "Passports",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_PendingEmails_userid",
                table: "PendingEmails",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_RejectedRequests_requestId",
                table: "RejectedRequests",
                column: "requestId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_shipperId",
                table: "Requests",
                column: "shipperId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_userId",
                table: "Reviews",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_requestId",
                table: "Shipments",
                column: "requestId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transportations_requestId",
                table: "Transportations",
                column: "requestId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transportations_transporterId",
                table: "Transportations",
                column: "transporterId");

            migrationBuilder.CreateIndex(
                name: "IX_TransportationStatusChanges_transportationId",
                table: "TransportationStatusChanges",
                column: "transportationId");

            migrationBuilder.CreateIndex(
                name: "IX_Trucks_transporterId",
                table: "Trucks",
                column: "transporterId",
                unique: true);
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
                name: "RejectedRequests");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "Shipments");

            migrationBuilder.DropTable(
                name: "TransportationStatusChanges");

            migrationBuilder.DropTable(
                name: "Trucks");

            migrationBuilder.DropTable(
                name: "Transportations");

            migrationBuilder.DropTable(
                name: "Requests");

            migrationBuilder.DropTable(
                name: "Transporters");

            migrationBuilder.DropTable(
                name: "Shippers");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
