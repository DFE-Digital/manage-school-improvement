using AutoFixture;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Moq;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.CreateSupportProjectNote.SetSupportProjectEngagementConcernResolvedDetails;

namespace Dfe.ManageSchoolImprovement.Application.Tests.CommandHandlers.SupportProject.EngagementConcern
{
    public class SetSupportProjectEngagementConcernResolvedDetailsTests
    {
        private readonly Mock<ISupportProjectRepository> _mockSupportProjectRepository;
        private readonly Domain.Entities.SupportProject.SupportProject _mockSupportProject;
        private readonly CancellationToken _cancellationToken;

        public SetSupportProjectEngagementConcernResolvedDetailsTests()
        {
            _mockSupportProjectRepository = new Mock<ISupportProjectRepository>();
            var fixture = new Fixture();
            _mockSupportProject = fixture.Create<Domain.Entities.SupportProject.SupportProject>();
            _cancellationToken = CancellationToken.None;
        }

        [Fact]
        public async Task Handle_ValidCommand_UpdatesSupportProject()
        {
            // Arrange
            var engagementConcernResolved = true;
            var engagementConcernResolvedDetails = "Concern was resolved through improved communication and additional support";
            var engagementConcernResolvedDate = DateTime.UtcNow;

            var command = new SetSupportProjectEngagementConcernResolvedDetailsCommand(
                _mockSupportProject.Id,
                engagementConcernResolved,
                engagementConcernResolvedDetails,
                engagementConcernResolvedDate
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(x => x == _mockSupportProject.Id),
                    It.IsAny<CancellationToken>())).ReturnsAsync(_mockSupportProject);

            var handler = new SetSupportProjectEngagementConcernResolvedDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.IsType<bool>(result);
            Assert.True(result);
            _mockSupportProjectRepository.Verify(
                repo => repo.UpdateAsync(
                    It.Is<Domain.Entities.SupportProject.SupportProject>(x =>
                        x.EngagementConcernResolved == engagementConcernResolved &&
                        x.EngagementConcernResolvedDetails == engagementConcernResolvedDetails &&
                        x.EngagementConcernResolvedDate == engagementConcernResolvedDate),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ValidEmptyCommand_UpdatesSupportProject()
        {
            // Arrange
            var command = new SetSupportProjectEngagementConcernResolvedDetailsCommand(
                _mockSupportProject.Id,
                null,
                null,
                null
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(x => x == _mockSupportProject.Id),
                    It.IsAny<CancellationToken>())).ReturnsAsync(_mockSupportProject);

            var handler = new SetSupportProjectEngagementConcernResolvedDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.IsType<bool>(result);
            Assert.True(result);
            _mockSupportProjectRepository.Verify(
                repo => repo.UpdateAsync(
                    It.Is<Domain.Entities.SupportProject.SupportProject>(x =>
                        x.EngagementConcernResolved == null &&
                        x.EngagementConcernResolvedDetails == null &&
                        x.EngagementConcernResolvedDate == null),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ProjectNotFound_ReturnsFalse()
        {
            // Arrange
            var engagementConcernResolved = true;
            var engagementConcernResolvedDetails = "Resolution details";
            var engagementConcernResolvedDate = DateTime.UtcNow;

            var command = new SetSupportProjectEngagementConcernResolvedDetailsCommand(
                _mockSupportProject.Id,
                engagementConcernResolved,
                engagementConcernResolvedDetails,
                engagementConcernResolvedDate
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(x => x == _mockSupportProject.Id),
                    It.IsAny<CancellationToken>())).ReturnsAsync(null as Domain.Entities.SupportProject.SupportProject);

            var handler = new SetSupportProjectEngagementConcernResolvedDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.IsType<bool>(result);
            Assert.False(result);
            _mockSupportProjectRepository.Verify(
                repo => repo.UpdateAsync(It.IsAny<Domain.Entities.SupportProject.SupportProject>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Theory]
        [InlineData(true, "Concern resolved successfully")]
        [InlineData(false, "Concern remains unresolved")]
        [InlineData(null, "Details without resolution status")]
        public async Task Handle_WithDifferentResolvedValues_UpdatesSupportProject(bool? resolved, string details)
        {
            // Arrange
            var resolvedDate = DateTime.UtcNow;
            var command = new SetSupportProjectEngagementConcernResolvedDetailsCommand(
                _mockSupportProject.Id,
                resolved,
                details,
                resolvedDate
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(x => x == _mockSupportProject.Id),
                    It.IsAny<CancellationToken>())).ReturnsAsync(_mockSupportProject);

            var handler = new SetSupportProjectEngagementConcernResolvedDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.True(result);
            _mockSupportProjectRepository.Verify(
                repo => repo.UpdateAsync(
                    It.Is<Domain.Entities.SupportProject.SupportProject>(x =>
                        x.EngagementConcernResolved == resolved &&
                        x.EngagementConcernResolvedDetails == details &&
                        x.EngagementConcernResolvedDate == resolvedDate),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_WithEmptyString_UpdatesSupportProject()
        {
            // Arrange
            var engagementConcernResolved = true;
            var engagementConcernResolvedDetails = "";
            var engagementConcernResolvedDate = DateTime.UtcNow;

            var command = new SetSupportProjectEngagementConcernResolvedDetailsCommand(
                _mockSupportProject.Id,
                engagementConcernResolved,
                engagementConcernResolvedDetails,
                engagementConcernResolvedDate
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(x => x == _mockSupportProject.Id),
                    It.IsAny<CancellationToken>())).ReturnsAsync(_mockSupportProject);

            var handler = new SetSupportProjectEngagementConcernResolvedDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.True(result);
            _mockSupportProjectRepository.Verify(
                repo => repo.UpdateAsync(
                    It.Is<Domain.Entities.SupportProject.SupportProject>(x =>
                        x.EngagementConcernResolved == engagementConcernResolved &&
                        x.EngagementConcernResolvedDetails == engagementConcernResolvedDetails &&
                        x.EngagementConcernResolvedDate == engagementConcernResolvedDate),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_WithLongDetails_UpdatesSupportProject()
        {
            // Arrange
            var engagementConcernResolved = true;
            var longDetails = new string('A', 2000); // Very long string
            var engagementConcernResolvedDate = DateTime.UtcNow;

            var command = new SetSupportProjectEngagementConcernResolvedDetailsCommand(
                _mockSupportProject.Id,
                engagementConcernResolved,
                longDetails,
                engagementConcernResolvedDate
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(x => x == _mockSupportProject.Id),
                    It.IsAny<CancellationToken>())).ReturnsAsync(_mockSupportProject);

            var handler = new SetSupportProjectEngagementConcernResolvedDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.True(result);
            _mockSupportProjectRepository.Verify(
                repo => repo.UpdateAsync(
                    It.Is<Domain.Entities.SupportProject.SupportProject>(x =>
                        x.EngagementConcernResolved == engagementConcernResolved &&
                        x.EngagementConcernResolvedDetails == longDetails &&
                        x.EngagementConcernResolvedDate == engagementConcernResolvedDate),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_WithPastDate_UpdatesSupportProject()
        {
            // Arrange
            var engagementConcernResolved = true;
            var engagementConcernResolvedDetails = "Concern was resolved last month";
            var pastDate = DateTime.UtcNow.AddDays(-30);

            var command = new SetSupportProjectEngagementConcernResolvedDetailsCommand(
                _mockSupportProject.Id,
                engagementConcernResolved,
                engagementConcernResolvedDetails,
                pastDate
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(x => x == _mockSupportProject.Id),
                    It.IsAny<CancellationToken>())).ReturnsAsync(_mockSupportProject);

            var handler = new SetSupportProjectEngagementConcernResolvedDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.True(result);
            _mockSupportProjectRepository.Verify(
                repo => repo.UpdateAsync(
                    It.Is<Domain.Entities.SupportProject.SupportProject>(x =>
                        x.EngagementConcernResolved == engagementConcernResolved &&
                        x.EngagementConcernResolvedDetails == engagementConcernResolvedDetails &&
                        x.EngagementConcernResolvedDate == pastDate),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_CallsGetSupportProjectByIdOnce_WhenValidCommand()
        {
            // Arrange
            var command = new SetSupportProjectEngagementConcernResolvedDetailsCommand(
                _mockSupportProject.Id,
                true,
                "Resolution details",
                DateTime.UtcNow
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(x => x == _mockSupportProject.Id),
                    It.IsAny<CancellationToken>())).ReturnsAsync(_mockSupportProject);

            var handler = new SetSupportProjectEngagementConcernResolvedDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            await handler.Handle(command, _cancellationToken);

            // Assert
            _mockSupportProjectRepository.Verify(
                repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(x => x == _mockSupportProject.Id),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_CallsUpdateAsyncOnce_WhenValidCommand()
        {
            // Arrange
            var command = new SetSupportProjectEngagementConcernResolvedDetailsCommand(
                _mockSupportProject.Id,
                true,
                "Resolution details",
                DateTime.UtcNow
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(x => x == _mockSupportProject.Id),
                    It.IsAny<CancellationToken>())).ReturnsAsync(_mockSupportProject);

            var handler = new SetSupportProjectEngagementConcernResolvedDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            await handler.Handle(command, _cancellationToken);

            // Assert
            _mockSupportProjectRepository.Verify(
                repo => repo.UpdateAsync(It.IsAny<Domain.Entities.SupportProject.SupportProject>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_WithSpecialCharacters_UpdatesSupportProject()
        {
            // Arrange
            var engagementConcernResolved = true;
            var detailsWithSpecialChars = "Concern resolved with special chars: !@#$%^&*()[]{}|\\:;\"'<>,.?/~`";
            var engagementConcernResolvedDate = DateTime.UtcNow;

            var command = new SetSupportProjectEngagementConcernResolvedDetailsCommand(
                _mockSupportProject.Id,
                engagementConcernResolved,
                detailsWithSpecialChars,
                engagementConcernResolvedDate
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(x => x == _mockSupportProject.Id),
                    It.IsAny<CancellationToken>())).ReturnsAsync(_mockSupportProject);

            var handler = new SetSupportProjectEngagementConcernResolvedDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.True(result);
            _mockSupportProjectRepository.Verify(
                repo => repo.UpdateAsync(
                    It.Is<Domain.Entities.SupportProject.SupportProject>(x =>
                        x.EngagementConcernResolved == engagementConcernResolved &&
                        x.EngagementConcernResolvedDetails == detailsWithSpecialChars &&
                        x.EngagementConcernResolvedDate == engagementConcernResolvedDate),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}