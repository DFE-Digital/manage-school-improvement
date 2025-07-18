using AutoFixture;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.CreateSupportProject;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Moq;

namespace Dfe.ManageSchoolImprovement.Application.Tests.CommandHandlers.SupportProject.CreateSupportProject
{
    public class CreateSupportProjectCommandHandlerTests
    {
        private readonly Mock<ISupportProjectRepository> _mockSupportProjectRepository;
        private readonly CancellationToken _cancellationToken;
        private readonly Fixture _fixture;

        public CreateSupportProjectCommandHandlerTests()
        {
            _mockSupportProjectRepository = new Mock<ISupportProjectRepository>();
            _cancellationToken = CancellationToken.None;
            _fixture = new Fixture();
        }

        [Fact]
        public async Task Handle_ValidCommand_CreatesSupportProject()
        {
            // Arrange
            var command = new CreateSupportProjectCommand(
                schoolName: "Test School",
                schoolUrn: "123456",
                localAuthority: "Test LA",
                region: "Test Region",
                trustName: "Test Trust",
                trustReferenceNumber: "TR12345"
            );

            var expectedId = new SupportProjectId(1);
            _mockSupportProjectRepository
                .Setup(repo => repo.AddAsync(It.IsAny<Domain.Entities.SupportProject.SupportProject>(), It.IsAny<CancellationToken>()))
                .Callback<Domain.Entities.SupportProject.SupportProject, CancellationToken>((project, ct) => 
                {
                    // Simulate setting the ID when added to repository
                    var idField = typeof(Domain.Entities.SupportProject.SupportProject).GetProperty("Id");
                    idField?.SetValue(project, expectedId);
                });

            var handler = new CreateSupportProjectCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.NotNull(result);
            _mockSupportProjectRepository.Verify(repo => repo.AddAsync(
                It.Is<Domain.Entities.SupportProject.SupportProject>(sp => 
                    sp.SchoolName == command.schoolName &&
                    sp.SchoolUrn == command.schoolUrn &&
                    sp.LocalAuthority == command.localAuthority &&
                    sp.Region == command.region &&
                    sp.TrustName == command.trustName &&
                    sp.TrustReferenceNumber == command.trustReferenceNumber), 
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ValidCommandWithNullTrustInfo_CreatesSupportProject()
        {
            // Arrange
            var command = new CreateSupportProjectCommand(
                schoolName: "Test School",
                schoolUrn: "123456",
                localAuthority: "Test LA",
                region: "Test Region",
                trustName: null,
                trustReferenceNumber: null
            );

            var expectedId = new SupportProjectId(1);
            _mockSupportProjectRepository
                .Setup(repo => repo.AddAsync(It.IsAny<Domain.Entities.SupportProject.SupportProject>(), It.IsAny<CancellationToken>()))
                .Callback<Domain.Entities.SupportProject.SupportProject, CancellationToken>((project, ct) => 
                {
                    var idField = typeof(Domain.Entities.SupportProject.SupportProject).GetProperty("Id");
                    idField?.SetValue(project, expectedId);
                });

            var handler = new CreateSupportProjectCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.NotNull(result);
            _mockSupportProjectRepository.Verify(repo => repo.AddAsync(
                It.Is<Domain.Entities.SupportProject.SupportProject>(sp => 
                    sp.SchoolName == command.schoolName &&
                    sp.SchoolUrn == command.schoolUrn &&
                    sp.LocalAuthority == command.localAuthority &&
                    sp.Region == command.region &&
                    sp.TrustName == null &&
                    sp.TrustReferenceNumber == null), 
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [InlineData("", "123456", "Test LA", "Test Region")]
        [InlineData("Test School", "", "Test LA", "Test Region")]
        [InlineData("Test School", "123456", "", "Test Region")]
        [InlineData("Test School", "123456", "Test LA", "")]
        public async Task Handle_CommandWithEmptyRequiredFields_StillCreatesSupportProject(
            string schoolName, string schoolUrn, string localAuthority, string region)
        {
            // Arrange
            var command = new CreateSupportProjectCommand(
                schoolName: schoolName,
                schoolUrn: schoolUrn,
                localAuthority: localAuthority,
                region: region,
                trustName: "Test Trust",
                trustReferenceNumber: "TR12345"
            );

            var expectedId = new SupportProjectId(1);
            _mockSupportProjectRepository
                .Setup(repo => repo.AddAsync(It.IsAny<Domain.Entities.SupportProject.SupportProject>(), It.IsAny<CancellationToken>()))
                .Callback<Domain.Entities.SupportProject.SupportProject, CancellationToken>((project, ct) => 
                {
                    var idField = typeof(Domain.Entities.SupportProject.SupportProject).GetProperty("Id");
                    idField?.SetValue(project, expectedId);
                });

            var handler = new CreateSupportProjectCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            var result = await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.NotNull(result);
            _mockSupportProjectRepository.Verify(repo => repo.AddAsync(
                It.IsAny<Domain.Entities.SupportProject.SupportProject>(), 
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_RepositoryThrowsException_ExceptionPropagates()
        {
            // Arrange
            var command = new CreateSupportProjectCommand(
                schoolName: "Test School",
                schoolUrn: "123456",
                localAuthority: "Test LA",
                region: "Test Region",
                trustName: "Test Trust",
                trustReferenceNumber: "TR12345"
            );

            _mockSupportProjectRepository
                .Setup(repo => repo.AddAsync(It.IsAny<Domain.Entities.SupportProject.SupportProject>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException("Database error"));

            var handler = new CreateSupportProjectCommandHandler(_mockSupportProjectRepository.Object);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => 
                handler.Handle(command, _cancellationToken));
        }

        [Fact]
        public async Task Handle_ValidCommand_CallsRepositoryAddOnlyOnce()
        {
            // Arrange
            var command = new CreateSupportProjectCommand(
                schoolName: "Test School",
                schoolUrn: "123456",
                localAuthority: "Test LA",
                region: "Test Region",
                trustName: "Test Trust",
                trustReferenceNumber: "TR12345"
            );

            var handler = new CreateSupportProjectCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            await handler.Handle(command, _cancellationToken);

            // Assert
            _mockSupportProjectRepository.Verify(repo => repo.AddAsync(
                It.IsAny<Domain.Entities.SupportProject.SupportProject>(), 
                It.IsAny<CancellationToken>()), Times.Once);
            _mockSupportProjectRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Handle_ValidCommand_CreatesProjectWithCorrectProperties()
        {
            // Arrange
            var command = new CreateSupportProjectCommand(
                schoolName: "Amazing Primary School",
                schoolUrn: "987654",
                localAuthority: "Westminster Council",
                region: "London",
                trustName: "Excellence Academy Trust",
                trustReferenceNumber: "TR98765"
            );

            Domain.Entities.SupportProject.SupportProject? capturedProject = null;
            _mockSupportProjectRepository
                .Setup(repo => repo.AddAsync(It.IsAny<Domain.Entities.SupportProject.SupportProject>(), It.IsAny<CancellationToken>()))
                .Callback<Domain.Entities.SupportProject.SupportProject, CancellationToken>((project, ct) => 
                {
                    capturedProject = project;
                });

            var handler = new CreateSupportProjectCommandHandler(_mockSupportProjectRepository.Object);

            // Act
            await handler.Handle(command, _cancellationToken);

            // Assert
            Assert.NotNull(capturedProject);
            Assert.Equal("Amazing Primary School", capturedProject.SchoolName);
            Assert.Equal("987654", capturedProject.SchoolUrn);
            Assert.Equal("Westminster Council", capturedProject.LocalAuthority);
            Assert.Equal("London", capturedProject.Region);
            Assert.Equal("Excellence Academy Trust", capturedProject.TrustName);
            Assert.Equal("TR98765", capturedProject.TrustReferenceNumber);
        }
    }
} 