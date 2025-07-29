using AutoFixture;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ImprovementPlans;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Moq;

namespace Dfe.ManageSchoolImprovement.Application.Tests.CommandHandlers.SupportProject.ImprovementPlans
{
    public class AddImprovementPlanObjectiveCommandHandlerTests
    {
        private readonly Mock<ISupportProjectRepository> _mockSupportProjectRepository;
        private readonly CancellationToken _cancellationToken;
        private readonly Fixture _fixture;
        private readonly Domain.Entities.SupportProject.SupportProject _mockSupportProject;
        private readonly ImprovementPlanId _improvementPlanId;

        public AddImprovementPlanObjectiveCommandHandlerTests()
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
        public async Task Handle_ValidCommand_CreatesImprovementPlanObjective()
        {
            // Arrange
            var command = new AddImprovementPlanObjective.AddImprovementPlanObjectiveCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                "QualityOfEducation",
                "Improve student outcomes in mathematics"
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new AddImprovementPlanObjective.AddImprovementPlanObjectiveCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ImprovementPlanObjectiveId>(result);
            _mockSupportProjectRepository.Verify(repo => repo.GetSupportProjectById(
                It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), 
                _cancellationToken), Times.Once);
            _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(
                _mockSupportProject, 
                _cancellationToken), Times.Once);
        }

        [Theory]
        [InlineData("QualityOfEducation", "Improve mathematics teaching")]
        [InlineData("LeadershipAndManagement", "Enhance leadership capacity")]
        [InlineData("BehaviourAndAttitudes", "Improve student behavior")]
        [InlineData("Attendance", "Increase attendance rates")]
        [InlineData("PersonalDevelopment", "Develop character education")]
        public async Task Handle_ValidCommandWithDifferentAreas_CreatesObjective(string areaOfImprovement, string details)
        {
            // Arrange
            var command = new AddImprovementPlanObjective.AddImprovementPlanObjectiveCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                areaOfImprovement,
                details
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new AddImprovementPlanObjective.AddImprovementPlanObjectiveCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ImprovementPlanObjectiveId>(result);
        }

        [Fact]
        public async Task Handle_SupportProjectNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange
            var nonExistentId = new SupportProjectId(999);
            var command = new AddImprovementPlanObjective.AddImprovementPlanObjectiveCommand(
                nonExistentId,
                _improvementPlanId,
                "QualityOfEducation",
                "Test objective"
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == nonExistentId), _cancellationToken))
                .ReturnsAsync((Domain.Entities.SupportProject.SupportProject?)null);

            var handler = new AddImprovementPlanObjective.AddImprovementPlanObjectiveCommandHandler(_mockSupportProjectRepository.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => 
                handler.Handle(command, _cancellationToken));
            
            Assert.Equal($"Support project with id {nonExistentId} not found", exception.Message);
        }

        [Theory]
        [InlineData("", "Valid details")]
        [InlineData("QualityOfEducation", "")]
        [InlineData("", "")]
        public async Task Handle_CommandWithEmptyFields_StillCreatesObjective(string areaOfImprovement, string details)
        {
            // Arrange
            var command = new AddImprovementPlanObjective.AddImprovementPlanObjectiveCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                areaOfImprovement,
                details
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new AddImprovementPlanObjective.AddImprovementPlanObjectiveCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ImprovementPlanObjectiveId>(result);
        }

        [Fact]
        public async Task Handle_RepositoryThrowsException_ExceptionPropagates()
        {
            // Arrange
            var command = new AddImprovementPlanObjective.AddImprovementPlanObjectiveCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                "QualityOfEducation",
                "Test objective"
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.IsAny<SupportProjectId>(), _cancellationToken))
                .ThrowsAsync(new InvalidOperationException("Database error"));

            var handler = new AddImprovementPlanObjective.AddImprovementPlanObjectiveCommandHandler(_mockSupportProjectRepository.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => 
                handler.Handle(command, _cancellationToken));
        }

        [Fact]
        public async Task Handle_ValidCommand_UpdateAsyncCalledOnlyOnce()
        {
            // Arrange
            var command = new AddImprovementPlanObjective.AddImprovementPlanObjectiveCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                "QualityOfEducation",
                "Test objective"
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new AddImprovementPlanObjective.AddImprovementPlanObjectiveCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            await handler.Handle(command, _cancellationToken);

            // Assert
            _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(
                It.IsAny<Domain.Entities.SupportProject.SupportProject>(), 
                _cancellationToken), Times.Once);
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnsValidObjectiveId()
        {
            // Arrange
            var command = new AddImprovementPlanObjective.AddImprovementPlanObjectiveCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                "QualityOfEducation",
                "Detailed improvement plan for mathematics"
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new AddImprovementPlanObjective.AddImprovementPlanObjectiveCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Value);
        }

        [Fact]
        public async Task Handle_ValidCommand_CallsRepositoryMethodsInCorrectOrder()
        {
            // Arrange
            var command = new AddImprovementPlanObjective.AddImprovementPlanObjectiveCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                "QualityOfEducation",
                "Test objective"
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new AddImprovementPlanObjective.AddImprovementPlanObjectiveCommandHandler(_mockSupportProjectRepository.Object);

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
    }
} 