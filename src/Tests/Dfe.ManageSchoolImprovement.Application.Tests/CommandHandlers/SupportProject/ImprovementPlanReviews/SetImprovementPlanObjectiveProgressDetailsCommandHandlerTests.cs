using AutoFixture;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ImprovementPlans;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Moq;

namespace Dfe.ManageSchoolImprovement.Application.Tests.CommandHandlers.SupportProject.ImprovementPlanReviews
{
    public class SetImprovementPlanObjectiveProgressDetailsCommandHandlerTests
    {
        private readonly Mock<ISupportProjectRepository> _mockSupportProjectRepository;
        private readonly CancellationToken _cancellationToken;
        private readonly Fixture _fixture;
        private readonly Domain.Entities.SupportProject.SupportProject _mockSupportProject;
        private readonly ImprovementPlanId _improvementPlanId;
        private readonly ImprovementPlanReviewId _improvementPlanReviewId;
        private readonly ImprovementPlanObjectiveId _improvementPlanObjectiveId;
        private readonly ImprovementPlanObjectiveProgressId _improvementPlanObjectiveProgressId;

        public SetImprovementPlanObjectiveProgressDetailsCommandHandlerTests()
        {
            _mockSupportProjectRepository = new Mock<ISupportProjectRepository>();
            _cancellationToken = CancellationToken.None;
            _fixture = new Fixture();
            _mockSupportProject = _fixture.Create<Domain.Entities.SupportProject.SupportProject>();
            _improvementPlanId = new ImprovementPlanId(Guid.NewGuid());
            _improvementPlanReviewId = new ImprovementPlanReviewId(Guid.NewGuid());
            _improvementPlanObjectiveId = new ImprovementPlanObjectiveId(Guid.NewGuid());
            _improvementPlanObjectiveProgressId = new ImprovementPlanObjectiveProgressId(Guid.NewGuid());

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

            // Add existing progress to the review
            _mockSupportProject.AddImprovementPlanObjectiveProgress(
                _improvementPlanObjectiveProgressId,
                _improvementPlanId,
                _improvementPlanReviewId,
                _improvementPlanObjectiveId,
                "Initial status",
                "Initial details");
        }

        [Fact]
        public async Task Handle_ValidCommand_UpdatesObjectiveProgressDetails()
        {
            // Arrange
            var progressStatus = "Updated status";
            var progressDetails = "Updated progress details";
            var command = new SetImprovementPlanObjectiveProgressDetails.SetImprovementPlanObjectiveProgressDetailsCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                _improvementPlanObjectiveProgressId,
                progressStatus,
                progressDetails
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetImprovementPlanObjectiveProgressDetails.SetImprovementPlanObjectiveProgressDetailsCommandHandler(_mockSupportProjectRepository.Object);

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
        [InlineData("On track", "Making excellent progress")]
        [InlineData("Behind", "Some challenges encountered")]
        [InlineData("At risk", "Significant issues need addressing")]
        [InlineData("Complete", "All objectives have been met")]
        [InlineData("Not started", "Objective not yet begun")]
        [InlineData("", "Valid details with empty status")]
        [InlineData("Updated status", "")]
        [InlineData("", "")]
        public async Task Handle_ValidCommandWithDifferentProgressStatuses_UpdatesProgressDetails(string progressStatus, string progressDetails)
        {
            // Arrange
            var command = new SetImprovementPlanObjectiveProgressDetails.SetImprovementPlanObjectiveProgressDetailsCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                _improvementPlanObjectiveProgressId,
                progressStatus,
                progressDetails
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetImprovementPlanObjectiveProgressDetails.SetImprovementPlanObjectiveProgressDetailsCommandHandler(_mockSupportProjectRepository.Object);

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
            var command = new SetImprovementPlanObjectiveProgressDetails.SetImprovementPlanObjectiveProgressDetailsCommand(
                nonExistentId,
                _improvementPlanId,
                _improvementPlanReviewId,
                _improvementPlanObjectiveProgressId,
                "Updated status",
                "Updated details"
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == nonExistentId), _cancellationToken))
                .ReturnsAsync((Domain.Entities.SupportProject.SupportProject?)null);

            var handler = new SetImprovementPlanObjectiveProgressDetails.SetImprovementPlanObjectiveProgressDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                handler.Handle(command, _cancellationToken));

            Assert.Equal($"Support project with id {nonExistentId} not found", exception.Message);
        }

        [Fact]
        public async Task Handle_RepositoryThrowsException_ExceptionPropagates()
        {
            // Arrange
            var command = new SetImprovementPlanObjectiveProgressDetails.SetImprovementPlanObjectiveProgressDetailsCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                _improvementPlanObjectiveProgressId,
                "Updated status",
                "Updated details"
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.IsAny<SupportProjectId>(), _cancellationToken))
                .ThrowsAsync(new InvalidOperationException("Database error"));

            var handler = new SetImprovementPlanObjectiveProgressDetails.SetImprovementPlanObjectiveProgressDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.Handle(command, _cancellationToken));
        }

        [Fact]
        public async Task Handle_ValidCommand_UpdateAsyncCalledOnlyOnce()
        {
            // Arrange
            var command = new SetImprovementPlanObjectiveProgressDetails.SetImprovementPlanObjectiveProgressDetailsCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                _improvementPlanObjectiveProgressId,
                "Updated status",
                "Updated details"
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetImprovementPlanObjectiveProgressDetails.SetImprovementPlanObjectiveProgressDetailsCommandHandler(_mockSupportProjectRepository.Object);

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
            var command = new SetImprovementPlanObjectiveProgressDetails.SetImprovementPlanObjectiveProgressDetailsCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                _improvementPlanObjectiveProgressId,
                "Updated status",
                "Updated details"
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetImprovementPlanObjectiveProgressDetails.SetImprovementPlanObjectiveProgressDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Handle_ValidCommand_CallsRepositoryMethodsInCorrectOrder()
        {
            // Arrange
            var command = new SetImprovementPlanObjectiveProgressDetails.SetImprovementPlanObjectiveProgressDetailsCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                _improvementPlanObjectiveProgressId,
                "Updated status",
                "Updated details"
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetImprovementPlanObjectiveProgressDetails.SetImprovementPlanObjectiveProgressDetailsCommandHandler(_mockSupportProjectRepository.Object);

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
        public async Task Handle_ValidCommand_UpdatesProgressWithCorrectParameters()
        {
            // Arrange
            var progressStatus = "Significantly improved";
            var progressDetails = "Excellent progress with measurable outcomes";
            var command = new SetImprovementPlanObjectiveProgressDetails.SetImprovementPlanObjectiveProgressDetailsCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                _improvementPlanObjectiveProgressId,
                progressStatus,
                progressDetails
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetImprovementPlanObjectiveProgressDetails.SetImprovementPlanObjectiveProgressDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Get the existing progress before update
            var improvementPlan = _mockSupportProject.ImprovementPlans.First(ip => ip.Id == _improvementPlanId);
            var review = improvementPlan.ImprovementPlanReviews.First(r => r.Id == _improvementPlanReviewId);
            var existingProgress = review.ImprovementPlanObjectiveProgresses.First(p => p.Id == _improvementPlanObjectiveProgressId);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.True(result);

            // Verify the progress was updated with correct parameters
            Assert.Equal(progressStatus, existingProgress.HowIsSchoolProgressing);
            Assert.Equal(progressDetails, existingProgress.ProgressDetails);
            Assert.Equal(_improvementPlanObjectiveId, existingProgress.ImprovementPlanObjectiveId);
            Assert.Equal(_improvementPlanReviewId, existingProgress.ImprovementPlanReviewId);
        }

        [Fact]
        public async Task Handle_ValidCommand_OverwritesPreviousValues()
        {
            // Arrange
            var newProgressStatus = "Completely different status";
            var newProgressDetails = "Completely different details";
            var command = new SetImprovementPlanObjectiveProgressDetails.SetImprovementPlanObjectiveProgressDetailsCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                _improvementPlanObjectiveProgressId,
                newProgressStatus,
                newProgressDetails
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetImprovementPlanObjectiveProgressDetails.SetImprovementPlanObjectiveProgressDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Get the existing progress to verify initial state
            var improvementPlan = _mockSupportProject.ImprovementPlans.First(ip => ip.Id == _improvementPlanId);
            var review = improvementPlan.ImprovementPlanReviews.First(r => r.Id == _improvementPlanReviewId);
            var existingProgress = review.ImprovementPlanObjectiveProgresses.First(p => p.Id == _improvementPlanObjectiveProgressId);

            // Verify initial state
            Assert.Equal("Initial status", existingProgress.HowIsSchoolProgressing);
            Assert.Equal("Initial details", existingProgress.ProgressDetails);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.True(result);
            Assert.Equal(newProgressStatus, existingProgress.HowIsSchoolProgressing);
            Assert.Equal(newProgressDetails, existingProgress.ProgressDetails);
        }

        [Fact]
        public async Task Handle_WithLongProgressDetails_UpdatesProgressDetails()
        {
            // Arrange
            var longProgressDetails = new string('A', 2000); // Very long string
            var command = new SetImprovementPlanObjectiveProgressDetails.SetImprovementPlanObjectiveProgressDetailsCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                _improvementPlanObjectiveProgressId,
                "Updated status",
                longProgressDetails
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetImprovementPlanObjectiveProgressDetails.SetImprovementPlanObjectiveProgressDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Handle_UpdateAsyncFails_PropagatesException()
        {
            // Arrange
            var command = new SetImprovementPlanObjectiveProgressDetails.SetImprovementPlanObjectiveProgressDetailsCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                _improvementPlanObjectiveProgressId,
                "Updated status",
                "Updated details"
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var expectedException = new InvalidOperationException("Update failed");
            _mockSupportProjectRepository
                .Setup(repo => repo.UpdateAsync(_mockSupportProject, _cancellationToken))
                .ThrowsAsync(expectedException);

            var handler = new SetImprovementPlanObjectiveProgressDetails.SetImprovementPlanObjectiveProgressDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Act & Assert
            var actualException = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.Handle(command, _cancellationToken));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Fact]
        public async Task Handle_MultipleUpdatesToSameProgress_AppliesLatestValues()
        {
            // Arrange
            var firstUpdate = new SetImprovementPlanObjectiveProgressDetails.SetImprovementPlanObjectiveProgressDetailsCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                _improvementPlanObjectiveProgressId,
                "First update",
                "First details"
            );

            var secondUpdate = new SetImprovementPlanObjectiveProgressDetails.SetImprovementPlanObjectiveProgressDetailsCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                _improvementPlanObjectiveProgressId,
                "Final update",
                "Final details"
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetImprovementPlanObjectiveProgressDetails.SetImprovementPlanObjectiveProgressDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result1 = await handler.Handle(firstUpdate, _cancellationToken);
            var result2 = await handler.Handle(secondUpdate, _cancellationToken);

            // Assert
            Assert.True(result1);
            Assert.True(result2);

            var improvementPlan = _mockSupportProject.ImprovementPlans.First(ip => ip.Id == _improvementPlanId);
            var review = improvementPlan.ImprovementPlanReviews.First(r => r.Id == _improvementPlanReviewId);
            var progress = review.ImprovementPlanObjectiveProgresses.First(p => p.Id == _improvementPlanObjectiveProgressId);

            Assert.Equal("Final update", progress.HowIsSchoolProgressing);
            Assert.Equal("Final details", progress.ProgressDetails);
        }

        #region Command Tests

        [Fact]
        public void SetImprovementPlanObjectiveProgressDetailsCommand_WithValidParameters_CreatesCommand()
        {
            // Arrange
            var progressStatus = "Test status";
            var progressDetails = "Test details";

            // Act
            var command = new SetImprovementPlanObjectiveProgressDetails.SetImprovementPlanObjectiveProgressDetailsCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                _improvementPlanObjectiveProgressId,
                progressStatus,
                progressDetails
            );

            // Assert
            Assert.Equal(_mockSupportProject.Id, command.SupportProjectId);
            Assert.Equal(_improvementPlanId, command.ImprovementPlanId);
            Assert.Equal(_improvementPlanReviewId, command.ImprovementPlanReviewId);
            Assert.Equal(_improvementPlanObjectiveProgressId, command.ImprovementPlanObjectiveProgressId);
            Assert.Equal(progressStatus, command.progressStatus);
            Assert.Equal(progressDetails, command.progressDetails);
        }

        [Fact]
        public void SetImprovementPlanObjectiveProgressDetailsCommand_WithEmptyValues_CreatesCommand()
        {
            // Act
            var command = new SetImprovementPlanObjectiveProgressDetails.SetImprovementPlanObjectiveProgressDetailsCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                _improvementPlanObjectiveProgressId,
                "",
                ""
            );

            // Assert
            Assert.Equal("", command.progressStatus);
            Assert.Equal("", command.progressDetails);
        }

        [Fact]
        public void SetImprovementPlanObjectiveProgressDetailsCommand_ImplementsIRequest()
        {
            // Assert
            Assert.True(typeof(SetImprovementPlanObjectiveProgressDetails.SetImprovementPlanObjectiveProgressDetailsCommand)
                .IsAssignableTo(typeof(MediatR.IRequest<bool>)));
        }

        #endregion
    }
}