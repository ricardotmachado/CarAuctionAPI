namespace CarAuctionAPI.Models;
public class Auction
{
    public int Id { get; set; }
    public int VehicleId { get; set; }
    public Vehicle Vehicle { get; set; }
    public decimal CurrentBid { get; set; }
    public bool IsActive { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
}