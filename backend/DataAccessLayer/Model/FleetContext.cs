using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace DataAccessLayer.Model
{
    public class FleetContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Vehicle> Vehicle { get; set; }
        public DbSet<FuelCard> FuelCard { get; set; }
        public DbSet<Driver> Driver { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Brand> Brand { get; set; }
        public DbSet<DriverLicenseType> DriverLicenseType { get; set; }
        public DbSet<FuelType> FuelType { get; set; }
        public DbSet<VehicleType> VehicleType { get; set; }
        public DbSet<DriverDriverLicenseType> DriverDriverLicenseType { get; set; }
        public DbSet<FuelCardFuelType> FuelCardFuelType { get; set; }

        private readonly IConfiguration _configuration;

        public FleetContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("FleetmanagementContext"));
            //optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DevelopmentContext"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vehicle>()
                .HasOne(d => d.Driver)
                .WithOne(v => v.Vehicle)
                .HasForeignKey<Driver>(d => d.VehicleID);

            modelBuilder.Entity<Vehicle>()
                .HasOne(d => d.Brand)
                .WithMany(v => v.Vehicle);

            modelBuilder.Entity<Vehicle>()
                .HasOne(d => d.VehicleType)
                .WithMany(v => v.Vehicle);

            modelBuilder.Entity<Vehicle>()
                .HasOne(d => d.FuelType)
                .WithMany(v => v.Vehicle);

            modelBuilder.Entity<FuelCard>()
                .HasOne(d => d.Driver)
                .WithOne(v => v.FuelCard)
                .HasForeignKey<Driver>(d => d.FuelCardID);

            modelBuilder.Entity<Driver>()
                .HasOne(v => v.Vehicle)
                .WithOne(d => d.Driver)
                .HasForeignKey<Vehicle>(v => v.DriverID);

            modelBuilder.Entity<Driver>()
                .HasOne(f => f.FuelCard)
                .WithOne(d => d.Driver)
                .HasForeignKey<FuelCard>(f => f.DriverID);

            modelBuilder.Entity<Driver>()
                .HasOne(f => f.Address)
                .WithOne(d => d.Driver)
                .HasForeignKey<Address>(f => f.AddressID);

            modelBuilder.Entity<DriverDriverLicenseType>()
                .HasKey(x => new { x.DriverID, x.DriverLicenseTypeID  });

            modelBuilder.Entity<DriverDriverLicenseType>()
                .HasOne(d => d.DriverLicenseType)
                .WithMany(x => x.DriverDriverLicenseType)
                .HasForeignKey(x => x.DriverLicenseTypeID);

            modelBuilder.Entity<DriverDriverLicenseType>()
               .HasOne(d => d.Driver)
               .WithMany(x => x.DriverDriverLicenseType)
               .HasForeignKey(x => x.DriverID);

            modelBuilder.Entity<FuelCardFuelType>()
                .HasKey(x => new { x.FuelCardID, x.FuelTypeID });

            modelBuilder.Entity<FuelCardFuelType>()
                .HasOne(d => d.FuelCard)
                .WithMany(x => x.FuelCardFuelType)
                .HasForeignKey(x => x.FuelCardID);

            modelBuilder.Entity<FuelCardFuelType>()
                .HasOne(d => d.FuelType)
                .WithMany(x => x.FuelCardFuelType)
                .HasForeignKey(x => x.FuelTypeID);
        }
    }
}
