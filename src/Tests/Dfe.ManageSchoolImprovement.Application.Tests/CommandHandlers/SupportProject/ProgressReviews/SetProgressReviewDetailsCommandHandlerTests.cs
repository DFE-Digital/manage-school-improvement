using AutoFixture;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ImprovementPlans;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Moq;

namespace Dfe.ManageSchoolImprovement.Application.Tests.CommandHandlers.SupportProject.ProgressReviews
{
    public class SetProgressReviewDetailsCommandHandlerTests
    {
        private readonly Mock<ISupportProjectRepository> _mockSupportProjectRepository;
        private readonly CancellationToken _cancellationToken;
        private readonly Fixture _fixture;
        private readonly Domain.Entities.SupportProject.SupportProject _mockSupportProject;
        private readonly ProgressReviewId _progressReviewId;

        public SetProgressReviewDetailsCommandHandlerTests()
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
                "Original Reviewer",
                DateTime.UtcNow.AddDays(-1));
        }

        [Fact]
        public async Task Handle_ValidCommand_UpdatesReviewDetails()
        {
            // Arrange
            var nextSteps = "Complete the action plan";
            var additionalDetails = "Follow up in two weeks";
            var command = new SetProgressReviewDetailsCommand(
                _mockSupportProject.Id,
                _progressReviewId,
                nextSteps,
                additionalDetails
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetProgressReviewDetails.SetProgressReviewDetailsCommandHandler(_mockSupportProjectRepository.Object);

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
        [InlineData("Complete objectives", "Additional context")]
        [InlineData("Review progress quarterly", null)]
        [InlineData("", "Details only")]
        [InlineData("Very long next steps with multiple actions and follow-up items", "Supporting notes")]
        public async Task Handle_ValidCommandWithDifferentValues_UpdatesReviewDetails(string nextSteps, string? additionalDetails)
        {
            // Arrange
            var command = new SetProgressReviewDetailsCommand(
                _mockSupportProject.Id,
                _progressReviewId,
                nextSteps,
                additionalDetails
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetProgressReviewDetails.SetProgressReviewDetailsCommandHandler(_mockSupportProjectRepository.Object);

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
            var command = new SetProgressReviewDetailsCommand(
                nonExistentId,
                _progressReviewId,
                "Next steps",
                "Additional details"
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == nonExistentId), _cancellationToken))
                .ReturnsAsync((Domain.Entities.SupportProject.SupportProject?)null);

            var handler = new SetProgressReviewDetails.SetProgressReviewDetailsCommandHandler(_mockSupportProjectRepository.Object);

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
            var command = new SetProgressReviewDetailsCommand(
                _mockSupportProject.Id,
                nonExistentReviewId,
                "Next steps",
                "Additional details"
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetProgressReviewDetails.SetProgressReviewDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                handler.Handle(command, _cancellationToken));

            Assert.Equal($"Progress review with id {nonExistentReviewId} not found", exception.Message);
        }

        [Fact]
        public async Task Handle_RepositoryThrowsException_ExceptionPropagates()
        {
            // Arrange
            var command = new SetProgressReviewDetailsCommand(
                _mockSupportProject.Id,
                _progressReviewId,
                "Next steps",
                "Additional details"
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.IsAny<SupportProjectId>(), _cancellationToken))
                .ThrowsAsync(new InvalidOperationException("Database error"));

            var handler = new SetProgressReviewDetails.SetProgressReviewDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.Handle(command, _cancellationToken));
        }

        [Fact]
        public async Task Handle_ValidCommand_UpdateAsyncCalledOnlyOnce()
        {
            // Arrange
            var command = new SetProgressReviewDetailsCommand(
                _mockSupportProject.Id,
                _progressReviewId,
                "Next steps",
                "Additional details"
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetProgressReviewDetails.SetProgressReviewDetailsCommandHandler(_mockSupportProjectRepository.Object);

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
            var command = new SetProgressReviewDetailsCommand(
                _mockSupportProject.Id,
                _progressReviewId,
                "Next steps",
                "Additional details"
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetProgressReviewDetails.SetProgressReviewDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Handle_ValidCommand_UpdatesReviewWithCorrectParameters()
        {
            // Arrange
            var nextSteps = "Implement improvement plan";
            var additionalDetails = "Schedule follow-up meeting";
            var command = new SetProgressReviewDetailsCommand(
                _mockSupportProject.Id,
                _progressReviewId,
                nextSteps,
                additionalDetails
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetProgressReviewDetails.SetProgressReviewDetailsCommandHandler(_mockSupportProjectRepository.Object);

            var existingReview = _mockSupportProject.ProgressReviews.First(r => r.Id == _progressReviewId);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.True(result);
            Assert.Equal(nextSteps, existingReview.NextSteps);
            Assert.Equal(additionalDetails, existingReview.AdditionalDetails);
            Assert.Equal(_mockSupportProject.Id, existingReview.SupportProjectId);
        }

        [Fact]
        public async Task Handle_ValidCommand_OverwritesPreviousValues()
        {
            // Arrange
            var existingReview = _mockSupportProject.ProgressReviews.First(r => r.Id == _progressReviewId);
            existingReview.SetDetails("Original next steps", "Original details");

            var newNextSteps = "Updated next steps";
            var newAdditionalDetails = "Updated details";
            var command = new SetProgressReviewDetailsCommand(
                _mockSupportProject.Id,
                _progressReviewId,
                newNextSteps,
                newAdditionalDetails
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetProgressReviewDetails.SetProgressReviewDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.True(result);
            Assert.Equal(newNextSteps, existingReview.NextSteps);
            Assert.Equal(newAdditionalDetails, existingReview.AdditionalDetails);
        }

        [Fact]
        public async Task Handle_UpdateAsyncFails_PropagatesException()
        {
            // Arrange
            var command = new SetProgressReviewDetailsCommand(
                _mockSupportProject.Id,
                _progressReviewId,
                "Next steps",
                "Additional details"
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var expectedException = new InvalidOperationException("Update failed");
            _mockSupportProjectRepository
                .Setup(repo => repo.UpdateAsync(_mockSupportProject, _cancellationToken))
                .ThrowsAsync(expectedException);

            var handler = new SetProgressReviewDetails.SetProgressReviewDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Act & Assert
            var actualException = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.Handle(command, _cancellationToken));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Fact]
        public async Task Handle_MultipleUpdatesToSameReview_AppliesLatestValues()
        {
            // Arrange
            var firstUpdate = new SetProgressReviewDetailsCommand(
                _mockSupportProject.Id,
                _progressReviewId,
                "First next steps",
                "First details"
            );

            var secondUpdate = new SetProgressReviewDetailsCommand(
                _mockSupportProject.Id,
                _progressReviewId,
                "Final next steps",
                "Final details"
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetProgressReviewDetails.SetProgressReviewDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result1 = await handler.Handle(firstUpdate, _cancellationToken);
            var result2 = await handler.Handle(secondUpdate, _cancellationToken);

            // Assert
            Assert.True(result1);
            Assert.True(result2);

            var review = _mockSupportProject.ProgressReviews.First(r => r.Id == _progressReviewId);
            Assert.Equal("Final next steps", review.NextSteps);
            Assert.Equal("Final details", review.AdditionalDetails);
        }

        #region Command Tests

        [Fact]
        public void SetProgressReviewDetailsCommand_WithValidParameters_CreatesCommand()
        {
            // Arrange
            var nextSteps = "Next steps";
            var additionalDetails = "Additional details";

            // Act
            var command = new SetProgressReviewDetailsCommand(
                _mockSupportProject.Id,
                _progressReviewId,
                nextSteps,
                additionalDetails
            );

            // Assert
            Assert.Equal(_mockSupportProject.Id, command.SupportProjectId);
            Assert.Equal(_progressReviewId, command.ProgressReviewId);
            Assert.Equal(nextSteps, command.NextSteps);
            Assert.Equal(additionalDetails, command.AdditionalDetails);
        }

        [Fact]
        public void SetProgressReviewDetailsCommand_WithNullAdditionalDetails_CreatesCommand()
        {
            // Act
            var command = new SetProgressReviewDetailsCommand(
                _mockSupportProject.Id,
                _progressReviewId,
                "Next steps",
                null
            );

            // Assert
            Assert.Null(command.AdditionalDetails);
        }

        [Fact]
        public void SetProgressReviewDetailsCommand_ImplementsIRequest()
        {
            // Assert
            Assert.True(typeof(SetProgressReviewDetailsCommand)
                .IsAssignableTo(typeof(MediatR.IRequest<bool>)));
        }

        #endregion
    }
}
