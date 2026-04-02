using AutoFixture;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.Eligibility;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Moq;
using System.Linq.Expressions;

namespace Dfe.ManageSchoolImprovement.Application.Tests.CommandHandlers.SupportProject.UpdateSupportProject
{
    public class UpdateEligibilityTests
    {
        private readonly Mock<ISupportProjectRepository> _mockSupportProjectRepository;
        private readonly Domain.Entities.SupportProject.SupportProject _mockSupportProject;
        private readonly CancellationToken _cancellationToken;

        public UpdateEligibilityTests()
        {
            _mockSupportProjectRepository = new Mock<ISupportProjectRepository>();
            var fixture = new Fixture();
            _mockSupportProject = fixture.Create<Domain.Entities.SupportProject.SupportProject>();
            _cancellationToken = CancellationToken.None;
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnsTrueAndCallsUpdate()
        {
            // Arrange
            var command = new UpdateEligibilityCommand(
                _mockSupportProject.Id,
                SupportProjectEligibilityStatus.EligibleForSupport,
                DateTime.UtcNow,
                "admin@example.com",
                DateTime.UtcNow.AddMonths(6),
                null
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.FindAsync(
                    It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(_mockSupportProject);

            var handler = new UpdateEligibilityCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.True(result);
            _mockSupportProjectRepository.Verify(
                repo => repo.UpdateAsync(
                    It.IsAny<Domain.Entities.SupportProject.SupportProject>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ProjectNotFound_ReturnsFalseAndDoesNotCallUpdate()
        {
            // Arrange
            var command = new UpdateEligibilityCommand(
                _mockSupportProject.Id,
                SupportProjectEligibilityStatus.EligibleForSupport,
                DateTime.UtcNow,
                "admin@example.com",
                DateTime.UtcNow.AddMonths(6),
                null
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.FindAsync(
                    It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, bool>>>(),
                    It.IsAny<CancellationToken>()))!
                .ReturnsAsync((Domain.Entities.SupportProject.SupportProject)null!);

            var handler = new UpdateEligibilityCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.False(result);
            _mockSupportProjectRepository.Verify(
                repo => repo.UpdateAsync(
                    It.IsAny<Domain.Entities.SupportProject.SupportProject>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_NotEligibleStatus_ReturnsTrueAndCallsUpdate()
        {
            // Arrange
            var command = new UpdateEligibilityCommand(
                _mockSupportProject.Id,
                SupportProjectEligibilityStatus.NotEligibleForSupport,
                DateTime.UtcNow,
                "admin@example.com",
                null,
                "School does not meet the criteria."
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.FindAsync(
                    It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(_mockSupportProject);

            var handler = new UpdateEligibilityCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.True(result);
            _mockSupportProjectRepository.Verify(
                repo => repo.UpdateAsync(
                    It.IsAny<Domain.Entities.SupportProject.SupportProject>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_NotEligibleMidInterventionStatus_ReturnsTrueAndCallsUpdate()
        {
            // Arrange
            var command = new UpdateEligibilityCommand(
                _mockSupportProject.Id,
                SupportProjectEligibilityStatus.NotEligibleForSupportMidIntervention,
                DateTime.UtcNow,
                "admin@example.com",
                null,
                "Mid-intervention ineligibility reason."
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.FindAsync(
                    It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(_mockSupportProject);

            var handler = new UpdateEligibilityCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.True(result);
            _mockSupportProjectRepository.Verify(
                repo => repo.UpdateAsync(
                    It.IsAny<Domain.Entities.SupportProject.SupportProject>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_AllNullOptionalFields_ReturnsTrueAndCallsUpdate()
        {
            // Arrange
            var command = new UpdateEligibilityCommand(
                _mockSupportProject.Id,
                null,
                null,
                null,
                null,
                null
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.FindAsync(
                    It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(_mockSupportProject);

            var handler = new UpdateEligibilityCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.True(result);
            _mockSupportProjectRepository.Verify(
                repo => repo.UpdateAsync(
                    It.IsAny<Domain.Entities.SupportProject.SupportProject>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ValidCommand_PassesCorrectProjectToUpdateAsync()
        {
            // Arrange
            Domain.Entities.SupportProject.SupportProject? capturedProject = null;

            var command = new UpdateEligibilityCommand(
                _mockSupportProject.Id,
                SupportProjectEligibilityStatus.EligibleForSupport,
                DateTime.UtcNow,
                "admin@example.com",
                DateTime.UtcNow.AddMonths(3),
                null
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.FindAsync(
                    It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(_mockSupportProject);

            _mockSupportProjectRepository
                .Setup(repo => repo.UpdateAsync(
                    It.IsAny<Domain.Entities.SupportProject.SupportProject>(),
                    It.IsAny<CancellationToken>()))
                .Callback<Domain.Entities.SupportProject.SupportProject, CancellationToken>(
                    (project, _) => capturedProject = project);

            var handler = new UpdateEligibilityCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.NotNull(capturedProject);
            Assert.Equal(_mockSupportProject.Id, capturedProject.Id);
        }
    }
}
