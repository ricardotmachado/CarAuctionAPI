namespace CarAuctionAPI.Entities;

public class Truck : Vehicle
{
    public double LoadCapacity { get; set; }

    public Truck(Guid id, string manufacturer, string model, int year, decimal startingBid, double loadCapacity)
        : base(id, manufacturer, model, year, startingBid)
    {
        LoadCapacity = loadCapacity;
    }
}