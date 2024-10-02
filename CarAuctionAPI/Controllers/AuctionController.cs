using CarAuctionAPI.DTOs;
using CarAuctionAPI.Models;
using CarAuctionAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarAuctionAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuctionController : ControllerBase
{
    private readonly IVehicleService _vehicleService;
    private readonly IAuctionService _auctionService;

    public AuctionController(IVehicleService vehicleService, IAuctionService auctionService)
    {
        _vehicleService = vehicleService;
        _auctionService = auctionService;
    }

    [HttpPost("add-vehicle")]
    public async Task<IActionResult> AddVehicle([FromBody] VehicleDTO vehicleDto)
    {
        Vehicle vehicle = vehicleDto.VehicleType.ToLower() switch
        {
            "sedan" => new Sedan { NumberOfDoors = vehicleDto.NumberOfDoors ?? 0 },
            "suv" => new SUV { NumberOfSeats = vehicleDto.NumberOfSeats ?? 0 },
            "hatchback" => new Hatchback { NumberOfDoors = vehicleDto.NumberOfDoors ?? 0 },
            "truck" => new Truck { LoadCapacity = vehicleDto.LoadCapacity ?? 0 },
            _ => throw new System.Exception("Invalid vehicle type")
        };

        vehicle.VehicleType = vehicleDto.VehicleType;
        vehicle.Manufacturer = vehicleDto.Manufacturer;
        vehicle.Model = vehicleDto.Model;
        vehicle.Year = vehicleDto.Year;
        vehicle.StartingBid = vehicleDto.StartingBid;

        await _vehicleService.AddVehicleAsync(vehicle);
        return Ok(vehicle);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchVehicles(string? vehicleType, string? manufacturer, string? model, int year)
    {
        var vehicles = await _vehicleService.SearchVehiclesAsync(vehicleType, manufacturer, model, year);
        return Ok(vehicles);
    }

    [HttpPost("start-auction/{vehicleId}")]
    public async Task<IActionResult> StartAuction(int vehicleId)
    {
        _auctionService.StartAuction(vehicleId);
        return Ok();
    }

    [HttpPost("place-bid/{auctionId}")]
    public async Task<IActionResult> PlaceBid(int auctionId, decimal bidAmount)
    {
        _auctionService.PlaceBid(auctionId, bidAmount);
        return Ok();
    }

    [HttpPost("close-auction/{auctionId}")]
    public async Task<IActionResult> CloseAuction(int auctionId)
    {
        _auctionService.CloseAuction(auctionId);
        return Ok();
    }
}