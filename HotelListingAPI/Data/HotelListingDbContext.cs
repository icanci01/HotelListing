using Microsoft.EntityFrameworkCore;

namespace HotelListingAPI.Data;

public class HotelListingDbContext : DbContext
{
    public HotelListingDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Country> Countries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Create initial Countries
        modelBuilder.Entity<Country>().HasData(
            new Country
            {
                Id = 1,
                Name = "United States",
                ShortName = "US"
            },
            new Country
            {
                Id = 2,
                Name = "Canada",
                ShortName = "CA"
            },
            new Country
            {
                Id = 3,
                Name = "Bahamas",
                ShortName = "BS"
            }
        );

        // Create initial Hotels
        modelBuilder.Entity<Hotel>().HasData(
            new Hotel
            {
                Id = 1,
                Name = "Sandals Resort and Spa",
                Address = "Negril",
                CountryId = 1,
                Rating = 4.5
            },
            new Hotel
            {
                Id = 2,
                Name = "Hotel Grand",
                Address = "Grand Palldium",
                CountryId = 2,
                Rating = 4
            },
            new Hotel
            {
                Id = 3,
                Name = "Comfort Suites",
                Address = "George Town",
                CountryId = 3,
                Rating = 4.7
            }
        );
    }
}