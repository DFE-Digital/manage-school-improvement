using AutoFixture;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.SupportProjectContacts; 
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Utils;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace Dfe.ManageSchoolImprovement.Application.Tests.CommandHandlers.SupportProject.SupportProjectContacts
{
    public class CreateSupportProjectContactTests
    {
        private readonly Mock<ISupportProjectRepository> _mockSupportProjectRepository;
        private readonly Mock<IDateTimeProvider> _mockDateTimeProvider;
        private readonly CreateSupportProjectContactHandler _handler;
        private readonly Domain.Entities.SupportProject.SupportProject _mockSupportProject;
        private readonly CancellationToken _cancellationToken;

        public CreateSupportProjectContactTests()
        {
            _mockSupportProjectRepository = new Mock<ISupportProjectRepository>();
            _mockDateTimeProvider = new Mock<IDateTimeProvider>(); 
            var fixture = new Fixture();
            _mockSupportProject = fixture.Create<Domain.Entities.SupportProject.SupportProject>();
            _cancellationToken = CancellationToken.None;
            _handler = new CreateSupportProjectContactHandler(_mockSupportProjectRepository.Object, _mockDateTimeProvider.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_ShouldAddContact()
        {
            // Arrange 
            var command = new CreateSupportProjectContactCommand(
                _mockSupportProject.Id,
                "John Doe",
                RolesIds.TrustAccountingOfficer,
                "Other Role",
                "Test Organisation",
                "john.doe@example.com",
                "07911123456",
                "Author"
            ); 
            _mockSupportProjectRepository.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(_mockSupportProject);

            // Act
            var result = await _handler.Handle(command, _cancellationToken);

            // Assert
            Assert.NotNull(result); 
            Assert.IsType<SupportProjectContactId>(result); 

            _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.SupportProject.SupportProject>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
