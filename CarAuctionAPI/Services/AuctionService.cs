using CarAuctionAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace CarAuctionAPI.Services;

    public class AuctionService : IAuctionService
    {
        private readonly List<Auction> _auctions = new();
        private readonly IVehicleService _vehicleService;

        public AuctionService(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        public void StartAuction(int vehicleId)
        {
            // var vehicle = _vehicleService.GetVehicleById(vehicleId);
            // if (vehicle == null)
            // {
            //     throw new System.Exception("Vehicle not found.");
            // }
            //
            // var auction = _auctions.FirstOrDefault(a => a.Vehicle.Id == vehicleId);
            // if (auction != null && auction.IsActive)
            //     throw new System.Exception("Auction is already active for this vehicle.");
            //
            // auction = new Auction { Vehicle = vehicle, IsActive = true, CurrentBid = 0 };
            // _auctions.Add(auction);
        }

        public void PlaceBid(int auctionId, decimal bidAmount)
        {
            var auction = _auctions.FirstOrDefault(a => a.Id == auctionId && a.IsActive);
            if (auction == null)
                throw new System.Exception("No active auction found.");

            if (bidAmount <= auction.CurrentBid)
                throw new System.Exception("Bid amount must be greater than the current bid.");

            auction.CurrentBid = bidAmount;
        }

        public void CloseAuction(int auctionId)
        {
            var auction = _auctions.FirstOrDefault(a => a.Id == auctionId);
            if (auction == null || !auction.IsActive)
                throw new System.Exception("Auction is not active.");

            auction.IsActive = false;
        }
    }
