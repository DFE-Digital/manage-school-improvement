using AutoFixture;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Moq;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.EngagementConcern.AddEngagementConcern;

namespace Dfe.ManageSchoolImprovement.Application.Tests.CommandHandlers.SupportProject.EngagementConcern
{
    public class AddEngagementConcernTests
    {
        private readonly Mock<ISupportProjectRepository> _mockSupportProjectRepository;
        private readonly Domain.Entities.SupportProject.SupportProject _mockSupportProject;
        private readonly CancellationToken _cancellationToken;

        public AddEngagementConcernTests()
        {

            _mockSupportProjectRepository = new Mock<ISupportProjectRepository>();
            var fixture = new Fixture();
            _mockSupportProject = fixture.Create<Domain.Entities.SupportProject.SupportProject>();
            _cancellationToken = CancellationToken.None;
        }

        [Fact]
        public async Task Handle_ValidCommand_CreatesEngagementConcern()
        {
            // Arrange
            var engagementConcernDetails = "test details";
            var engagementConcernRaisedDate =  DateTime.UtcNow;
            var engagementConcernResolved = true;
            var engagementConcernResolvedDetails =  "concern resolved";
            var engagementConcernResolvedDate =  DateTime.UtcNow;

            var command = new AddEngagementConcernCommand(
                _mockSupportProject.Id, 
                engagementConcernDetails, 
                engagementConcernRaisedDate,
                engagementConcernResolved,
                engagementConcernResolvedDetails,
                engagementConcernResolvedDate
            );
            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(x => x == _mockSupportProject.Id),
                    It.IsAny<CancellationToken>())).ReturnsAsync(_mockSupportProject);
            var setSupportProjectEngagementConcernDetailsCommandHandler =
                new AddEngagementConcernCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await setSupportProjectEngagementConcernDetailsCommandHandler.Handle(command, _cancellationToken);

            // Verify
            Assert.NotNull(result);
            Assert.IsType<bool>(result);
            _mockSupportProjectRepository.Verify(repo => repo.GetSupportProjectById(
                It.Is<SupportProjectId>(id => id == _mockSupportProject.Id),
                _cancellationToken), Times.Once);
            _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(
                _mockSupportProject,
                _cancellationToken), Times.Once);
        }

        [Fact]
        public async Task Handle_ValidEmptyCommand_UpdatesSupportProject()
        {
            // Arrange
            var command = new AddEngagementConcernCommand(
                _mockSupportProject.Id, null, null, null, null, null
            );
            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(x => x == _mockSupportProject.Id),
                    It.IsAny<CancellationToken>())).ReturnsAsync(_mockSupportProject);
            var setSupportProjectEngagementConcernDetailsCommandHandler = new AddEngagementConcernCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await setSupportProjectEngagementConcernDetailsCommandHandler.Handle(command, _cancellationToken);

            // Verify
            Assert.IsType<bool>(result);
            Assert.True(result);
            _mockSupportProjectRepository.Verify(
                repo => repo.UpdateAsync(
                    It.Is<Domain.Entities.SupportProject.SupportProject>(x =>
                        x.EngagementConcernRecorded == null && 
                        x.EngagementConcernDetails == null && 
                        x.EngagementConcernRaisedDate == null), 
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ProjectNotFound_ReturnsFalse()
        {
            var engagementConcernDetails = "test details";
            var engagementConcernRaisedDate =  DateTime.UtcNow;
            var engagementConcernResolved = true;
            var engagementConcernResolvedDetails =  "concern resolved";
            var engagementConcernResolvedDate =  DateTime.UtcNow;
            var nonExistentId = new SupportProjectId(999);
            var command = new AddEngagementConcernCommand(nonExistentId, engagementConcernDetails, engagementConcernRaisedDate, engagementConcernResolved, engagementConcernResolvedDetails, engagementConcernResolvedDate);

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == nonExistentId), _cancellationToken))
                .ReturnsAsync((Domain.Entities.SupportProject.SupportProject?)null);

            var handler = new AddEngagementConcernCommandHandler(_mockSupportProjectRepository.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                handler.Handle(command, _cancellationToken));

            Assert.Equal($"Support project with id {nonExistentId} not found", exception.Message);

        }
    }
}
