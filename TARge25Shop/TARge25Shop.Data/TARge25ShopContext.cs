using Microsoft.EntityFrameworkCore;
using TARge25Shop.Core.Domain;

namespace TARge25Shop.Data
{
    // Ühendus: connection string asub appsettings.json failis (DefaultConnection),
    // Program.cs-is registreeritakse see AddDbContext ja UseSqlServer abil.
    public class TARge25ShopContext : DbContext
    {
        public TARge25ShopContext(
            DbContextOptions<TARge25ShopContext> options)
            : base(options)
        {
        }

        public DbSet<Spaceship> Spaceships { get; set; }
        public DbSet<FileToApi> FileToApis { get; set; }
        public DbSet<RealEstate> RealEstates { get; set; }
        public DbSet<Kindergarten> Kindergartens { get; set; }

        public DbSet<Book> Books { get; set; }
        public DbSet<Reader> Readers { get; set; }
        public DbSet<Loan> Loans { get; set; }
    }
}