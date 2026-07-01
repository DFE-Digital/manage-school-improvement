using AutoFixture;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.ImprovementPlans;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Moq;

namespace Dfe.ManageSchoolImprovement.Application.Tests.CommandHandlers.SupportProject.ProgressReviews
{
    public class SetProgressReviewNextReviewDateCommandHandlerTests
    {
        private readonly Mock<ISupportProjectRepository> _mockSupportProjectRepository;
        private readonly CancellationToken _cancellationToken;
        private readonly Fixture _fixture;
        private readonly Domain.Entities.SupportProject.SupportProject _mockSupportProject;
        private readonly ProgressReviewId _progressReviewId;

        public SetProgressReviewNextReviewDateCommandHandlerTests()
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
        }

        [Fact]
        public async Task Handle_ValidCommand_SetsNextReviewDate()
        {
            // Arrange
            var nextReviewDate = DateTime.UtcNow.AddDays(30);
            var command = new SetProgressReviewNextReviewDateCommand(
                _mockSupportProject.Id,
                _progressReviewId,
                nextReviewDate
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetProgressReviewNextReviewDate.SetProgressReviewNextReviewDateCommandHandler(_mockSupportProjectRepository.Object);

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
        [InlineData(30)]
        [InlineData(7)]
        [InlineData(90)]
        [InlineData(-30)]
        [InlineData(0)]
        public async Task Handle_ValidCommandWithDifferentDates_SetsNextReviewDate(int daysFromNow)
        {
            // Arrange
            var nextReviewDate = DateTime.UtcNow.AddDays(daysFromNow);
            var command = new SetProgressReviewNextReviewDateCommand(
                _mockSupportProject.Id,
                _progressReviewId,
                nextReviewDate
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetProgressReviewNextReviewDate.SetProgressReviewNextReviewDateCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Handle_ValidCommandWithNullDate_ClearsNextReviewDate()
        {
            // Arrange
            var command = new SetProgressReviewNextReviewDateCommand(
                _mockSupportProject.Id,
                _progressReviewId,
                null
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetProgressReviewNextReviewDate.SetProgressReviewNextReviewDateCommandHandler(_mockSupportProjectRepository.Object);

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
        public async Task Handle_SupportProjectNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange
            var nonExistentId = new SupportProjectId(999);
            var command = new SetProgressReviewNextReviewDateCommand(
                nonExistentId,
                _progressReviewId,
                DateTime.UtcNow.AddDays(30)
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == nonExistentId), _cancellationToken))
                .ReturnsAsync((Domain.Entities.SupportProject.SupportProject?)null);

            var handler = new SetProgressReviewNextReviewDate.SetProgressReviewNextReviewDateCommandHandler(_mockSupportProjectRepository.Object);

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
            var command = new SetProgressReviewNextReviewDateCommand(
                _mockSupportProject.Id,
                nonExistentReviewId,
                DateTime.UtcNow.AddDays(30)
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetProgressReviewNextReviewDate.SetProgressReviewNextReviewDateCommandHandler(_mockSupportProjectRepository.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                handler.Handle(command, _cancellationToken));

            Assert.Equal($"Progress review with id {nonExistentReviewId} not found", exception.Message);
        }

        [Fact]
        public async Task Handle_RepositoryThrowsException_ExceptionPropagates()
        {
            // Arrange
            var command = new SetProgressReviewNextReviewDateCommand(
                _mockSupportProject.Id,
                _progressReviewId,
                DateTime.UtcNow.AddDays(30)
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.IsAny<SupportProjectId>(), _cancellationToken))
                .ThrowsAsync(new InvalidOperationException("Database error"));

            var handler = new SetProgressReviewNextReviewDate.SetProgressReviewNextReviewDateCommandHandler(_mockSupportProjectRepository.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.Handle(command, _cancellationToken));
        }

        [Fact]
        public async Task Handle_ValidCommand_UpdateAsyncCalledOnlyOnce()
        {
            // Arrange
            var command = new SetProgressReviewNextReviewDateCommand(
                _mockSupportProject.Id,
                _progressReviewId,
                DateTime.UtcNow.AddDays(30)
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetProgressReviewNextReviewDate.SetProgressReviewNextReviewDateCommandHandler(_mockSupportProjectRepository.Object);

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
            var command = new SetProgressReviewNextReviewDateCommand(
                _mockSupportProject.Id,
                _progressReviewId,
                DateTime.UtcNow.AddDays(30)
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetProgressReviewNextReviewDate.SetProgressReviewNextReviewDateCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Handle_ValidCommand_SetsNextReviewDateWithCorrectParameters()
        {
            // Arrange
            var nextReviewDate = DateTime.UtcNow.AddDays(45).Date;
            var command = new SetProgressReviewNextReviewDateCommand(
                _mockSupportProject.Id,
                _progressReviewId,
                nextReviewDate
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetProgressReviewNextReviewDate.SetProgressReviewNextReviewDateCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.True(result);

            var review = _mockSupportProject.ProgressReviews.First(r => r.Id == _progressReviewId);
            Assert.Equal(nextReviewDate, review.NextReviewDate);
        }

        [Fact]
        public async Task Handle_ValidCommandWithNull_ClearsExistingNextReviewDate()
        {
            // Arrange
            var review = _mockSupportProject.ProgressReviews.First(r => r.Id == _progressReviewId);
            review.SetNextReviewDate(DateTime.UtcNow.AddDays(30));

            var command = new SetProgressReviewNextReviewDateCommand(
                _mockSupportProject.Id,
                _progressReviewId,
                null
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetProgressReviewNextReviewDate.SetProgressReviewNextReviewDateCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.True(result);
            Assert.Null(review.NextReviewDate);
        }

        [Fact]
        public async Task Handle_UpdateAsyncFails_PropagatesException()
        {
            // Arrange
            var command = new SetProgressReviewNextReviewDateCommand(
                _mockSupportProject.Id,
                _progressReviewId,
                DateTime.UtcNow.AddDays(30)
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var expectedException = new InvalidOperationException("Update failed");
            _mockSupportProjectRepository
                .Setup(repo => repo.UpdateAsync(_mockSupportProject, _cancellationToken))
                .ThrowsAsync(expectedException);

            var handler = new SetProgressReviewNextReviewDate.SetProgressReviewNextReviewDateCommandHandler(_mockSupportProjectRepository.Object);

            // Act & Assert
            var actualException = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.Handle(command, _cancellationToken));

            Assert.Equal(expectedException.Message, actualException.Message);
        }

        [Fact]
        public async Task Handle_MultipleCallsWithDifferentDates_UpdatesCorrectly()
        {
            // Arrange
            var firstDate = DateTime.UtcNow.AddDays(30);
            var secondDate = DateTime.UtcNow.AddDays(60);

            var command1 = new SetProgressReviewNextReviewDateCommand(
                _mockSupportProject.Id,
                _progressReviewId,
                firstDate
            );

            var command2 = new SetProgressReviewNextReviewDateCommand(
                _mockSupportProject.Id,
                _progressReviewId,
                secondDate
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(id => id == _mockSupportProject.Id), _cancellationToken))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetProgressReviewNextReviewDate.SetProgressReviewNextReviewDateCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result1 = await handler.Handle(command1, _cancellationToken);
            var result2 = await handler.Handle(command2, _cancellationToken);

            // Assert
            Assert.True(result1);
            Assert.True(result2);

            var review = _mockSupportProject.ProgressReviews.First(r => r.Id == _progressReviewId);
            Assert.Equal(secondDate, review.NextReviewDate);
        }

        #region Command Tests

        [Fact]
        public void SetProgressReviewNextReviewDateCommand_WithValidParameters_CreatesCommand()
        {
            // Arrange
            var nextReviewDate = DateTime.UtcNow.AddDays(30);

            // Act
            var command = new SetProgressReviewNextReviewDateCommand(
                _mockSupportProject.Id,
                _progressReviewId,
                nextReviewDate
            );

            // Assert
            Assert.Equal(_mockSupportProject.Id, command.SupportProjectId);
            Assert.Equal(_progressReviewId, command.ProgressReviewId);
            Assert.Equal(nextReviewDate, command.NextReviewDate);
        }

        [Fact]
        public void SetProgressReviewNextReviewDateCommand_WithNullDate_CreatesCommand()
        {
            // Act
            var command = new SetProgressReviewNextReviewDateCommand(
                _mockSupportProject.Id,
                _progressReviewId,
                null
            );

            // Assert
            Assert.Equal(_mockSupportProject.Id, command.SupportProjectId);
            Assert.Equal(_progressReviewId, command.ProgressReviewId);
            Assert.Null(command.NextReviewDate);
        }

        [Fact]
        public void SetProgressReviewNextReviewDateCommand_ImplementsIRequest()
        {
            // Assert
            Assert.True(typeof(SetProgressReviewNextReviewDateCommand)
                .IsAssignableTo(typeof(MediatR.IRequest<bool>)));
        }

        #endregion
    }
}
