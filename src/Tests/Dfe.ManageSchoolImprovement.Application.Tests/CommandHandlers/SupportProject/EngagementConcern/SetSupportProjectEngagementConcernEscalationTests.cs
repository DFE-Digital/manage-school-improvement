using AutoFixture;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.EngagementConcern;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Moq;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.EngagementConcern.SetSupportProjectEngagementConcernEscalation;

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
        public async Task Handle_ValidCommand_UpdatesEngagementConcern()
        {
            // Arrange
            var engagementConcernId = new EngagementConcernId(Guid.NewGuid());
            var engagementConcernEscalationConfirmStepsTaken = true;
            var engagementConcernEscalationPrimaryReason = "a very good reason";
            var engagementConcernEscalationDetails = "some excellent details";
            var engagementConcernEscalationDateOfDecision =  DateTime.UtcNow;
            var engagementConcernEscalationWarningNotice = "TWN issued";

            var command = new SetSupportProjectEngagementConcernEscalationCommand(
                engagementConcernId,
                _mockSupportProject.Id,
                engagementConcernEscalationConfirmStepsTaken,
                engagementConcernEscalationPrimaryReason,
                engagementConcernEscalationDetails,
                engagementConcernEscalationDateOfDecision,
                engagementConcernEscalationWarningNotice
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
            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(x => x == _mockSupportProject.Id),
                    It.IsAny<CancellationToken>())).ReturnsAsync(_mockSupportProject);
            
            var addEngagementConcernCommandHandler =
                new AddEngagementConcern.AddEngagementConcernCommandHandler(_mockSupportProjectRepository.Object);

            var addEngagementConcernCommand = new AddEngagementConcern.AddEngagementConcernCommand(
                _mockSupportProject.Id, 
                "details", 
                DateTime.UtcNow,
                false,
                null,
                null
            );
            await addEngagementConcernCommandHandler.Handle(addEngagementConcernCommand, _cancellationToken);
            
            var engagementConcernId = _mockSupportProject.EngagementConcerns.First().Id;
            var command = new SetSupportProjectEngagementConcernEscalationCommand(
                engagementConcernId, _mockSupportProject.Id, null, null, null, null, null
            );
            
            var setSupportProjectEngagementConcernEscalationCommandHandler = new SetSupportProjectEngagementConcernEscalationCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await setSupportProjectEngagementConcernEscalationCommandHandler.Handle(command, _cancellationToken);

            // Verify
            Assert.IsType<bool>(result);
            Assert.True(result);
            _mockSupportProjectRepository.Verify(
                repo => repo.UpdateAsync(
                    It.Is<Domain.Entities.SupportProject.SupportProject>(x =>
                        x.EngagementConcerns.First().EngagementConcernEscalationConfirmStepsTaken == null && 
                        x.EngagementConcerns.First().EngagementConcernEscalationPrimaryReason == null && 
                        x.EngagementConcerns.First().EngagementConcernEscalationDetails == null &&
                        x.EngagementConcerns.First().EngagementConcernEscalationDateOfDecision == null &&
                        x.EngagementConcerns.First().EngagementConcernEscalationWarningNotice == null), 
                    It.IsAny<CancellationToken>()),
                Times.AtLeastOnce);
        }

        [Fact]
        public async Task Handle_ProjectNotFound_ReturnsFalse()
        {
            var engagementConcernId = new EngagementConcernId(Guid.NewGuid());
            var engagementConcernEscalationConfirmStepsTaken = true;
            var engagementConcernEscalationPrimaryReason = "a very good reason";
            var engagementConcernEscalationDetails = "some excellent details";
            var engagementConcernEscalationDateOfDecision =  DateTime.UtcNow;
            var engagementConcernEscalationWarningNotice = "TWN issued";

            var command = new SetSupportProjectEngagementConcernEscalationCommand(
                engagementConcernId,
                _mockSupportProject.Id, 
                engagementConcernEscalationConfirmStepsTaken, 
                engagementConcernEscalationPrimaryReason, 
                engagementConcernEscalationDetails, 
                engagementConcernEscalationDateOfDecision,
                engagementConcernEscalationWarningNotice
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
