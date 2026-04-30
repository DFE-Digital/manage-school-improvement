using AutoFixture;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.Watchlist;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Moq;

namespace Dfe.ManageSchoolImprovement.Application.Tests.CommandHandlers.SupportProject.Watchlist
{
    public class AddSchoolToWatchlistTests
    {
        private readonly Mock<IWatchlistRepository> _mockWatchlistRepository;
        private readonly CancellationToken _cancellationToken;
        private readonly Fixture _fixture;

        public AddSchoolToWatchlistTests()
        {
            _mockWatchlistRepository = new Mock<IWatchlistRepository>();
            _cancellationToken = CancellationToken.None;
            _fixture = new Fixture();
        }

        [Fact]
        public async Task Handle_ValidCommand_AddsWatchlistRecordAndReturnsTrue()
        {
            var supportProjectId = _fixture.Create<SupportProjectId>();
            const string user = "test.user@education.gov.uk";
            var command = new AddSchoolToWatchlist.AddSchoolToWatchlistCommand(supportProjectId, user);

            _mockWatchlistRepository
                .Setup(repo => repo.AddAsync(It.IsAny<Domain.Entities.SupportProject.Watchlist>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Domain.Entities.SupportProject.Watchlist w, CancellationToken _) => w);

            var handler = new AddSchoolToWatchlist.AddSchoolToWatchlistCommandHandler(_mockWatchlistRepository.Object);

            var result = await handler.Handle(command, _cancellationToken);

            Assert.True(result);
            _mockWatchlistRepository.Verify(repo => repo.AddAsync(
                It.Is<Domain.Entities.SupportProject.Watchlist>(w =>
                    w.SupportProjectId == supportProjectId &&
                    w.User == user &&
                    w.Id.Value != Guid.Empty),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_RepositoryThrowsException_ExceptionPropagates()
        {
            var supportProjectId = _fixture.Create<SupportProjectId>();
            var command = new AddSchoolToWatchlist.AddSchoolToWatchlistCommand(supportProjectId, "user");

            _mockWatchlistRepository
                .Setup(repo => repo.AddAsync(It.IsAny<Domain.Entities.SupportProject.Watchlist>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException("Database error"));

            var handler = new AddSchoolToWatchlist.AddSchoolToWatchlistCommandHandler(_mockWatchlistRepository.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.Handle(command, _cancellationToken));
        }
    }
}
