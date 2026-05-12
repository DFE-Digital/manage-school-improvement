using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Moq;

namespace Dfe.ManageSchoolImprovement.Application.Tests.SupportProject.Queries;

public class WatchlistQueryServiceTests
{
    private readonly Mock<IWatchlistRepository> _mockWatchlistRepository;
    private readonly WatchlistQueryService _service;
    private readonly CancellationToken _cancellationToken;

    public WatchlistQueryServiceTests()
    {
        _mockWatchlistRepository = new Mock<IWatchlistRepository>();
        _service = new WatchlistQueryService(_mockWatchlistRepository.Object);
        _cancellationToken = CancellationToken.None;
    }

    [Fact]
    public async Task GetAllSchoolsForUser_ReturnsSuccessWithWatchlists_WhenRepositoryReturnsWatchlists()
    {
        const string user = "user@education.gov.uk";
        var watchlists = new[]
        {
            new Watchlist(new WatchlistId(Guid.NewGuid()), new SupportProjectId(10), user),
            new Watchlist(new WatchlistId(Guid.NewGuid()), new SupportProjectId(20), user),
            new Watchlist(new WatchlistId(Guid.NewGuid()), new SupportProjectId(30), user),
        };

        _mockWatchlistRepository
            .Setup(r => r.GetAllSchoolsForUser(user, It.IsAny<CancellationToken>()))
            .ReturnsAsync(watchlists);

        var result = await _service.GetAllSchoolsForUser(user, _cancellationToken);

        Assert.True(result.IsSuccess);
        Assert.Null(result.Error);
        Assert.Equal(watchlists, result.Value);
        _mockWatchlistRepository.Verify(
            r => r.GetAllSchoolsForUser(user, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAllSchoolsForUser_ReturnsSuccessWithEmptySequence_WhenRepositoryReturnsNone()
    {
        const string user = "user@education.gov.uk";
        var empty = Array.Empty<Watchlist>();

        _mockWatchlistRepository
            .Setup(r => r.GetAllSchoolsForUser(user, It.IsAny<CancellationToken>()))
            .ReturnsAsync(empty);

        var result = await _service.GetAllSchoolsForUser(user, _cancellationToken);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Empty(result.Value!);
    }

    [Fact]
    public async Task GetAllSchoolsForUser_PropagatesException_WhenRepositoryThrows()
    {
        const string user = "user@education.gov.uk";

        _mockWatchlistRepository
            .Setup(r => r.GetAllSchoolsForUser(user, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.GetAllSchoolsForUser(user, _cancellationToken));
    }

    [Fact]
    public async Task GetAllWatchlistsForSchool_ReturnsSuccessWithWatchlists_WhenRepositoryReturnsWatchlists()
    {
        var supportProjectId = new SupportProjectId(10);
        var watchlists = new[]
        {
            new Watchlist(new WatchlistId(Guid.NewGuid()), supportProjectId, "user1@education.gov.uk"),
            new Watchlist(new WatchlistId(Guid.NewGuid()), supportProjectId, "user2@education.gov.uk"),
        };

        _mockWatchlistRepository
            .Setup(r => r.GetAllWatchlistsForSchool(supportProjectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(watchlists);

        var result = await _service.GetAllWatchlistsForSchool(supportProjectId, _cancellationToken);

        Assert.True(result.IsSuccess);
        Assert.Null(result.Error);
        Assert.Equal(watchlists, result.Value);
        _mockWatchlistRepository.Verify(
            r => r.GetAllWatchlistsForSchool(supportProjectId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAllWatchlistsForSchool_ReturnsSuccessWithEmptySequence_WhenRepositoryReturnsNone()
    {
        var supportProjectId = new SupportProjectId(99);
        var empty = Array.Empty<Watchlist>();

        _mockWatchlistRepository
            .Setup(r => r.GetAllWatchlistsForSchool(supportProjectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(empty);

        var result = await _service.GetAllWatchlistsForSchool(supportProjectId, _cancellationToken);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Empty(result.Value!);
    }

    [Fact]
    public async Task GetAllWatchlistsForSchool_PropagatesException_WhenRepositoryThrows()
    {
        var supportProjectId = new SupportProjectId(5);

        _mockWatchlistRepository
            .Setup(r => r.GetAllWatchlistsForSchool(supportProjectId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.GetAllWatchlistsForSchool(supportProjectId, _cancellationToken));
    }
}
