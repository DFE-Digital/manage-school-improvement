using AutoFixture;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Moq;
using System.Linq.Expressions;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject.SetDueDiligenceOnPreferredSupportingOrganisationDetails;

namespace Dfe.ManageSchoolImprovement.Application.Tests.CommandHandlers.SupportProject.UpdateSupportProject
{
    public class SetDueDiligenceOnPreferredSupportingOrganisationDetailsHandlerTests
    {
        private readonly Mock<ISupportProjectRepository> _mockSupportProjectRepository;
        private readonly Domain.Entities.SupportProject.SupportProject _mockSupportProject;
        private readonly CancellationToken _cancellationToken;

        public SetDueDiligenceOnPreferredSupportingOrganisationDetailsHandlerTests()
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
            bool? checkOrganisationHasCapacityAndWillingToProvideSupport = true;
            bool? checkChoiceWithTrustRelationshipManagerOrLaLead = false;
            bool? discussChoiceWithSfso = true;
            bool? checkTheOrganisationHasAVendorAccount = true;
            DateTime? dateDueDiligenceCompleted = DateTime.UtcNow;

            var command = new SetDueDiligenceOnPreferredSupportingOrganisationDetailsCommand(
                _mockSupportProject.Id,
                checkOrganisationHasCapacityAndWillingToProvideSupport,
                checkChoiceWithTrustRelationshipManagerOrLaLead,
                discussChoiceWithSfso,
                checkTheOrganisationHasAVendorAccount, dateDueDiligenceCompleted
            );
            _mockSupportProjectRepository.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(_mockSupportProject);
            var setDueDiligenceOnPreferredSupportingOrganisationDetailsCommandHandler = new SetDueDiligenceOnPreferredSupportingOrganisationDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await setDueDiligenceOnPreferredSupportingOrganisationDetailsCommandHandler.Handle(command, _cancellationToken);

            // Verify
            Assert.True(result);
            _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.SupportProject.SupportProject>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ValidEmptyCommand_UpdatesSupportProject()
        {
            // Arrange 
            var command = new SetDueDiligenceOnPreferredSupportingOrganisationDetailsCommand(_mockSupportProject.Id, null, null, null, null, null);

            _mockSupportProjectRepository.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(_mockSupportProject);
            var setDueDiligenceOnPreferredSupportingOrganisationDetailsCommandHandler = new SetDueDiligenceOnPreferredSupportingOrganisationDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await setDueDiligenceOnPreferredSupportingOrganisationDetailsCommandHandler.Handle(command, _cancellationToken);

            // Verify
            Assert.True(result);
            _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.SupportProject.SupportProject>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ProjectNotFound_ReturnsFalse()
        {
            // Arrange
            bool? checkOrganisationHasCapacityAndWillingToProvideSupport = true;
            bool? checkChoiceWithTrustRelationshipManagerOrLaLead = false;
            bool? discussChoiceWithSfso = true;
            bool? checkTheOrganisationHasAVendorAccount = true;
            DateTime? dateDueDiligenceCompleted = DateTime.UtcNow;

            var command = new SetDueDiligenceOnPreferredSupportingOrganisationDetailsCommand(
                _mockSupportProject.Id,
                checkOrganisationHasCapacityAndWillingToProvideSupport,
                checkChoiceWithTrustRelationshipManagerOrLaLead,
                discussChoiceWithSfso,
                checkTheOrganisationHasAVendorAccount, dateDueDiligenceCompleted
            );
            _mockSupportProjectRepository.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync((Domain.Entities.SupportProject.SupportProject)null);
            var setDueDiligenceOnPreferredSupportingOrganisationDetailsCommandHandler = new SetDueDiligenceOnPreferredSupportingOrganisationDetailsCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await setDueDiligenceOnPreferredSupportingOrganisationDetailsCommandHandler.Handle(command, _cancellationToken);

            // Verify
            Assert.False(result);
        }
    }
}
