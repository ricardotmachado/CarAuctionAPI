using CarAuctionAPI.Data;
using CarAuctionAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarAuctionAPI.Repositories;

public class AuctionRepository : IAuctionRepository
{
    private readonly AuctionDbContext _context;
    
    public AuctionRepository(AuctionDbContext context)
    {
        _context = context;
    }

    public async Task AddAuctionAsync(Auction auction)
    {
        await _context.Auctions.AddAsync(auction);
        await _context.SaveChangesAsync();
    }
    
    public async Task UpdateAuctionAsync(Auction auction)
    {
        _context.Auctions.Update(auction);
        await _context.SaveChangesAsync();
    }
    
    public async Task<IEnumerable<Auction>> GetAuctionsByVehicleIdAsync(Guid vehicleId)
    {
        return await _context.Auctions
            .Where(a => a.VehicleId == vehicleId)
            .ToListAsync();
    }
    
    public async Task<Auction> GetAuctionByIdAsync(Guid auctionId)
    {
        return await _context.Auctions.FindAsync(auctionId);
    }
}