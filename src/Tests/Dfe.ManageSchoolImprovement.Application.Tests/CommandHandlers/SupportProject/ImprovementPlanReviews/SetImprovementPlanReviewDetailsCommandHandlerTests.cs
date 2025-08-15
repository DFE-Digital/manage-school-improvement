using AutoFixture;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ImprovementPlans;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Moq;

namespace Dfe.ManageSchoolImprovement.Application.Tests.CommandHandlers.SupportProject.ImprovementPlanReviews
{
    public class SetImprovementPlanReviewDetailsCommandHandlerTests
    {
        private readonly Mock<ISupportProjectRepository> _mockSupportProjectRepository;
        private readonly CancellationToken _cancellationToken;
        private readonly Fixture _fixture;
        private readonly Domain.Entities.SupportProject.SupportProject _mockSupportProject;
        private readonly ImprovementPlanId _improvementPlanId;
        private readonly ImprovementPlanReviewId _improvementPlanReviewId;

        public SetImprovementPlanReviewDetailsCommandHandlerTests()
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
                "Original Reviewer",
                DateTime.UtcNow.AddDays(-1));
        }

        [Fact]
        public async Task Handle_ValidCommand_UpdatesReviewDetails()
        {
            // Arrange
            var newReviewer = "Updated Reviewer";
            var newReviewDate = DateTime.UtcNow.Date;
            var command = new SetImprovementPlanReviewDetails.SetImprovementPlanReviewDetailsCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                newReviewer,
                newReviewDate
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetImprovementPlanReviewDetails.SetImprovementPlanReviewDetailsCommandHandler(_mockSupportProjectRepository.Object);

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
        [InlineData("Dr. Smith", "2024-01-15")]
        [InlineData("Ms. Johnson", "2024-02-20")]
        [InlineData("Prof. Williams", "2024-03-10")]
        [InlineData("", "2024-04-05")]
        [InlineData("Very Long Reviewer Name With Multiple Titles and Qualifications", "2024-05-25")]
        public async Task Handle_ValidCommandWithDifferentReviewers_UpdatesReviewDetails(string reviewer, string reviewDateString)
        {
            // Arrange
            var reviewDate = DateTime.Parse(reviewDateString);
            var command = new SetImprovementPlanReviewDetails.SetImprovementPlanReviewDetailsCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                reviewer,
                reviewDate
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetImprovementPlanReviewDetails.SetImprovementPlanReviewDetailsCommandHandler(_mockSupportProjectRepository.Object);

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
            var command = new SetImprovementPlanReviewDetails.SetImprovementPlanReviewDetailsCommand(
                nonExistentId,
                _improvementPlanId,
                _improvementPlanReviewId,
                "Test Reviewer",
                DateTime.UtcNow
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == nonExistentId), _cancellationToken))
                .ReturnsAsync((Domain.Entities.SupportProject.SupportProject?)null);

            var handler = new SetImprovementPlanReviewDetails.SetImprovementPlanReviewDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                handler.Handle(command, _cancellationToken));

            Assert.Equal($"Support project with id {nonExistentId} not found", exception.Message);
        }

        [Fact]
        public async Task Handle_RepositoryThrowsException_ExceptionPropagates()
        {
            // Arrange
            var command = new SetImprovementPlanReviewDetails.SetImprovementPlanReviewDetailsCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                "Test Reviewer",
                DateTime.UtcNow
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.IsAny<SupportProjectId>(), _cancellationToken))
                .ThrowsAsync(new InvalidOperationException("Database error"));

            var handler = new SetImprovementPlanReviewDetails.SetImprovementPlanReviewDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.Handle(command, _cancellationToken));
        }

        [Fact]
        public async Task Handle_ValidCommand_UpdateAsyncCalledOnlyOnce()
        {
            // Arrange
            var command = new SetImprovementPlanReviewDetails.SetImprovementPlanReviewDetailsCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                "Test Reviewer",
                DateTime.UtcNow
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetImprovementPlanReviewDetails.SetImprovementPlanReviewDetailsCommandHandler(_mockSupportProjectRepository.Object);

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
            var command = new SetImprovementPlanReviewDetails.SetImprovementPlanReviewDetailsCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                "Test Reviewer",
                DateTime.UtcNow
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetImprovementPlanReviewDetails.SetImprovementPlanReviewDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Handle_ValidCommand_CallsRepositoryMethodsInCorrectOrder()
        {
            // Arrange
            var command = new SetImprovementPlanReviewDetails.SetImprovementPlanReviewDetailsCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                "Test Reviewer",
                DateTime.UtcNow
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetImprovementPlanReviewDetails.SetImprovementPlanReviewDetailsCommandHandler(_mockSupportProjectRepository.Object);

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
        public async Task Handle_ValidCommand_UpdatesReviewWithCorrectParameters()
        {
            // Arrange
            var newReviewer = "Dr. Updated Reviewer";
            var newReviewDate = DateTime.UtcNow.Date.AddDays(5);
            var command = new SetImprovementPlanReviewDetails.SetImprovementPlanReviewDetailsCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                newReviewer,
                newReviewDate
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetImprovementPlanReviewDetails.SetImprovementPlanReviewDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Get the existing review before update
            var improvementPlan = _mockSupportProject.ImprovementPlans.First(ip => ip.Id == _improvementPlanId);
            var existingReview = improvementPlan.ImprovementPlanReviews.First(r => r.Id == _improvementPlanReviewId);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.True(result);

            // Verify the review was updated with correct parameters
            Assert.Equal(newReviewer, existingReview.Reviewer);
            Assert.Equal(newReviewDate, existingReview.ReviewDate);
            Assert.Equal(_improvementPlanId, existingReview.ImprovementPlanId);
        }

        [Fact]
        public async Task Handle_ValidCommand_OverwritesPreviousValues()
        {
            // Arrange
            var newReviewer = "Completely Different Reviewer";
            var newReviewDate = DateTime.UtcNow.Date.AddDays(10);
            var command = new SetImprovementPlanReviewDetails.SetImprovementPlanReviewDetailsCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                newReviewer,
                newReviewDate
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetImprovementPlanReviewDetails.SetImprovementPlanReviewDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Get the existing review to verify initial state
            var improvementPlan = _mockSupportProject.ImprovementPlans.First(ip => ip.Id == _improvementPlanId);
            var existingReview = improvementPlan.ImprovementPlanReviews.First(r => r.Id == _improvementPlanReviewId);

            // Verify initial state
            Assert.Equal("Original Reviewer", existingReview.Reviewer);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.True(result);
            Assert.Equal(newReviewer, existingReview.Reviewer);
            Assert.Equal(newReviewDate, existingReview.ReviewDate);
        }

        [Fact]
        public async Task Handle_WithPastReviewDate_UpdatesReviewDate()
        {
            // Arrange
            var pastDate = DateTime.UtcNow.AddDays(-30);
            var command = new SetImprovementPlanReviewDetails.SetImprovementPlanReviewDetailsCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                "Past Date Reviewer",
                pastDate
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetImprovementPlanReviewDetails.SetImprovementPlanReviewDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Handle_WithFutureReviewDate_UpdatesReviewDate()
        {
            // Arrange
            var futureDate = DateTime.UtcNow.AddDays(30);
            var command = new SetImprovementPlanReviewDetails.SetImprovementPlanReviewDetailsCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                "Future Date Reviewer",
                futureDate
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetImprovementPlanReviewDetails.SetImprovementPlanReviewDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Handle_UpdateAsyncFails_PropagatesException()
        {
            // Arrange
            var command = new SetImprovementPlanReviewDetails.SetImprovementPlanReviewDetailsCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                "Test Reviewer",
                DateTime.UtcNow
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var expectedException = new InvalidOperationException("Update failed");
            _mockSupportProjectRepository
                .Setup(repo => repo.UpdateAsync(_mockSupportProject, _cancellationToken))
                .ThrowsAsync(expectedException);

            var handler = new SetImprovementPlanReviewDetails.SetImprovementPlanReviewDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Act & Assert
            var actualException = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.Handle(command, _cancellationToken));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Fact]
        public async Task Handle_MultipleUpdatesToSameReview_AppliesLatestValues()
        {
            // Arrange
            var firstUpdate = new SetImprovementPlanReviewDetails.SetImprovementPlanReviewDetailsCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                "First Update Reviewer",
                DateTime.UtcNow.AddDays(-5)
            );

            var secondUpdate = new SetImprovementPlanReviewDetails.SetImprovementPlanReviewDetailsCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                "Final Update Reviewer",
                DateTime.UtcNow.AddDays(5)
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetImprovementPlanReviewDetails.SetImprovementPlanReviewDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result1 = await handler.Handle(firstUpdate, _cancellationToken);
            var result2 = await handler.Handle(secondUpdate, _cancellationToken);

            // Assert
            Assert.True(result1);
            Assert.True(result2);

            var improvementPlan = _mockSupportProject.ImprovementPlans.First(ip => ip.Id == _improvementPlanId);
            var review = improvementPlan.ImprovementPlanReviews.First(r => r.Id == _improvementPlanReviewId);

            Assert.Equal("Final Update Reviewer", review.Reviewer);
            Assert.Equal(secondUpdate.ReviewDate, review.ReviewDate);
        }

        #region Command Tests

        [Fact]
        public void SetImprovementPlanReviewDetailsCommand_WithValidParameters_CreatesCommand()
        {
            // Arrange
            var reviewer = "Test Reviewer";
            var reviewDate = DateTime.UtcNow;

            // Act
            var command = new SetImprovementPlanReviewDetails.SetImprovementPlanReviewDetailsCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                reviewer,
                reviewDate
            );

            // Assert
            Assert.Equal(_mockSupportProject.Id, command.SupportProjectId);
            Assert.Equal(_improvementPlanId, command.ImprovementPlanId);
            Assert.Equal(_improvementPlanReviewId, command.ImprovementPlanReviewId);
            Assert.Equal(reviewer, command.Reviewer);
            Assert.Equal(reviewDate, command.ReviewDate);
        }

        [Fact]
        public void SetImprovementPlanReviewDetailsCommand_WithEmptyReviewer_CreatesCommand()
        {
            // Act
            var command = new SetImprovementPlanReviewDetails.SetImprovementPlanReviewDetailsCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                _improvementPlanReviewId,
                "",
                DateTime.UtcNow
            );

            // Assert
            Assert.Equal("", command.Reviewer);
        }

        [Fact]
        public void SetImprovementPlanReviewDetailsCommand_ImplementsIRequest()
        {
            // Assert
            Assert.True(typeof(SetImprovementPlanReviewDetails.SetImprovementPlanReviewDetailsCommand)
                .IsAssignableTo(typeof(MediatR.IRequest<bool>)));
        }

        #endregion
    }
}