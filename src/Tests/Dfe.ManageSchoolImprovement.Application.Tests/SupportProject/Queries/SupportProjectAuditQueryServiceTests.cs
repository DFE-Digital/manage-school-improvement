using AutoFixture;
using AutoMapper;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Models;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.Entities.Audit;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Moq;
using System.Linq.Expressions;

namespace Dfe.ManageSchoolImprovement.Application.Tests.SupportProject.Queries;

public class SupportProjectAuditQueryServiceTests
{
    private readonly Mock<ISupportProjectAuditRepository> _mockAuditRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly SupportProjectAuditQueryService _service;
    private readonly IFixture _fixture;

    public SupportProjectAuditQueryServiceTests()
    {
        _fixture = new Fixture();
        _mockAuditRepository = new Mock<ISupportProjectAuditRepository>();
        _mockMapper = new Mock<IMapper>();
        _service = new SupportProjectAuditQueryService(_mockAuditRepository.Object, _mockMapper.Object);
    }

    #region GetSupportProjectHistoryAsync Tests

    [Fact]
    public async Task GetSupportProjectHistoryAsync_ShouldReturnMappedDtos_WhenProjectsExist()
    {
        // Arrange
        var supportProjectId = 1;
        var projects = CreateSupportProjects(3);
        var projectDtos = CreateSupportProjectDtos(projects);

        _mockAuditRepository.Setup(r => r.GetSupportProjectHistoryAsync(
                It.IsAny<SupportProjectId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(projects);

        SetupMapperForProjects(projects, projectDtos);

        // Act
        var result = await _service.GetSupportProjectHistoryAsync(supportProjectId, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(projects.Count, result.Value!.Count());
        _mockAuditRepository.Verify(r => r.GetSupportProjectHistoryAsync(
            It.Is<SupportProjectId>(id => id.Value == supportProjectId),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetSupportProjectHistoryAsync_ShouldReturnFailure_WhenExceptionThrown()
    {
        // Arrange
        var supportProjectId = 1;
        var exceptionMessage = "Database connection failed";

        _mockAuditRepository.Setup(r => r.GetSupportProjectHistoryAsync(
                It.IsAny<SupportProjectId>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception(exceptionMessage));

        // Act
        var result = await _service.GetSupportProjectHistoryAsync(supportProjectId, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(exceptionMessage, result.Error);
    }

    #endregion

    #region GetSupportProjectAsOfAsync Tests

    [Fact]
    public async Task GetSupportProjectAsOfAsync_ShouldReturnMappedDto_WhenProjectExists()
    {
        // Arrange
        var supportProjectId = 1;
        var asOfDate = DateTime.UtcNow.AddDays(-1);
        var project = CreateSupportProjects(1)[0];
        var projectDto = CreateSupportProjectDtos([project])[0];

        _mockAuditRepository.Setup(r => r.GetSupportProjectAsOfAsync(
                It.IsAny<SupportProjectId>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(project);

        _mockMapper.Setup(m => m.Map<SupportProjectDto?>(project)).Returns(projectDto);

        // Act
        var result = await _service.GetSupportProjectAsOfAsync(supportProjectId, asOfDate, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(projectDto.Id, result.Value.Id);
        _mockAuditRepository.Verify(r => r.GetSupportProjectAsOfAsync(
            It.Is<SupportProjectId>(id => id.Value == supportProjectId),
            asOfDate,
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetSupportProjectAsOfAsync_ShouldReturnNull_WhenProjectNotFound()
    {
        // Arrange
        var supportProjectId = 1;
        var asOfDate = DateTime.UtcNow.AddDays(-1);

        _mockAuditRepository.Setup(r => r.GetSupportProjectAsOfAsync(
                It.IsAny<SupportProjectId>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.SupportProject.SupportProject?)null);

        _mockMapper.Setup(m => m.Map<SupportProjectDto?>(It.IsAny<object>())).Returns((SupportProjectDto?)null);

        // Act
        var result = await _service.GetSupportProjectAsOfAsync(supportProjectId, asOfDate, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Null(result.Value);
    }

    [Fact]
    public async Task GetSupportProjectAsOfAsync_ShouldReturnFailure_WhenExceptionThrown()
    {
        // Arrange
        var supportProjectId = 1;
        var asOfDate = DateTime.UtcNow.AddDays(-1);
        var exceptionMessage = "Database timeout";

        _mockAuditRepository.Setup(r => r.GetSupportProjectAsOfAsync(
                It.IsAny<SupportProjectId>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception(exceptionMessage));

        // Act
        var result = await _service.GetSupportProjectAsOfAsync(supportProjectId, asOfDate, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(exceptionMessage, result.Error);
    }

    #endregion

    #region GetSupportProjectBetweenDatesAsync Tests

    [Fact]
    public async Task GetSupportProjectBetweenDatesAsync_ShouldReturnMappedDtos_WhenProjectsExist()
    {
        // Arrange
        var supportProjectId = 1;
        var fromDate = DateTime.UtcNow.AddDays(-7);
        var toDate = DateTime.UtcNow;
        var projects = CreateSupportProjects(2);
        var projectDtos = CreateSupportProjectDtos(projects);

        _mockAuditRepository.Setup(r => r.GetSupportProjectBetweenDatesAsync(
                It.IsAny<SupportProjectId>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(projects);

        SetupMapperForProjects(projects, projectDtos);

        // Act
        var result = await _service.GetSupportProjectBetweenDatesAsync(supportProjectId, fromDate, toDate, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(projects.Count, result.Value!.Count());
        _mockAuditRepository.Verify(r => r.GetSupportProjectBetweenDatesAsync(
            It.Is<SupportProjectId>(id => id.Value == supportProjectId),
            fromDate,
            toDate,
            It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region GetSupportProjectFromDateAsync Tests

    [Fact]
    public async Task GetSupportProjectFromDateAsync_ShouldReturnMappedDtos_WhenProjectsExist()
    {
        // Arrange
        var supportProjectId = 1;
        var fromDate = DateTime.UtcNow.AddDays(-30);
        var projects = CreateSupportProjects(4);
        var projectDtos = CreateSupportProjectDtos(projects);

        _mockAuditRepository.Setup(r => r.GetSupportProjectFromDateAsync(
                It.IsAny<SupportProjectId>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(projects);

        SetupMapperForProjects(projects, projectDtos);

        // Act
        var result = await _service.GetSupportProjectFromDateAsync(supportProjectId, fromDate, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(projects.Count, result.Value!.Count());
        _mockAuditRepository.Verify(r => r.GetSupportProjectFromDateAsync(
            It.Is<SupportProjectId>(id => id.Value == supportProjectId),
            fromDate,
            It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region GetAllSupportProjectsAsOfAsync Tests

    [Fact]
    public async Task GetAllSupportProjectsAsOfAsync_ShouldReturnMappedDtos_WhenProjectsExist()
    {
        // Arrange
        var asOfDate = DateTime.UtcNow.AddDays(-1);
        var projects = CreateSupportProjects(5);
        var projectDtos = CreateSupportProjectDtos(projects);

        _mockAuditRepository.Setup(r => r.GetAllSupportProjectsAsOfAsync(
                It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(projects);

        SetupMapperForProjects(projects, projectDtos);

        // Act
        var result = await _service.GetAllSupportProjectsAsOfAsync(asOfDate, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(projects.Count, result.Value!.Count());
        _mockAuditRepository.Verify(r => r.GetAllSupportProjectsAsOfAsync(
            asOfDate,
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAllSupportProjectsAsOfAsync_ShouldReturnEmptyList_WhenNoProjectsExist()
    {
        // Arrange
        var asOfDate = DateTime.UtcNow.AddDays(-1);
        var emptyProjects = new List<Domain.Entities.SupportProject.SupportProject>();

        _mockAuditRepository.Setup(r => r.GetAllSupportProjectsAsOfAsync(
                It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(emptyProjects);

        // Act
        var result = await _service.GetAllSupportProjectsAsOfAsync(asOfDate, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Value!);
    }

    #endregion

    #region GetFieldAuditTrailAsync Tests

    [Fact]
    public async Task GetFieldAuditTrailAsync_ShouldReturnMappedDtos_WhenAuditTrailExists()
    {
        // Arrange
        var supportProjectId = 1;
        Expression<Func<Domain.Entities.SupportProject.SupportProject, string?>> fieldSelector = sp => sp.SupportOrganisationName;
        var auditTrail = CreateFieldAuditTrail<string?>();

        _mockAuditRepository.Setup(r => r.GetFieldAuditTrailAsync(
                It.IsAny<SupportProjectId>(), It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, string?>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(auditTrail);

        // Act
        var result = await _service.GetFieldAuditTrailAsync(supportProjectId, fieldSelector, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(auditTrail.Count, result.Value!.Count());

        var firstAudit = result.Value!.First();
        var firstExpected = auditTrail.First();
        Assert.Equal(firstExpected.SupportProjectId.Value, firstAudit.SupportProjectId);
        Assert.Equal(firstExpected.FieldName, firstAudit.FieldName);
        Assert.Equal(firstExpected.FieldValue, firstAudit.FieldValue);
        Assert.Equal(firstExpected.ValidFrom, firstAudit.ValidFrom);
        Assert.Equal(firstExpected.ValidTo, firstAudit.ValidTo);
        Assert.Equal(firstExpected.LastModifiedBy, firstAudit.LastModifiedBy);
        Assert.Equal(firstExpected.LastModifiedOn, firstAudit.LastModifiedOn);

        _mockAuditRepository.Verify(r => r.GetFieldAuditTrailAsync(
            It.Is<SupportProjectId>(id => id.Value == supportProjectId),
            fieldSelector,
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetFieldAuditTrailAsync_ShouldReturnEmptyList_WhenNoAuditTrailExists()
    {
        // Arrange
        var supportProjectId = 1;
        Expression<Func<Domain.Entities.SupportProject.SupportProject, string?>> fieldSelector = sp => sp.SupportOrganisationName;
        var emptyAuditTrail = new List<SupportProjectFieldAudit<string?>>();

        _mockAuditRepository.Setup(r => r.GetFieldAuditTrailAsync(
                It.IsAny<SupportProjectId>(), It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, string?>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(emptyAuditTrail);

        // Act
        var result = await _service.GetFieldAuditTrailAsync(supportProjectId, fieldSelector, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Value!);
    }

    [Fact]
    public async Task GetFieldAuditTrailAsync_ShouldReturnFailure_WhenExceptionThrown()
    {
        // Arrange
        var supportProjectId = 1;
        Expression<Func<Domain.Entities.SupportProject.SupportProject, string?>> fieldSelector = sp => sp.SupportOrganisationName;
        var exceptionMessage = "Audit repository error";

        _mockAuditRepository.Setup(r => r.GetFieldAuditTrailAsync(
                It.IsAny<SupportProjectId>(), It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, string?>>>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception(exceptionMessage));

        // Act
        var result = await _service.GetFieldAuditTrailAsync(supportProjectId, fieldSelector, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(exceptionMessage, result.Error);
    }

    [Fact]
    public async Task GetFieldAuditTrailAsync_WithBooleanField_ShouldReturnCorrectDtos()
    {
        // Arrange
        var supportProjectId = 1;
        Expression<Func<Domain.Entities.SupportProject.SupportProject, bool?>> fieldSelector = sp => sp.AssessmentToolTwoCompleted;
        var auditTrail = CreateFieldAuditTrail<bool?>();

        _mockAuditRepository.Setup(r => r.GetFieldAuditTrailAsync(
                It.IsAny<SupportProjectId>(), It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, bool?>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(auditTrail);

        // Act
        var result = await _service.GetFieldAuditTrailAsync(supportProjectId, fieldSelector, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(auditTrail.Count, result.Value!.Count());
    }

    [Fact]
    public async Task GetFieldAuditTrailAsync_WithDateTimeField_ShouldReturnCorrectDtos()
    {
        // Arrange
        var supportProjectId = 1;
        Expression<Func<Domain.Entities.SupportProject.SupportProject, DateTime?>> fieldSelector = sp => sp.DateSupportOrganisationChosen;
        var auditTrail = CreateFieldAuditTrail<DateTime?>();

        _mockAuditRepository.Setup(r => r.GetFieldAuditTrailAsync(
                It.IsAny<SupportProjectId>(), It.IsAny<Expression<Func<Domain.Entities.SupportProject.SupportProject, DateTime?>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(auditTrail);

        // Act
        var result = await _service.GetFieldAuditTrailAsync(supportProjectId, fieldSelector, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(auditTrail.Count, result.Value!.Count());
    }

    #endregion

    #region Helper Methods

    private List<Domain.Entities.SupportProject.SupportProject> CreateSupportProjects(int count)
    {
        var projects = new List<Domain.Entities.SupportProject.SupportProject>();
        for (int i = 0; i < count; i++)
        {
            projects.Add(new Domain.Entities.SupportProject.SupportProject(
                new SupportProjectId(i + 1),
                _fixture.Create<string>(),
                _fixture.Create<string>(),
                _fixture.Create<string>(),
                _fixture.Create<string>()));
        }
        return projects;
    }

    private List<SupportProjectDto> CreateSupportProjectDtos(List<Domain.Entities.SupportProject.SupportProject> projects)
    {
        return projects.Select(project => new SupportProjectDto(
            project.Id!.Value,
            project.CreatedOn,
            project.LastModifiedOn,
            project.SchoolName,
            project.SchoolUrn,
            project.LocalAuthority,
            project.Region
        )).ToList();
    }

    private void SetupMapperForProjects(List<Domain.Entities.SupportProject.SupportProject> projects, List<SupportProjectDto> dtos)
    {
        for (int i = 0; i < projects.Count; i++)
        {
            _mockMapper.Setup(m => m.Map<SupportProjectDto>(projects[i])).Returns(dtos[i]);
        }
    }

    private List<SupportProjectFieldAudit<T>> CreateFieldAuditTrail<T>(int count = 3)
    {
        var auditTrail = new List<SupportProjectFieldAudit<T>>();
        for (int i = 0; i < count; i++)
        {
            auditTrail.Add(new SupportProjectFieldAudit<T>
            {
                SupportProjectId = new SupportProjectId(1),
                FieldName = _fixture.Create<string>(),
                FieldValue = _fixture.Create<T>(),
                ValidFrom = DateTime.UtcNow.AddDays(-i * 2),
                ValidTo = DateTime.UtcNow.AddDays(-i * 2 + 1),
                LastModifiedBy = _fixture.Create<string>(),
                LastModifiedOn = DateTime.UtcNow.AddDays(-i * 2)
            });
        }
        return auditTrail;
    }

    #endregion
}