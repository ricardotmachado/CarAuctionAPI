using CarAuctionAPI.Entities;

namespace CarAuctionAPI.Repositories;

public interface IAuctionRepository
{ 
    Task AddAuctionAsync(Auction auction);
    Task UpdateAuctionAsync(Auction auction);
    Task<IEnumerable<Auction>> GetAuctionsByVehicleIdAsync(Guid vehicleId);
    Task<Auction> GetAuctionByIdAsync(Guid auctionId);
}