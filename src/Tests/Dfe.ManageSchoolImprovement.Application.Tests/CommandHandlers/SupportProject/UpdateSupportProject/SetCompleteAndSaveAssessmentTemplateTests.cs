using AutoFixture;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Moq;
using System.Linq.Expressions;

namespace Dfe.ManageSchoolImprovement.Application.Tests.CommandHandlers.SupportProject.UpdateSupportProject
{
    public class SetCompleteAndSaveInitialDiagnosisTemplateTests
    {
        private readonly Mock<ISupportProjectRepository> _mockSupportProjectRepository;
        private readonly Domain.Entities.SupportProject.SupportProject _mockSupportProject;
        private readonly CancellationToken _cancellationToken;

        public SetCompleteAndSaveInitialDiagnosisTemplateTests()
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
            var savedAssessmentTemplateInSharePointDate = DateTime.UtcNow;
            var hasTalkToAdviserAboutFindings = true;
            var hasCompleteAssessmentTemplate = true;

            var command = new SetCompleteAndSaveInitialDiagnosisTemplateCommand(
                _mockSupportProject.Id,
                savedAssessmentTemplateInSharePointDate,
                hasTalkToAdviserAboutFindings,
                hasCompleteAssessmentTemplate
            );
            _mockSupportProjectRepository.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(_mockSupportProject);
            var handler = new SetCompleteAndSaveInitialDiagnosisTemplateCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Verify
            Assert.True(result);
            _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.SupportProject.SupportProject>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ValidEmptyCommand_UpdatesSupportProject()
        {
            // Arrange
            var command = new SetCompleteAndSaveInitialDiagnosisTemplateCommand(
                _mockSupportProject.Id,
                null,
                null,
                null
            );
            _mockSupportProjectRepository.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(_mockSupportProject);
            var handler = new SetCompleteAndSaveInitialDiagnosisTemplateCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Verify
            Assert.True(result);
            _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.SupportProject.SupportProject>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ProjectNotFound_ReturnsFalse()
        {
            // Arrange
            var savedAssessmentTemplateInSharePointDate = DateTime.UtcNow;
            var hasTalkToAdviserAboutFindings = true;
            var hasCompleteAssessmentTemplate = true;

            var command = new SetCompleteAndSaveInitialDiagnosisTemplateCommand(
                _mockSupportProject.Id,
                savedAssessmentTemplateInSharePointDate,
                hasTalkToAdviserAboutFindings,
                hasCompleteAssessmentTemplate
            );
            
            _mockSupportProjectRepository.Setup(repo => repo.FindAsync(
                It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, bool>>>(), 
                It.IsAny<CancellationToken>()))!.ReturnsAsync((Domain.Entities.SupportProject.SupportProject)null!);
            var handler = new SetCompleteAndSaveInitialDiagnosisTemplateCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Verify
            Assert.False(result);
        }
    }
}
