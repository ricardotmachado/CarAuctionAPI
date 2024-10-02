using CarAuctionAPI.DTOs;
using CarAuctionAPI.Entities;

namespace CarAuctionAPI.Services;

    public interface IVehicleService
    {
        Task<Vehicle> AddVehicleAsync(VehicleDTO vehicleDto);
        Task<List<Vehicle>> SearchVehiclesAsync(string? vehicleType, string? manufacturer, string? model, int year);
    }

