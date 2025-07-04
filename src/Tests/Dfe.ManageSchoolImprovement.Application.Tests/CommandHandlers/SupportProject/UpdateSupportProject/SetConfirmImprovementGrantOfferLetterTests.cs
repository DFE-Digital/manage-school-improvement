using AutoFixture;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Moq;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject.SetConfirmImprovementGrantOfferLetterDetails;

namespace Dfe.ManageSchoolImprovement.Application.Tests.CommandHandlers.SupportProject.UpdateSupportProject
{
    public class SetConfirmImprovementGrantOfferLetterTests
    {
        private readonly Mock<ISupportProjectRepository> _mockSupportProjectRepository;
        private readonly Domain.Entities.SupportProject.SupportProject _mockSupportProject;
        private readonly CancellationToken _cancellationToken;


        public SetConfirmImprovementGrantOfferLetterTests()
        {
            _mockSupportProjectRepository = new Mock<ISupportProjectRepository>();
            var fixture = new Fixture();
            _mockSupportProject = fixture.Create<Domain.Entities.SupportProject.SupportProject>();
            _cancellationToken = new CancellationToken();
            _cancellationToken = CancellationToken.None;
        }

        [Fact]
        public async Task Handle_ValidCommand_UpdatesSupportProject()
        {
            // Arrange
            DateTime? dateLetterSent = DateTime.UtcNow;

            var command = new SetConfirmImprovementGrantOfferLetterDetailsCommand(
                _mockSupportProject.Id,
                dateLetterSent
            );
            _mockSupportProjectRepository
                .Setup(repo =>
                    repo.FindAsync(It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, bool>>>(),
                        It.IsAny<CancellationToken>())).ReturnsAsync(_mockSupportProject);
            var setConfirmImprovementGrantOfferLetterSentCommandHandler =
                new SetConfirmImprovementGrantOfferLetterDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await setConfirmImprovementGrantOfferLetterSentCommandHandler.Handle(command, _cancellationToken);

            // Verify
            Assert.True(result);
            _mockSupportProjectRepository.Verify(
                repo => repo.UpdateAsync(It.IsAny<Domain.Entities.SupportProject.SupportProject>(),
                    It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ValidEmptyCommand_UpdatesSupportProject()
        {
            // Arrange
            DateTime? dateLetterSent = DateTime.UtcNow;

            var command = new SetConfirmImprovementGrantOfferLetterDetailsCommand(
                _mockSupportProject.Id,
                dateLetterSent
            );
            _mockSupportProjectRepository
                .Setup(repo =>
                    repo.FindAsync(It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, bool>>>(),
                        It.IsAny<CancellationToken>())).ReturnsAsync(_mockSupportProject);
            var setConfirmPlanningGrantOfferLetterSentCommandHandler =
                new SetConfirmImprovementGrantOfferLetterDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await setConfirmPlanningGrantOfferLetterSentCommandHandler.Handle(command, _cancellationToken);

            // Verify
            Assert.True(result);
            _mockSupportProjectRepository.Verify(
                repo => repo.UpdateAsync(It.IsAny<Domain.Entities.SupportProject.SupportProject>(),
                    It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ProjectNotFound_ReturnsFalse()
        {
            // Arrange
            DateTime? dateLetterSent = DateTime.UtcNow;

            var command = new SetConfirmImprovementGrantOfferLetterDetailsCommand(
                _mockSupportProject.Id,
                dateLetterSent
            );
            _mockSupportProjectRepository
                .Setup(repo =>
                    repo.FindAsync(It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, bool>>>(),
                        It.IsAny<CancellationToken>()))!.ReturnsAsync((Domain.Entities.SupportProject.SupportProject)null!);
            var setConfirmPlanningGrantOfferLetterSentCommandHandler =
                new SetConfirmImprovementGrantOfferLetterDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await setConfirmPlanningGrantOfferLetterSentCommandHandler.Handle(command, _cancellationToken);

            // Verify
            Assert.False(result);
        }
    }
}
