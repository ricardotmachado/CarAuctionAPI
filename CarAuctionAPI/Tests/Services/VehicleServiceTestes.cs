using CarAuctionAPI.Entities;
using CarAuctionAPI.Repositories;
using CarAuctionAPI.Services;
using Moq;
using Xunit;

namespace CarAuctionAPI.Tests.Services;

public class VehicleServiceTests
{
    private readonly VehicleService _vehicleService;
    private readonly Mock<IVehicleRepository> _mockVehicleRepository;

    public VehicleServiceTests()
    {
        _mockVehicleRepository = new Mock<IVehicleRepository>();
        _vehicleService = new VehicleService(_mockVehicleRepository.Object);
    }

    [Fact]
    public async Task SearchVehiclesAsync_ShouldReturnMatchingVehicles()
    {
        // Arrange
        var vehicle1 = new Vehicle
        {
            VehicleType = "Sedan",
            Manufacturer = "Toyota",
            Model = "Camry",
            Year = 2020
        };

        var vehicle2 = new Vehicle
        {
            VehicleType = "SUV",
            Manufacturer = "Honda",
            Model = "CR-V",
            Year = 2021
        };

        var vehicles = new List<Vehicle> { vehicle1, vehicle2 };

        _mockVehicleRepository
            .Setup(repo => repo.GetAllVehiclesAsync())
            .ReturnsAsync(vehicles);

        // Act
        var result = await _vehicleService
            .SearchVehiclesAsync("Sedan", "Toyota", "Camry", 2020);

        // Assert
        Assert.Single(result);
        Assert.Equal("Sedan", result[0].VehicleType);
        Assert.Equal("Toyota", result[0].Manufacturer);
        Assert.Equal("Camry", result[0].Model);
        _mockVehicleRepository.Verify(repo => repo.GetAllVehiclesAsync(), Times.Once);
    }

    [Fact]
    public async Task SearchVehiclesAsync_ShouldReturnEmptyList_WhenNoMatchingVehicles()
    {
        // Arrange
        var vehicle1 = new Vehicle
        {
            VehicleType = "Sedan",
            Manufacturer = "Toyota",
            Model = "Camry",
            Year = 2020
        };

        var vehicles = new List<Vehicle> { vehicle1 };

        _mockVehicleRepository.Setup(repo => repo.GetAllVehiclesAsync())
            .ReturnsAsync(vehicles);

        // Act
        var result = await _vehicleService
            .SearchVehiclesAsync("SUV", "Honda", "Civic", 2021);

        // Assert
        Assert.Empty(result); // No matching vehicles should be returned
        _mockVehicleRepository.Verify(repo => repo.GetAllVehiclesAsync(), Times.Once);
    }
}
