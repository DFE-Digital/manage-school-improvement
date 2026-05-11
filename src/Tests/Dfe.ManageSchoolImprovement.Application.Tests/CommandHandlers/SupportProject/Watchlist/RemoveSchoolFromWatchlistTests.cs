using AutoFixture;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.Watchlist;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Moq;
using WatchlistEntity = Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject.Watchlist;

namespace Dfe.ManageSchoolImprovement.Application.Tests.CommandHandlers.SupportProject.Watchlist;

public class RemoveSchoolFromWatchlistTests
{
    private readonly Mock<IWatchlistRepository> _mockWatchlistRepository;
    private readonly CancellationToken _cancellationToken;
    private readonly Fixture _fixture;

    public RemoveSchoolFromWatchlistTests()
    {
        _mockWatchlistRepository = new Mock<IWatchlistRepository>();
        _cancellationToken = CancellationToken.None;
        _fixture = new Fixture();
    }

    [Fact]
    public async Task Handle_ValidCommand_RemovesWatchlistRecordAndReturnsTrue()
    {
        var watchlistIdGuid = Guid.NewGuid();
        var watchlistId = new WatchlistId(watchlistIdGuid);
        var supportProjectId = _fixture.Create<SupportProjectId>();
        var watchlistItem = new WatchlistEntity(watchlistId, supportProjectId, "user@test.gov.uk");
        var command = new RemoveSchoolFromWatchlistCommand(watchlistIdGuid);

        _mockWatchlistRepository
            .Setup(repo => repo.GetAsync(It.IsAny<object[]>()))
            .ReturnsAsync(watchlistItem);

        _mockWatchlistRepository
            .Setup(repo => repo.RemoveAsync(It.IsAny<WatchlistEntity>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((WatchlistEntity w, CancellationToken _) => w);

        var handler = new RemoveSchoolFromWatchlist.RemoveSchoolFromWatchlistCommandHandler(_mockWatchlistRepository.Object);

        var result = await handler.Handle(command, _cancellationToken);

        Assert.True(result);
        _mockWatchlistRepository.Verify(repo => repo.RemoveAsync(
            It.Is<WatchlistEntity>(w => w.Id.Value == watchlistIdGuid),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_GetAsyncThrowsException_ExceptionPropagates()
    {
        var watchlistIdGuid = Guid.NewGuid();
        var command = new RemoveSchoolFromWatchlistCommand(watchlistIdGuid);

        _mockWatchlistRepository
            .Setup(repo => repo.GetAsync(It.IsAny<object[]>()))
            .ThrowsAsync(new InvalidOperationException("Not found"));

        var handler = new RemoveSchoolFromWatchlist.RemoveSchoolFromWatchlistCommandHandler(_mockWatchlistRepository.Object);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            handler.Handle(command, _cancellationToken));

        _mockWatchlistRepository.Verify(repo => repo.RemoveAsync(It.IsAny<WatchlistEntity>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_RemoveAsyncThrowsException_ExceptionPropagates()
    {
        var watchlistIdGuid = Guid.NewGuid();
        var watchlistId = new WatchlistId(watchlistIdGuid);
        var watchlistItem = new WatchlistEntity(watchlistId, _fixture.Create<SupportProjectId>(), "user");
        var command = new RemoveSchoolFromWatchlistCommand(watchlistIdGuid);

        _mockWatchlistRepository
            .Setup(repo => repo.GetAsync(It.IsAny<object[]>()))
            .ReturnsAsync(watchlistItem);

        _mockWatchlistRepository
            .Setup(repo => repo.RemoveAsync(It.IsAny<WatchlistEntity>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        var handler = new RemoveSchoolFromWatchlist.RemoveSchoolFromWatchlistCommandHandler(_mockWatchlistRepository.Object);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            handler.Handle(command, _cancellationToken));
    }
}
