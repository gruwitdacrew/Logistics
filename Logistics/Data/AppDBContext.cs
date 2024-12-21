using Microsoft.EntityFrameworkCore;
using Logistics.Data.Account.Models;
using Logistics.Data.Requests.Models;
using Logistics.Data.Documents.Models;
using Logistics.Data.Transportations.Models;
using Logistics.Data.Account.AccountDTOs.Requests;
using Logistics.Data.Common.DTOs.Responses;
using Logistics.Data.Accounts.DTOs.Requests;
using Logistics.Data.Documents.DTOs.Requests;
using Logistics.Data.Requests.DTOs.Requests;
using Logistics.Services.Utils;
using Microsoft.AspNetCore.Identity.Data;
using System.Text;
using System.Security.Cryptography;

namespace Logistics.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options){}

        public DbSet<User> Users { get; set; }
        public DbSet<PendingEmail> PendingEmails { get; set; }

        public DbSet<Truck> Trucks { get; set; }
        public DbSet<Transporter> Transporters { get; set; }
        public DbSet<Shipper> Shippers { get; set; }

        public DbSet<Request> Requests{ get; set; }
        public DbSet<RejectedRequest> RejectedRequests { get; set; }
        public DbSet<Transportation> Transportations { get; set; }
        public DbSet<Shipment> Shipments { get; set; }

        public DbSet<Passport> Passports { get; set; }
        public DbSet<DriverLicense> Licenses { get; set; }

        public DbSet<TransportationStatusChange> TransportationStatusChanges { get; set; }

        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasKey(u => u.id);
            modelBuilder.Entity<User>().UseTptMappingStrategy();

            modelBuilder.Entity<Passport>().HasOne(x => x.user);

            modelBuilder.Entity<PendingEmail>().HasOne(u => u.user);
            modelBuilder.Entity<PendingEmail>().HasKey(u => u.id);

            modelBuilder.Entity<DriverLicense>().HasOne(x => x.transporter);

            modelBuilder.Entity<Request>().HasOne(x => x.shipper);
            modelBuilder.Entity<Request>().HasOne(x => x.shipment).WithOne().HasForeignKey<Shipment>(x => x.requestId);
            modelBuilder.Entity<Request>().HasOne(x => x.transportation).WithOne(x => x.request).HasForeignKey<Transportation>(x => x.requestId);

            modelBuilder.Entity<RejectedRequest>().HasOne<Request>().WithMany().HasForeignKey(x => x.requestId);
            modelBuilder.Entity<RejectedRequest>().HasOne<Transporter>().WithMany().HasForeignKey(x => x.transporterId);
            modelBuilder.Entity<RejectedRequest>().HasKey(x => new { x.transporterId, x.requestId });

            modelBuilder.Entity<TransportationStatusChange>().HasOne(x => x.transportation);

            modelBuilder.Entity<Transporter>().HasOne(x => x.truck).WithOne().HasForeignKey<Truck>(x => x.transporterId);

            modelBuilder.Entity<Review>().HasOne<User>().WithMany().HasForeignKey(x => x.userId);
            modelBuilder.Entity<Review>().HasOne<Transportation>().WithMany().HasForeignKey(x => x.transportationId);
            modelBuilder.Entity<Review>().HasKey(x => new { x.transportationId, x.reviewerId, x.userId });

            ////////////////////////////////////////////////////////////инициализация базы с данными

            Transporter transporter = new Transporter
            {
                id = Guid.NewGuid(),
                email = "transporter@gmail.com",
                fullName = "Петров Анатолий Степанович",
                phone = "+7 932 812 96 69",
                password = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes("volvo123"))),
                permanentResidence = City.Tomsk,
                role = Role.Transporter
            };

            Truck truck = new Truck(
                new CreateTruckRequestDTO
                {
                    truckBrand = TruckBrand.Volvo,
                    model = "5Sjp",
                    truckType = TruckType.Flatbed,
                    loadCapacityInTons = 20,
                    yearOfProduction = 1999,
                    number = "A000AA",
                    regionCode = 70,
                    lengthInMeters = 10,
                    widthInMeters = 2.5f,
                    heightInMeters = 3
                })
                { transporterId = transporter.id };

            Shipper shipper = new Shipper
            {
                id = Guid.NewGuid(),
                email = "shipper@gmail.com",
                fullName = "Семенов Александр Никитич",
                phone = "+7 931 555 35 35",
                password = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes("admin123"))),
                role = Role.Shipper
            };

            modelBuilder.Entity<Truck>().HasData(truck);

            modelBuilder.Entity<Transporter>(b =>
                {
                    b.HasData(transporter);
                    b.OwnsOne(t => t.company).HasData(
                        new
                        {
                            Userid = transporter.id,
                            organizationalForm = OrganizationForm.Individual,
                            INN = "345055094345"
                        }
                    );
                }
            );
            modelBuilder.Entity<Shipper>(b =>
            {
                b.HasData(shipper);
                b.OwnsOne(s => s.company).HasData(
                    new
                    {
                        Userid = shipper.id,
                        organizationalForm = OrganizationForm.OOO,
                        companyName = "Herriot-Watt",
                        INN = "3450550943"
                    }
                );
            }
            );

            modelBuilder.Entity<Passport>().HasData(
                new Passport(new CreatePassportDTO { series = "5305", number = "540964", issuedBy = "УМВД РОССИИ ПО ТОМСКОЙ ОБЛАСТИ", code = "540-345", dateOfIssue = "21.08.2000" }, null) { userId = transporter.id },
                new Passport(new CreatePassportDTO { series = "9997", number = "952812", issuedBy = "УМВД РОССИИ ПО ТОМСКОЙ ОБЛАСТИ", code = "540-666", dateOfIssue = "30.10.1991" }, null) { userId = shipper.id }
            );

            modelBuilder.Entity<DriverLicense>().HasData(
                new DriverLicense(new CreateDriverLicenseRequestDTO { series = "5305", number = "540964" }, null) { transporterId = transporter.id }
            );

            Request acceptedRequest = new Request
            {
                id = Guid.NewGuid(),
                shipperId = shipper.id,
                creationTime = DateTime.UtcNow.AddMinutes(-40),
                status = RequestStatus.Accepted,
                loadCity = City.Tomsk,
                loadAddress = "ул. Комсомольская, д. 33",
                unloadCity = City.Kemerovo,
                unloadAddress = "ул. Ленина, д. 55",
                sendingTime = DateTime.UtcNow.AddDays(2).AddHours(3),
                sendingTimeFrom = DateTime.UtcNow.AddDays(2),
                truckType = TruckType.Flatbed,
                costInRubles = CostCalculator.calculateCostInRubles(300),
                additionalCostInRubles = 0
            };

            Request finishedRequest = new Request
            {
                id = Guid.NewGuid(),
                shipperId = shipper.id,
                creationTime = DateTime.UtcNow.AddDays(-7),
                status = RequestStatus.ArchivedTransportationFinished,
                loadCity = City.Tomsk,
                loadAddress = "ул. Нахимова, д. 8",
                unloadCity = City.Novosibirsk,
                unloadAddress = "ул. Советская, д. 76",
                sendingTime = DateTime.UtcNow.AddDays(-7).AddHours(3),
                sendingTimeFrom = DateTime.UtcNow.AddDays(-7),
                arrivalTime = DateTime.UtcNow.AddDays(-1),
                truckType = TruckType.Flatbed,
                costInRubles = CostCalculator.calculateCostInRubles(300),
                additionalCostInRubles = 0
            };

            Request rejectedRequest = new Request
            {
                id = Guid.NewGuid(),
                shipperId = shipper.id,
                creationTime = DateTime.UtcNow.AddDays(-7),
                status = RequestStatus.Active,
                loadCity = City.Tomsk,
                loadAddress = "ул. Комсомольская, д. 33",
                unloadCity = City.Kemerovo,
                unloadAddress = "ул. Ленина, д. 55",
                sendingTime = DateTime.UtcNow.AddDays(3),
                truckType = TruckType.Dump,
                costInRubles = CostCalculator.calculateCostInRubles(300),
                additionalCostInRubles = 0
            };

            List<Request> activeRequests = new List<Request>
            {
                new Request
                {
                    id = Guid.NewGuid(),
                    shipperId = shipper.id,
                    creationTime = DateTime.UtcNow.AddDays(-7),
                    status = RequestStatus.Active,
                    loadCity = City.Tomsk,
                    loadAddress = "ул. Комсомольская, д. 33",
                    unloadCity = City.Kemerovo,
                    unloadAddress = "ул. Ленина, д. 55",
                    sendingTime = DateTime.UtcNow.AddDays(3).AddMinutes(40),
                    sendingTimeFrom = DateTime.UtcNow.AddDays(3),
                    truckType = TruckType.Dump,
                    costInRubles = CostCalculator.calculateCostInRubles(200),
                    additionalCostInRubles = 0
                },

                new Request
                {
                    id = Guid.NewGuid(),
                    shipperId = shipper.id,
                    creationTime = DateTime.UtcNow.AddDays(-7),
                    status = RequestStatus.Active,
                    loadCity = City.Tomsk,
                    loadAddress = "ул. Комсомольская, д. 33",
                    unloadCity = City.Kemerovo,
                    unloadAddress = "ул. Ленина, д. 55",
                    sendingTime = DateTime.UtcNow.AddDays(3).AddMinutes(40),
                    truckType = TruckType.Dump,
                    costInRubles = CostCalculator.calculateCostInRubles(200),
                    additionalCostInRubles = 0
                },

                new Request
                {
                    id = Guid.NewGuid(),
                    shipperId = shipper.id,
                    creationTime = DateTime.UtcNow.AddDays(-7),
                    status = RequestStatus.Delayed,
                    loadCity = City.Tomsk,
                    loadAddress = "ул. Комсомольская, д. 33",
                    unloadCity = City.Kemerovo,
                    unloadAddress = "ул. Ленина, д. 55",
                    sendingTime = DateTime.UtcNow.AddDays(3).AddMinutes(40),
                    truckType = TruckType.Dump,
                    costInRubles = CostCalculator.calculateCostInRubles(200),
                    additionalCostInRubles = 0
                }
            };

            modelBuilder.Entity<Request>().HasData(

                acceptedRequest,

                finishedRequest,

                rejectedRequest,

                activeRequests[0],
                activeRequests[1],
                activeRequests[2]
            );

            modelBuilder.Entity<Shipment>().HasData(
                new Shipment
                { 
                    id = Guid.NewGuid(),
                    requestId = acceptedRequest.id,
                    type = ShipmentType.Bulk,
                    heightInMeters = 2,
                    widthInMeters = 2,
                    lengthInMeters = 5,
                    weightInTons = 5
                },
                new Shipment
                {
                    id = Guid.NewGuid(),
                    requestId = rejectedRequest.id,
                    type = ShipmentType.Bulk,
                    heightInMeters = 2,
                    widthInMeters = 2,
                    lengthInMeters = 5,
                    weightInTons = 5
                },
                new Shipment
                {
                    id = Guid.NewGuid(),
                    requestId = finishedRequest.id,
                    type = ShipmentType.Liquid,
                    heightInMeters = 2,
                    widthInMeters = 2,
                    lengthInMeters = 5,
                    weightInTons = 5
                },
                new Shipment
                {
                    id = Guid.NewGuid(),
                    requestId = activeRequests[0].id,
                    type = ShipmentType.Liquid,
                    heightInMeters = 2,
                    widthInMeters = 2,
                    lengthInMeters = 5,
                    weightInTons = 5
                },
                new Shipment
                {
                    id = Guid.NewGuid(),
                    requestId = activeRequests[1].id,
                    type = ShipmentType.Liquid,
                    heightInMeters = 2,
                    widthInMeters = 2,
                    lengthInMeters = 5,
                    weightInTons = 5
                },
                new Shipment
                {
                    id = Guid.NewGuid(),
                    requestId = activeRequests[2].id,
                    type = ShipmentType.Liquid,
                    heightInMeters = 2,
                    widthInMeters = 2,
                    lengthInMeters = 5,
                    weightInTons = 5
                }
            );

            modelBuilder.Entity<RejectedRequest>().HasData(
                new RejectedRequest(transporter.id, rejectedRequest.id)
            );

            List<Transportation> transportations = new List<Transportation>
            {
                new Transportation
                {
                    id = Guid.NewGuid(),
                    requestId = acceptedRequest.id,
                    transporterId = transporter.id,
                    status = TransportationStatus.Loading
                },
                new Transportation
                {
                    id = Guid.NewGuid(),
                    requestId = finishedRequest.id,
                    transporterId = transporter.id,
                    status = TransportationStatus.Finished
                }
            };

            modelBuilder.Entity<Transportation>().HasData(
                transportations[0],
                transportations[1]

            );

            modelBuilder.Entity<TransportationStatusChange>().HasData(
                new TransportationStatusChange
                { 
                    id = Guid.NewGuid(),
                    transportationId = transportations[0].id,
                    status = TransportationStatus.WaitingForStart,
                    time = DateTime.UtcNow.AddDays(-1)
                },
                new TransportationStatusChange
                {
                    id = Guid.NewGuid(),
                    transportationId = transportations[0].id,
                    status = TransportationStatus.OnWayToLoading,
                    time = DateTime.UtcNow.AddHours(-4)
                },
                new TransportationStatusChange
                {
                    id = Guid.NewGuid(),
                    transportationId = transportations[0].id,
                    status = TransportationStatus.Loading,
                    time = DateTime.UtcNow.AddMinutes(-40)
                },



                new TransportationStatusChange
                {
                    id = Guid.NewGuid(),
                    transportationId = transportations[1].id,
                    status = TransportationStatus.WaitingForStart,
                    time = DateTime.UtcNow.AddDays(-6)
                },
                new TransportationStatusChange
                {
                    id = Guid.NewGuid(),
                    transportationId = transportations[1].id,
                    status = TransportationStatus.OnWayToLoading,
                    time = DateTime.UtcNow.AddDays(-5)
                },
                new TransportationStatusChange
                {
                    id = Guid.NewGuid(),
                    transportationId = transportations[1].id,
                    status = TransportationStatus.Loading,
                    time = DateTime.UtcNow.AddDays(-4)
                },
                new TransportationStatusChange
                {
                    id = Guid.NewGuid(),
                    transportationId = transportations[1].id,
                    status = TransportationStatus.OnWayToUnloading,
                    time = DateTime.UtcNow.AddDays(-3)
                },
                new TransportationStatusChange
                {
                    id = Guid.NewGuid(),
                    transportationId = transportations[1].id,
                    status = TransportationStatus.Repairing,
                    time = DateTime.UtcNow.AddDays(-2)
                },
                new TransportationStatusChange
                {
                    id = Guid.NewGuid(),
                    transportationId = transportations[1].id,
                    status = TransportationStatus.Unloading,
                    time = DateTime.UtcNow.AddDays(-1)
                },
                new TransportationStatusChange
                {
                    id = Guid.NewGuid(),
                    transportationId = transportations[1].id,
                    status = TransportationStatus.Finished,
                    time = DateTime.UtcNow.AddMinutes(-40)
                }
            );
        }
    }
}
