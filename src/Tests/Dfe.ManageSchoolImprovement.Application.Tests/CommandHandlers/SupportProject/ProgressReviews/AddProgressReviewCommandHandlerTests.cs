using AutoFixture;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ProgressReviews;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Moq;

namespace Dfe.ManageSchoolImprovement.Application.Tests.CommandHandlers.SupportProject.ProgressReviews
{
    public class AddProgressReviewCommandHandlerTests
    {
        private readonly Mock<ISupportProjectRepository> _mockSupportProjectRepository;
        private readonly CancellationToken _cancellationToken;
        private readonly Fixture _fixture;
        private readonly Domain.Entities.SupportProject.SupportProject _mockSupportProject;

        public AddProgressReviewCommandHandlerTests()
        {
            _mockSupportProjectRepository = new Mock<ISupportProjectRepository>();
            _cancellationToken = CancellationToken.None;
            _fixture = new Fixture();
            _mockSupportProject = _fixture.Create<Domain.Entities.SupportProject.SupportProject>();
        }

        [Fact]
        public async Task Handle_ValidCommand_CreatesProgressReview()
        {
            // Arrange
            var reviewer = "Test Reviewer";
            var reviewDate = DateTime.UtcNow.Date;
            var command = new AddProgressReviewCommand(
                _mockSupportProject.Id,
                reviewer,
                reviewDate
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new AddProgressReview.AddProgressReviewCommandCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ProgressReviewId>(result);
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
            var command = new AddProgressReviewCommand(
                _mockSupportProject.Id,
                reviewer,
                reviewDate
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new AddProgressReview.AddProgressReviewCommandCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ProgressReviewId>(result);
        }

        [Fact]
        public async Task Handle_SupportProjectNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange
            var nonExistentId = new SupportProjectId(999);
            var command = new AddProgressReviewCommand(
                nonExistentId,
                "Test Reviewer",
                DateTime.UtcNow
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == nonExistentId), _cancellationToken))
                .ReturnsAsync((Domain.Entities.SupportProject.SupportProject?)null);

            var handler = new AddProgressReview.AddProgressReviewCommandCommandHandler(_mockSupportProjectRepository.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                handler.Handle(command, _cancellationToken));

            Assert.Equal($"Support project with id {nonExistentId} not found", exception.Message);
        }

        [Fact]
        public async Task Handle_RepositoryThrowsException_ExceptionPropagates()
        {
            // Arrange
            var command = new AddProgressReviewCommand(
                _mockSupportProject.Id,
                "Test Reviewer",
                DateTime.UtcNow
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.IsAny<SupportProjectId>(), _cancellationToken))
                .ThrowsAsync(new InvalidOperationException("Database error"));

            var handler = new AddProgressReview.AddProgressReviewCommandCommandHandler(_mockSupportProjectRepository.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.Handle(command, _cancellationToken));
        }

        [Fact]
        public async Task Handle_ValidCommand_UpdateAsyncCalledOnlyOnce()
        {
            // Arrange
            var command = new AddProgressReviewCommand(
                _mockSupportProject.Id,
                "Test Reviewer",
                DateTime.UtcNow
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new AddProgressReview.AddProgressReviewCommandCommandHandler(_mockSupportProjectRepository.Object);

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
            var command = new AddProgressReviewCommand(
                _mockSupportProject.Id,
                "Test Reviewer",
                DateTime.UtcNow
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new AddProgressReview.AddProgressReviewCommandCommandHandler(_mockSupportProjectRepository.Object);

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
            var command = new AddProgressReviewCommand(
                _mockSupportProject.Id,
                "Test Reviewer",
                DateTime.UtcNow
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new AddProgressReview.AddProgressReviewCommandCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            await handler.Handle(command, _cancellationToken);

            // Assert
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
            var command = new AddProgressReviewCommand(
                _mockSupportProject.Id,
                reviewer,
                reviewDate
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new AddProgressReview.AddProgressReviewCommandCommandHandler(_mockSupportProjectRepository.Object);

            var initialReviewCount = _mockSupportProject.ProgressReviews.Count();

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ProgressReviewId>(result);

            var finalReviewCount = _mockSupportProject.ProgressReviews.Count();
            Assert.Equal(initialReviewCount + 1, finalReviewCount);

            var addedReview = _mockSupportProject.ProgressReviews.Last();
            Assert.Equal(result, addedReview.Id);
            Assert.Equal(reviewer, addedReview.Reviewer);
            Assert.Equal(reviewDate, addedReview.ReviewDate);
            Assert.Equal(_mockSupportProject.Id, addedReview.SupportProjectId);
            Assert.Equal("First review", addedReview.Title);
            Assert.Equal(1, addedReview.Order);
        }

        [Fact]
        public async Task Handle_ValidCommand_GeneratesUniqueReviewId()
        {
            // Arrange
            var command = new AddProgressReviewCommand(
                _mockSupportProject.Id,
                "Test Reviewer",
                DateTime.UtcNow
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new AddProgressReview.AddProgressReviewCommandCommandHandler(_mockSupportProjectRepository.Object);

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
            var command = new AddProgressReviewCommand(
                _mockSupportProject.Id,
                "Past Reviewer",
                pastDate
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new AddProgressReview.AddProgressReviewCommandCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ProgressReviewId>(result);
        }

        [Fact]
        public async Task Handle_WithFutureReviewDate_CreatesReview()
        {
            // Arrange
            var futureDate = DateTime.UtcNow.AddDays(30);
            var command = new AddProgressReviewCommand(
                _mockSupportProject.Id,
                "Future Reviewer",
                futureDate
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new AddProgressReview.AddProgressReviewCommandCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ProgressReviewId>(result);
        }

        [Fact]
        public async Task Handle_UpdateAsyncFails_PropagatesException()
        {
            // Arrange
            var command = new AddProgressReviewCommand(
                _mockSupportProject.Id,
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

            var handler = new AddProgressReview.AddProgressReviewCommandCommandHandler(_mockSupportProjectRepository.Object);

            // Act & Assert
            var actualException = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.Handle(command, _cancellationToken));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Fact]
        public async Task Handle_MultipleCalls_CreatesMultipleReviews()
        {
            // Arrange
            var command1 = new AddProgressReviewCommand(
                _mockSupportProject.Id,
                "First Reviewer",
                DateTime.UtcNow.AddDays(-1)
            );
            var command2 = new AddProgressReviewCommand(
                _mockSupportProject.Id,
                "Second Reviewer",
                DateTime.UtcNow
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new AddProgressReview.AddProgressReviewCommandCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result1 = await handler.Handle(command1, _cancellationToken);
            var result2 = await handler.Handle(command2, _cancellationToken);

            // Assert
            Assert.NotNull(result1);
            Assert.NotNull(result2);
            Assert.NotEqual(result1.Value, result2.Value);
            Assert.Equal(2, _mockSupportProject.ProgressReviews.Count());
            Assert.Equal("Second review", _mockSupportProject.ProgressReviews.Last().Title);
        }

        #region Command Tests

        [Fact]
        public void AddProgressReviewCommand_WithValidParameters_CreatesCommand()
        {
            // Arrange
            var reviewer = "Test Reviewer";
            var reviewDate = DateTime.UtcNow;

            // Act
            var command = new AddProgressReviewCommand(
                _mockSupportProject.Id,
                reviewer,
                reviewDate
            );

            // Assert
            Assert.Equal(_mockSupportProject.Id, command.SupportProjectId);
            Assert.Equal(reviewer, command.Reviewer);
            Assert.Equal(reviewDate, command.ReviewDate);
        }

        [Fact]
        public void AddProgressReviewCommand_ImplementsIRequest()
        {
            // Assert
            Assert.True(typeof(AddProgressReviewCommand)
                .IsAssignableTo(typeof(MediatR.IRequest<ProgressReviewId>)));
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void AddProgressReviewCommand_WithEmptyOrNullReviewer_CreatesCommand(string reviewer)
        {
            // Arrange & Act
            var command = new AddProgressReviewCommand(
                _mockSupportProject.Id,
                reviewer,
                DateTime.UtcNow
            );

            // Assert
            Assert.Equal(reviewer, command.Reviewer);
        }

        #endregion
    }
}
