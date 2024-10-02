using CarAuctionAPI.Data;
using CarAuctionAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CarAuctionAPI.Repositories;

public class VehicleRepository : IVehicleRepository
{
    private readonly AuctionDbContext _context;

    public VehicleRepository(AuctionDbContext context)
    {
        _context = context;
    }

    public async Task<Vehicle> GetVehicleByIdAsync(Guid id)
    {
        return await _context.Vehicles.FindAsync(id);
    }

    public async Task<IEnumerable<Vehicle>> GetAllVehiclesAsync()
    {
        return await _context.Vehicles.ToListAsync();
    }

    public async Task AddVehicleAsync(Vehicle vehicle)
    {
        await _context.Vehicles.AddAsync(vehicle);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateVehicleAsync(Vehicle vehicle)
    {
        _context.Vehicles.Update(vehicle);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteVehicleAsync(Guid id)
    {
        var vehicle = await GetVehicleByIdAsync(id);
        if (vehicle != null)
        {
            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
        }
    }
}
