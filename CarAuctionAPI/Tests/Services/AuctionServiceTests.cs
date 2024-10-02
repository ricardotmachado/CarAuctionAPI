using CarAuctionAPI.Entities;
using CarAuctionAPI.Repositories;
using CarAuctionAPI.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace CarAuctionAPI.Tests.Services
{
    public class AuctionServiceTests
    {
        private readonly Mock<IAuctionRepository> _auctionRepositoryMock;
        private readonly Mock<IVehicleRepository> _vehicleRepositoryMock;
        private readonly AuctionService _auctionService;

        public AuctionServiceTests()
        {
            _auctionRepositoryMock = new Mock<IAuctionRepository>();
            _vehicleRepositoryMock = new Mock<IVehicleRepository>();
            _auctionService = new AuctionService(_auctionRepositoryMock.Object, _vehicleRepositoryMock.Object);
        }

        [Fact]
        public async Task StartAuction_ShouldThrowException_WhenVehicleNotFound()
        {
            // Arrange
            var vehicleId = Guid.NewGuid();
            
            _vehicleRepositoryMock.Setup(
                    repo => repo.GetVehicleByIdAsync(vehicleId))
                .ReturnsAsync((Vehicle)null);

            // Act
            Func<Task> act = async () => await _auctionService.StartAuctionAsync(vehicleId);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Vehicle not found.");
        }

        [Fact]
        public async Task StartAuction_ShouldThrowException_WhenAuctionAlreadyActive()
        {
            // Arrange
            var vehicleId = Guid.NewGuid();
            var vehicle = new Vehicle { Id = vehicleId, StartingBid = 1000 };
            var auction = new Auction { VehicleId = vehicleId, IsActive = true };
            var auctions = new List<Auction> { auction };

            _vehicleRepositoryMock.Setup(
                    repo => repo.GetVehicleByIdAsync(vehicleId))
                .ReturnsAsync(vehicle);
            _auctionRepositoryMock.Setup(
                    repo => repo.GetAuctionsByVehicleIdAsync(vehicleId))
                .ReturnsAsync(auctions);

            // Act
            Func<Task> act = async () => await _auctionService.StartAuctionAsync(vehicleId);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Auction is already active for this vehicle.");
        }

        [Fact]
        public async Task StartAuction_ShouldCreateAuction_WhenValid()
        {
            // Arrange
            var vehicleId = Guid.NewGuid();
            var vehicle = new Vehicle { Id = vehicleId, StartingBid = 1000 };

            _vehicleRepositoryMock.Setup(
                    repo => repo.GetVehicleByIdAsync(vehicleId))
                .ReturnsAsync(vehicle);
            _auctionRepositoryMock.Setup(
                    repo => repo.GetAuctionsByVehicleIdAsync(vehicleId))
                .ReturnsAsync((IEnumerable<Auction>)null);

            // Act
            var result = await _auctionService.StartAuctionAsync(vehicleId);

            // Assert
            result.Should().NotBeNull();
            result.VehicleId.Should().Be(vehicleId);
            result.IsActive.Should().BeTrue();
            result.CurrentBid.Should().Be(1000);
            _auctionRepositoryMock.Verify(repo => repo.AddAuctionAsync(It.IsAny<Auction>()), Times.Once);
        }
        
        [Fact]
        public async Task PlaceBid_ShouldThrowException_WhenAuctionNotFound()
        {
            // Arrange
            var auctionId = Guid.NewGuid();
            
            _auctionRepositoryMock.Setup(
                    repo => repo.GetAuctionByIdAsync(auctionId))
                .ReturnsAsync((Auction)null);

            // Act
            Func<Task> act = async () => await _auctionService.PlaceBidAsync(auctionId, 2000);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("No active auction found.");
        }

        [Fact]
        public async Task PlaceBid_ShouldThrowException_WhenBidIsLowerThanCurrentBid()
        {
            // Arrange
            var auctionId = Guid.NewGuid();
            var auction = new Auction { Id = auctionId, IsActive = true, CurrentBid = 1500 };

            _auctionRepositoryMock.Setup(
                    repo => repo.GetAuctionByIdAsync(auctionId))
                .ReturnsAsync(auction);

            // Act
            Func<Task> act = async () => await _auctionService.PlaceBidAsync(auctionId, 1000);

            // Assert
            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Bid amount must be greater than the current bid.");
        }

        [Fact]
        public async Task PlaceBid_ShouldUpdateAuction_WhenValidBid()
        {
            // Arrange
            var auctionId = Guid.NewGuid();
            var auction = new Auction { Id = auctionId, IsActive = true, CurrentBid = 1500 };

            _auctionRepositoryMock.Setup(
                    repo => repo.GetAuctionByIdAsync(auctionId))
                .ReturnsAsync(auction);

            // Act
            await _auctionService.PlaceBidAsync(auctionId, 2000);

            // Assert
            auction.CurrentBid.Should().Be(2000);
            _auctionRepositoryMock
                .Verify(repo => repo.UpdateAuctionAsync(It.IsAny<Auction>()), Times.Once);
        }
        
        [Fact]
        public async Task CloseAuction_ShouldCloseActiveAuction()
        {
            // Arrange
            var auctionId = Guid.NewGuid();
            var auction = new Auction
            {
                Id = auctionId,
                IsActive = true,
                StartDate = DateTime.UtcNow.AddHours(-1), // Auction started an hour ago
                EndDate = null
            };

            _auctionRepositoryMock.Setup(repo => repo.GetAuctionByIdAsync(auctionId))
                .ReturnsAsync(auction);

            // Act
            await _auctionService.CloseAuctionAsync(auctionId);

            // Assert
            Assert.False(auction.IsActive);
            Assert.NotNull(auction.EndDate);
            
            _auctionRepositoryMock
                .Verify(repo => repo.UpdateAuctionAsync(It.IsAny<Auction>()), Times.Once);
        }

        [Fact]
        public async Task CloseAuction_ShouldThrowException_WhenAuctionNotFound()
        {
            // Arrange
            var auctionId = Guid.NewGuid();

            _auctionRepositoryMock.Setup(
                    repo => repo.GetAuctionByIdAsync(auctionId))
                .ReturnsAsync((Auction)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _auctionService.CloseAuctionAsync(auctionId));
            Assert.Equal("Auction is not active.", exception.Message);
        }

        [Fact]
        public async Task CloseAuction_ShouldThrowException_WhenAuctionIsNotActive()
        {
            // Arrange
            var auctionId = Guid.NewGuid();
            var auction = new Auction
            {
                Id = auctionId,
                IsActive = false,
                EndDate = DateTime.UtcNow.AddHours(-1)
            };

            _auctionRepositoryMock.Setup(repo => repo.GetAuctionByIdAsync(auctionId))
                .ReturnsAsync(auction);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _auctionService.CloseAuctionAsync(auctionId));
            Assert.Equal("Auction is not active.", exception.Message);
        }
        
        [Fact]
        public async Task GetAuctionsByVehicleIdAsync_ShouldReturnAuctions()
        {
            // Arrange
            var vehicleId = Guid.NewGuid();
            var auctions = new List<Auction>
            {
                new Auction
                {
                    Id = Guid.NewGuid(),
                    VehicleId = vehicleId,
                    IsActive = true,
                    CurrentBid = 10000,
                    StartDate = DateTime.UtcNow
                },
                new Auction
                {
                    Id = Guid.NewGuid(),
                    VehicleId = vehicleId,
                    IsActive = false,
                    CurrentBid = 15000,
                    StartDate = DateTime.UtcNow
                }
            };

            _auctionRepositoryMock.Setup(repo => repo.GetAuctionsByVehicleIdAsync(vehicleId))
                .ReturnsAsync(auctions);

            // Act
            var result = await _auctionService.GetAuctionsByVehicleIdAsync(vehicleId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, a => Assert.Equal(vehicleId, a.VehicleId));
        }

        [Fact]
        public async Task GetAuctionsByVehicleIdAsync_ShouldReturnEmptyList_WhenNoAuctionsFound()
        {
            // Arrange
            var vehicleId = Guid.NewGuid();
            _auctionRepositoryMock.Setup(
                    repo => repo.GetAuctionsByVehicleIdAsync(vehicleId))
                .ReturnsAsync(new List<Auction>());

            // Act
            var result = await _auctionService.GetAuctionsByVehicleIdAsync(vehicleId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
        
        [Fact]
        public async Task GetAuctionByIdAsync_ShouldReturnAuction()
        {
            // Arrange
            var auctionId = Guid.NewGuid();
            var auction = new Auction
            {
                Id = auctionId,
                VehicleId = Guid.NewGuid(),
                IsActive = true,
                CurrentBid = 20000,
                StartDate = DateTime.UtcNow
            };

            _auctionRepositoryMock.Setup(
                    repo => repo.GetAuctionByIdAsync(auctionId))
                .ReturnsAsync(auction);

            // Act
            var result = await _auctionService.GetAuctionByIdAsync(auctionId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(auctionId, result.Id);
            Assert.Equal(20000, result.CurrentBid);
        }

        [Fact]
        public async Task GetAuctionByIdAsync_ShouldReturnNull_WhenAuctionNotFound()
        {
            // Arrange
            var auctionId = Guid.NewGuid();
            _auctionRepositoryMock.Setup(
                    repo => repo.GetAuctionByIdAsync(auctionId))
                .ReturnsAsync((Auction)null);

            // Act
            var result = await _auctionService.GetAuctionByIdAsync(auctionId);

            // Assert
            Assert.Null(result);
        }
    }
}
