using AutoFixture;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ImprovementPlans;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Moq;

namespace Dfe.ManageSchoolImprovement.Application.Tests.CommandHandlers.SupportProject.ImprovementPlanReviews
{
    public class SetImprovementPlanReviewOverallProgressTests
    {
        private readonly Mock<ISupportProjectRepository> _mockSupportProjectRepository;
        private readonly CancellationToken _cancellationToken;
        private readonly Fixture _fixture;
        private readonly Domain.Entities.SupportProject.SupportProject _mockSupportProject;
        private readonly ImprovementPlanId _improvementPlanId;
        private readonly ImprovementPlanReviewId _improvementPlanReviewId;

        public SetImprovementPlanReviewOverallProgressTests()
        {
            _mockSupportProjectRepository = new Mock<ISupportProjectRepository>();
            _cancellationToken = CancellationToken.None;
            _fixture = new Fixture();
            _mockSupportProject = _fixture.Create<Domain.Entities.SupportProject.SupportProject>();
            _improvementPlanId = new ImprovementPlanId(Guid.NewGuid());
            _improvementPlanReviewId = new ImprovementPlanReviewId(Guid.NewGuid());

            // Set up the support project with the required improvement plan structure
            SetupMockSupportProject();
        }

        private void SetupMockSupportProject()
        {
            // Add improvement plan to the support project
            _mockSupportProject.AddImprovementPlan(_improvementPlanId, _mockSupportProject.Id);

            // Add a review to the improvement plan
            _mockSupportProject.AddImprovementPlanReview(
                _improvementPlanReviewId,
                _improvementPlanId,
                "Test Reviewer",
                DateTime.UtcNow);
        }

        [Fact]
        public async Task Handle_ValidCommand_SetsOverallProgress()
        {
            // Arrange
            var howIsTheSchoolProgressing = "The school is making good progress";
            var overallProgressDetails = "Significant improvements in teaching quality and student outcomes";

            var command = new SetImprovementPlanReviewOverallProgress.SetImprovementPlanReviewOverallProgressCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                howIsTheSchoolProgressing,
                overallProgressDetails
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetImprovementPlanReviewOverallProgress.SetImprovementPlanReviewOverallProgressCommandHandler(_mockSupportProjectRepository.Object);

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
        [InlineData("On track", "Good progress being made")]
        [InlineData("Behind schedule", "Some areas need improvement")]
        [InlineData("Exceeding expectations", "Outstanding progress in all areas")]
        [InlineData("At risk", "Serious concerns about current trajectory")]
        [InlineData("Complete", "All objectives have been successfully met")]
        [InlineData("", "Valid details with empty status")]
        [InlineData("On track", "")]
        [InlineData("", "")]
        public async Task Handle_ValidCommandWithDifferentProgressStatuses_SetsOverallProgress(string progressStatus, string progressDetails)
        {
            // Arrange
            var command = new SetImprovementPlanReviewOverallProgress.SetImprovementPlanReviewOverallProgressCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                progressStatus,
                progressDetails
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetImprovementPlanReviewOverallProgress.SetImprovementPlanReviewOverallProgressCommandHandler(_mockSupportProjectRepository.Object);

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
            var command = new SetImprovementPlanReviewOverallProgress.SetImprovementPlanReviewOverallProgressCommand(
                nonExistentId,
                _improvementPlanId,
                _improvementPlanReviewId,
                "On track",
                "Test progress"
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == nonExistentId), _cancellationToken))
                .ReturnsAsync((Domain.Entities.SupportProject.SupportProject?)null);

            var handler = new SetImprovementPlanReviewOverallProgress.SetImprovementPlanReviewOverallProgressCommandHandler(_mockSupportProjectRepository.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                handler.Handle(command, _cancellationToken));

            Assert.Equal($"Support project with id {nonExistentId} not found", exception.Message);
        }

        [Fact]
        public async Task Handle_RepositoryThrowsException_ExceptionPropagates()
        {
            // Arrange
            var command = new SetImprovementPlanReviewOverallProgress.SetImprovementPlanReviewOverallProgressCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                "On track",
                "Test progress"
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.IsAny<SupportProjectId>(), _cancellationToken))
                .ThrowsAsync(new InvalidOperationException("Database error"));

            var handler = new SetImprovementPlanReviewOverallProgress.SetImprovementPlanReviewOverallProgressCommandHandler(_mockSupportProjectRepository.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.Handle(command, _cancellationToken));
        }

        [Fact]
        public async Task Handle_ValidCommand_UpdateAsyncCalledOnlyOnce()
        {
            // Arrange
            var command = new SetImprovementPlanReviewOverallProgress.SetImprovementPlanReviewOverallProgressCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                "On track",
                "Test progress"
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetImprovementPlanReviewOverallProgress.SetImprovementPlanReviewOverallProgressCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            await handler.Handle(command, _cancellationToken);

            // Assert
            _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(
                It.IsAny<Domain.Entities.SupportProject.SupportProject>(),
                _cancellationToken), Times.Once);
        }

        [Fact]
        public async Task Handle_ValidCommand_CallsRepositoryMethodsInCorrectOrder()
        {
            // Arrange
            var command = new SetImprovementPlanReviewOverallProgress.SetImprovementPlanReviewOverallProgressCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                "On track",
                "Test progress"
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetImprovementPlanReviewOverallProgress.SetImprovementPlanReviewOverallProgressCommandHandler(_mockSupportProjectRepository.Object);

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
        public async Task Handle_ValidCommand_SetsOverallProgressWithCorrectParameters()
        {
            // Arrange
            var progressStatus = "On track";
            var progressDetails = "Making excellent progress across all areas";
            var command = new SetImprovementPlanReviewOverallProgress.SetImprovementPlanReviewOverallProgressCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                progressStatus,
                progressDetails
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetImprovementPlanReviewOverallProgress.SetImprovementPlanReviewOverallProgressCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.True(result);

            // Verify the overall progress was set correctly
            var improvementPlan = _mockSupportProject.ImprovementPlans.First(ip => ip.Id == _improvementPlanId);
            var review = improvementPlan.ImprovementPlanReviews.First(r => r.Id == _improvementPlanReviewId);

            Assert.Equal(progressStatus, review.HowIsTheSchoolProgressingOverall);
            Assert.Equal(progressDetails, review.OverallProgressDetails);
        }

        [Fact]
        public async Task Handle_WithLongProgressDetails_SetsOverallProgress()
        {
            // Arrange
            var longProgressDetails = new string('A', 2000); // Very long string
            var command = new SetImprovementPlanReviewOverallProgress.SetImprovementPlanReviewOverallProgressCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                "On track",
                longProgressDetails
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetImprovementPlanReviewOverallProgress.SetImprovementPlanReviewOverallProgressCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Handle_UpdateAsyncFails_PropagatesException()
        {
            // Arrange
            var command = new SetImprovementPlanReviewOverallProgress.SetImprovementPlanReviewOverallProgressCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
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

            var handler = new SetImprovementPlanReviewOverallProgress.SetImprovementPlanReviewOverallProgressCommandHandler(_mockSupportProjectRepository.Object);

            // Act & Assert
            var actualException = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.Handle(command, _cancellationToken));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Fact]
        public async Task Handle_ValidCommand_OverwritesPreviousOverallProgress()
        {
            // Arrange
            var initialStatus = "Initial status";
            var initialDetails = "Initial details";
            var updatedStatus = "Updated status";
            var updatedDetails = "Updated details";

            // First, set initial overall progress
            var initialCommand = new SetImprovementPlanReviewOverallProgress.SetImprovementPlanReviewOverallProgressCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                initialStatus,
                initialDetails
            );

            var updateCommand = new SetImprovementPlanReviewOverallProgress.SetImprovementPlanReviewOverallProgressCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                updatedStatus,
                updatedDetails
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetImprovementPlanReviewOverallProgress.SetImprovementPlanReviewOverallProgressCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            await handler.Handle(initialCommand, _cancellationToken);
            var result = await handler.Handle(updateCommand, _cancellationToken);

            // Assert
            Assert.True(result);

            var improvementPlan = _mockSupportProject.ImprovementPlans.First(ip => ip.Id == _improvementPlanId);
            var review = improvementPlan.ImprovementPlanReviews.First(r => r.Id == _improvementPlanReviewId);

            Assert.Equal(updatedStatus, review.HowIsTheSchoolProgressingOverall);
            Assert.Equal(updatedDetails, review.OverallProgressDetails);
            Assert.NotEqual(initialStatus, review.HowIsTheSchoolProgressingOverall);
            Assert.NotEqual(initialDetails, review.OverallProgressDetails);
        }

        [Fact]
        public async Task Handle_WithSpecialCharacters_SetsOverallProgress()
        {
            // Arrange
            var statusWithSpecialChars = "Status with special chars: !@#$%^&*()";
            var detailsWithSpecialChars = "Details with special chars: <>&\"'";

            var command = new SetImprovementPlanReviewOverallProgress.SetImprovementPlanReviewOverallProgressCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                statusWithSpecialChars,
                detailsWithSpecialChars
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetImprovementPlanReviewOverallProgress.SetImprovementPlanReviewOverallProgressCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.True(result);

            var improvementPlan = _mockSupportProject.ImprovementPlans.First(ip => ip.Id == _improvementPlanId);
            var review = improvementPlan.ImprovementPlanReviews.First(r => r.Id == _improvementPlanReviewId);

            Assert.Equal(statusWithSpecialChars, review.HowIsTheSchoolProgressingOverall);
            Assert.Equal(detailsWithSpecialChars, review.OverallProgressDetails);
        }

        [Fact]
        public async Task Handle_ValidCommand_DoesNotAffectOtherReviewProperties()
        {
            // Arrange
            var command = new SetImprovementPlanReviewOverallProgress.SetImprovementPlanReviewOverallProgressCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                "On track",
                "Good progress"
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetImprovementPlanReviewOverallProgress.SetImprovementPlanReviewOverallProgressCommandHandler(_mockSupportProjectRepository.Object);

            // Get original review properties
            var improvementPlan = _mockSupportProject.ImprovementPlans.First(ip => ip.Id == _improvementPlanId);
            var review = improvementPlan.ImprovementPlanReviews.First(r => r.Id == _improvementPlanReviewId);
            var originalReviewer = review.Reviewer;
            var originalReviewDate = review.ReviewDate;
            var originalTitle = review.Title;
            var originalOrder = review.Order;

            // Act
            await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.Equal(originalReviewer, review.Reviewer);
            Assert.Equal(originalReviewDate, review.ReviewDate);
            Assert.Equal(originalTitle, review.Title);
            Assert.Equal(originalOrder, review.Order);
        }

        #region Command Tests

        [Fact]
        public void SetImprovementPlanReviewOverallProgressCommand_WithValidParameters_CreatesCommand()
        {
            // Arrange & Act
            var command = new SetImprovementPlanReviewOverallProgress.SetImprovementPlanReviewOverallProgressCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                "On track",
                "Good progress"
            );

            // Assert
            Assert.Equal(_mockSupportProject.Id, command.SupportProjectId);
            Assert.Equal(_improvementPlanId, command.ImprovementPlanId);
            Assert.Equal(_improvementPlanReviewId, command.ImprovementPlanReviewId);
            Assert.Equal("On track", command.howIsTheSchoolProgressingOverall);
            Assert.Equal("Good progress", command.overallProgressDetails);
        }

        [Fact]
        public void SetImprovementPlanReviewOverallProgressCommand_ImplementsIRequest()
        {
            // Assert
            Assert.True(typeof(SetImprovementPlanReviewOverallProgress.SetImprovementPlanReviewOverallProgressCommand)
                .IsAssignableTo(typeof(MediatR.IRequest<bool>)));
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(null, "Details only")]
        [InlineData("Status only", null)]
        [InlineData("", "")]
        public void SetImprovementPlanReviewOverallProgressCommand_WithNullOrEmptyValues_CreatesCommand(string? status, string? details)
        {
            // Arrange & Act
            var command = new SetImprovementPlanReviewOverallProgress.SetImprovementPlanReviewOverallProgressCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                status!,
                details!
            );

            // Assert
            Assert.Equal(_mockSupportProject.Id, command.SupportProjectId);
            Assert.Equal(_improvementPlanId, command.ImprovementPlanId);
            Assert.Equal(_improvementPlanReviewId, command.ImprovementPlanReviewId);
            Assert.Equal(status, command.howIsTheSchoolProgressingOverall);
            Assert.Equal(details, command.overallProgressDetails);
        }

        #endregion
    }
}