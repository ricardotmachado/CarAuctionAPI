using CarAuctionAPI.Data;
using CarAuctionAPI.Entities;
using CarAuctionAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace CarAuctionAPI.Tests.Repositories;

public class AuctionRepositoryTests
{
    private readonly Mock<AuctionDbContext> _mockContext;
    private readonly AuctionRepository _auctionRepository;

    public AuctionRepositoryTests()
    {
        _mockContext = new Mock<AuctionDbContext>();
        _auctionRepository = new AuctionRepository(_mockContext.Object);
    }

    [Fact]
    public async Task AddAuctionAsync_ShouldAddAuction()
    {
        // Arrange
        var auction = new Auction
        {
            Id = Guid.NewGuid(),
            VehicleId = Guid.NewGuid(),
            IsActive = true,
            CurrentBid = 10000,
            StartDate = DateTime.UtcNow
        };

        var mockSet = new Mock<DbSet<Auction>>();
        _mockContext.Setup(m => m.Auctions).Returns(mockSet.Object);

        // Act
        await _auctionRepository.AddAuctionAsync(auction);

        // Assert
        mockSet.Verify(m => m.AddAsync(auction, default), Times.Once);
        _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task GetAuctionsByVehicleIdAsync_ShouldReturnAuctions()
    {
        // Arrange
        var auctions = new List<Auction>
        {
            new Auction
            {
                Id = Guid.NewGuid(),
                VehicleId = Guid.NewGuid(),
                IsActive = true,
                CurrentBid = 10000,
                StartDate = DateTime.UtcNow
            },
            new Auction
            {
                Id = Guid.NewGuid(),
                VehicleId = Guid.NewGuid(),
                IsActive = false,
                CurrentBid = 15000,
                StartDate = DateTime.UtcNow
            }
        };

        var mockSet = new Mock<DbSet<Auction>>();
        mockSet.As<IQueryable<Auction>>().Setup(m => m.Provider).Returns(auctions.AsQueryable().Provider);
        mockSet.As<IQueryable<Auction>>().Setup(m => m.Expression).Returns(auctions.AsQueryable().Expression);
        mockSet.As<IQueryable<Auction>>().Setup(m => m.ElementType).Returns(auctions.AsQueryable().ElementType);
        mockSet.As<IQueryable<Auction>>().Setup(m => m.GetEnumerator()).Returns(auctions.GetEnumerator());

        _mockContext.Setup(m => m.Auctions).Returns(mockSet.Object);

        // Act
        var result = await _auctionRepository.GetAuctionsByVehicleIdAsync(auctions[0].VehicleId);

        // Assert
        Assert.NotEmpty(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetAuctionByIdAsync_ShouldReturnAuction()
    {
        // Arrange
        var auction = new Auction
        {
            Id = Guid.NewGuid(),
            VehicleId = Guid.NewGuid(),
            IsActive = true,
            CurrentBid = 10000,
            StartDate = DateTime.UtcNow
        };

        _mockContext.Setup(m => m.Auctions.FindAsync(auction.Id)).ReturnsAsync(auction);

        // Act
        var result = await _auctionRepository.GetAuctionByIdAsync(auction.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(auction.Id, result.Id);
    }

    [Fact]
    public async Task GetAuctionByIdAsync_ShouldReturnNull_WhenAuctionNotFound()
    {
        // Arrange
        var auctionId = Guid.NewGuid();
        _mockContext.Setup(m => m.Auctions.FindAsync(auctionId)).ReturnsAsync((Auction)null);

        // Act
        var result = await _auctionRepository.GetAuctionByIdAsync(auctionId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAuctionAsync_ShouldUpdateAuction()
    {
        // Arrange
        var auction = new Auction
        {
            Id = Guid.NewGuid(),
            VehicleId = Guid.NewGuid(),
            IsActive = true,
            CurrentBid = 10000,
            StartDate = DateTime.UtcNow
        };

        var mockSet = new Mock<DbSet<Auction>>();
        _mockContext.Setup(m => m.Auctions).Returns(mockSet.Object);

        // Act
        await _auctionRepository.UpdateAuctionAsync(auction);

        // Assert
        mockSet.Verify(m => m.Update(auction), Times.Once);
        _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
    }
}