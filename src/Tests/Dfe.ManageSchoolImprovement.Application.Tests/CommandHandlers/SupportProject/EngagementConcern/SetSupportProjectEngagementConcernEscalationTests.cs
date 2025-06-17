using AutoFixture;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.CreateSupportProjectNote;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Moq;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.CreateSupportProjectNote.SetSupportProjectEngagementConcernEscalation;

namespace Dfe.ManageSchoolImprovement.Application.Tests.CommandHandlers.SupportProject.EngagementConcern
{
    public class SetSupportProjectEngagementConcernEscalationTests
    {
        private readonly Mock<ISupportProjectRepository> _mockSupportProjectRepository;
        private readonly Domain.Entities.SupportProject.SupportProject _mockSupportProject;
        private readonly CancellationToken _cancellationToken;

        public SetSupportProjectEngagementConcernEscalationTests()
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
            var engagementConcernEscalationConfirmStepsTaken = true;
            var engagementConcernEscalationPrimaryReason = "a very good reason";
            var engagementConcernEscalationDetails = "some excellent details";
            var engagementConcernEscalationDateOfDecision =  DateTime.UtcNow;

            var command = new SetSupportProjectEngagementConcernEscalationCommand(
                _mockSupportProject.Id,
                engagementConcernEscalationConfirmStepsTaken,
                engagementConcernEscalationPrimaryReason,
                engagementConcernEscalationDetails,
                engagementConcernEscalationDateOfDecision
            );
            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(x => x == _mockSupportProject.Id),
                    It.IsAny<CancellationToken>())).ReturnsAsync(_mockSupportProject);
            var setSupportProjectEngagementConcernDetailsCommandHandler =
                new SetSupportProjectEngagementConcernEscalationCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await setSupportProjectEngagementConcernDetailsCommandHandler.Handle(command, _cancellationToken);

            // Verify
            Assert.IsType<bool>(result);
            Assert.True(result);
            _mockSupportProjectRepository.Verify(
                repo => repo.UpdateAsync(
                    It.Is<Domain.Entities.SupportProject.SupportProject>(x =>
                        x.EngagementConcernEscalationConfirmStepsTaken == engagementConcernEscalationConfirmStepsTaken &&
                        x.EngagementConcernEscalationPrimaryReason == engagementConcernEscalationPrimaryReason &&
                        x.EngagementConcernEscalationDetails == engagementConcernEscalationDetails &&
                        x.EngagementConcernEscalationDateOfDecision == engagementConcernEscalationDateOfDecision), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ValidEmptyCommand_UpdatesSupportProject()
        {
            // Arrange
            var command = new SetSupportProjectEngagementConcernEscalationCommand(
                _mockSupportProject.Id, null, null, null, null
            );
            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(x => x == _mockSupportProject.Id),
                    It.IsAny<CancellationToken>())).ReturnsAsync(_mockSupportProject);
            var setSupportProjectEngagementConcernEscalationCommandHandler = new SetSupportProjectEngagementConcernEscalationCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await setSupportProjectEngagementConcernEscalationCommandHandler.Handle(command, _cancellationToken);

            // Verify
            Assert.IsType<bool>(result);
            Assert.True(result);
            _mockSupportProjectRepository.Verify(
                repo => repo.UpdateAsync(
                    It.Is<Domain.Entities.SupportProject.SupportProject>(x =>
                        x.EngagementConcernEscalationConfirmStepsTaken == null && 
                        x.EngagementConcernEscalationPrimaryReason == null && 
                        x.EngagementConcernEscalationDetails == null &&
                        x.EngagementConcernEscalationDateOfDecision == null), 
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ProjectNotFound_ReturnsFalse()
        {
            var engagementConcernEscalationConfirmStepsTaken = true;
            var engagementConcernEscalationPrimaryReason = "a very good reason";
            var engagementConcernEscalationDetails = "some excellent details";
            var engagementConcernEscalationDateOfDecision =  DateTime.UtcNow;

            var command = new SetSupportProjectEngagementConcernEscalationCommand(
                _mockSupportProject.Id, 
                engagementConcernEscalationConfirmStepsTaken, 
                engagementConcernEscalationPrimaryReason, 
                engagementConcernEscalationDetails, 
                engagementConcernEscalationDateOfDecision
            );
            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(x => x == _mockSupportProject.Id),
                    It.IsAny<CancellationToken>())).ReturnsAsync(null as Domain.Entities.SupportProject.SupportProject);
            var setSupportProjectEngagementConcernEscalationCommandHandler = new SetSupportProjectEngagementConcernEscalationCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await setSupportProjectEngagementConcernEscalationCommandHandler.Handle(command, _cancellationToken);

            // Verify
            Assert.IsType<bool>(result);
            Assert.False(result);
            _mockSupportProjectRepository.Verify(
                repo => repo.UpdateAsync(It.IsAny<Domain.Entities.SupportProject.SupportProject>(),
                    It.IsAny<CancellationToken>()), Times.Never);

        }
    }
}
