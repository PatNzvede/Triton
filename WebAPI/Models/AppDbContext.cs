using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<WayBill> WayBills { get; set; }
        public DbSet<WayBillItem> WayBillItems { get; set; }

        public DbSet<DepotVehicleDetail> DepotVehicleDetails { get; set; }
        public DbSet<WayBillCapture> WayBillCaptures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<WayBill>()
                .Property(e => e.LoadingFrom)
                .HasConversion(
                    v => v.ToString(),
                    v => (Depot)Enum.Parse(typeof(Depot), v));
            modelBuilder
                .Entity<Vehicle>()
                .Property(e => e.Depot)
                .HasConversion(
                    v => v.ToString(),
                    v => (Depot)Enum.Parse(typeof(Depot), v));
            modelBuilder
                .Entity<WayBill>()
                .Property(e => e.Status)
                .HasConversion(
                    v => v.ToString(),
                    v => (Status)Enum.Parse(typeof(Status), v));
            modelBuilder
                .Entity<DepotVehicleDetail>()
                .Property(e => e.Depot)
                .HasConversion(
                    v => v.ToString(),
                    v => (Depot)Enum.Parse(typeof(Depot), v));
        }
    }
}
