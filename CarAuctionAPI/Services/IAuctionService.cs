using CarAuctionAPI.Entities;

namespace CarAuctionAPI.Services;

public interface IAuctionService
{
    Task<Auction> StartAuction(Guid vehicleId);
    Task PlaceBid(Guid auctionId, decimal bidAmount);
    Task CloseAuction(Guid auctionId);
}