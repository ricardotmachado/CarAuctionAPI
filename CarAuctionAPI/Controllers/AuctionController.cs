using CarAuctionAPI.DTOs;
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
        try
        {
            var vehicle = await _vehicleService.AddVehicleAsync(vehicleDto);
            return Ok(vehicle);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchVehicles(string? vehicleType, string? manufacturer, string? model, int year)
    {
        try
        {
            var vehicles = await _vehicleService.SearchVehiclesAsync(vehicleType, manufacturer, model, year);
            return Ok(vehicles);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("start-auction/{vehicleId}")]
    public async Task<IActionResult> StartAuction(Guid vehicleId)
    {
        try
        {
            var auction = await _auctionService.StartAuction(vehicleId);
            return Ok(auction);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("place-bid/{auctionId}")]
    public async Task<IActionResult> PlaceBid(Guid auctionId, decimal bidAmount)
    {
        try
        {
            await _auctionService.PlaceBid(auctionId, bidAmount);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("close-auction/{auctionId}")]
    public async Task<IActionResult> CloseAuction(Guid auctionId)
    {
        try
        {
            await _auctionService.CloseAuction(auctionId);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}