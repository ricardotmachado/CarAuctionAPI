namespace CarAuctionAPI.Models;

public abstract class Vehicle
{
    public Guid Id { get; set; }
    public string? VehicleType { get; set; } 
    public string? Manufacturer { get; set; }
    public string? Model { get; set; }
    public int Year { get; set; }
    public decimal StartingBid { get; set; }
}