namespace CarAuctionAPI.Services;

public interface IAuctionService
{
    void StartAuction(int vehicleId);
    void PlaceBid(int auctionId, decimal bidAmount);
    void CloseAuction(int auctionId);
}