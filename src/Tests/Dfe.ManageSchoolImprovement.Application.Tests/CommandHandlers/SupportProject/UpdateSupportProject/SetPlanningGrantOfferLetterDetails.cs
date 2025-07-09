using AutoFixture;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Moq;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject.SetRequestPlanningGrantOfferLetterDetails;

namespace Dfe.ManageSchoolImprovement.Application.Tests.CommandHandlers.SupportProject.UpdateSupportProject
{
    public class SetRequestPlanningGrantOfferLetterDetailsTests
    {
        private readonly Mock<ISupportProjectRepository> _mockSupportProjectRepository;
        private readonly Domain.Entities.SupportProject.SupportProject _mockSupportProject;
        private readonly CancellationToken _cancellationToken;


        public SetRequestPlanningGrantOfferLetterDetailsTests()
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
            DateTime? dateRequested = DateTime.UtcNow;

            var command = new SetRequestPlanningGrantOfferLetterDetailsCommand(
                _mockSupportProject.Id,
                dateRequested,
                true,
                true,
                false,
                false
            );
            _mockSupportProjectRepository.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(_mockSupportProject);
            var setRequestPlanningGrantOfferLetterDetailsCommandHandler = new SetRequestPlanningGrantOfferLetterDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await setRequestPlanningGrantOfferLetterDetailsCommandHandler.Handle(command, _cancellationToken);

            // Verify
            Assert.True(result);
            _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.SupportProject.SupportProject>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ValidEmptyCommand_UpdatesSupportProject()
        {
            // Arrange
            var command = new SetRequestPlanningGrantOfferLetterDetailsCommand(_mockSupportProject.Id, null, false, false, false, false);

            _mockSupportProjectRepository.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(_mockSupportProject);
            var setRequestPlanningGrantOfferLetterDetailsCommandHandler = new SetRequestPlanningGrantOfferLetterDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await setRequestPlanningGrantOfferLetterDetailsCommandHandler.Handle(command, _cancellationToken);

            // Verify
            Assert.True(result);
            _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.SupportProject.SupportProject>(), It.IsAny<CancellationToken>()), Times.Once); _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.SupportProject.SupportProject>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ProjectNotFound_ReturnsFalse()
        {
            // Arrange
            DateTime? dateRequested = DateTime.UtcNow;

            var command = new SetRequestPlanningGrantOfferLetterDetailsCommand(
                _mockSupportProject.Id,
                dateRequested,
                false,
                false,
                false,
                false
            );
            _mockSupportProjectRepository.Setup(repo => 
                repo.FindAsync(
                    It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, bool>>>(), 
                    It.IsAny<CancellationToken>()))!.ReturnsAsync((Domain.Entities.SupportProject.SupportProject)null!);
            var setRequestPlanningGrantOfferLetterDetailsCommandHandler = new SetRequestPlanningGrantOfferLetterDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await setRequestPlanningGrantOfferLetterDetailsCommandHandler.Handle(command, _cancellationToken);

            // Verify
            Assert.False(result);
        }
    }
}
