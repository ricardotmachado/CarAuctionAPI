using CarAuctionAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarAuctionAPI.Data;

public class AuctionDbContext : DbContext
{
    public AuctionDbContext(DbContextOptions<AuctionDbContext> options) : base(options) { }

    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Auction> Auctions { get; set; }
}
