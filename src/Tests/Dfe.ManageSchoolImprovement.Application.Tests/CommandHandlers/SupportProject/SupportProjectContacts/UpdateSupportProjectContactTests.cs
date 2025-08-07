using AutoFixture;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.SupportProjectContacts;
using Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Utils;
using Moq;
using System.Linq.Expressions;

namespace Dfe.ManageSchoolImprovement.Application.Tests.CommandHandlers.SupportProject.SupportProjectContacts
{
    public class UpdateSupportProjectContactTests
    {
        private readonly Mock<ISupportProjectRepository> _mockSupportProjectRepository;
        private readonly Mock<IDateTimeProvider> _mockDateTimeProvider;
        private readonly UpdateSupportProjectContactHandler _handler;
        private readonly Domain.Entities.SupportProject.SupportProject _mockSupportProject;
        private readonly CancellationToken _cancellationToken;

        public UpdateSupportProjectContactTests()
        {
            _mockSupportProjectRepository = new Mock<ISupportProjectRepository>();
            _mockDateTimeProvider = new Mock<IDateTimeProvider>();
            var fixture = new Fixture();
            _mockSupportProject = fixture.Create<Domain.Entities.SupportProject.SupportProject>();
            _cancellationToken = CancellationToken.None;
            _handler = new UpdateSupportProjectContactHandler(_mockSupportProjectRepository.Object, _mockDateTimeProvider.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_ShouldUpdateContact()
        {
            // Arrange 
            var author = "Author";
            var createdOn = DateTime.UtcNow;

            var details = new SupportProjectContactDetails
            {
                Name = "John",
                RoleId = RolesIds.ChairOfGovernors,
                OtherRoleName = null,
                Organisation = "Organisation",
                Email = "john@school.gov.uk",
                Phone = "0123456789"
            };

            var supportProjectContactId = new SupportProjectContactId(Guid.NewGuid());
            _mockSupportProject.AddContact(
                supportProjectContactId,
                details,
                author,
                createdOn,
                _mockSupportProject.Id!);

            var roleId = RolesIds.Other;
            var otherRoleName = "Other Role";
            var command = new UpdateSupportProjectContactCommand(
                _mockSupportProject.Id!,
                supportProjectContactId,
                "John Doe",
                roleId,
                otherRoleName, details.Organisation, details.Email, details.Phone, author);

            _mockSupportProjectRepository.Setup(repo => repo.GetSupportProjectById(_mockSupportProject.Id!, _cancellationToken)).ReturnsAsync(_mockSupportProject);

            // Act
            var result = await _handler.Handle(command, _cancellationToken);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<SupportProjectContactId>(result);
            _mockSupportProjectRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.SupportProject.SupportProject>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
