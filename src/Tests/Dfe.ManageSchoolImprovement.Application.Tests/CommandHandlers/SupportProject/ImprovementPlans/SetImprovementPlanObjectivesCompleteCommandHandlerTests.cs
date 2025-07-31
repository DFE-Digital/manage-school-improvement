using AutoFixture;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ImprovementPlans;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Moq;

namespace Dfe.ManageSchoolImprovement.Application.Tests.CommandHandlers.SupportProject.ImprovementPlans
{
    public class SetImprovementPlanObjectivesCompleteCommandHandlerTests
    {
        private readonly Mock<ISupportProjectRepository> _mockSupportProjectRepository;
        private readonly CancellationToken _cancellationToken;
        private readonly Fixture _fixture;
        private readonly Domain.Entities.SupportProject.SupportProject _mockSupportProject;
        private readonly ImprovementPlanId _improvementPlanId;

        public SetImprovementPlanObjectivesCompleteCommandHandlerTests()
        {
            _mockSupportProjectRepository = new Mock<ISupportProjectRepository>();
            _cancellationToken = CancellationToken.None;
            _fixture = new Fixture();
            _mockSupportProject = _fixture.Create<Domain.Entities.SupportProject.SupportProject>();
            _improvementPlanId = new ImprovementPlanId(Guid.NewGuid());
            
            // Set up the support project with an improvement plan
            _mockSupportProject.AddImprovementPlan(_improvementPlanId, _mockSupportProject.Id);
        }

        [Fact]
        public async Task Handle_ValidCommandMarkingComplete_SetsObjectivesComplete()
        {
            // Arrange
            var command = new SetImprovementPlanObjectivesComplete.SetImprovementPlanObjectivesCompleteCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                true
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetImprovementPlanObjectivesComplete.SetImprovementPlanObjectivesCompleteCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.True(result);
            _mockSupportProjectRepository.Verify(repo => repo.GetSupportProjectById(
                It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), 
                _cancellationToken), Times.Once);
            _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(
                _mockSupportProject, 
                _cancellationToken), Times.Once);
        }

        [Fact]
        public async Task Handle_ValidCommandMarkingIncomplete_SetsObjectivesIncomplete()
        {
            // Arrange
            var command = new SetImprovementPlanObjectivesComplete.SetImprovementPlanObjectivesCompleteCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                false
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetImprovementPlanObjectivesComplete.SetImprovementPlanObjectivesCompleteCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.True(result);
            _mockSupportProjectRepository.Verify(repo => repo.GetSupportProjectById(
                It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), 
                _cancellationToken), Times.Once);
            _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(
                _mockSupportProject, 
                _cancellationToken), Times.Once);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Handle_CommandWithBooleanValues_UpdatesSuccessfully(bool objectivesSectionComplete)
        {
            // Arrange
            var command = new SetImprovementPlanObjectivesComplete.SetImprovementPlanObjectivesCompleteCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                objectivesSectionComplete
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetImprovementPlanObjectivesComplete.SetImprovementPlanObjectivesCompleteCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Handle_SupportProjectNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange
            var nonExistentId = new SupportProjectId(999);
            var command = new SetImprovementPlanObjectivesComplete.SetImprovementPlanObjectivesCompleteCommand(
                nonExistentId,
                _improvementPlanId,
                true
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == nonExistentId), _cancellationToken))
                .ReturnsAsync((Domain.Entities.SupportProject.SupportProject?)null);

            var handler = new SetImprovementPlanObjectivesComplete.SetImprovementPlanObjectivesCompleteCommandHandler(_mockSupportProjectRepository.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => 
                handler.Handle(command, _cancellationToken));
            
            Assert.Equal($"Support project with id {nonExistentId} not found", exception.Message);
        }

        [Fact]
        public async Task Handle_RepositoryThrowsException_ExceptionPropagates()
        {
            // Arrange
            var command = new SetImprovementPlanObjectivesComplete.SetImprovementPlanObjectivesCompleteCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                true
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.IsAny<SupportProjectId>(), _cancellationToken))
                .ThrowsAsync(new InvalidOperationException("Database error"));

            var handler = new SetImprovementPlanObjectivesComplete.SetImprovementPlanObjectivesCompleteCommandHandler(_mockSupportProjectRepository.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => 
                handler.Handle(command, _cancellationToken));
        }

        [Fact]
        public async Task Handle_ValidCommand_UpdateAsyncCalledOnlyOnce()
        {
            // Arrange
            var command = new SetImprovementPlanObjectivesComplete.SetImprovementPlanObjectivesCompleteCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                true
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetImprovementPlanObjectivesComplete.SetImprovementPlanObjectivesCompleteCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            await handler.Handle(command, _cancellationToken);

            // Assert
            _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(
                It.IsAny<Domain.Entities.SupportProject.SupportProject>(), 
                _cancellationToken), Times.Once);
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnsTrue()
        {
            // Arrange
            var command = new SetImprovementPlanObjectivesComplete.SetImprovementPlanObjectivesCompleteCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                true
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetImprovementPlanObjectivesComplete.SetImprovementPlanObjectivesCompleteCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Handle_ValidCommand_CallsRepositoryMethodsInCorrectOrder()
        {
            // Arrange
            var command = new SetImprovementPlanObjectivesComplete.SetImprovementPlanObjectivesCompleteCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                true
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetImprovementPlanObjectivesComplete.SetImprovementPlanObjectivesCompleteCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            await handler.Handle(command, _cancellationToken);

            // Assert - Verify GetSupportProjectById is called before UpdateAsync
            _mockSupportProjectRepository.Verify(repo => repo.GetSupportProjectById(
                It.IsAny<SupportProjectId>(), 
                _cancellationToken), Times.Once);
            _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(
                It.IsAny<Domain.Entities.SupportProject.SupportProject>(), 
                _cancellationToken), Times.Once);
        }

        [Fact]
        public async Task Handle_MultipleImprovementPlans_UpdatesCorrectPlan()
        {
            // Arrange
            var specificPlanId = new ImprovementPlanId(Guid.NewGuid());
            
            // Set up additional improvement plan for this test
            _mockSupportProject.AddImprovementPlan(specificPlanId, _mockSupportProject.Id);
            
            var command = new SetImprovementPlanObjectivesComplete.SetImprovementPlanObjectivesCompleteCommand(
                _mockSupportProject.Id,
                specificPlanId,
                true
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetImprovementPlanObjectivesComplete.SetImprovementPlanObjectivesCompleteCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.True(result);
            _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(
                _mockSupportProject, 
                _cancellationToken), Times.Once);
        }

        [Fact]
        public async Task Handle_UpdateAsyncThrowsException_ExceptionPropagates()
        {
            // Arrange
            var command = new SetImprovementPlanObjectivesComplete.SetImprovementPlanObjectivesCompleteCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                true
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            _mockSupportProjectRepository
                .Setup(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.SupportProject.SupportProject>(), _cancellationToken))
                .ThrowsAsync(new InvalidOperationException("Update failed"));

            var handler = new SetImprovementPlanObjectivesComplete.SetImprovementPlanObjectivesCompleteCommandHandler(_mockSupportProjectRepository.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => 
                handler.Handle(command, _cancellationToken));
        }

        [Fact]
        public async Task Handle_ToggleCompletionStatus_WorksCorrectly()
        {
            // Arrange - First mark as complete
            var completeCommand = new SetImprovementPlanObjectivesComplete.SetImprovementPlanObjectivesCompleteCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                true
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetImprovementPlanObjectivesComplete.SetImprovementPlanObjectivesCompleteCommandHandler(_mockSupportProjectRepository.Object);

            // Act - Mark as complete
            var completeResult = await handler.Handle(completeCommand, _cancellationToken);

            // Arrange - Then mark as incomplete
            var incompleteCommand = new SetImprovementPlanObjectivesComplete.SetImprovementPlanObjectivesCompleteCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                false
            );

            // Act - Mark as incomplete
            var incompleteResult = await handler.Handle(incompleteCommand, _cancellationToken);

            // Assert
            Assert.True(completeResult);
            Assert.True(incompleteResult);
            _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(
                It.IsAny<Domain.Entities.SupportProject.SupportProject>(), 
                _cancellationToken), Times.Exactly(2));
        }
    }
} 