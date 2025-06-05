using AutoFixture;
using AutoMapper;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Models;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Moq;
using System.Linq.Expressions;

namespace Dfe.ManageSchoolImprovement.Application.Tests.SupportProject.Queries
{
    public class SupportProjectQueryServiceTests
    {
        private readonly Mock<ISupportProjectRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly SupportProjectQueryService _service;
        private readonly IFixture fixture;

        public SupportProjectQueryServiceTests()
        {
            fixture = new Fixture();
            _mockRepository = new Mock<ISupportProjectRepository>();
            _mockMapper = new Mock<IMapper>();
            _service = new SupportProjectQueryService(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllSupportProjects_ShouldReturnMappedDtos()
        {
            // Arrange
            var projects = GetSchoolProjects(2);
            var supportProjectDtos = GetSupportProjectDtos(projects);

            _mockRepository.Setup(r => r.FetchAsync(It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, bool>>>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync(projects);

            foreach (var supportProjectDto in supportProjectDtos)
            {
                _mockMapper.Setup(m => m.Map<SupportProjectDto>(It.IsAny<Domain.Entities.SupportProject.SupportProject>())).Returns(supportProjectDto);
            }

            // Act
            var result = await _service.GetAllSupportProjects(CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(supportProjectDtos.Count, result.Value!.Count());
            VerifySupportProjectProperties(result.Value!, projects);
            _mockRepository.Verify(r => r.FetchAsync(It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, bool>>>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SearchForSupportProjects_ShouldReturnPagedData()
        {
            // Arrange
            var projects = GetSchoolProjects();
            var supportProjectDtos = GetSupportProjectDtos(projects);
            var totalCount = 1;

            _mockRepository.Setup(r => r.SearchForSupportProjects(It.IsAny<string?>(), It.IsAny<IEnumerable<string>?>(),
                    It.IsAny<IEnumerable<string>?>(), It.IsAny<IEnumerable<string>?>(), It.IsAny<IEnumerable<string>?>(),
                    It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((projects, totalCount));
            foreach (var supportProjectDto in supportProjectDtos)
            {
                _mockMapper.Setup(m => m.Map<SupportProjectDto>(It.IsAny<Domain.Entities.SupportProject.SupportProject>())).Returns(supportProjectDto);
            }

            // Act
            var result = await _service.SearchForSupportProjects(null, null, null, null, null, "/path", 1, 10, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(projects.Count, result.Value.Data.Count());
            VerifySupportProjectProperties(result.Value.Data!, projects);
        }

        [Fact]
        public async Task GetSupportProject_ShouldReturnMappedDto_WhenProjectExists()
        {
            // Arrange
            var project = GetSchoolProjects().First();
            var supportProjectDto = GetSupportProjectDtos(GetSchoolProjects()).First();

            _mockRepository.Setup(r => r.GetSupportProjectById(It.IsAny<SupportProjectId>(), It.IsAny<CancellationToken>()))!.ReturnsAsync(project);

            _mockMapper.Setup(m => m.Map<SupportProjectDto?>(project)).Returns(supportProjectDto);

            // Act
            var result = await _service.GetSupportProject(1, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            _mockRepository.Verify(r => r.GetSupportProjectById(It.IsAny<SupportProjectId>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetSupportProject_ShouldReturnFailure_WhenProjectNotFound()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetSupportProjectById(It.IsAny<SupportProjectId>(), It.IsAny<CancellationToken>()))!.ReturnsAsync((Domain.Entities.SupportProject.SupportProject?)null);

            // Act
            var result = await _service.GetSupportProject(1, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task GetAllProjectLocalAuthorities_ShouldReturnSuccess_WhenDataExists()
        {
            // Arrange
            var localAuthorities = new List<string> { "Authority1", "Authority2" };

            _mockRepository.Setup(r => r.GetAllProjectLocalAuthorities(It.IsAny<CancellationToken>()))
                           .ReturnsAsync(localAuthorities);

            // Act
            var result = await _service.GetAllProjectLocalAuthorities(CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(localAuthorities, result.Value);
        }

        [Fact]
        public async Task GetAllProjectLocalAuthorities_ShouldReturnFailure_WhenDataIsNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAllProjectLocalAuthorities(It.IsAny<CancellationToken>()))!.ReturnsAsync((IEnumerable<string>?)null);

            // Act
            var result = await _service.GetAllProjectLocalAuthorities(CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task GetAllProjectRegions_ShouldReturnSuccess_WhenDataExists()
        {
            // Arrange
            var regions = new List<string> { "Region1", "Region2" };

            _mockRepository.Setup(r => r.GetAllProjectRegions(It.IsAny<CancellationToken>()))
                           .ReturnsAsync(regions);

            // Act
            var result = await _service.GetAllProjectRegions(CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(regions, result.Value);
        }

        [Fact]
        public async Task GetAllProjectRegions_ShouldReturnFailure_WhenDataIsNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAllProjectRegions(It.IsAny<CancellationToken>()))!.ReturnsAsync((IEnumerable<string>?)null);

            // Act
            var result = await _service.GetAllProjectRegions(CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Value);
        }


        [Fact]
        public async Task GetAllProjectAssignedUsers_ShouldReturnSuccess_WhenDataExists()
        {
            // Arrange
            var users = new List<string> { "User 1", "User 2" };

            _mockRepository.Setup(r => r.GetAllProjectAssignedUsers(It.IsAny<CancellationToken>()))
                           .ReturnsAsync(users);

            // Act
            var result = await _service.GetAllProjectLocalAuthorities(CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(users, result.Value);
        }

        [Fact]
        public async Task GetAllProjectAssignedUsers_ShouldReturnFailure_WhenDataIsNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAllProjectAssignedUsers(It.IsAny<CancellationToken>()))!.ReturnsAsync((IEnumerable<string>?)null);

            // Act
            var result = await _service.GetAllProjectAssignedUsers(CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Value);
        }

        private List<Domain.Entities.SupportProject.SupportProject> GetSchoolProjects(int count = 1)
        {
            var projects = new List<Domain.Entities.SupportProject.SupportProject>();
            {
                for (int i = 0; i < count; i++)
                {
                    projects.Add(new Domain.Entities.SupportProject.SupportProject(new SupportProjectId(i + 1), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>()));
                }
                return projects;
            }
        }

        private static List<SupportProjectDto> GetSupportProjectDtos(List<Domain.Entities.SupportProject.SupportProject> supportProjects)
        {
            var supportProjectDto = new List<SupportProjectDto>();
            foreach (var project in supportProjects)
            {
                supportProjectDto.Add(new SupportProjectDto(project.Id.Value, project.CreatedOn, project.SchoolName, project.SchoolUrn, project.LocalAuthority, project.Region));
            }
            return supportProjectDto;
        }

        private static void VerifySupportProjectProperties(IEnumerable<SupportProjectDto> schools, IList<Domain.Entities.SupportProject.SupportProject> projects)
        {
            foreach (var item in schools)
            {
                var project = projects.FirstOrDefault(p => p.Id.Value == item.Id);
                Assert.NotNull(project);
                Assert.Equal(item.Id, project.Id.Value);
                Assert.Equal(item.SchoolName, project.SchoolName);
                Assert.Equal(item.SchoolUrn, project.SchoolUrn);
                Assert.Equal(item.LocalAuthority, project.LocalAuthority);
                Assert.Equal(item.Region, project.Region);
            }
        }

    }
}
