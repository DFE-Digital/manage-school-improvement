using AutoFixture;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Moq;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.FundingHistory.AddFundingHistory;

namespace Dfe.ManageSchoolImprovement.Application.Tests.SupportProject.Commands.FundingHistory
{
    public class AddFundingHistoryTests
    {
        private readonly Mock<ISupportProjectRepository> _mockSupportProjectRepository;
        private readonly Domain.Entities.SupportProject.SupportProject _mockSupportProject;
        private readonly CancellationToken _cancellationToken;

        public AddFundingHistoryTests()
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
            var fundingType = "funding type";
            var fundingAmount = 100.10;
            var financialYear = "financial year";
            var fundingRounds = 10;
            var comments = "comments";

            var command = new AddFundingHistoryCommand(
                _mockSupportProject.Id,
                fundingType, fundingAmount, financialYear, fundingRounds, comments
            );
            _mockSupportProjectRepository.Setup(repo => repo.GetSupportProjectById(It.IsAny<SupportProjectId>(), It.IsAny<CancellationToken>())).ReturnsAsync(_mockSupportProject);
            var addFundingHistoryCommandHandler = new AddFundingHistoryCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await addFundingHistoryCommandHandler.Handle(command, _cancellationToken);

            // Verify
            Assert.NotNull(result);
            Assert.IsType<FundingHistoryId>(result);
            _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.SupportProject.SupportProject>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ValidEmptyCommand_UpdatesSupportProject()
        {
            // Arrange
            var fundingType = "funding type";
            var fundingAmount = 100.10;
            var financialYear = "financial year";
            var fundingRounds = 10;
            var comments = "comments";

            var command = new AddFundingHistoryCommand(
                _mockSupportProject.Id,
                null, 0, null, 0, null
            );
            _mockSupportProjectRepository.Setup(repo => repo.GetSupportProjectById(It.IsAny<SupportProjectId>(), It.IsAny<CancellationToken>())).ReturnsAsync(_mockSupportProject);
            var addFundingHistoryCommandHandler = new AddFundingHistoryCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await addFundingHistoryCommandHandler.Handle(command, _cancellationToken);

            // Verify
            Assert.NotNull(result);
            Assert.IsType<FundingHistoryId>(result);
            _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.SupportProject.SupportProject>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ProjectNotFound_ReturnsFalse()
        {
            // Arrange
            // Arrange
            var fundingType = "funding type";
            var fundingAmount = 100.10;
            var financialYear = "financial year";
            var fundingRounds = 10;
            var comments = "comments";

            var command = new AddFundingHistoryCommand(
                _mockSupportProject.Id,
                fundingType, fundingAmount, financialYear, fundingRounds, comments
            );

            _mockSupportProjectRepository.Setup(repo => repo.GetSupportProjectById(It.IsAny<SupportProjectId>(), It.IsAny<CancellationToken>())).ReturnsAsync((Domain.Entities.SupportProject.SupportProject)null);
            var addFundingHistoryCommandHandler = new AddFundingHistoryCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => addFundingHistoryCommandHandler.Handle(command, _cancellationToken));

            // Optional: Verify the exception message
            Assert.Contains($"Support project with id {_mockSupportProject.Id} not found", exception.Message);
        }
    }
}
