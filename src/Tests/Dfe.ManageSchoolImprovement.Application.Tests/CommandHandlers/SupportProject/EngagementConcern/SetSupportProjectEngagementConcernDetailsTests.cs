using AutoFixture;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Moq;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.CreateSupportProjectNote.SetSupportProjectEngagementConcernDetails;

namespace Dfe.ManageSchoolImprovement.Application.Tests.CommandHandlers.SupportProject.EngagementConcern
{
    public class SetSupportProjectEngagementConcernDetailsTests
    {
        private readonly Mock<ISupportProjectRepository> _mockSupportProjectRepository;
        private readonly Domain.Entities.SupportProject.SupportProject _mockSupportProject;
        private readonly CancellationToken _cancellationToken;

        public SetSupportProjectEngagementConcernDetailsTests()
        {

            _mockSupportProjectRepository = new Mock<ISupportProjectRepository>();
            var fixture = new Fixture();
            _mockSupportProject = fixture.Create<Domain.Entities.SupportProject.SupportProject>();
            _cancellationToken = CancellationToken.None;
        }

        [Fact]
        public async Task Handle_ValidCommand_UpdatesSupportProject()
        {
            // Arrange
            var engagementConcernRecorded = true;
            var engagementConcernDetails = "test details";
            var engagementConcernRaisedDate =  DateTime.UtcNow;

            var command = new SetSupportProjectEngagementConcernDetailsCommand(
                _mockSupportProject.Id, engagementConcernRecorded, engagementConcernDetails, engagementConcernRaisedDate
            );
            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(x => x == _mockSupportProject.Id),
                    It.IsAny<CancellationToken>())).ReturnsAsync(_mockSupportProject);
            var setSupportProjectEngagementConcernDetailsCommandHandler =
                new SetSupportProjectEngagementConcernDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await setSupportProjectEngagementConcernDetailsCommandHandler.Handle(command, _cancellationToken);

            // Verify
            Assert.IsType<bool>(result);
            Assert.True(result);
            _mockSupportProjectRepository.Verify(
                repo => repo.UpdateAsync(
                    It.Is<Domain.Entities.SupportProject.SupportProject>(x =>
                        x.EngagementConcernRecorded == engagementConcernRecorded &&
                        x.EngagementConcernDetails == engagementConcernDetails &&
                        x.EngagementConcernRaisedDate == engagementConcernRaisedDate), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ValidEmptyCommand_UpdatesSupportProject()
        {
            // Arrange
            var command = new SetSupportProjectEngagementConcernDetailsCommand(
                _mockSupportProject.Id, null, null, null
            );
            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(x => x == _mockSupportProject.Id),
                    It.IsAny<CancellationToken>())).ReturnsAsync(_mockSupportProject);
            var setSupportProjectEngagementConcernDetailsCommandHandler = new SetSupportProjectEngagementConcernDetailsCommandHandler(_mockSupportProjectRepository.Object);

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
            var engagementConcernRecorded = true;
            var engagementConcernDetails = "test details";
            var engagementConcernRaisedDate =  DateTime.UtcNow;

            var command = new SetSupportProjectEngagementConcernDetailsCommand(
                _mockSupportProject.Id, engagementConcernRecorded, engagementConcernDetails, engagementConcernRaisedDate
            );
            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(x => x == _mockSupportProject.Id),
                    It.IsAny<CancellationToken>())).ReturnsAsync(null as Domain.Entities.SupportProject.SupportProject);
            var setSupportProjectEngagementConcernDetailsCommandHandler = new SetSupportProjectEngagementConcernDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await setSupportProjectEngagementConcernDetailsCommandHandler.Handle(command, _cancellationToken);

            // Verify
            Assert.IsType<bool>(result);
            Assert.False(result);
            _mockSupportProjectRepository.Verify(
                repo => repo.UpdateAsync(It.IsAny<Domain.Entities.SupportProject.SupportProject>(),
                    It.IsAny<CancellationToken>()), Times.Never);

        }
    }
}
