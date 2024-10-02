using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarAuctionAPI.Entities;

[Table("Auctions")]
public class Auction
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public Guid VehicleId { get; set; }
    
    [Required]
    public bool IsActive { get; set; }
    
    [Required]
    public decimal CurrentBid { get; set; }
    
    [Required]
    public DateTime StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }
}