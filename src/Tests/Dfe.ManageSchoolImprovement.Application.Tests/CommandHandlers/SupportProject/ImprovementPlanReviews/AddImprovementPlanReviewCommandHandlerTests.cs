using AutoFixture;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ImprovementPlansReviews;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Moq;

namespace Dfe.ManageSchoolImprovement.Application.Tests.CommandHandlers.SupportProject.ImprovementPlanReviews
{
    public class AddImprovementPlanReviewCommandHandlerTests
    {
        private readonly Mock<ISupportProjectRepository> _mockSupportProjectRepository;
        private readonly CancellationToken _cancellationToken;
        private readonly Fixture _fixture;
        private readonly Domain.Entities.SupportProject.SupportProject _mockSupportProject;
        private readonly ImprovementPlanId _improvementPlanId;

        public AddImprovementPlanReviewCommandHandlerTests()
        {
            _mockSupportProjectRepository = new Mock<ISupportProjectRepository>();
            _cancellationToken = CancellationToken.None;
            _fixture = new Fixture();
            _mockSupportProject = _fixture.Create<Domain.Entities.SupportProject.SupportProject>();
            _improvementPlanId = new ImprovementPlanId(Guid.NewGuid());

            // Set up the support project with the required improvement plan structure
            SetupMockSupportProject();
        }

        private void SetupMockSupportProject()
        {
            // Add improvement plan to the support project
            _mockSupportProject.AddImprovementPlan(_improvementPlanId, _mockSupportProject.Id);
        }

        [Fact]
        public async Task Handle_ValidCommand_CreatesImprovementPlanReview()
        {
            // Arrange
            var reviewer = "Test Reviewer";
            var reviewDate = DateTime.UtcNow.Date;
            var command = new AddImprovementPlanReview.AddImprovementPlanReviewCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                reviewer,
                reviewDate
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new AddImprovementPlanReview.AddImprovementPlanReviewCommandCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ImprovementPlanReviewId>(result);
            _mockSupportProjectRepository.Verify(repo => repo.GetSupportProjectById(
                It.Is<SupportProjectId>(id => id == _mockSupportProject.Id),
                _cancellationToken), Times.Once);
            _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(
                _mockSupportProject,
                _cancellationToken), Times.Once);
        }

        [Theory]
        [InlineData("John Smith", "2024-01-15")]
        [InlineData("Jane Doe", "2024-02-20")]
        [InlineData("Dr. Williams", "2024-03-10")]
        [InlineData("", "2024-04-05")]
        [InlineData("Very Long Reviewer Name With Multiple Words", "2024-05-25")]
        public async Task Handle_ValidCommandWithDifferentReviewers_CreatesReview(string reviewer, string reviewDateString)
        {
            // Arrange
            var reviewDate = DateTime.Parse(reviewDateString);
            var command = new AddImprovementPlanReview.AddImprovementPlanReviewCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                reviewer,
                reviewDate
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new AddImprovementPlanReview.AddImprovementPlanReviewCommandCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ImprovementPlanReviewId>(result);
        }

        [Fact]
        public async Task Handle_SupportProjectNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange
            var nonExistentId = new SupportProjectId(999);
            var command = new AddImprovementPlanReview.AddImprovementPlanReviewCommand(
                nonExistentId,
                _improvementPlanId,
                "Test Reviewer",
                DateTime.UtcNow
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == nonExistentId), _cancellationToken))
                .ReturnsAsync((Domain.Entities.SupportProject.SupportProject?)null);

            var handler = new AddImprovementPlanReview.AddImprovementPlanReviewCommandCommandHandler(_mockSupportProjectRepository.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                handler.Handle(command, _cancellationToken));

            Assert.Equal($"Support project with id {nonExistentId} not found", exception.Message);
        }

        [Fact]
        public async Task Handle_RepositoryThrowsException_ExceptionPropagates()
        {
            // Arrange
            var command = new AddImprovementPlanReview.AddImprovementPlanReviewCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                "Test Reviewer",
                DateTime.UtcNow
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.IsAny<SupportProjectId>(), _cancellationToken))
                .ThrowsAsync(new InvalidOperationException("Database error"));

            var handler = new AddImprovementPlanReview.AddImprovementPlanReviewCommandCommandHandler(_mockSupportProjectRepository.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.Handle(command, _cancellationToken));
        }

        [Fact]
        public async Task Handle_ValidCommand_UpdateAsyncCalledOnlyOnce()
        {
            // Arrange
            var command = new AddImprovementPlanReview.AddImprovementPlanReviewCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                "Test Reviewer",
                DateTime.UtcNow
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new AddImprovementPlanReview.AddImprovementPlanReviewCommandCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            await handler.Handle(command, _cancellationToken);

            // Assert
            _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(
                It.IsAny<Domain.Entities.SupportProject.SupportProject>(),
                _cancellationToken), Times.Once);
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnsValidReviewId()
        {
            // Arrange
            var command = new AddImprovementPlanReview.AddImprovementPlanReviewCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                "Test Reviewer",
                DateTime.UtcNow
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new AddImprovementPlanReview.AddImprovementPlanReviewCommandCommandHandler(_mockSupportProjectRepository.Object);

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
            var command = new AddImprovementPlanReview.AddImprovementPlanReviewCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                "Test Reviewer",
                DateTime.UtcNow
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new AddImprovementPlanReview.AddImprovementPlanReviewCommandCommandHandler(_mockSupportProjectRepository.Object);

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
        public async Task Handle_ValidCommand_AddsReviewWithCorrectParameters()
        {
            // Arrange
            var reviewer = "Dr. Smith";
            var reviewDate = DateTime.UtcNow.Date;
            var command = new AddImprovementPlanReview.AddImprovementPlanReviewCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                reviewer,
                reviewDate
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new AddImprovementPlanReview.AddImprovementPlanReviewCommandCommandHandler(_mockSupportProjectRepository.Object);

            // Get the initial count of reviews in the improvement plan
            var improvementPlan = _mockSupportProject.ImprovementPlans.First(ip => ip.Id == _improvementPlanId);
            var initialReviewCount = improvementPlan.ImprovementPlanReviews.Count();

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ImprovementPlanReviewId>(result);

            // Verify that a new review was added
            var finalReviewCount = improvementPlan.ImprovementPlanReviews.Count();
            Assert.Equal(initialReviewCount + 1, finalReviewCount);

            // Verify the added review has the correct properties
            var addedReview = improvementPlan.ImprovementPlanReviews.Last();
            Assert.Equal(reviewer, addedReview.Reviewer);
            Assert.Equal(reviewDate, addedReview.ReviewDate);
            Assert.Equal(_improvementPlanId, addedReview.ImprovementPlanId);
        }

        [Fact]
        public async Task Handle_ValidCommand_GeneratesUniqueReviewId()
        {
            // Arrange
            var command = new AddImprovementPlanReview.AddImprovementPlanReviewCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                "Test Reviewer",
                DateTime.UtcNow
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new AddImprovementPlanReview.AddImprovementPlanReviewCommandCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result1 = await handler.Handle(command, _cancellationToken);

            var result2 = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.NotEqual(result1.Value, result2.Value);
        }

        [Fact]
        public async Task Handle_WithPastReviewDate_CreatesReview()
        {
            // Arrange
            var pastDate = DateTime.UtcNow.AddDays(-30);
            var command = new AddImprovementPlanReview.AddImprovementPlanReviewCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                "Past Reviewer",
                pastDate
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new AddImprovementPlanReview.AddImprovementPlanReviewCommandCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ImprovementPlanReviewId>(result);
        }

        [Fact]
        public async Task Handle_WithFutureReviewDate_CreatesReview()
        {
            // Arrange
            var futureDate = DateTime.UtcNow.AddDays(30);
            var command = new AddImprovementPlanReview.AddImprovementPlanReviewCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                "Future Reviewer",
                futureDate
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new AddImprovementPlanReview.AddImprovementPlanReviewCommandCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ImprovementPlanReviewId>(result);
        }

        [Fact]
        public async Task Handle_UpdateAsyncFails_PropagatesException()
        {
            // Arrange
            var command = new AddImprovementPlanReview.AddImprovementPlanReviewCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
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

            var handler = new AddImprovementPlanReview.AddImprovementPlanReviewCommandCommandHandler(_mockSupportProjectRepository.Object);

            // Act & Assert
            var actualException = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.Handle(command, _cancellationToken));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Fact]
        public async Task Handle_MultipleCalls_CreatesMultipleReviews()
        {
            // Arrange
            var command1 = new AddImprovementPlanReview.AddImprovementPlanReviewCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                "First Reviewer",
                DateTime.UtcNow.AddDays(-1)
            );
            var command2 = new AddImprovementPlanReview.AddImprovementPlanReviewCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                "Second Reviewer",
                DateTime.UtcNow
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new AddImprovementPlanReview.AddImprovementPlanReviewCommandCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result1 = await handler.Handle(command1, _cancellationToken);
            var result2 = await handler.Handle(command2, _cancellationToken);

            // Assert
            Assert.NotNull(result1);
            Assert.NotNull(result2);
            Assert.NotEqual(result1.Value, result2.Value);

            var improvementPlan = _mockSupportProject.ImprovementPlans.First(ip => ip.Id == _improvementPlanId);
            Assert.Equal(2, improvementPlan.ImprovementPlanReviews.Count());
        }

        #region Command Tests

        [Fact]
        public void AddImprovementPlanReviewCommand_WithValidParameters_CreatesCommand()
        {
            // Arrange
            var reviewer = "Test Reviewer";
            var reviewDate = DateTime.UtcNow;

            // Act
            var command = new AddImprovementPlanReview.AddImprovementPlanReviewCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                reviewer,
                reviewDate
            );

            // Assert
            Assert.Equal(_mockSupportProject.Id, command.SupportProjectId);
            Assert.Equal(_improvementPlanId, command.ImprovementPlanId);
            Assert.Equal(reviewer, command.Reviewer);
            Assert.Equal(reviewDate, command.ReviewDate);
        }

        [Fact]
        public void AddImprovementPlanReviewCommand_ImplementsIRequest()
        {
            // Assert
            Assert.True(typeof(AddImprovementPlanReview.AddImprovementPlanReviewCommand)
                .IsAssignableTo(typeof(MediatR.IRequest<ImprovementPlanReviewId>)));
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void AddImprovementPlanReviewCommand_WithEmptyOrNullReviewer_CreatesCommand(string reviewer)
        {
            // Arrange & Act
            var command = new AddImprovementPlanReview.AddImprovementPlanReviewCommand(
                _mockSupportProject.Id,
                _improvementPlanId,
                reviewer,
                DateTime.UtcNow
            );

            // Assert
            Assert.Equal(reviewer, command.Reviewer);
        }

        #endregion
    }
}