using AutoFixture;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ImprovementPlans;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Moq;

namespace Dfe.ManageSchoolImprovement.Application.Tests.CommandHandlers.SupportProject.ProgressReviews
{
    public class DeleteProgressCommandHandlerTests
    {
        private readonly Mock<ISupportProjectRepository> _mockSupportProjectRepository;
        private readonly CancellationToken _cancellationToken;
        private readonly Fixture _fixture;
        private readonly Domain.Entities.SupportProject.SupportProject _mockSupportProject;
        private readonly ProgressReviewId _progressReviewId;

        public DeleteProgressCommandHandlerTests()
        {
            _mockSupportProjectRepository = new Mock<ISupportProjectRepository>();
            _cancellationToken = CancellationToken.None;
            _fixture = new Fixture();
            _mockSupportProject = _fixture.Create<Domain.Entities.SupportProject.SupportProject>();
            _progressReviewId = new ProgressReviewId(Guid.NewGuid());

            SetupMockSupportProject();
        }

        private void SetupMockSupportProject()
        {
            _mockSupportProject.AddProgressReview(
                _progressReviewId,
                _mockSupportProject.Id,
                "Test Reviewer",
                DateTime.UtcNow);

            var review = _mockSupportProject.ProgressReviews.First(r => r.Id == _progressReviewId);
            review.SetDetails("Existing next steps", "Existing additional details");
        }

        [Fact]
        public async Task Handle_ValidCommand_DeletesProgress()
        {
            // Arrange
            var command = new DeleteProgressCommand(
                _mockSupportProject.Id,
                _progressReviewId
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new DeleteProgress.DeleteProgressCommandHandler(_mockSupportProjectRepository.Object);

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
        public async Task Handle_ValidCommand_ClearsNextStepsAndAdditionalDetails()
        {
            // Arrange
            var command = new DeleteProgressCommand(
                _mockSupportProject.Id,
                _progressReviewId
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new DeleteProgress.DeleteProgressCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.True(result);

            var review = _mockSupportProject.ProgressReviews.First(r => r.Id == _progressReviewId);
            Assert.Null(review.NextSteps);
            Assert.Null(review.AdditionalDetails);
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnsTrue()
        {
            // Arrange
            var command = new DeleteProgressCommand(
                _mockSupportProject.Id,
                _progressReviewId
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new DeleteProgress.DeleteProgressCommandHandler(_mockSupportProjectRepository.Object);

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
            var command = new DeleteProgressCommand(
                nonExistentId,
                _progressReviewId
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == nonExistentId), _cancellationToken))
                .ReturnsAsync((Domain.Entities.SupportProject.SupportProject?)null);

            var handler = new DeleteProgress.DeleteProgressCommandHandler(_mockSupportProjectRepository.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                handler.Handle(command, _cancellationToken));

            Assert.Equal($"Support project with id {nonExistentId} not found", exception.Message);
        }

        [Fact]
        public async Task Handle_ProgressReviewNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange
            var nonExistentReviewId = new ProgressReviewId(Guid.NewGuid());
            var command = new DeleteProgressCommand(
                _mockSupportProject.Id,
                nonExistentReviewId
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new DeleteProgress.DeleteProgressCommandHandler(_mockSupportProjectRepository.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                handler.Handle(command, _cancellationToken));

            Assert.Equal($"Progress review with id {nonExistentReviewId} not found", exception.Message);
        }

        [Fact]
        public async Task Handle_RepositoryThrowsException_ExceptionPropagates()
        {
            // Arrange
            var command = new DeleteProgressCommand(
                _mockSupportProject.Id,
                _progressReviewId
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.IsAny<SupportProjectId>(), _cancellationToken))
                .ThrowsAsync(new InvalidOperationException("Database error"));

            var handler = new DeleteProgress.DeleteProgressCommandHandler(_mockSupportProjectRepository.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.Handle(command, _cancellationToken));
        }

        [Fact]
        public async Task Handle_ValidCommand_UpdateAsyncCalledOnlyOnce()
        {
            // Arrange
            var command = new DeleteProgressCommand(
                _mockSupportProject.Id,
                _progressReviewId
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new DeleteProgress.DeleteProgressCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            await handler.Handle(command, _cancellationToken);

            // Assert
            _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(
                It.IsAny<Domain.Entities.SupportProject.SupportProject>(),
                _cancellationToken), Times.Once);
        }

        [Fact]
        public async Task Handle_UpdateAsyncFails_PropagatesException()
        {
            // Arrange
            var command = new DeleteProgressCommand(
                _mockSupportProject.Id,
                _progressReviewId
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var expectedException = new InvalidOperationException("Update failed");
            _mockSupportProjectRepository
                .Setup(repo => repo.UpdateAsync(_mockSupportProject, _cancellationToken))
                .ThrowsAsync(expectedException);

            var handler = new DeleteProgress.DeleteProgressCommandHandler(_mockSupportProjectRepository.Object);

            // Act & Assert
            var actualException = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.Handle(command, _cancellationToken));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Fact]
        public async Task Handle_MultipleReviews_DeletesOnlyTargetReview()
        {
            // Arrange
            var secondReviewId = new ProgressReviewId(Guid.NewGuid());
            _mockSupportProject.AddProgressReview(
                secondReviewId,
                _mockSupportProject.Id,
                "Second Reviewer",
                DateTime.UtcNow.AddDays(-7));

            var secondReview = _mockSupportProject.ProgressReviews.First(r => r.Id == secondReviewId);
            secondReview.SetDetails("Second review next steps", "Second review details");

            var command = new DeleteProgressCommand(
                _mockSupportProject.Id,
                _progressReviewId
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new DeleteProgress.DeleteProgressCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.True(result);

            var deletedReview = _mockSupportProject.ProgressReviews.First(r => r.Id == _progressReviewId);
            Assert.Null(deletedReview.NextSteps);
            Assert.Null(deletedReview.AdditionalDetails);

            Assert.Equal("Second review next steps", secondReview.NextSteps);
            Assert.Equal("Second review details", secondReview.AdditionalDetails);
        }

        #region Command Tests

        [Fact]
        public void DeleteProgressCommand_WithValidParameters_CreatesCommand()
        {
            // Act
            var command = new DeleteProgressCommand(
                _mockSupportProject.Id,
                _progressReviewId
            );

            // Assert
            Assert.Equal(_mockSupportProject.Id, command.SupportProjectId);
            Assert.Equal(_progressReviewId, command.ProgressReviewId);
        }

        [Fact]
        public void DeleteProgressCommand_ImplementsIRequest()
        {
            // Assert
            Assert.True(typeof(DeleteProgressCommand)
                .IsAssignableTo(typeof(MediatR.IRequest<bool>)));
        }

        #endregion
    }
}
