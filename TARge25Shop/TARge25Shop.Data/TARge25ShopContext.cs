using Microsoft.EntityFrameworkCore;
using TARge25Shop.Core.Domain;

namespace TARge25Shop.Data
{
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
    }
}