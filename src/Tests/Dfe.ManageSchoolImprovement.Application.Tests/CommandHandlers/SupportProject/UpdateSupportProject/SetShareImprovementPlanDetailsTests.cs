using AutoFixture;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Moq;
using System.Linq.Expressions;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject.SetIndicativeFundingBandAndImprovementPlanTemplateDetails;

namespace Dfe.ManageSchoolImprovement.Application.Tests.CommandHandlers.SupportProject.UpdateSupportProject
{
    public class SetShareImprovementPlanDetailsTests
    {
        private readonly Mock<ISupportProjectRepository> _mockSupportProjectRepository;
        private readonly Domain.Entities.SupportProject.SupportProject _mockSupportProject;
        private readonly CancellationToken _cancellationToken;

        public SetShareImprovementPlanDetailsTests()
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
            DateTime? dateTemplatesSent = DateTime.UtcNow;
            var command = new SetIndicativeFundingBandAndImprovementPlanTemplateDetailsCommand(
                _mockSupportProject.Id,
                true, // calculateFundingBand
                "40000", // fundingBand
                true, // sendTemplate
                dateTemplatesSent
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.FindAsync(
                    It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetIndicativeFundingBandAndImprovementPlanTemplateDetailsHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Verify
            Assert.True(result);
            _mockSupportProjectRepository.Verify(
                repo => repo.UpdateAsync(
                    It.IsAny<Domain.Entities.SupportProject.SupportProject>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ValidEmptyCommand_UpdatesSupportProject()
        {
            // Arrange
            var command = new SetIndicativeFundingBandAndImprovementPlanTemplateDetailsCommand(
                _mockSupportProject.Id,
                false, // calculateFundingBand
                null, // fundingBand
                false, // sendTemplate
                null // dateTemplatesSent
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.FindAsync(
                    It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(_mockSupportProject);

            var handler = new SetIndicativeFundingBandAndImprovementPlanTemplateDetailsHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Verify
            Assert.True(result);
            _mockSupportProjectRepository.Verify(
                repo => repo.UpdateAsync(
                    It.IsAny<Domain.Entities.SupportProject.SupportProject>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ProjectNotFound_ReturnsFalse()
        {
            // Arrange
            var command = new SetIndicativeFundingBandAndImprovementPlanTemplateDetailsCommand(
                _mockSupportProject.Id,
                true,
                "40000",
                true,
                DateTime.UtcNow
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.FindAsync(
                    It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Domain.Entities.SupportProject.SupportProject?)null!);

            var handler = new SetIndicativeFundingBandAndImprovementPlanTemplateDetailsHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Verify
            Assert.False(result);
        }
    }
}
