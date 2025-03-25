using AutoFixture;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Moq;
using System.Reflection;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.FundingHistory.EditFundingHistory;

namespace Dfe.ManageSchoolImprovement.Application.Tests.SupportProject.Commands.FundingHistory
{
    public class EditFundingHistoryTests
    {
        private readonly Mock<ISupportProjectRepository> _mockSupportProjectRepository;
        private readonly Domain.Entities.SupportProject.SupportProject _mockSupportProject;
        private readonly CancellationToken _cancellationToken;

        public EditFundingHistoryTests()
        {

            _mockSupportProjectRepository = new Mock<ISupportProjectRepository>();
            var fixture = new Fixture();
            _mockSupportProject = fixture.Create<Domain.Entities.SupportProject.SupportProject>();

            // Use reflection to access the private field
            var field = typeof(Domain.Entities.SupportProject.SupportProject).GetField("_fundingHistories", BindingFlags.NonPublic | BindingFlags.Instance);

            if (field == null)
            {
                throw new InvalidOperationException("Private backing field not found.");
            }

            // Set the field using AutoFixture
            var children = fixture.CreateMany<Domain.Entities.SupportProject.FundingHistory>(1).ToList();
            field.SetValue(_mockSupportProject, children);

            _cancellationToken = CancellationToken.None;
        }

        [Fact]
        public async Task Handle_ValidCommand_UpdatesSupportProject()
        {
            // Arrange
            var fundingType = "funding type";
            var fundingAmount = (decimal)100.10;
            var financialYear = "financial year";
            var fundingRounds = 10;
            var comments = "comments";

            var command = new EditFundingHistoryCommand(
                _mockSupportProject.FundingHistories.First().Id,
                _mockSupportProject.Id,
                fundingType, fundingAmount, financialYear, fundingRounds, comments
            );
            _mockSupportProjectRepository.Setup(repo => repo.GetSupportProjectById(It.IsAny<SupportProjectId>(), It.IsAny<CancellationToken>())).ReturnsAsync(_mockSupportProject);
            var editFundingHistoryCommandHandler = new EditFundingHistoryCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await editFundingHistoryCommandHandler.Handle(command, _cancellationToken);

            // Verify
            Assert.True(result);
            _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.SupportProject.SupportProject>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ValidEmptyCommand_UpdatesSupportProject()
        {
            // Arrange
            var command = new EditFundingHistoryCommand(
                _mockSupportProject.FundingHistories.First().Id,
                _mockSupportProject.Id,
                null, 0, null, 0, null
            );

            _mockSupportProjectRepository.Setup(repo => repo.GetSupportProjectById(It.IsAny<SupportProjectId>(), It.IsAny<CancellationToken>())).ReturnsAsync(_mockSupportProject);
            var editFundingHistoryCommandHandler = new EditFundingHistoryCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await editFundingHistoryCommandHandler.Handle(command, _cancellationToken);

            // Verify
            Assert.True(result);
            _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.SupportProject.SupportProject>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ProjectNotFound_ReturnsFalse()
        {
            // Arrange
            var fundingType = "funding type";
            var fundingAmount = (decimal)100.10;
            var financialYear = "financial year";
            var fundingRounds = 10;
            var comments = "comments";

            var command = new EditFundingHistoryCommand(
                _mockSupportProject.FundingHistories.First().Id,
                _mockSupportProject.Id,
                fundingType, fundingAmount, financialYear, fundingRounds, comments
            );

            _mockSupportProjectRepository.Setup(repo => repo.GetSupportProjectById(It.IsAny<SupportProjectId>(), It.IsAny<CancellationToken>())).ReturnsAsync((Domain.Entities.SupportProject.SupportProject)null);
            var editFundingHistoryCommandHandler = new EditFundingHistoryCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await editFundingHistoryCommandHandler.Handle(command, _cancellationToken);

            // Verify
            Assert.False(result);
        }
    }
}
