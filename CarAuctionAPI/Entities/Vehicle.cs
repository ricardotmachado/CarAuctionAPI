using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarAuctionAPI.Entities;

[Table("Vehicles")]
public class Vehicle
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string VehicleType { get; set; } 
    
    [Required]
    [MaxLength(50)]
    public string Manufacturer { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Model { get; set; }
    
    [Required]
    public int Year { get; set; }
    
    [Required]
    public decimal StartingBid { get; set; }
    
    public int NumberOfDoors { get; set; }
    
    public int NumberOfSeats { get; set; }
    
    public int LoadCapacity { get; set; }
}