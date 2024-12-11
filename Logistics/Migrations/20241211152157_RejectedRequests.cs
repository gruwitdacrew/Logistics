using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Logistics.Migrations
{
    /// <inheritdoc />
    public partial class RejectedRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateIndex(
                name: "IX_RejectedRequests_requestId",
                table: "RejectedRequests",
                column: "requestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RejectedRequests");
        }
    }
}
