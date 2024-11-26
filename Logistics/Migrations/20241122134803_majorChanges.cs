using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Logistics.Migrations
{
    /// <inheritdoc />
    public partial class majorChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "directorContact",
                table: "Shippers");

            migrationBuilder.DropColumn(
                name: "organizationName",
                table: "Shippers");

            migrationBuilder.DropColumn(
                name: "personInChargeContact",
                table: "Shippers");

            migrationBuilder.AlterColumn<string>(
                name: "INN",
                table: "Users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "companyName",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "organizationalForm",
                table: "Users",
                type: "integer",
                nullable: true);

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
                    costInRubles = table.Column<float>(type: "real", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_Licenses_transporterid",
                table: "Licenses",
                column: "transporterid");

            migrationBuilder.CreateIndex(
                name: "IX_Passports_userid",
                table: "Passports",
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Licenses");

            migrationBuilder.DropTable(
                name: "Passports");

            migrationBuilder.DropTable(
                name: "Requests");

            migrationBuilder.DropTable(
                name: "Shipments");

            migrationBuilder.DropTable(
                name: "Transportations");

            migrationBuilder.DropColumn(
                name: "companyName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "organizationalForm",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "INN",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "directorContact",
                table: "Shippers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "organizationName",
                table: "Shippers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "personInChargeContact",
                table: "Shippers",
                type: "text",
                nullable: true);
        }
    }
}
