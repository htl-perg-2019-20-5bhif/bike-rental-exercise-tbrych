using BikeRentalAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace BikeRentalAPI.Data
{
    public class BikeRentalDbContext : DbContext
    {
        public DbSet<Customer> Customer { get; set; }

        public DbSet<Bike> Bike { get; set; }

        public DbSet<Rental> Rental { get; set; }

        public BikeRentalDbContext(DbContextOptions<BikeRentalDbContext> options) : base(options) { }
    }
}
