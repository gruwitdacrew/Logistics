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
                    { new Guid("015833d6-dbbf-405e-9479-8fe3ae959a28"), "transporter@gmail.com", "Петров Анатолий Степанович", "BDD0E4C431DEF2F5CF7549034C0CB76B0F992F3991F0E6357B542F4B67DBE859", "+7 932 812 96 69", null, 1, null, "345055094345", null, 2 },
                    { new Guid("fb17c769-149d-4e2c-b386-73aa6abde8ad"), "shipper@gmail.com", "Семенов Александр Никитич", "240BE518FABD2724DDB6F04EEB1DA5967448D7E831C08C8FA822809F74C720A9", "+7 931 555 35 35", null, 0, null, "3450550943", "Herriot-Watt", 0 }
                });

            migrationBuilder.InsertData(
                table: "Passports",
                columns: new[] { "id", "code", "dateOfIssue", "issuedBy", "number", "series", "userId" },
                values: new object[,]
                {
                    { new Guid("0654bd9b-4bee-4134-993a-f96e14a344fc"), "540-666", "30.10.1991", "УМВД РОССИИ ПО ТОМСКОЙ ОБЛАСТИ", "952812", "9997", new Guid("fb17c769-149d-4e2c-b386-73aa6abde8ad") },
                    { new Guid("28732ac2-8351-47f7-b92e-861a962ef56f"), "540-345", "21.08.2000", "УМВД РОССИИ ПО ТОМСКОЙ ОБЛАСТИ", "540964", "5305", new Guid("015833d6-dbbf-405e-9479-8fe3ae959a28") }
                });

            migrationBuilder.InsertData(
                table: "Shippers",
                column: "id",
                value: new Guid("fb17c769-149d-4e2c-b386-73aa6abde8ad"));

            migrationBuilder.InsertData(
                table: "Transporters",
                columns: new[] { "id", "permanentResidence" },
                values: new object[] { new Guid("015833d6-dbbf-405e-9479-8fe3ae959a28"), 0 });

            migrationBuilder.InsertData(
                table: "Licenses",
                columns: new[] { "id", "number", "series", "transporterId" },
                values: new object[] { new Guid("2bdf9ccd-9059-492a-9554-cbae8d0bb92c"), "540964", "5305", new Guid("015833d6-dbbf-405e-9479-8fe3ae959a28") });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "id", "additionalCostInRubles", "arrivalTime", "costInRubles", "creationTime", "loadAddress", "loadCity", "receiverContacts", "receiverFullName", "sendingTime", "sendingTimeFrom", "shipperId", "status", "truckType", "unloadAddress", "unloadCity" },
                values: new object[,]
                {
                    { new Guid("194c7df6-1bfc-4edb-9921-7c5048946aef"), 0f, null, 196000f, new DateTime(2025, 1, 10, 18, 30, 8, 576, DateTimeKind.Utc).AddTicks(3396), "ул. Комсомольская, д. 33", 0, null, null, new DateTime(2025, 1, 20, 19, 10, 8, 576, DateTimeKind.Utc).AddTicks(3397), null, new Guid("fb17c769-149d-4e2c-b386-73aa6abde8ad"), 0, 4, "ул. Ленина, д. 55", 2 },
                    { new Guid("398d9ab7-8b9d-4319-8378-6dd30ad9894a"), 0f, null, 196000f, new DateTime(2025, 1, 10, 18, 30, 8, 576, DateTimeKind.Utc).AddTicks(3400), "ул. Комсомольская, д. 33", 0, null, null, new DateTime(2025, 1, 20, 19, 10, 8, 576, DateTimeKind.Utc).AddTicks(3401), null, new Guid("fb17c769-149d-4e2c-b386-73aa6abde8ad"), 1, 4, "ул. Ленина, д. 55", 2 },
                    { new Guid("6b5a6b7b-3e7d-40d2-9531-42e70d2ad3c4"), 0f, new DateTime(2025, 1, 16, 18, 30, 8, 576, DateTimeKind.Utc).AddTicks(3382), 293750f, new DateTime(2025, 1, 10, 18, 30, 8, 576, DateTimeKind.Utc).AddTicks(3379), "ул. Нахимова, д. 8", 0, null, null, new DateTime(2025, 1, 10, 21, 30, 8, 576, DateTimeKind.Utc).AddTicks(3381), new DateTime(2025, 1, 10, 18, 30, 8, 576, DateTimeKind.Utc).AddTicks(3382), new Guid("fb17c769-149d-4e2c-b386-73aa6abde8ad"), 5, 1, "ул. Советская, д. 76", 1 },
                    { new Guid("a328fa87-c15f-4de3-8198-2c9f108bd35e"), 0f, null, 293750f, new DateTime(2025, 1, 17, 17, 50, 8, 576, DateTimeKind.Utc).AddTicks(3354), "ул. Комсомольская, д. 33", 0, null, null, new DateTime(2025, 1, 19, 21, 30, 8, 576, DateTimeKind.Utc).AddTicks(3360), new DateTime(2025, 1, 19, 18, 30, 8, 576, DateTimeKind.Utc).AddTicks(3362), new Guid("fb17c769-149d-4e2c-b386-73aa6abde8ad"), 2, 1, "ул. Ленина, д. 55", 2 },
                    { new Guid("ac165515-d588-4b54-a6c8-bd95eda93539"), 0f, null, 196000f, new DateTime(2025, 1, 10, 18, 30, 8, 576, DateTimeKind.Utc).AddTicks(3391), "ул. Комсомольская, д. 33", 0, null, null, new DateTime(2025, 1, 20, 19, 10, 8, 576, DateTimeKind.Utc).AddTicks(3392), new DateTime(2025, 1, 20, 18, 30, 8, 576, DateTimeKind.Utc).AddTicks(3393), new Guid("fb17c769-149d-4e2c-b386-73aa6abde8ad"), 0, 4, "ул. Ленина, д. 55", 2 },
                    { new Guid("d3316acf-7462-4fa3-9e9c-e5922eed5985"), 0f, null, 293750f, new DateTime(2025, 1, 10, 18, 30, 8, 576, DateTimeKind.Utc).AddTicks(3385), "ул. Комсомольская, д. 33", 0, null, null, new DateTime(2025, 1, 20, 18, 30, 8, 576, DateTimeKind.Utc).AddTicks(3387), null, new Guid("fb17c769-149d-4e2c-b386-73aa6abde8ad"), 0, 4, "ул. Ленина, д. 55", 2 }
                });

            migrationBuilder.InsertData(
                table: "Trucks",
                columns: new[] { "id", "heightInMeters", "lengthInMeters", "loadCapacityInTons", "model", "number", "regionCode", "transporterId", "truckBrand", "truckType", "widthInMeters", "yearOfProduction" },
                values: new object[] { new Guid("aba1a0d7-61b9-46c6-aa74-2cb98059f948"), 3f, 10f, 20, "5Sjp", "A000AA", 70, new Guid("015833d6-dbbf-405e-9479-8fe3ae959a28"), 0, 1, 2.5f, 1999 });

            migrationBuilder.InsertData(
                table: "RejectedRequests",
                columns: new[] { "requestId", "transporterId" },
                values: new object[] { new Guid("d3316acf-7462-4fa3-9e9c-e5922eed5985"), new Guid("015833d6-dbbf-405e-9479-8fe3ae959a28") });

            migrationBuilder.InsertData(
                table: "Shipments",
                columns: new[] { "id", "heightInMeters", "lengthInMeters", "requestId", "type", "weightInTons", "widthInMeters" },
                values: new object[,]
                {
                    { new Guid("49cb99f5-0e23-4c08-82f2-38ef97bd5970"), 2f, 5f, new Guid("398d9ab7-8b9d-4319-8378-6dd30ad9894a"), 6, 5f, 2f },
                    { new Guid("5bcb239a-d497-4d98-8691-098813830371"), 2f, 5f, new Guid("d3316acf-7462-4fa3-9e9c-e5922eed5985"), 1, 5f, 2f },
                    { new Guid("7495fe7c-578f-44dc-a35f-13a8d092b171"), 2f, 5f, new Guid("194c7df6-1bfc-4edb-9921-7c5048946aef"), 6, 5f, 2f },
                    { new Guid("78cddb33-52fc-4ec7-b584-cb1cec902f5e"), 2f, 5f, new Guid("6b5a6b7b-3e7d-40d2-9531-42e70d2ad3c4"), 6, 5f, 2f },
                    { new Guid("aeeba33c-ebbf-4691-bda7-6f60434b0b06"), 2f, 5f, new Guid("a328fa87-c15f-4de3-8198-2c9f108bd35e"), 1, 5f, 2f },
                    { new Guid("b2daa6bb-70e2-4582-94bd-b53074579fb1"), 2f, 5f, new Guid("ac165515-d588-4b54-a6c8-bd95eda93539"), 6, 5f, 2f }
                });

            migrationBuilder.InsertData(
                table: "Transportations",
                columns: new[] { "id", "requestId", "status", "transporterId" },
                values: new object[,]
                {
                    { new Guid("0e1a5053-4a1c-4794-8f64-d00acf04a016"), new Guid("a328fa87-c15f-4de3-8198-2c9f108bd35e"), 2, new Guid("015833d6-dbbf-405e-9479-8fe3ae959a28") },
                    { new Guid("f3f4c557-36a5-4be5-92fa-09ad9031bd84"), new Guid("6b5a6b7b-3e7d-40d2-9531-42e70d2ad3c4"), 6, new Guid("015833d6-dbbf-405e-9479-8fe3ae959a28") }
                });

            migrationBuilder.InsertData(
                table: "TransportationStatusChanges",
                columns: new[] { "id", "status", "time", "transportationId" },
                values: new object[,]
                {
                    { new Guid("16849ec1-2fef-4e80-8898-6b37930e8b10"), 4, new DateTime(2025, 1, 15, 18, 30, 8, 576, DateTimeKind.Utc).AddTicks(3567), new Guid("f3f4c557-36a5-4be5-92fa-09ad9031bd84") },
                    { new Guid("1de5b4ee-9867-439d-be52-00eec2a2176c"), 2, new DateTime(2025, 1, 13, 18, 30, 8, 576, DateTimeKind.Utc).AddTicks(3537), new Guid("f3f4c557-36a5-4be5-92fa-09ad9031bd84") },
                    { new Guid("4514a21a-d41e-4cd4-90c5-00657196482c"), 5, new DateTime(2025, 1, 16, 18, 30, 8, 576, DateTimeKind.Utc).AddTicks(3568), new Guid("f3f4c557-36a5-4be5-92fa-09ad9031bd84") },
                    { new Guid("564d6165-3c7b-4bae-b02d-ad0bae2dce7a"), 3, new DateTime(2025, 1, 14, 18, 30, 8, 576, DateTimeKind.Utc).AddTicks(3565), new Guid("f3f4c557-36a5-4be5-92fa-09ad9031bd84") },
                    { new Guid("5a01db1c-5f7a-44d5-afe3-c96e5c052fe2"), 1, new DateTime(2025, 1, 12, 18, 30, 8, 576, DateTimeKind.Utc).AddTicks(3535), new Guid("f3f4c557-36a5-4be5-92fa-09ad9031bd84") },
                    { new Guid("72533809-3865-4330-8e45-7c3d9a8b871e"), 6, new DateTime(2025, 1, 17, 17, 50, 8, 576, DateTimeKind.Utc).AddTicks(3570), new Guid("f3f4c557-36a5-4be5-92fa-09ad9031bd84") },
                    { new Guid("956307dd-3cac-4ec3-ac54-38d8ed66e0cc"), 2, new DateTime(2025, 1, 17, 17, 50, 8, 576, DateTimeKind.Utc).AddTicks(3530), new Guid("0e1a5053-4a1c-4794-8f64-d00acf04a016") },
                    { new Guid("9cb06eaa-2b7c-4fcd-a897-b35fb0ef1032"), 0, new DateTime(2025, 1, 16, 18, 30, 8, 576, DateTimeKind.Utc).AddTicks(3525), new Guid("0e1a5053-4a1c-4794-8f64-d00acf04a016") },
                    { new Guid("ddd753b5-1a38-4e99-8818-2b005fbd011a"), 1, new DateTime(2025, 1, 17, 14, 30, 8, 576, DateTimeKind.Utc).AddTicks(3528), new Guid("0e1a5053-4a1c-4794-8f64-d00acf04a016") },
                    { new Guid("e83b88c8-1a4f-41b5-ba7f-27dff3e8de7e"), 0, new DateTime(2025, 1, 11, 18, 30, 8, 576, DateTimeKind.Utc).AddTicks(3533), new Guid("f3f4c557-36a5-4be5-92fa-09ad9031bd84") }
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
