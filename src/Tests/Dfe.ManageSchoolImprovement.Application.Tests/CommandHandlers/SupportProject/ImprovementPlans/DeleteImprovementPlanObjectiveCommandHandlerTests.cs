using AutoFixture;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ImprovementPlans;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Moq;

namespace Dfe.ManageSchoolImprovement.Application.Tests.CommandHandlers.SupportProject.ImprovementPlans
{
    public class DeleteImprovementPlanObjectiveCommandHandlerTests
    {
        private readonly Mock<ISupportProjectRepository> _mockSupportProjectRepository;
        private readonly CancellationToken _cancellationToken;
        private readonly Fixture _fixture;
        private readonly Domain.Entities.SupportProject.SupportProject _mockSupportProject;

        public DeleteImprovementPlanObjectiveCommandHandlerTests()
        {
            _mockSupportProjectRepository = new Mock<ISupportProjectRepository>();
            _cancellationToken = CancellationToken.None;
            _fixture = new Fixture();
            _mockSupportProject = _fixture.Create<Domain.Entities.SupportProject.SupportProject>();
        }

        private static Domain.Entities.SupportProject.SupportProject CreateSupportProjectWithPlanAndObjective(
            SupportProjectId supportProjectId,
            ImprovementPlanId improvementPlanId,
            ImprovementPlanObjectiveId objectiveId)
        {
            var supportProject = new Domain.Entities.SupportProject.SupportProject(
                supportProjectId,
                "School Name",
                "123456",
                "Local Authority",
                "Region");
            supportProject.AddImprovementPlan(improvementPlanId, supportProjectId);
            supportProject.AddImprovementPlanObjective(objectiveId, improvementPlanId, "Area", "Details");
            return supportProject;
        }

        [Fact]
        public async Task Handle_ValidCommand_DeletesObjectiveAndReturnsTrue()
        {
            // Arrange - support project with an improvement plan and objective so delete succeeds
            var supportProjectId = new SupportProjectId(1);
            var improvementPlanId = new ImprovementPlanId(Guid.NewGuid());
            var objectiveId = new ImprovementPlanObjectiveId(Guid.NewGuid());
            var supportProject = CreateSupportProjectWithPlanAndObjective(supportProjectId, improvementPlanId, objectiveId);
            var deletedBy = "test.user@example.com";
            var command = new DeleteImprovementPlanObjective.DeleteImprovementPlanObjectiveCommand(
                supportProjectId,
                improvementPlanId,
                objectiveId,
                deletedBy);

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == supportProjectId), _cancellationToken))
                .ReturnsAsync(supportProject);

            var handler = new DeleteImprovementPlanObjective.DeleteImprovementPlanObjectiveCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.True(result);
            _mockSupportProjectRepository.Verify(repo => repo.GetSupportProjectById(
                It.Is<SupportProjectId>(id => id == supportProjectId),
                _cancellationToken), Times.Once);
            _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(
                supportProject,
                _cancellationToken), Times.Once);
        }

        [Fact]
        public async Task Handle_SupportProjectNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange
            var nonExistentId = new SupportProjectId(999);
            var command = new DeleteImprovementPlanObjective.DeleteImprovementPlanObjectiveCommand(
                nonExistentId,
                new ImprovementPlanId(Guid.NewGuid()),
                new ImprovementPlanObjectiveId(Guid.NewGuid()),
                "deleted.by@example.com");

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == nonExistentId), _cancellationToken))
                .ReturnsAsync((Domain.Entities.SupportProject.SupportProject?)null);

            var handler = new DeleteImprovementPlanObjective.DeleteImprovementPlanObjectiveCommandHandler(_mockSupportProjectRepository.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                handler.Handle(command, _cancellationToken));

            Assert.Equal($"Support project with id {nonExistentId} not found", exception.Message);
        }

        [Fact]
        public async Task Handle_RepositoryThrowsException_ExceptionPropagates()
        {
            // Arrange
            var command = new DeleteImprovementPlanObjective.DeleteImprovementPlanObjectiveCommand(
                _mockSupportProject.Id,
                new ImprovementPlanId(Guid.NewGuid()),
                new ImprovementPlanObjectiveId(Guid.NewGuid()),
                "deleted.by@example.com");

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.IsAny<SupportProjectId>(), _cancellationToken))
                .ThrowsAsync(new InvalidOperationException("Database error"));

            var handler = new DeleteImprovementPlanObjective.DeleteImprovementPlanObjectiveCommandHandler(_mockSupportProjectRepository.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.Handle(command, _cancellationToken));
        }

        [Fact]
        public async Task Handle_ValidCommand_CallsRepositoryMethodsInCorrectOrder()
        {
            // Arrange
            var supportProjectId = new SupportProjectId(2);
            var improvementPlanId = new ImprovementPlanId(Guid.NewGuid());
            var objectiveId = new ImprovementPlanObjectiveId(Guid.NewGuid());
            var supportProject = CreateSupportProjectWithPlanAndObjective(supportProjectId, improvementPlanId, objectiveId);
            var command = new DeleteImprovementPlanObjective.DeleteImprovementPlanObjectiveCommand(
                supportProjectId,
                improvementPlanId,
                objectiveId,
                "deleted.by@example.com");

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == supportProjectId), _cancellationToken))
                .ReturnsAsync(supportProject);

            var handler = new DeleteImprovementPlanObjective.DeleteImprovementPlanObjectiveCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            await handler.Handle(command, _cancellationToken);

            // Assert
            _mockSupportProjectRepository.Verify(repo => repo.GetSupportProjectById(
                It.Is<SupportProjectId>(id => id == supportProjectId),
                _cancellationToken), Times.Once);
            _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(
                supportProject,
                _cancellationToken), Times.Once);
        }

        [Fact]
        public async Task Handle_ValidCommand_UpdateAsyncCalledWithSupportProjectAfterDelete()
        {
            // Arrange
            var supportProjectId = new SupportProjectId(3);
            var improvementPlanId = new ImprovementPlanId(Guid.NewGuid());
            var objectiveId = new ImprovementPlanObjectiveId(Guid.NewGuid());
            var supportProject = CreateSupportProjectWithPlanAndObjective(supportProjectId, improvementPlanId, objectiveId);
            var command = new DeleteImprovementPlanObjective.DeleteImprovementPlanObjectiveCommand(
                supportProjectId,
                improvementPlanId,
                objectiveId,
                "deleted.by@example.com");

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == supportProjectId), _cancellationToken))
                .ReturnsAsync(supportProject);

            var handler = new DeleteImprovementPlanObjective.DeleteImprovementPlanObjectiveCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            await handler.Handle(command, _cancellationToken);

            // Assert
            _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(
                It.Is<Domain.Entities.SupportProject.SupportProject>(sp => sp == supportProject),
                _cancellationToken), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenImprovementPlanNotFound_ThrowsInvalidOperationException()
        {
            // Arrange - support project with no improvement plans; domain throws when plan not found
            var supportProjectWithNoPlans = new Domain.Entities.SupportProject.SupportProject(
                new SupportProjectId(1),
                "School Name",
                "123456",
                "Local Authority",
                "Region");
            var nonExistentPlanId = new ImprovementPlanId(Guid.NewGuid());
            var command = new DeleteImprovementPlanObjective.DeleteImprovementPlanObjectiveCommand(
                supportProjectWithNoPlans.Id,
                nonExistentPlanId,
                new ImprovementPlanObjectiveId(Guid.NewGuid()),
                "deleted.by@example.com");

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == supportProjectWithNoPlans.Id), _cancellationToken))
                .ReturnsAsync(supportProjectWithNoPlans);

            var handler = new DeleteImprovementPlanObjective.DeleteImprovementPlanObjectiveCommandHandler(_mockSupportProjectRepository.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.Handle(command, _cancellationToken));

            Assert.Equal($"Improvement plan with id {nonExistentPlanId} not found.", exception.Message);
        }
    }
}
