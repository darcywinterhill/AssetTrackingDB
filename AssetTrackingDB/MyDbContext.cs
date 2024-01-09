using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace AssetTrackingDB
{
    internal class MyDbContext : DbContext
    {
        string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Assets;Integrated Security=True";

        public DbSet<Asset> Assets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // We tell the app to use the connectionstring.
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder ModelBuilder)
        {
            ModelBuilder.Entity<Asset>().HasData(new Asset { Id = 1, Type = "Laptop", Brand = "Lenovo", Model = "IdeaPad", PurchaseDate = Convert.ToDateTime("21-03-30"), Price = 6290});
            ModelBuilder.Entity<Asset>().HasData(new Asset { Id = 2, Type = "MobilePhone", Brand = "IPhone", Model = "SE", PurchaseDate = Convert.ToDateTime("21-05-15"), Price = 6590});
            ModelBuilder.Entity<Asset>().HasData(new Asset { Id = 3, Type = "Laptop", Brand = "Asus", Model = "Vivobook", PurchaseDate = Convert.ToDateTime("21-07-06"), Price = 5499});
            ModelBuilder.Entity<Asset>().HasData(new Asset { Id = 4, Type = "MobilePhone", Brand = "Samsung", Model = "Galaxy S", PurchaseDate = Convert.ToDateTime("22-01-10"), Price = 8990});
            ModelBuilder.Entity<Asset>().HasData(new Asset { Id = 5, Type = "Laptop", Brand = "MacBook", Model = "Air", PurchaseDate = Convert.ToDateTime("23-12-22"), Price = 18975 });
            ModelBuilder.Entity<Asset>().HasData(new Asset { Id = 6, Type = "MobilePhone", Brand = "IPhone", Model = "15", PurchaseDate = Convert.ToDateTime("24-01-07"), Price = 11990});
        }
    }
}
