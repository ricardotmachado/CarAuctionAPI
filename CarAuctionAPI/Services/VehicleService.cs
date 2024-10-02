using CarAuctionAPI.Models;
using CarAuctionAPI.Repositories;

namespace CarAuctionAPI.Services;

public class VehicleService : IVehicleService
{
    private readonly IVehicleRepository _vehicleRepository;

    public VehicleService(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    public async Task AddVehicleAsync(Vehicle vehicle)
    {
        await _vehicleRepository.AddVehicleAsync(vehicle);
    }

    public async Task<List<Vehicle>> SearchVehiclesAsync(string? vehicleType, string? manufacturer, string? model, int year)
    {
        var allVehicles = await _vehicleRepository.GetAllVehiclesAsync();

        return allVehicles
            .Where(v => 
                (string.IsNullOrEmpty(vehicleType) || v.VehicleType == vehicleType) &&
                (string.IsNullOrEmpty(manufacturer) || v.Manufacturer == manufacturer) &&
                (string.IsNullOrEmpty(model) || v.Model == model) &&
                (year == 0 || v.Year == year))
            .ToList();
    }
}