using AutoFixture;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Moq;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.CreateSupportProjectNote.SetSupportProjectCaseStudyDetails;

namespace Dfe.ManageSchoolImprovement.Application.Tests.CommandHandlers.SupportProject.CaseStudy
{
    public class SetSupportProjectCaseStudyDetailsTests
    {
        private readonly Mock<ISupportProjectRepository> _mockSupportProjectRepository;
        private readonly Domain.Entities.SupportProject.SupportProject _mockSupportProject;
        private readonly CancellationToken _cancellationToken;

        public SetSupportProjectCaseStudyDetailsTests()
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
            var caseStudyCandidate = true;
            var caseStudyDetails = "test details";

            var command = new SetSupportProjectCaseStudyDetailsCommand(
                _mockSupportProject.Id, caseStudyCandidate, caseStudyDetails
            );
            _mockSupportProjectRepository.Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(x => x == _mockSupportProject.Id), It.IsAny<CancellationToken>())).ReturnsAsync(_mockSupportProject);
            var setSupportProjectCaseStudyDetailsCommandHandler = new SetSupportProjectCaseStudyDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await setSupportProjectCaseStudyDetailsCommandHandler.Handle(command, _cancellationToken);

            // Verify
            Assert.IsType<bool>(result);
            Assert.True(result);
            _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(It.Is<Domain.Entities.SupportProject.SupportProject>(x => x.CaseStudyCandidate == caseStudyCandidate && x.CaseStudyDetails == caseStudyDetails), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ValidEmptyCommand_UpdatesSupportProject()
        {
            // Arrange
            var command = new SetSupportProjectCaseStudyDetailsCommand(
                _mockSupportProject.Id, null, null
            );
            _mockSupportProjectRepository.Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(x => x == _mockSupportProject.Id), It.IsAny<CancellationToken>())).ReturnsAsync(_mockSupportProject);
            var setSupportProjectCaseStudyDetailsCommandHandler = new SetSupportProjectCaseStudyDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await setSupportProjectCaseStudyDetailsCommandHandler.Handle(command, _cancellationToken);

            // Verify
            Assert.IsType<bool>(result);
            Assert.True(result);
            _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(It.Is<Domain.Entities.SupportProject.SupportProject>(x => x.CaseStudyCandidate == null && x.CaseStudyDetails == null), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ProjectNotFound_ReturnsFalse()
        {
            var caseStudyCandidate = true;
            var caseStudyDetails = "test details";

            var command = new SetSupportProjectCaseStudyDetailsCommand(
                _mockSupportProject.Id, caseStudyCandidate, caseStudyDetails
            );
            _mockSupportProjectRepository.Setup(repo => repo.GetSupportProjectById(It.Is<SupportProjectId>(x => x == _mockSupportProject.Id), It.IsAny<CancellationToken>())).ReturnsAsync(null as Domain.Entities.SupportProject.SupportProject);
            var setSupportProjectCaseStudyDetailsCommandHandler = new SetSupportProjectCaseStudyDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await setSupportProjectCaseStudyDetailsCommandHandler.Handle(command, _cancellationToken);

            // Verify
            Assert.IsType<bool>(result);
            Assert.False(result);
            _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.SupportProject.SupportProject>(), It.IsAny<CancellationToken>()), Times.Never);

        }
    }
}
