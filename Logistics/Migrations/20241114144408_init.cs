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
                name: "Users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    fullName = table.Column<string>(type: "text", nullable: false),
                    phone = table.Column<string>(type: "text", nullable: false),
                    INN = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: true),
                    role = table.Column<int>(type: "integer", nullable: false),
                    password = table.Column<string>(type: "text", nullable: true),
                    token = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
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
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    organizationName = table.Column<string>(type: "text", nullable: true),
                    directorContact = table.Column<string>(type: "text", nullable: true),
                    personInChargeContact = table.Column<string>(type: "text", nullable: true)
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
                    permanentResidence = table.Column<string>(type: "text", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_PendingEmails_userid",
                table: "PendingEmails",
                column: "userid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PendingEmails");

            migrationBuilder.DropTable(
                name: "Shippers");

            migrationBuilder.DropTable(
                name: "Transporters");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
