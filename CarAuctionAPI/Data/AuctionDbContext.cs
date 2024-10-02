using CarAuctionAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarAuctionAPI.Data;

public class AuctionDbContext : DbContext
{
    public AuctionDbContext(DbContextOptions<AuctionDbContext> options) : base(options) { }

    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Auction> Auctions { get; set; }

    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     modelBuilder.Entity<Vehicle>()
    //         .HasDiscriminator<string>("VehicleType")
    //         .HasValue<Hatchback>("Hatchback")
    //         .HasValue<Sedan>("Sedan")
    //         .HasValue<SUV>("SUV")
    //         .HasValue<Truck>("Truck");
    // }
}
