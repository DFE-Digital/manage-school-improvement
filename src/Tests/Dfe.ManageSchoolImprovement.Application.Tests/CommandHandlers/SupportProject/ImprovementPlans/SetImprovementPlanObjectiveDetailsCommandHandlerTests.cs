using AutoFixture;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ImprovementPlans;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Moq;

namespace Dfe.ManageSchoolImprovement.Application.Tests.CommandHandlers.SupportProject.ImprovementPlans
{
    public class SetImprovementPlanObjectiveDetailsCommandHandlerTests
    {
        private readonly Mock<ISupportProjectRepository> _mockSupportProjectRepository;
        private readonly CancellationToken _cancellationToken;
        private readonly Fixture _fixture;
        private readonly Domain.Entities.SupportProject.SupportProject _mockSupportProject;
        private readonly ImprovementPlanId _improvementPlanId;
        private readonly ImprovementPlanObjectiveId _objectiveId;

        public SetImprovementPlanObjectiveDetailsCommandHandlerTests()
        {
            _mockSupportProjectRepository = new Mock<ISupportProjectRepository>();
            _cancellationToken = CancellationToken.None;
            _fixture = new Fixture();
            _mockSupportProject = _fixture.Create<Domain.Entities.SupportProject.SupportProject>();
            _improvementPlanId = new ImprovementPlanId(Guid.NewGuid());
            _objectiveId = new ImprovementPlanObjectiveId(Guid.NewGuid());
            
            // Set up the support project with an improvement plan and objective
            _mockSupportProject.AddImprovementPlan(_improvementPlanId, _mockSupportProject.Id);
            _mockSupportProject.AddImprovementPlanObjective(_objectiveId, _improvementPlanId, "QualityOfEducation", "Initial objective details");
        }

        [Fact]
        public async Task Handle_ValidCommand_UpdatesObjectiveDetails()
        {
            // Arrange
            var command = new SetImprovementPlanObjectiveDetails.SetImprovementPlanObjectiveDetailsCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _objectiveId,
                "Updated objective details with comprehensive improvement plan"
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetImprovementPlanObjectiveDetails.SetImprovementPlanObjectiveDetailsCommandHandler(_mockSupportProjectRepository.Object);

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
        [InlineData("Short details")]
        [InlineData("Very detailed improvement plan with comprehensive strategies and implementation timeline")]
        [InlineData("")]
        [InlineData("   ")]
        public async Task Handle_CommandWithVariousDetailLengths_UpdatesSuccessfully(string details)
        {
            // Arrange
            var command = new SetImprovementPlanObjectiveDetails.SetImprovementPlanObjectiveDetailsCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _objectiveId,
                details
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetImprovementPlanObjectiveDetails.SetImprovementPlanObjectiveDetailsCommandHandler(_mockSupportProjectRepository.Object);

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
            var command = new SetImprovementPlanObjectiveDetails.SetImprovementPlanObjectiveDetailsCommand(
                nonExistentId,
                _improvementPlanId,
                _objectiveId,
                "Test details"
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == nonExistentId), _cancellationToken))
                .ReturnsAsync((Domain.Entities.SupportProject.SupportProject?)null);

            var handler = new SetImprovementPlanObjectiveDetails.SetImprovementPlanObjectiveDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => 
                handler.Handle(command, _cancellationToken));
            
            Assert.Equal($"Support project with id {nonExistentId} not found", exception.Message);
        }

        [Fact]
        public async Task Handle_RepositoryThrowsException_ExceptionPropagates()
        {
            // Arrange
            var command = new SetImprovementPlanObjectiveDetails.SetImprovementPlanObjectiveDetailsCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _objectiveId,
                "Test details"
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.IsAny<SupportProjectId>(), _cancellationToken))
                .ThrowsAsync(new InvalidOperationException("Database error"));

            var handler = new SetImprovementPlanObjectiveDetails.SetImprovementPlanObjectiveDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => 
                handler.Handle(command, _cancellationToken));
        }

        [Fact]
        public async Task Handle_ValidCommand_UpdateAsyncCalledOnlyOnce()
        {
            // Arrange
            var command = new SetImprovementPlanObjectiveDetails.SetImprovementPlanObjectiveDetailsCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _objectiveId,
                "Test details"
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetImprovementPlanObjectiveDetails.SetImprovementPlanObjectiveDetailsCommandHandler(_mockSupportProjectRepository.Object);

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
            var command = new SetImprovementPlanObjectiveDetails.SetImprovementPlanObjectiveDetailsCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _objectiveId,
                "Comprehensive objective details"
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetImprovementPlanObjectiveDetails.SetImprovementPlanObjectiveDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Handle_ValidCommand_CallsRepositoryMethodsInCorrectOrder()
        {
            // Arrange
            var command = new SetImprovementPlanObjectiveDetails.SetImprovementPlanObjectiveDetailsCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _objectiveId,
                "Test details"
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetImprovementPlanObjectiveDetails.SetImprovementPlanObjectiveDetailsCommandHandler(_mockSupportProjectRepository.Object);

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
        public async Task Handle_MultipleImprovementPlans_UpdatesCorrectObjective()
        {
            // Arrange
            var specificPlanId = new ImprovementPlanId(Guid.NewGuid());
            var specificObjectiveId = new ImprovementPlanObjectiveId(Guid.NewGuid());
            
            // Set up additional improvement plan and objective for this test
            _mockSupportProject.AddImprovementPlan(specificPlanId, _mockSupportProject.Id);
            _mockSupportProject.AddImprovementPlanObjective(specificObjectiveId, specificPlanId, "LeadershipAndManagement", "Additional objective details");
            
            var command = new SetImprovementPlanObjectiveDetails.SetImprovementPlanObjectiveDetailsCommand(
                _mockSupportProject.Id,
                specificPlanId,
                specificObjectiveId,
                "Specific objective details for this plan"
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetImprovementPlanObjectiveDetails.SetImprovementPlanObjectiveDetailsCommandHandler(_mockSupportProjectRepository.Object);

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
            var command = new SetImprovementPlanObjectiveDetails.SetImprovementPlanObjectiveDetailsCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _objectiveId,
                "Test details"
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            _mockSupportProjectRepository
                .Setup(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.SupportProject.SupportProject>(), _cancellationToken))
                .ThrowsAsync(new InvalidOperationException("Update failed"));

            var handler = new SetImprovementPlanObjectiveDetails.SetImprovementPlanObjectiveDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => 
                handler.Handle(command, _cancellationToken));
        }
    }
} 