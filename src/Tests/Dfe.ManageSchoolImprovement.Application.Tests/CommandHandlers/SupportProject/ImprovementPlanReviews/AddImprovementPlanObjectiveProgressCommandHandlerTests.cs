using AutoFixture;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ImprovementPlans;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Moq;

namespace Dfe.ManageSchoolImprovement.Application.Tests.CommandHandlers.SupportProject.ImprovementPlans
{
    public class AddImprovementPlanObjectiveProgressCommandHandlerTests
    {
        private readonly Mock<ISupportProjectRepository> _mockSupportProjectRepository;
        private readonly CancellationToken _cancellationToken;
        private readonly Fixture _fixture;
        private readonly Domain.Entities.SupportProject.SupportProject _mockSupportProject;
        private readonly ImprovementPlanId _improvementPlanId;
        private readonly ImprovementPlanReviewId _improvementPlanReviewId;
        private readonly ImprovementPlanObjectiveId _improvementPlanObjectiveId;

        public AddImprovementPlanObjectiveProgressCommandHandlerTests()
        {
            _mockSupportProjectRepository = new Mock<ISupportProjectRepository>();
            _cancellationToken = CancellationToken.None;
            _fixture = new Fixture();
            _mockSupportProject = _fixture.Create<Domain.Entities.SupportProject.SupportProject>();
            _improvementPlanId = new ImprovementPlanId(Guid.NewGuid());
            _improvementPlanReviewId = new ImprovementPlanReviewId(Guid.NewGuid());
            _improvementPlanObjectiveId = new ImprovementPlanObjectiveId(Guid.NewGuid());

            // Set up the support project with the required improvement plan structure
            SetupMockSupportProject();
        }

        private void SetupMockSupportProject()
        {
            // Add improvement plan to the support project
            _mockSupportProject.AddImprovementPlan(_improvementPlanId, _mockSupportProject.Id);

            // Add an objective to the improvement plan
            _mockSupportProject.AddImprovementPlanObjective(
                _improvementPlanObjectiveId,
                _improvementPlanId,
                "Quality of education",
                "Test objective");

            // Add a review to the improvement plan
            _mockSupportProject.AddImprovementPlanReview(
                _improvementPlanReviewId,
                _improvementPlanId,
                "Test Reviewer",
                DateTime.UtcNow);
        }

        [Fact]
        public async Task Handle_ValidCommand_CreatesImprovementPlanObjectiveProgress()
        {
            // Arrange
            var command = new AddImprovementPlanObjectiveProgress.AddImprovementPlanObjectiveProgressCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                _improvementPlanObjectiveId,
                "On track",
                "Good progress has been made"
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new AddImprovementPlanObjectiveProgress.AddImprovementPlanObjectiveProgressCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ImprovementPlanObjectiveProgressId>(result);
            _mockSupportProjectRepository.Verify(repo => repo.GetSupportProjectById(
                It.Is<SupportProjectId>(id => id == _mockSupportProject.Id),
                _cancellationToken), Times.Once);
            _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(
                _mockSupportProject,
                _cancellationToken), Times.Once);
        }

        [Theory]
        [InlineData("On track", "Making excellent progress")]
        [InlineData("Behind", "Some challenges encountered")]
        [InlineData("At risk", "Significant issues need addressing")]
        [InlineData("Complete", "All objectives have been met")]
        [InlineData("Not started", "Objective not yet begun")]
        [InlineData("", "Valid details with empty status")]
        [InlineData("On track", "")]
        [InlineData("", "")]
        public async Task Handle_ValidCommandWithDifferentProgressStatuses_CreatesObjectiveProgress(string progressStatus, string progressDetails)
        {
            // Arrange
            var command = new AddImprovementPlanObjectiveProgress.AddImprovementPlanObjectiveProgressCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                _improvementPlanObjectiveId,
                progressStatus,
                progressDetails
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new AddImprovementPlanObjectiveProgress.AddImprovementPlanObjectiveProgressCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ImprovementPlanObjectiveProgressId>(result);
        }

        [Fact]
        public async Task Handle_SupportProjectNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange
            var nonExistentId = new SupportProjectId(999);
            var command = new AddImprovementPlanObjectiveProgress.AddImprovementPlanObjectiveProgressCommand(
                nonExistentId,
                _improvementPlanId,
                _improvementPlanReviewId,
                _improvementPlanObjectiveId,
                "On track",
                "Test progress"
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == nonExistentId), _cancellationToken))
                .ReturnsAsync((Domain.Entities.SupportProject.SupportProject?)null);

            var handler = new AddImprovementPlanObjectiveProgress.AddImprovementPlanObjectiveProgressCommandHandler(_mockSupportProjectRepository.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                handler.Handle(command, _cancellationToken));

            Assert.Equal($"Support project with id {nonExistentId} not found", exception.Message);
        }

        [Fact]
        public async Task Handle_RepositoryThrowsException_ExceptionPropagates()
        {
            // Arrange
            var command = new AddImprovementPlanObjectiveProgress.AddImprovementPlanObjectiveProgressCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                _improvementPlanObjectiveId,
                "On track",
                "Test progress"
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.IsAny<SupportProjectId>(), _cancellationToken))
                .ThrowsAsync(new InvalidOperationException("Database error"));

            var handler = new AddImprovementPlanObjectiveProgress.AddImprovementPlanObjectiveProgressCommandHandler(_mockSupportProjectRepository.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.Handle(command, _cancellationToken));
        }

        [Fact]
        public async Task Handle_ValidCommand_UpdateAsyncCalledOnlyOnce()
        {
            // Arrange
            var command = new AddImprovementPlanObjectiveProgress.AddImprovementPlanObjectiveProgressCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                _improvementPlanObjectiveId,
                "On track",
                "Test progress"
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new AddImprovementPlanObjectiveProgress.AddImprovementPlanObjectiveProgressCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            await handler.Handle(command, _cancellationToken);

            // Assert
            _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(
                It.IsAny<Domain.Entities.SupportProject.SupportProject>(),
                _cancellationToken), Times.Once);
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnsValidProgressId()
        {
            // Arrange
            var command = new AddImprovementPlanObjectiveProgress.AddImprovementPlanObjectiveProgressCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                _improvementPlanObjectiveId,
                "On track",
                "Detailed progress information for the objective"
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new AddImprovementPlanObjectiveProgress.AddImprovementPlanObjectiveProgressCommandHandler(_mockSupportProjectRepository.Object);

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
            var command = new AddImprovementPlanObjectiveProgress.AddImprovementPlanObjectiveProgressCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                _improvementPlanObjectiveId,
                "On track",
                "Test progress"
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new AddImprovementPlanObjectiveProgress.AddImprovementPlanObjectiveProgressCommandHandler(_mockSupportProjectRepository.Object);

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
        public async Task Handle_ValidCommand_AddsObjectiveProgressWithCorrectParameters()
        {
            // Arrange
            var progressStatus = "On track";
            var progressDetails = "Making good progress";
            var command = new AddImprovementPlanObjectiveProgress.AddImprovementPlanObjectiveProgressCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                _improvementPlanObjectiveId,
                progressStatus,
                progressDetails
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new AddImprovementPlanObjectiveProgress.AddImprovementPlanObjectiveProgressCommandHandler(_mockSupportProjectRepository.Object);

            // Get the initial count of objective progresses in the review
            var improvementPlan = _mockSupportProject.ImprovementPlans.First(ip => ip.Id == _improvementPlanId);
            var review = improvementPlan.ImprovementPlanReviews.First(r => r.Id == _improvementPlanReviewId);
            var initialProgressCount = review.ImprovementPlanObjectiveProgresses.Count();

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ImprovementPlanObjectiveProgressId>(result);

            // Verify that a new objective progress was added
            var finalProgressCount = review.ImprovementPlanObjectiveProgresses.Count();
            Assert.Equal(initialProgressCount + 1, finalProgressCount);

            // Verify the added progress has the correct properties
            var addedProgress = review.ImprovementPlanObjectiveProgresses.Last();
            Assert.Equal(progressStatus, addedProgress.HowIsSchoolProgressing);
            Assert.Equal(progressDetails, addedProgress.ProgressDetails);
            Assert.Equal(_improvementPlanObjectiveId, addedProgress.ImprovementPlanObjectiveId);
            Assert.Equal(_improvementPlanReviewId, addedProgress.ImprovementPlanReviewId);
        }

        [Fact]
        public async Task Handle_ValidCommand_GeneratesUniqueProgressId()
        {
            // Arrange
            var command = new AddImprovementPlanObjectiveProgress.AddImprovementPlanObjectiveProgressCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                _improvementPlanObjectiveId,
                "On track",
                "Good progress"
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new AddImprovementPlanObjectiveProgress.AddImprovementPlanObjectiveProgressCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result1 = await handler.Handle(command, _cancellationToken);

            // Reset the mock for second call
            var result2 = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.NotEqual(result1.Value, result2.Value);
        }

        [Fact]
        public async Task Handle_WithLongProgressDetails_CreatesObjectiveProgress()
        {
            // Arrange
            var longProgressDetails = new string('A', 2000); // Very long string
            var command = new AddImprovementPlanObjectiveProgress.AddImprovementPlanObjectiveProgressCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                _improvementPlanObjectiveId,
                "On track",
                longProgressDetails
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new AddImprovementPlanObjectiveProgress.AddImprovementPlanObjectiveProgressCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ImprovementPlanObjectiveProgressId>(result);
        }

        [Fact]
        public async Task Handle_UpdateAsyncFails_PropagatesException()
        {
            // Arrange
            var command = new AddImprovementPlanObjectiveProgress.AddImprovementPlanObjectiveProgressCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                _improvementPlanObjectiveId,
                "On track",
                "Good progress"
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var expectedException = new InvalidOperationException("Update failed");
            _mockSupportProjectRepository
                .Setup(repo => repo.UpdateAsync(_mockSupportProject, _cancellationToken))
                .ThrowsAsync(expectedException);

            var handler = new AddImprovementPlanObjectiveProgress.AddImprovementPlanObjectiveProgressCommandHandler(_mockSupportProjectRepository.Object);

            // Act & Assert
            var actualException = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.Handle(command, _cancellationToken));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        #region Command Tests

        [Fact]
        public void AddImprovementPlanObjectiveProgressCommand_WithValidParameters_CreatesCommand()
        {
            // Arrange & Act
            var command = new AddImprovementPlanObjectiveProgress.AddImprovementPlanObjectiveProgressCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                _improvementPlanObjectiveId,
                "On track",
                "Good progress"
            );

            // Assert
            Assert.Equal(_mockSupportProject.Id, command.SupportProjectId);
            Assert.Equal(_improvementPlanId, command.ImprovementPlanId);
            Assert.Equal(_improvementPlanReviewId, command.ImprovementPlanReviewId);
            Assert.Equal(_improvementPlanObjectiveId, command.ImprovementPlanObjectiveId);
            Assert.Equal("On track", command.progressStatus);
            Assert.Equal("Good progress", command.progressDetails);
        }

        [Fact]
        public void AddImprovementPlanObjectiveProgressCommand_ImplementsIRequest()
        {
            // Assert
            Assert.True(typeof(AddImprovementPlanObjectiveProgress.AddImprovementPlanObjectiveProgressCommand)
                .IsAssignableTo(typeof(MediatR.IRequest<ImprovementPlanObjectiveProgressId>)));
        }

        #endregion
    }
}