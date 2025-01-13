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
                    photoId = table.Column<Guid>(type: "uuid", nullable: true),
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
                    scan_fileId = table.Column<Guid>(type: "uuid", nullable: true)
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
                    scan_fileId = table.Column<Guid>(type: "uuid", nullable: true)
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
                columns: new[] { "id", "email", "fullName", "password", "phone", "photoId", "role", "token", "INN", "companyName", "organizationalForm" },
                values: new object[,]
                {
                    { new Guid("2ff859e1-4105-4a14-b24c-32f2f446c24a"), "shipper@gmail.com", "Семенов Александр Никитич", "240BE518FABD2724DDB6F04EEB1DA5967448D7E831C08C8FA822809F74C720A9", "+7 931 555 35 35", null, 0, null, "3450550943", "Herriot-Watt", 0 },
                    { new Guid("551ea8b1-747e-46fd-9a34-6e6f74f721bb"), "transporter@gmail.com", "Петров Анатолий Степанович", "BDD0E4C431DEF2F5CF7549034C0CB76B0F992F3991F0E6357B542F4B67DBE859", "+7 932 812 96 69", null, 1, null, "345055094345", null, 2 }
                });

            migrationBuilder.InsertData(
                table: "Passports",
                columns: new[] { "id", "code", "dateOfIssue", "issuedBy", "number", "series", "userId" },
                values: new object[,]
                {
                    { new Guid("0ff49b4a-daff-4202-a3e8-5a3e8f23fd6b"), "540-345", "21.08.2000", "УМВД РОССИИ ПО ТОМСКОЙ ОБЛАСТИ", "540964", "5305", new Guid("551ea8b1-747e-46fd-9a34-6e6f74f721bb") },
                    { new Guid("d105a3e3-e5e6-4ae3-9682-adc4f5a43426"), "540-666", "30.10.1991", "УМВД РОССИИ ПО ТОМСКОЙ ОБЛАСТИ", "952812", "9997", new Guid("2ff859e1-4105-4a14-b24c-32f2f446c24a") }
                });

            migrationBuilder.InsertData(
                table: "Shippers",
                column: "id",
                value: new Guid("2ff859e1-4105-4a14-b24c-32f2f446c24a"));

            migrationBuilder.InsertData(
                table: "Transporters",
                columns: new[] { "id", "permanentResidence" },
                values: new object[] { new Guid("551ea8b1-747e-46fd-9a34-6e6f74f721bb"), 0 });

            migrationBuilder.InsertData(
                table: "Licenses",
                columns: new[] { "id", "number", "series", "transporterId" },
                values: new object[] { new Guid("1420aab3-6c76-437b-a5c3-333aaa639af3"), "540964", "5305", new Guid("551ea8b1-747e-46fd-9a34-6e6f74f721bb") });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "id", "additionalCostInRubles", "arrivalTime", "costInRubles", "creationTime", "loadAddress", "loadCity", "receiverContacts", "receiverFullName", "sendingTime", "sendingTimeFrom", "shipperId", "status", "truckType", "unloadAddress", "unloadCity" },
                values: new object[,]
                {
                    { new Guid("10e0b282-f3a9-4389-90f9-9631b6c9f703"), 0f, null, 293750f, new DateTime(2025, 1, 13, 12, 27, 0, 442, DateTimeKind.Utc).AddTicks(5590), "ул. Комсомольская, д. 33", 0, null, null, new DateTime(2025, 1, 15, 16, 7, 0, 442, DateTimeKind.Utc).AddTicks(5595), new DateTime(2025, 1, 15, 13, 7, 0, 442, DateTimeKind.Utc).AddTicks(5598), new Guid("2ff859e1-4105-4a14-b24c-32f2f446c24a"), 2, 1, "ул. Ленина, д. 55", 2 },
                    { new Guid("34d15192-2707-4c45-943f-7ea3bc22f886"), 0f, null, 196000f, new DateTime(2025, 1, 6, 13, 7, 0, 442, DateTimeKind.Utc).AddTicks(5650), "ул. Комсомольская, д. 33", 0, null, null, new DateTime(2025, 1, 16, 13, 47, 0, 442, DateTimeKind.Utc).AddTicks(5651), null, new Guid("2ff859e1-4105-4a14-b24c-32f2f446c24a"), 1, 4, "ул. Ленина, д. 55", 2 },
                    { new Guid("5bc92ceb-97cd-42bd-9dc0-c9b19d909a83"), 0f, null, 196000f, new DateTime(2025, 1, 6, 13, 7, 0, 442, DateTimeKind.Utc).AddTicks(5646), "ул. Комсомольская, д. 33", 0, null, null, new DateTime(2025, 1, 16, 13, 47, 0, 442, DateTimeKind.Utc).AddTicks(5647), null, new Guid("2ff859e1-4105-4a14-b24c-32f2f446c24a"), 0, 4, "ул. Ленина, д. 55", 2 },
                    { new Guid("883be808-d7eb-461f-b9e9-984f452618d1"), 0f, null, 293750f, new DateTime(2025, 1, 6, 13, 7, 0, 442, DateTimeKind.Utc).AddTicks(5637), "ул. Комсомольская, д. 33", 0, null, null, new DateTime(2025, 1, 16, 13, 7, 0, 442, DateTimeKind.Utc).AddTicks(5638), null, new Guid("2ff859e1-4105-4a14-b24c-32f2f446c24a"), 0, 4, "ул. Ленина, д. 55", 2 },
                    { new Guid("a96d1d36-e374-4e2a-b2f0-86fd89bcc519"), 0f, null, 196000f, new DateTime(2025, 1, 6, 13, 7, 0, 442, DateTimeKind.Utc).AddTicks(5642), "ул. Комсомольская, д. 33", 0, null, null, new DateTime(2025, 1, 16, 13, 47, 0, 442, DateTimeKind.Utc).AddTicks(5643), new DateTime(2025, 1, 16, 13, 7, 0, 442, DateTimeKind.Utc).AddTicks(5643), new Guid("2ff859e1-4105-4a14-b24c-32f2f446c24a"), 0, 4, "ул. Ленина, д. 55", 2 },
                    { new Guid("c45aeff9-d1a8-4142-a0d1-8eebd29b0929"), 0f, new DateTime(2025, 1, 12, 13, 7, 0, 442, DateTimeKind.Utc).AddTicks(5634), 293750f, new DateTime(2025, 1, 6, 13, 7, 0, 442, DateTimeKind.Utc).AddTicks(5631), "ул. Нахимова, д. 8", 0, null, null, new DateTime(2025, 1, 6, 16, 7, 0, 442, DateTimeKind.Utc).AddTicks(5633), new DateTime(2025, 1, 6, 13, 7, 0, 442, DateTimeKind.Utc).AddTicks(5633), new Guid("2ff859e1-4105-4a14-b24c-32f2f446c24a"), 5, 1, "ул. Советская, д. 76", 1 }
                });

            migrationBuilder.InsertData(
                table: "Trucks",
                columns: new[] { "id", "heightInMeters", "lengthInMeters", "loadCapacityInTons", "model", "number", "regionCode", "transporterId", "truckBrand", "truckType", "widthInMeters", "yearOfProduction" },
                values: new object[] { new Guid("1ce9a0cc-3292-47a7-b0b4-e5365dfaa603"), 3f, 10f, 20, "5Sjp", "A000AA", 70, new Guid("551ea8b1-747e-46fd-9a34-6e6f74f721bb"), 0, 1, 2.5f, 1999 });

            migrationBuilder.InsertData(
                table: "RejectedRequests",
                columns: new[] { "requestId", "transporterId" },
                values: new object[] { new Guid("883be808-d7eb-461f-b9e9-984f452618d1"), new Guid("551ea8b1-747e-46fd-9a34-6e6f74f721bb") });

            migrationBuilder.InsertData(
                table: "Shipments",
                columns: new[] { "id", "heightInMeters", "lengthInMeters", "requestId", "type", "weightInTons", "widthInMeters" },
                values: new object[,]
                {
                    { new Guid("16eeeb98-5480-4142-a248-f1056e6b6cf4"), 2f, 5f, new Guid("34d15192-2707-4c45-943f-7ea3bc22f886"), 6, 5f, 2f },
                    { new Guid("4eb23557-b415-409c-8e02-aa9aeb6bd3d5"), 2f, 5f, new Guid("10e0b282-f3a9-4389-90f9-9631b6c9f703"), 1, 5f, 2f },
                    { new Guid("5f96e0dc-45f6-4faf-915a-db3e64096f6e"), 2f, 5f, new Guid("c45aeff9-d1a8-4142-a0d1-8eebd29b0929"), 6, 5f, 2f },
                    { new Guid("631057a7-b9b8-42b0-859c-f7abdfa8c9e0"), 2f, 5f, new Guid("883be808-d7eb-461f-b9e9-984f452618d1"), 1, 5f, 2f },
                    { new Guid("6aab8759-d911-485e-bb33-16e74581cbd7"), 2f, 5f, new Guid("a96d1d36-e374-4e2a-b2f0-86fd89bcc519"), 6, 5f, 2f },
                    { new Guid("fe8bcac9-738d-4305-b510-330c5e94bcb4"), 2f, 5f, new Guid("5bc92ceb-97cd-42bd-9dc0-c9b19d909a83"), 6, 5f, 2f }
                });

            migrationBuilder.InsertData(
                table: "Transportations",
                columns: new[] { "id", "requestId", "status", "transporterId" },
                values: new object[,]
                {
                    { new Guid("52dc6a12-f4cb-495f-acfc-26c0e48259f8"), new Guid("c45aeff9-d1a8-4142-a0d1-8eebd29b0929"), 6, new Guid("551ea8b1-747e-46fd-9a34-6e6f74f721bb") },
                    { new Guid("e6a1a126-72a8-4977-9e24-7b8a276c7b2e"), new Guid("10e0b282-f3a9-4389-90f9-9631b6c9f703"), 2, new Guid("551ea8b1-747e-46fd-9a34-6e6f74f721bb") }
                });

            migrationBuilder.InsertData(
                table: "TransportationStatusChanges",
                columns: new[] { "id", "status", "time", "transportationId" },
                values: new object[,]
                {
                    { new Guid("08640e44-d184-4c15-96ae-beff89b41ea4"), 4, new DateTime(2025, 1, 11, 13, 7, 0, 442, DateTimeKind.Utc).AddTicks(5782), new Guid("52dc6a12-f4cb-495f-acfc-26c0e48259f8") },
                    { new Guid("0c73878d-c382-4f01-888c-7d6bea29ff28"), 2, new DateTime(2025, 1, 13, 12, 27, 0, 442, DateTimeKind.Utc).AddTicks(5772), new Guid("e6a1a126-72a8-4977-9e24-7b8a276c7b2e") },
                    { new Guid("4a91bd25-a53a-4a8d-ab1f-36ad87118632"), 3, new DateTime(2025, 1, 10, 13, 7, 0, 442, DateTimeKind.Utc).AddTicks(5781), new Guid("52dc6a12-f4cb-495f-acfc-26c0e48259f8") },
                    { new Guid("503c855d-43c4-48e8-a0c1-00b24ffd8072"), 5, new DateTime(2025, 1, 12, 13, 7, 0, 442, DateTimeKind.Utc).AddTicks(5784), new Guid("52dc6a12-f4cb-495f-acfc-26c0e48259f8") },
                    { new Guid("64346a6d-c1e3-4180-a00c-70da1c8b26ba"), 2, new DateTime(2025, 1, 9, 13, 7, 0, 442, DateTimeKind.Utc).AddTicks(5779), new Guid("52dc6a12-f4cb-495f-acfc-26c0e48259f8") },
                    { new Guid("7cc22088-9068-43ae-ae3d-b125eac42737"), 0, new DateTime(2025, 1, 12, 13, 7, 0, 442, DateTimeKind.Utc).AddTicks(5768), new Guid("e6a1a126-72a8-4977-9e24-7b8a276c7b2e") },
                    { new Guid("90b31e34-1f8f-4ec2-86bf-36680c7da8a6"), 0, new DateTime(2025, 1, 7, 13, 7, 0, 442, DateTimeKind.Utc).AddTicks(5776), new Guid("52dc6a12-f4cb-495f-acfc-26c0e48259f8") },
                    { new Guid("bc154f23-4072-48b4-8bec-0eb0372f2805"), 1, new DateTime(2025, 1, 13, 9, 7, 0, 442, DateTimeKind.Utc).AddTicks(5770), new Guid("e6a1a126-72a8-4977-9e24-7b8a276c7b2e") },
                    { new Guid("d38e64db-3b34-484e-a049-9a5f2dbde991"), 1, new DateTime(2025, 1, 8, 13, 7, 0, 442, DateTimeKind.Utc).AddTicks(5777), new Guid("52dc6a12-f4cb-495f-acfc-26c0e48259f8") },
                    { new Guid("fede16f2-a602-4120-b9f9-bf30a7416d27"), 6, new DateTime(2025, 1, 13, 12, 27, 0, 442, DateTimeKind.Utc).AddTicks(5786), new Guid("52dc6a12-f4cb-495f-acfc-26c0e48259f8") }
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
