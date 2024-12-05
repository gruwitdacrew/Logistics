﻿// <auto-generated />
using System;
using Logistics.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Logistics.Migrations
{
    [DbContext(typeof(AppDBContext))]
    [Migration("20241205133338_changePassport")]
    partial class changePassport
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Logistics.Data.Account.Models.PendingEmail", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("userid")
                        .HasColumnType("uuid");

                    b.Property<string>("value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.HasIndex("userid");

                    b.ToTable("PendingEmails");
                });

            modelBuilder.Entity("Logistics.Data.Account.Models.Truck", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("carBrand")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<float>("heightInMeters")
                        .HasColumnType("real");

                    b.Property<float>("lengthInMeters")
                        .HasColumnType("real");

                    b.Property<int>("loadCapacityInTons")
                        .HasColumnType("integer");

                    b.Property<string>("model")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("number")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("regionCode")
                        .HasColumnType("integer");

                    b.Property<int>("truckType")
                        .HasColumnType("integer");

                    b.Property<float>("widthInMeters")
                        .HasColumnType("real");

                    b.Property<int>("yearOfProduction")
                        .HasColumnType("integer");

                    b.HasKey("id");

                    b.ToTable("Truck");
                });

            modelBuilder.Entity("Logistics.Data.Account.Models.User", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("email")
                        .HasColumnType("text");

                    b.Property<string>("fullName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("password")
                        .HasColumnType("text");

                    b.Property<string>("phone")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("role")
                        .HasColumnType("integer");

                    b.Property<string>("token")
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.ToTable("Users");

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("Logistics.Data.Documents.Models.DriverLicense", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("number")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<byte[]>("scan")
                        .HasColumnType("bytea");

                    b.Property<string>("series")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("transporterid")
                        .HasColumnType("uuid");

                    b.HasKey("id");

                    b.HasIndex("transporterid");

                    b.ToTable("Licenses");
                });

            modelBuilder.Entity("Logistics.Data.Documents.Models.Passport", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("dateOfIssue")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("issuedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("number")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<byte[]>("scan")
                        .HasColumnType("bytea");

                    b.Property<string>("series")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("userid")
                        .HasColumnType("uuid");

                    b.HasKey("id");

                    b.HasIndex("userid");

                    b.ToTable("Passports");
                });

            modelBuilder.Entity("Logistics.Data.Requests.Models.Request", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<float>("additionalCostInRubles")
                        .HasColumnType("real");

                    b.Property<float>("costInRubles")
                        .HasColumnType("real");

                    b.Property<DateTime>("creationTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("loadAddress")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("loadCity")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("receiverContacts")
                        .HasColumnType("text");

                    b.Property<string>("receiverFullName")
                        .HasColumnType("text");

                    b.Property<DateTime>("sendingTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("sendingTimeFrom")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("shipmentid")
                        .HasColumnType("uuid");

                    b.Property<Guid>("shipperid")
                        .HasColumnType("uuid");

                    b.Property<int>("status")
                        .HasColumnType("integer");

                    b.Property<Guid?>("transportationid")
                        .HasColumnType("uuid");

                    b.Property<int>("truckType")
                        .HasColumnType("integer");

                    b.Property<string>("unloadAddress")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("unloadCity")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.HasIndex("shipmentid");

                    b.HasIndex("shipperid");

                    b.HasIndex("transportationid");

                    b.ToTable("Requests");
                });

            modelBuilder.Entity("Logistics.Data.Requests.Models.Shipment", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<float>("heightInMeters")
                        .HasColumnType("real");

                    b.Property<float>("lengthInMeters")
                        .HasColumnType("real");

                    b.Property<int>("type")
                        .HasColumnType("integer");

                    b.Property<float>("weightInTons")
                        .HasColumnType("real");

                    b.Property<float>("widthInMeters")
                        .HasColumnType("real");

                    b.HasKey("id");

                    b.ToTable("Shipments");
                });

            modelBuilder.Entity("Logistics.Data.Transportations.Models.Transportation", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("status")
                        .HasColumnType("integer");

                    b.Property<Guid>("transporterid")
                        .HasColumnType("uuid");

                    b.HasKey("id");

                    b.HasIndex("transporterid");

                    b.ToTable("Transportations");
                });

            modelBuilder.Entity("Logistics.Data.Transportations.Models.TransportationStatusChange", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("status")
                        .HasColumnType("integer");

                    b.Property<DateTime>("time")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("transportationid")
                        .HasColumnType("uuid");

                    b.HasKey("id");

                    b.HasIndex("transportationid");

                    b.ToTable("TransportationStatusChanges");
                });

            modelBuilder.Entity("Logistics.Data.Account.Models.Shipper", b =>
                {
                    b.HasBaseType("Logistics.Data.Account.Models.User");

                    b.ToTable("Shippers");
                });

            modelBuilder.Entity("Logistics.Data.Account.Models.Transporter", b =>
                {
                    b.HasBaseType("Logistics.Data.Account.Models.User");

                    b.Property<string>("permanentResidence")
                        .HasColumnType("text");

                    b.Property<Guid?>("truckid")
                        .HasColumnType("uuid");

                    b.HasIndex("truckid");

                    b.ToTable("Transporters");
                });

            modelBuilder.Entity("Logistics.Data.Account.Models.PendingEmail", b =>
                {
                    b.HasOne("Logistics.Data.Account.Models.User", "user")
                        .WithMany()
                        .HasForeignKey("userid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("user");
                });

            modelBuilder.Entity("Logistics.Data.Account.Models.User", b =>
                {
                    b.OwnsOne("Logistics.Data.Common.DTOs.Responses.Company", "company", b1 =>
                        {
                            b1.Property<Guid>("Userid")
                                .HasColumnType("uuid");

                            b1.Property<string>("INN")
                                .HasColumnType("text")
                                .HasColumnName("INN");

                            b1.Property<string>("companyName")
                                .HasColumnType("text")
                                .HasColumnName("companyName");

                            b1.Property<int?>("organizationalForm")
                                .HasColumnType("integer")
                                .HasColumnName("organizationalForm");

                            b1.HasKey("Userid");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("Userid");
                        });

                    b.Navigation("company")
                        .IsRequired();
                });

            modelBuilder.Entity("Logistics.Data.Documents.Models.DriverLicense", b =>
                {
                    b.HasOne("Logistics.Data.Account.Models.Transporter", "transporter")
                        .WithMany()
                        .HasForeignKey("transporterid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("transporter");
                });

            modelBuilder.Entity("Logistics.Data.Documents.Models.Passport", b =>
                {
                    b.HasOne("Logistics.Data.Account.Models.User", "user")
                        .WithMany()
                        .HasForeignKey("userid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("user");
                });

            modelBuilder.Entity("Logistics.Data.Requests.Models.Request", b =>
                {
                    b.HasOne("Logistics.Data.Requests.Models.Shipment", "shipment")
                        .WithMany()
                        .HasForeignKey("shipmentid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Logistics.Data.Account.Models.Shipper", "shipper")
                        .WithMany()
                        .HasForeignKey("shipperid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Logistics.Data.Transportations.Models.Transportation", "transportation")
                        .WithMany()
                        .HasForeignKey("transportationid");

                    b.Navigation("shipment");

                    b.Navigation("shipper");

                    b.Navigation("transportation");
                });

            modelBuilder.Entity("Logistics.Data.Transportations.Models.Transportation", b =>
                {
                    b.HasOne("Logistics.Data.Account.Models.Transporter", "transporter")
                        .WithMany()
                        .HasForeignKey("transporterid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("transporter");
                });

            modelBuilder.Entity("Logistics.Data.Transportations.Models.TransportationStatusChange", b =>
                {
                    b.HasOne("Logistics.Data.Transportations.Models.Transportation", "transportation")
                        .WithMany()
                        .HasForeignKey("transportationid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("transportation");
                });

            modelBuilder.Entity("Logistics.Data.Account.Models.Shipper", b =>
                {
                    b.HasOne("Logistics.Data.Account.Models.User", null)
                        .WithOne()
                        .HasForeignKey("Logistics.Data.Account.Models.Shipper", "id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Logistics.Data.Account.Models.Transporter", b =>
                {
                    b.HasOne("Logistics.Data.Account.Models.User", null)
                        .WithOne()
                        .HasForeignKey("Logistics.Data.Account.Models.Transporter", "id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Logistics.Data.Account.Models.Truck", "truck")
                        .WithMany()
                        .HasForeignKey("truckid");

                    b.Navigation("truck");
                });
#pragma warning restore 612, 618
        }
    }
}
