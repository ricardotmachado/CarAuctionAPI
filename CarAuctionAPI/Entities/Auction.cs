namespace CarAuctionAPI.Entities;
public class Auction
{
    public Guid Id { get; set; }
    public Guid VehicleId { get; set; }
    public bool IsActive { get; set; }
    public decimal CurrentBid { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}