using CarAuctionAPI.Models;

namespace CarAuctionAPI.Services;

    public interface IVehicleService
    {
        Task AddVehicleAsync(Vehicle vehicle);
        Task<List<Vehicle>> SearchVehiclesAsync(string? vehicleType, string? manufacturer, string? model, int year);
    }

