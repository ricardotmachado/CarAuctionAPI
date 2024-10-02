using CarAuctionAPI.Entities;

namespace CarAuctionAPI.Services;

public interface IAuctionService
{
    Task<Auction> StartAuctionAsync(Guid vehicleId);
    Task PlaceBidAsync(Guid auctionId, decimal bidAmount);
    Task CloseAuctionAsync(Guid auctionId);
    Task<IEnumerable<Auction>> GetAuctionsByVehicleIdAsync(Guid vehicleId);
    Task<Auction> GetAuctionByIdAsync(Guid auctionId);
}