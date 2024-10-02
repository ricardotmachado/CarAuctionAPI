using CarAuctionAPI.DTOs;
using CarAuctionAPI.Entities;
using CarAuctionAPI.Repositories;

namespace CarAuctionAPI.Services;

public class VehicleService : IVehicleService
{
    private readonly IVehicleRepository _vehicleRepository;

    public VehicleService(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    public async Task<Vehicle> AddVehicleAsync(VehicleDTO vehicleDto)
    {
        var vehicle = new Vehicle()
        {
            VehicleType = vehicleDto.VehicleType,
            Manufacturer = vehicleDto.Manufacturer,
            Model = vehicleDto.Model,
            Year = vehicleDto.Year,
            StartingBid = vehicleDto.StartingBid,
            NumberOfDoors = vehicleDto.NumberOfDoors,
            NumberOfSeats = vehicleDto.NumberOfSeats,
            LoadCapacity = vehicleDto.LoadCapacity,
        };
        
        await _vehicleRepository.AddVehicleAsync(vehicle);
        
        return vehicle;
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