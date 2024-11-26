using Microsoft.EntityFrameworkCore;
using Logistics.Data.Account.Models;
using Logistics.Data.Requests.Models;
using Logistics.Data.Documents.Models;
using Logistics.Data.Transportations.Models;

namespace Logistics.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options){}

        public DbSet<User> Users { get; set; }
        public DbSet<PendingEmail> PendingEmails { get; set; }

        public DbSet<Transporter> Transporters { get; set; }
        public DbSet<Shipper> Shippers { get; set; }

        public DbSet<Request> Requests{ get; set; }
        public DbSet<Transportation> Transportations { get; set; }
        public DbSet<Shipment> Shipments { get; set; }

        public DbSet<Passport> Passports { get; set; }
        public DbSet<DriverLicense> Licenses { get; set; }

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
            modelBuilder.Entity<Request>().HasOne(x => x.shipment);
            modelBuilder.Entity<Request>().HasOne(x => x.transportation);
        }
    }
}
