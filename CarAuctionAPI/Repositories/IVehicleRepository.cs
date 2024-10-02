using CarAuctionAPI.Entities;

namespace CarAuctionAPI.Repositories;

public interface IVehicleRepository
{
    Task<Vehicle> GetVehicleByIdAsync(Guid id);
    Task<IEnumerable<Vehicle>> GetAllVehiclesAsync();
    Task AddVehicleAsync(Vehicle vehicle);
    Task UpdateVehicleAsync(Vehicle vehicle);
    Task DeleteVehicleAsync(Guid id);
}