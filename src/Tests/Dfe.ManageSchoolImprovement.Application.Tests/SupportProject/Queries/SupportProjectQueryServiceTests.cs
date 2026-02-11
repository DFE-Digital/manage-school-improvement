using AutoFixture;
using AutoMapper;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Models;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject;
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

            _mockRepository.Setup(r =>
                    r.FetchAsync(It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, bool>>>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(projects);

            foreach (var supportProjectDto in supportProjectDtos)
            {
                _mockMapper.Setup(m =>
                        m.Map<SupportProjectDto>(It.IsAny<Domain.Entities.SupportProject.SupportProject>()))
                    .Returns(supportProjectDto);
            }

            // Act
            var result = await _service.GetAllSupportProjects(CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(supportProjectDtos.Count, result.Value!.Count());
            VerifySupportProjectProperties(result.Value!, projects);
            _mockRepository.Verify(
                r => r.FetchAsync(It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, bool>>>(),
                    It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SearchForSupportProjects_ShouldReturnPagedData()
        {
            // Arrange
            var projects = GetSchoolProjects();
            var supportProjectDtos = GetSupportProjectDtos(projects);
            var totalCount = 1;

            _mockRepository.Setup(r => r.SearchForSupportProjects(It.IsAny<SupportProjectSearchCriteria>(),
                    It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((projects, totalCount));
            foreach (var supportProjectDto in supportProjectDtos)
            {
                _mockMapper.Setup(m =>
                        m.Map<SupportProjectDto>(It.IsAny<Domain.Entities.SupportProject.SupportProject>()))
                    .Returns(supportProjectDto);
            }

            var supportProjectSearchRequest = new SupportProjectSearchRequest
            {
                Years = new[] { "2024" },  // Add some sample years
                Months = new[] { "2023 1", "2023 2" }  // Add some sample months
            };

            // Act
            var result = await _service.SearchForSupportProjects(supportProjectSearchRequest, CancellationToken.None);

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
            var project = GetSchoolProjects()[0];
            var supportProjectDto = GetSupportProjectDtos(GetSchoolProjects())[0];

            _mockRepository.Setup(r =>
                    r.GetSupportProjectById(It.IsAny<SupportProjectId>(), It.IsAny<CancellationToken>()))!
                .ReturnsAsync(project);

            _mockMapper.Setup(m => m.Map<SupportProjectDto?>(project)).Returns(supportProjectDto);

            // Act
            var result = await _service.GetSupportProject(1, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            _mockRepository.Verify(
                r => r.GetSupportProjectById(It.IsAny<SupportProjectId>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetSupportProject_ShouldReturnFailure_WhenProjectNotFound()
        {
            // Arrange
            _mockRepository.Setup(r =>
                    r.GetSupportProjectById(It.IsAny<SupportProjectId>(), It.IsAny<CancellationToken>()))!
                .ReturnsAsync((Domain.Entities.SupportProject.SupportProject?)null);

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
            _mockRepository.Setup(r => r.GetAllProjectLocalAuthorities(It.IsAny<CancellationToken>()))!.ReturnsAsync(
                (IEnumerable<string>?)null);

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
            _mockRepository.Setup(r => r.GetAllProjectRegions(It.IsAny<CancellationToken>()))!.ReturnsAsync(
                (IEnumerable<string>?)null);

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
            var result = await _service.GetAllProjectAssignedUsers(CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(users, result.Value);
        }

        [Fact]
        public async Task GetAllProjectAssignedUsers_ShouldReturnFailure_WhenDataIsNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAllProjectAssignedUsers(It.IsAny<CancellationToken>()))!.ReturnsAsync(
                (IEnumerable<string>?)null);

            // Act
            var result = await _service.GetAllProjectAssignedUsers(CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task GetAllProjectAssignedAdvisers_ShouldReturnSuccess_WhenDataExists()
        {
            // Arrange
            var advisers = new List<string> { "User1@adviser.com", "User2@adviser.com" };

            _mockRepository.Setup(r => r.GetAllProjectAssignedAdvisers(It.IsAny<CancellationToken>()))
                .ReturnsAsync(advisers);

            // Act
            var result = await _service.GetAllProjectAssignedAdvisers(CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(advisers, result.Value);
        }

        [Fact]
        public async Task GetAllProjectAssignedAdvisers_ShouldReturnFailure_WhenDataIsNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAllProjectAssignedAdvisers(It.IsAny<CancellationToken>()))!.ReturnsAsync(
                (IEnumerable<string>?)null);

            // Act
            var result = await _service.GetAllProjectAssignedAdvisers(CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task GetAllProjectTrusts_ShouldReturnSuccess_WhenDataExists()
        {
            // Arrange
            var trusts = new List<string> { "Trust A", "Trust B", "Trust C" };

            _mockRepository.Setup(r => r.GetAllProjectTrusts(It.IsAny<CancellationToken>()))
                .ReturnsAsync(trusts);

            // Act
            var result = await _service.GetAllProjectTrusts(CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(trusts, result.Value);
            _mockRepository.Verify(r => r.GetAllProjectTrusts(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetAllProjectTrusts_ShouldReturnFailure_WhenDataIsNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAllProjectTrusts(It.IsAny<CancellationToken>()))!
                .ReturnsAsync((IEnumerable<string>?)null);

            // Act
            var result = await _service.GetAllProjectTrusts(CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Value);
            _mockRepository.Verify(r => r.GetAllProjectTrusts(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetAllProjectTrusts_ShouldReturnEmptyList_WhenNoTrustsExist()
        {
            // Arrange
            var emptyTrusts = new List<string>();

            _mockRepository.Setup(r => r.GetAllProjectTrusts(It.IsAny<CancellationToken>()))
                .ReturnsAsync(emptyTrusts);

            // Act
            var result = await _service.GetAllProjectTrusts(CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value);
            _mockRepository.Verify(r => r.GetAllProjectTrusts(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void AddAllSelectedMonths_WhenNoMonthsExistForYear_ShouldAddAllMonths()
        {
            // Arrange
            var years = new[] { "2024" };
            var months = new[] { "2023 January", "2023 February" };

            // Act
            var result = _service.AddAllSelectedMonths(years, months);

            // Assert
            Assert.Equal(14, result.Length); // 2 existing months + 12 new months
            Assert.Contains("2024 January", result);
            Assert.Contains("2024 December", result);
            Assert.Contains("2023 January", result);
            Assert.Contains("2023 February", result);
        }

        [Fact]
        public void AddAllSelectedMonths_WhenMonthsExistForYear_ShouldNotAddMonths()
        {
            // Arrange
            var years = new[] { "2024" };
            var months = new[] { "2024 1", "2024 2" };

            // Act
            var result = _service.AddAllSelectedMonths(years, months);

            // Assert
            Assert.Equal(2, result.Length);
            Assert.Contains("2024 1", result);
            Assert.Contains("2024 2", result);
        }

        [Fact]
        public void AddAllSelectedMonths_WithMultipleYears_ShouldAddMonthsForMissingYearsOnly()
        {
            // Arrange
            var years = new[] { "2023", "2024" };
            var months = new[] { "2023 January", "2023 February" };

            // Act
            var result = _service.AddAllSelectedMonths(years, months);

            // Assert
            Assert.Equal(14, result.Length); // 2 existing months + 12 new months for 2024
            Assert.Contains("2023 January", result);
            Assert.Contains("2023 February", result);
            Assert.Contains("2024 January", result);
            Assert.Contains("2024 December", result);
        }

        [Fact]
        public void AddAllSelectedMonths_WithNullInputs_ShouldHandleGracefully()
        {
            // Arrange
            IEnumerable<string>? years = new List<string>();
            IEnumerable<string>? months = new List<string>();

            // Act
            var result = _service.AddAllSelectedMonths(years, months);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void AddAllSelectedMonths_WithEmptyInputs_ShouldReturnEmptyArray()
        {
            // Arrange
            var years = Array.Empty<string>();
            var months = Array.Empty<string>();

            // Act
            var result = _service.AddAllSelectedMonths(years, months);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllProjectYears_ShouldReturnSuccess_WhenDataExists()
        {
            // Arrange
            var years = new List<string> { "2023", "2024" };

            _mockRepository.Setup(r => r.GetAllProjectYears(It.IsAny<CancellationToken>()))
                .ReturnsAsync(years);

            // Act
            var result = await _service.GetAllProjectYears(CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(years, result.Value);
            _mockRepository.Verify(r => r.GetAllProjectYears(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetAllProjectYears_ShouldReturnFailure_WhenDataIsNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAllProjectYears(It.IsAny<CancellationToken>()))!
                .ReturnsAsync((IEnumerable<string>?)null);

            // Act
            var result = await _service.GetAllProjectYears(CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Value);
            _mockRepository.Verify(r => r.GetAllProjectYears(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetAllProjectYears_ShouldReturnEmptyList_WhenNoYearsExist()
        {
            // Arrange
            var emptyYears = new List<string>();

            _mockRepository.Setup(r => r.GetAllProjectYears(It.IsAny<CancellationToken>()))
                .ReturnsAsync(emptyYears);

            // Act
            var result = await _service.GetAllProjectYears(CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value);
            _mockRepository.Verify(r => r.GetAllProjectYears(It.IsAny<CancellationToken>()), Times.Once);
        }

        private List<Domain.Entities.SupportProject.SupportProject> GetSchoolProjects(int count = 1)
        {
            var projects = new List<Domain.Entities.SupportProject.SupportProject>();
            for (int i = 0; i < count; i++)
            {
                projects.Add(new Domain.Entities.SupportProject.SupportProject(new SupportProjectId(i + 1),
                    fixture.Create<string>(), fixture.Create<string>(), fixture.Create<string>(),
                    fixture.Create<string>()));
            }

            return projects;
        }

        private static List<SupportProjectDto> GetSupportProjectDtos(
            List<Domain.Entities.SupportProject.SupportProject> supportProjects)
        {
            var supportProjectDto = new List<SupportProjectDto>();
            foreach (var project in supportProjects)
            {
                supportProjectDto.Add(new SupportProjectDto(project.Id!.Value, project.CreatedOn, project.LastModifiedOn, project.SchoolName,
                    project.SchoolUrn, project.LocalAuthority, project.Region));
            }

            return supportProjectDto;
        }

        private static void VerifySupportProjectProperties(IEnumerable<SupportProjectDto> schools,
            IList<Domain.Entities.SupportProject.SupportProject> projects)
        {
            foreach (var item in schools)
            {
                var project = projects.FirstOrDefault(p => p.Id!.Value == item.Id);
                Assert.NotNull(project);
                Assert.Equal(item.Id, project.Id!.Value);
                Assert.Equal(item.SchoolName, project.SchoolName);
                Assert.Equal(item.SchoolUrn, project.SchoolUrn);
                Assert.Equal(item.LocalAuthority, project.LocalAuthority);
                Assert.Equal(item.Region, project.Region);
            }
        }

        [Fact]
        public async Task GetAllProjectStatuses_ShouldReturnStatuses_WhenRepositoryReturnsStatuses()
        {
            // Arrange
            var statuses = new List<string> { "Open", "Closed", "InProgress" };
            _mockRepository.Setup(r => r.GetAllProjectStatuses(It.IsAny<CancellationToken>()))
                .ReturnsAsync(statuses);

            // Act
            var result = await _service.GetAllProjectStatuses(CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(statuses, result.Value);
        }

        [Fact]
        public async Task GetAllProjectStatuses_ShouldReturnFailure_WhenRepositoryReturnsNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetAllProjectStatuses(It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<string>?)null);

            // Act
            var result = await _service.GetAllProjectStatuses(CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Value);
        }
    }
}