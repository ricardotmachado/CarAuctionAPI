namespace CarAuctionAPI.Entities;

public abstract class Vehicle
{
    public Guid Id { get; set; }
    public string Manufacturer { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public decimal StartingBid { get; set; }
    
    protected Vehicle(Guid id, string manufacturer, string model, int year, decimal startingBid)
    {
        Id = id;
        Manufacturer = manufacturer ?? throw new ArgumentNullException(nameof(manufacturer));
        Model = model ?? throw new ArgumentNullException(nameof(model));
        Year = year;
        StartingBid = startingBid;
    }
}