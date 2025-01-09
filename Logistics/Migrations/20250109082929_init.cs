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
                    { new Guid("ad00bd02-91cf-49b5-9519-0f75aa6feb19"), "transporter@gmail.com", "Петров Анатолий Степанович", "BDD0E4C431DEF2F5CF7549034C0CB76B0F992F3991F0E6357B542F4B67DBE859", "+7 932 812 96 69", null, 1, null, "345055094345", null, 2 },
                    { new Guid("c652f8fd-aa0e-4a91-99b1-199c75dfd8a8"), "shipper@gmail.com", "Семенов Александр Никитич", "240BE518FABD2724DDB6F04EEB1DA5967448D7E831C08C8FA822809F74C720A9", "+7 931 555 35 35", null, 0, null, "3450550943", "Herriot-Watt", 0 }
                });

            migrationBuilder.InsertData(
                table: "Passports",
                columns: new[] { "id", "code", "dateOfIssue", "issuedBy", "number", "series", "userId" },
                values: new object[,]
                {
                    { new Guid("7ed61137-76f8-47c4-968f-a7a2d5363b86"), "540-666", "30.10.1991", "УМВД РОССИИ ПО ТОМСКОЙ ОБЛАСТИ", "952812", "9997", new Guid("c652f8fd-aa0e-4a91-99b1-199c75dfd8a8") },
                    { new Guid("ab72c67d-67b9-4c2a-955c-062cf4b7ccc3"), "540-345", "21.08.2000", "УМВД РОССИИ ПО ТОМСКОЙ ОБЛАСТИ", "540964", "5305", new Guid("ad00bd02-91cf-49b5-9519-0f75aa6feb19") }
                });

            migrationBuilder.InsertData(
                table: "Shippers",
                column: "id",
                value: new Guid("c652f8fd-aa0e-4a91-99b1-199c75dfd8a8"));

            migrationBuilder.InsertData(
                table: "Transporters",
                columns: new[] { "id", "permanentResidence" },
                values: new object[] { new Guid("ad00bd02-91cf-49b5-9519-0f75aa6feb19"), 0 });

            migrationBuilder.InsertData(
                table: "Licenses",
                columns: new[] { "id", "number", "series", "transporterId" },
                values: new object[] { new Guid("a823874d-9a95-47a2-8db5-95dbdc679a45"), "540964", "5305", new Guid("ad00bd02-91cf-49b5-9519-0f75aa6feb19") });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "id", "additionalCostInRubles", "arrivalTime", "costInRubles", "creationTime", "loadAddress", "loadCity", "receiverContacts", "receiverFullName", "sendingTime", "sendingTimeFrom", "shipperId", "status", "truckType", "unloadAddress", "unloadCity" },
                values: new object[,]
                {
                    { new Guid("1ac8e0d5-4f7c-41ba-a435-9968daa5c429"), 0f, null, 196000f, new DateTime(2025, 1, 2, 8, 29, 28, 296, DateTimeKind.Utc).AddTicks(847), "ул. Комсомольская, д. 33", 0, null, null, new DateTime(2025, 1, 12, 9, 9, 28, 296, DateTimeKind.Utc).AddTicks(848), new DateTime(2025, 1, 12, 8, 29, 28, 296, DateTimeKind.Utc).AddTicks(849), new Guid("c652f8fd-aa0e-4a91-99b1-199c75dfd8a8"), 0, 4, "ул. Ленина, д. 55", 2 },
                    { new Guid("404a6b20-7b7a-475d-a3ed-241389b84d71"), 0f, null, 293750f, new DateTime(2025, 1, 9, 7, 49, 28, 296, DateTimeKind.Utc).AddTicks(822), "ул. Комсомольская, д. 33", 0, null, null, new DateTime(2025, 1, 11, 11, 29, 28, 296, DateTimeKind.Utc).AddTicks(828), new DateTime(2025, 1, 11, 8, 29, 28, 296, DateTimeKind.Utc).AddTicks(830), new Guid("c652f8fd-aa0e-4a91-99b1-199c75dfd8a8"), 2, 1, "ул. Ленина, д. 55", 2 },
                    { new Guid("5d77b8c9-382b-4b04-8eab-ff14634f447f"), 0f, null, 196000f, new DateTime(2025, 1, 2, 8, 29, 28, 296, DateTimeKind.Utc).AddTicks(855), "ул. Комсомольская, д. 33", 0, null, null, new DateTime(2025, 1, 12, 9, 9, 28, 296, DateTimeKind.Utc).AddTicks(856), null, new Guid("c652f8fd-aa0e-4a91-99b1-199c75dfd8a8"), 1, 4, "ул. Ленина, д. 55", 2 },
                    { new Guid("a303c657-df3f-4b83-8055-921adf86c7b7"), 0f, null, 196000f, new DateTime(2025, 1, 2, 8, 29, 28, 296, DateTimeKind.Utc).AddTicks(852), "ул. Комсомольская, д. 33", 0, null, null, new DateTime(2025, 1, 12, 9, 9, 28, 296, DateTimeKind.Utc).AddTicks(853), null, new Guid("c652f8fd-aa0e-4a91-99b1-199c75dfd8a8"), 0, 4, "ул. Ленина, д. 55", 2 },
                    { new Guid("ab3f99db-9320-4c7d-bac6-878743a4c088"), 0f, new DateTime(2025, 1, 8, 8, 29, 28, 296, DateTimeKind.Utc).AddTicks(841), 293750f, new DateTime(2025, 1, 2, 8, 29, 28, 296, DateTimeKind.Utc).AddTicks(838), "ул. Нахимова, д. 8", 0, null, null, new DateTime(2025, 1, 2, 11, 29, 28, 296, DateTimeKind.Utc).AddTicks(840), new DateTime(2025, 1, 2, 8, 29, 28, 296, DateTimeKind.Utc).AddTicks(840), new Guid("c652f8fd-aa0e-4a91-99b1-199c75dfd8a8"), 5, 1, "ул. Советская, д. 76", 1 },
                    { new Guid("d6e1d164-2fa4-4902-961e-a134ab0ac947"), 0f, null, 293750f, new DateTime(2025, 1, 2, 8, 29, 28, 296, DateTimeKind.Utc).AddTicks(843), "ул. Комсомольская, д. 33", 0, null, null, new DateTime(2025, 1, 12, 8, 29, 28, 296, DateTimeKind.Utc).AddTicks(844), null, new Guid("c652f8fd-aa0e-4a91-99b1-199c75dfd8a8"), 0, 4, "ул. Ленина, д. 55", 2 }
                });

            migrationBuilder.InsertData(
                table: "Trucks",
                columns: new[] { "id", "heightInMeters", "lengthInMeters", "loadCapacityInTons", "model", "number", "regionCode", "transporterId", "truckBrand", "truckType", "widthInMeters", "yearOfProduction" },
                values: new object[] { new Guid("d4d8fd86-a0dc-4850-85ab-59ac23fc86b4"), 3f, 10f, 20, "5Sjp", "A000AA", 70, new Guid("ad00bd02-91cf-49b5-9519-0f75aa6feb19"), 0, 1, 2.5f, 1999 });

            migrationBuilder.InsertData(
                table: "RejectedRequests",
                columns: new[] { "requestId", "transporterId" },
                values: new object[] { new Guid("d6e1d164-2fa4-4902-961e-a134ab0ac947"), new Guid("ad00bd02-91cf-49b5-9519-0f75aa6feb19") });

            migrationBuilder.InsertData(
                table: "Shipments",
                columns: new[] { "id", "heightInMeters", "lengthInMeters", "requestId", "type", "weightInTons", "widthInMeters" },
                values: new object[,]
                {
                    { new Guid("08159fab-72ca-4933-93f9-0f7c4ba8b801"), 2f, 5f, new Guid("1ac8e0d5-4f7c-41ba-a435-9968daa5c429"), 6, 5f, 2f },
                    { new Guid("09c2fa8c-6c57-4638-b16b-4f431a114358"), 2f, 5f, new Guid("5d77b8c9-382b-4b04-8eab-ff14634f447f"), 6, 5f, 2f },
                    { new Guid("27469b5e-645d-4cbc-a717-bdef5a1bbb06"), 2f, 5f, new Guid("d6e1d164-2fa4-4902-961e-a134ab0ac947"), 1, 5f, 2f },
                    { new Guid("35f51937-4522-4302-a0b3-011906dac771"), 2f, 5f, new Guid("ab3f99db-9320-4c7d-bac6-878743a4c088"), 6, 5f, 2f },
                    { new Guid("853dea98-37a4-402d-8895-3ee03183542a"), 2f, 5f, new Guid("a303c657-df3f-4b83-8055-921adf86c7b7"), 6, 5f, 2f },
                    { new Guid("dc5db599-f3c0-446b-8a16-7baa4842c50a"), 2f, 5f, new Guid("404a6b20-7b7a-475d-a3ed-241389b84d71"), 1, 5f, 2f }
                });

            migrationBuilder.InsertData(
                table: "Transportations",
                columns: new[] { "id", "requestId", "status", "transporterId" },
                values: new object[,]
                {
                    { new Guid("0fe37385-0d20-4c5a-be16-54564f72b569"), new Guid("ab3f99db-9320-4c7d-bac6-878743a4c088"), 6, new Guid("ad00bd02-91cf-49b5-9519-0f75aa6feb19") },
                    { new Guid("e3f4bcc5-6c47-44d7-bd32-a30e2c4f92a4"), new Guid("404a6b20-7b7a-475d-a3ed-241389b84d71"), 2, new Guid("ad00bd02-91cf-49b5-9519-0f75aa6feb19") }
                });

            migrationBuilder.InsertData(
                table: "TransportationStatusChanges",
                columns: new[] { "id", "status", "time", "transportationId" },
                values: new object[,]
                {
                    { new Guid("1382bc15-c122-44a4-98e9-91c5e7a94a8a"), 5, new DateTime(2025, 1, 8, 8, 29, 28, 296, DateTimeKind.Utc).AddTicks(980), new Guid("0fe37385-0d20-4c5a-be16-54564f72b569") },
                    { new Guid("3a33e845-44c4-490f-90e1-5552fe5b64f7"), 4, new DateTime(2025, 1, 7, 8, 29, 28, 296, DateTimeKind.Utc).AddTicks(979), new Guid("0fe37385-0d20-4c5a-be16-54564f72b569") },
                    { new Guid("3bbd5318-4ff7-4940-8eb9-38ef358195c0"), 1, new DateTime(2025, 1, 9, 4, 29, 28, 296, DateTimeKind.Utc).AddTicks(967), new Guid("e3f4bcc5-6c47-44d7-bd32-a30e2c4f92a4") },
                    { new Guid("4985675a-710c-4f35-a02c-3cd0eb8cb9c8"), 6, new DateTime(2025, 1, 9, 7, 49, 28, 296, DateTimeKind.Utc).AddTicks(982), new Guid("0fe37385-0d20-4c5a-be16-54564f72b569") },
                    { new Guid("72b6ea99-c58b-449a-85b9-c4bbdcb11bc3"), 3, new DateTime(2025, 1, 6, 8, 29, 28, 296, DateTimeKind.Utc).AddTicks(977), new Guid("0fe37385-0d20-4c5a-be16-54564f72b569") },
                    { new Guid("72bee96c-9740-4327-b354-6fb0681e80ae"), 0, new DateTime(2025, 1, 8, 8, 29, 28, 296, DateTimeKind.Utc).AddTicks(965), new Guid("e3f4bcc5-6c47-44d7-bd32-a30e2c4f92a4") },
                    { new Guid("7f1b9170-7488-406b-bc3c-397923023383"), 2, new DateTime(2025, 1, 5, 8, 29, 28, 296, DateTimeKind.Utc).AddTicks(975), new Guid("0fe37385-0d20-4c5a-be16-54564f72b569") },
                    { new Guid("d618eef5-087a-4cc3-8cc6-0bdd5187935f"), 1, new DateTime(2025, 1, 4, 8, 29, 28, 296, DateTimeKind.Utc).AddTicks(974), new Guid("0fe37385-0d20-4c5a-be16-54564f72b569") },
                    { new Guid("dc7298ac-a16a-4742-8483-5c7b5c628308"), 0, new DateTime(2025, 1, 3, 8, 29, 28, 296, DateTimeKind.Utc).AddTicks(972), new Guid("0fe37385-0d20-4c5a-be16-54564f72b569") },
                    { new Guid("f745d5b5-d6f1-4908-90a7-c891cfd82d78"), 2, new DateTime(2025, 1, 9, 7, 49, 28, 296, DateTimeKind.Utc).AddTicks(971), new Guid("e3f4bcc5-6c47-44d7-bd32-a30e2c4f92a4") }
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
