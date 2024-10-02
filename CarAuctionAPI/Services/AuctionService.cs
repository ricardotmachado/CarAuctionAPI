using CarAuctionAPI.Entities;
using CarAuctionAPI.Repositories;

namespace CarAuctionAPI.Services;

    public class AuctionService : IAuctionService
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IVehicleRepository _vehicleRepository;

        public AuctionService(IAuctionRepository auctionRepository, IVehicleRepository vehicleRepository)
        {
            _auctionRepository = auctionRepository;
            _vehicleRepository = vehicleRepository;
        }

        public async Task<Auction> StartAuction(Guid vehicleId)
        {
            var vehicle = await _vehicleRepository.GetVehicleByIdAsync(vehicleId);
            if (vehicle == null)
                throw new System.Exception("Vehicle not found.");
        
            var auctions = await _auctionRepository.GetAuctionsByVehicleIdAsync(vehicleId);

            if (auctions != null && (auctions.Any(a => a.VehicleId == vehicleId && a.IsActive)))
                throw new System.Exception("Auction is already active for this vehicle.");
        
            var auction = new Auction
            {
                VehicleId = vehicleId,
                IsActive = true,
                CurrentBid = vehicle.StartingBid,
                StartDate = DateTime.UtcNow
            };
            
            await _auctionRepository.AddAuctionAsync(auction);
            
            return auction;
        }

        public async Task PlaceBid(Guid auctionId, decimal bidAmount)
        {
            var auction = await _auctionRepository.GetAuctionByIdAsync(auctionId);
            
            if (auction == null || !auction.IsActive)
                throw new System.Exception("No active auction found.");

            if (bidAmount <= auction.CurrentBid)
                throw new System.Exception("Bid amount must be greater than the current bid.");

            auction.CurrentBid = bidAmount;
            await _auctionRepository.UpdateAuctionAsync(auction);
        }

        public async Task CloseAuction(Guid auctionId)
        {
            var auction = await _auctionRepository.GetAuctionByIdAsync(auctionId);
            if (auction == null || !auction.IsActive)
                throw new System.Exception("Auction is not active.");

            auction.IsActive = false;
            auction.EndDate = DateTime.UtcNow;
            await _auctionRepository.UpdateAuctionAsync(auction);
        }
    }
