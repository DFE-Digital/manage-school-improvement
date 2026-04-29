using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
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
    public async Task GetAllSchoolsForUser_ReturnsSuccessWithIds_WhenRepositoryReturnsIds()
    {
        const string user = "user@education.gov.uk";
        var ids = new[] { 10, 20, 30 };

        _mockWatchlistRepository
            .Setup(r => r.GetAllSchoolsForUser(user, It.IsAny<CancellationToken>()))
            .ReturnsAsync(ids);

        var result = await _service.GetAllSchoolsForUser(user, _cancellationToken);

        Assert.True(result.IsSuccess);
        Assert.Null(result.Error);
        Assert.Equal(ids, result.Value);
        _mockWatchlistRepository.Verify(
            r => r.GetAllSchoolsForUser(user, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAllSchoolsForUser_ReturnsSuccessWithEmptySequence_WhenRepositoryReturnsNone()
    {
        const string user = "user@education.gov.uk";
        var empty = Array.Empty<int>();

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
}
