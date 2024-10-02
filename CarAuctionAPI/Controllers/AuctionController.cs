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

    [HttpPost("AddVehicle")]
    public async Task<IActionResult> AddVehicleAsync([FromBody] VehicleDTO vehicleDto)
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

    [HttpGet("SearchVehicles")]
    public async Task<IActionResult> SearchVehiclesAsync(string? vehicleType, string? manufacturer, string? model, int year)
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

    [HttpPost("StartAuction/{vehicleId}")]
    public async Task<IActionResult> StartAuctionAsync(Guid vehicleId)
    {
        try
        {
            var auction = await _auctionService.StartAuctionAsync(vehicleId);
            return Ok(auction);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("PlaceBid/{auctionId}")]
    public async Task<IActionResult> PlaceBidAsync(Guid auctionId, decimal bidAmount)
    {
        try
        {
            await _auctionService.PlaceBidAsync(auctionId, bidAmount);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("CloseAuction/{auctionId}")]
    public async Task<IActionResult> CloseAuctionAsync(Guid auctionId)
    {
        try
        {
            await _auctionService.CloseAuctionAsync(auctionId);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpGet("SearchAuctionById")]
    public async Task<IActionResult> SearchAuctionByIdAsync(Guid auctionId)
    {
        try
        {
            var auction = await _auctionService.GetAuctionByIdAsync(auctionId);
            return Ok(auction);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpGet("SearchAuctionsByVehicleId")]
    public async Task<IActionResult> SearchAuctionsByVehicleIdAsync(Guid vehicleId)
    {
        try
        {
            var auctions = await _auctionService.GetAuctionsByVehicleIdAsync(vehicleId);
            return Ok(auctions);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}