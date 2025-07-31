using AutoFixture;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Moq;
using System.Linq.Expressions;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject.SetAdviserDetails;

namespace Dfe.ManageSchoolImprovement.Application.Tests.CommandHandlers.SupportProject.UpdateSupportProject
{
    public class SetAdviserDetailsTests
    {
        private readonly Mock<ISupportProjectRepository> _mockSupportProjectRepository;
        private readonly Domain.Entities.SupportProject.SupportProject _mockSupportProject;
        private readonly CancellationToken _cancellationToken;


        public SetAdviserDetailsTests()
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
            string? adviserEmailAddress = "rise.test.test@gov.uk";
            DateTime? assignedDate = DateTime.UtcNow;
            string? assignedAdviserFullName = "Test Adviser";

            var command = new SetAdviserDetailsCommand(
                _mockSupportProject.Id,
                assignedDate,
                adviserEmailAddress,
                assignedAdviserFullName
            );
            _mockSupportProjectRepository.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(_mockSupportProject);
            var SetAdviserDetailsCommandHandler = new SetAdviserDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await SetAdviserDetailsCommandHandler.Handle(command, _cancellationToken);

            // Verify
            Assert.True(result);
            _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.SupportProject.SupportProject>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ValidEmptyCommand_UpdatesSupportProject()
        {
            // Arrange
            var command = new SetAdviserDetailsCommand(
                _mockSupportProject.Id,
                null,
                null,
                null
            );
            _mockSupportProjectRepository.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(_mockSupportProject);
            var SetAdviserDetailsCommandHandler = new SetAdviserDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await SetAdviserDetailsCommandHandler.Handle(command, _cancellationToken);

            // Verify
            Assert.True(result);
            _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.SupportProject.SupportProject>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ProjectNotFound_ReturnsFalse()
        {
            // Arrange
            string? adviserEmailAddress = "rise.test.test@gov.uk";
            DateTime? assignedDate = DateTime.UtcNow;
            string? assignedAdviserFullName = "Test Adviser";

            var command = new SetAdviserDetailsCommand(
                _mockSupportProject.Id,
                assignedDate,
                adviserEmailAddress,
                assignedAdviserFullName
            );

            _mockSupportProjectRepository.Setup(repo => repo.FindAsync(
                It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, bool>>>(),
                It.IsAny<CancellationToken>()))!.ReturnsAsync((Domain.Entities.SupportProject.SupportProject)null!);

            var setAdviserDetailsCommandHandler = new SetAdviserDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await setAdviserDetailsCommandHandler.Handle(command, _cancellationToken);

            // Verify
            Assert.False(result);
        }
    }
}
