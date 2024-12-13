using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Logistics.Migrations
{
    /// <inheritdoc />
    public partial class shipmentChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Shipments_shipmentid",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_shipmentid",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "shipmentid",
                table: "Requests");

            migrationBuilder.AddColumn<Guid>(
                name: "requestId",
                table: "Shipments",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_requestId",
                table: "Shipments",
                column: "requestId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Shipments_Requests_requestId",
                table: "Shipments",
                column: "requestId",
                principalTable: "Requests",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shipments_Requests_requestId",
                table: "Shipments");

            migrationBuilder.DropIndex(
                name: "IX_Shipments_requestId",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "requestId",
                table: "Shipments");

            migrationBuilder.AddColumn<Guid>(
                name: "shipmentid",
                table: "Requests",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Requests_shipmentid",
                table: "Requests",
                column: "shipmentid");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Shipments_shipmentid",
                table: "Requests",
                column: "shipmentid",
                principalTable: "Shipments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
