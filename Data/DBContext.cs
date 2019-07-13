using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace smartacfe.Data
{
    public class DBContext : DbContext
    {
        public DBContext (DbContextOptions<DBContext> options)
            : base(options)
        {
        }

        public DbSet<smartacfe.Models.ACDevice> ACDevices { get; set; }
        public DbSet<smartacfe.Models.ACDeviceReading> AcDeviceReadings { get; set; }
        public DbSet<smartacfe.Models.User> Users { get; set; }
        public DbSet<smartacfe.Models.ACDeviceAlert> ACDeviceAlerts { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Models.User>()
                .HasIndex(u => u.Username)
                .IsUnique();
            
            builder.Entity<Models.ACDevice>()
                .HasIndex(u => u.SerialNumber)
                .IsUnique();
        }
    }
}