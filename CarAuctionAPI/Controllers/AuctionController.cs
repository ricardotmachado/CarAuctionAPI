using CarAuctionAPI.DTOs;
using CarAuctionAPI.Entities;
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
        var vehicle = await _vehicleService.AddVehicleAsync(vehicleDto);
        return Ok(vehicle);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchVehicles(string? vehicleType, string? manufacturer, string? model, int year)
    {
        var vehicles = await _vehicleService.SearchVehiclesAsync(vehicleType, manufacturer, model, year);
        return Ok(vehicles);
    }

    [HttpPost("start-auction/{vehicleId}")]
    public async Task<IActionResult> StartAuction(Guid vehicleId)
    {
        var auction = await _auctionService.StartAuction(vehicleId);
        return Ok(auction);
    }

    [HttpPost("place-bid/{auctionId}")]
    public async Task<IActionResult> PlaceBid(Guid auctionId, decimal bidAmount)
    {
        await _auctionService.PlaceBid(auctionId, bidAmount);
        return Ok();
    }

    [HttpPost("close-auction/{auctionId}")]
    public async Task<IActionResult> CloseAuction(Guid auctionId)
    {
        await _auctionService.CloseAuction(auctionId);
        return Ok();
    }
}