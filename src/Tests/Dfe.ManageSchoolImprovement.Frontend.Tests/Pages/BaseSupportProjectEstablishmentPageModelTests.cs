using Dfe.ManageSchoolImprovement.Application.Common.Models;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Frontend.Pages;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.Pages
{
    public class BaseSupportProjectEstablishmentPageModelTests
    {
        private readonly Mock<ISupportProjectQueryService> _mockQueryService;
        private readonly Mock<IGetEstablishment> _mockGetEstablishment;
        private readonly Mock<ErrorService> _mockErrorService;
        private readonly BaseSupportProjectEstablishmentPageModel _pageModel;
        private readonly Mock<ITempDataDictionary> _mockTempData;

        public BaseSupportProjectEstablishmentPageModelTests()
        {
            _mockQueryService = new Mock<ISupportProjectQueryService>();
            _mockGetEstablishment = new Mock<IGetEstablishment>();
            _mockErrorService = new Mock<ErrorService>();
            _mockTempData = new Mock<ITempDataDictionary>();
            _pageModel = new BaseSupportProjectEstablishmentPageModel(
                _mockQueryService.Object,
                _mockGetEstablishment.Object,
                _mockErrorService.Object)
            {
                TempData = _mockTempData.Object
            };
        }

        [Fact]
        public async Task GetSupportProject_ReturnsPageResult_WhenProjectExists()
        {
            // Arrange
            var projectId = 1;
            var mockProject = new SupportProjectDto(projectId, DateTime.Now, "schoolName", "URN234", "local Authority", "Region");
            var mockEstablishment = new DfE.CoreLibs.Contracts.Academies.V4.Establishments.EstablishmentDto
            {
                MISEstablishment = new()
                {
                    QualityOfEducation = "Good",
                    BehaviourAndAttitudes = "Outstanding",
                    PersonalDevelopment = "Good",
                    EffectivenessOfLeadershipAndManagement = "Good"
                },
                OfstedLastInspection = "2022-06-01"
            };

            var result = () => Result<SupportProjectDto?>.Success(mockProject);
            _mockQueryService.Setup(s => s.GetSupportProject(projectId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(result());
            _mockGetEstablishment.Setup(s => s.GetEstablishmentByUrn(mockProject.SchoolUrn))
                .ReturnsAsync(mockEstablishment);

            // Act
            var response = await _pageModel.GetSupportProject(projectId, CancellationToken.None);

            // Assert
            Assert.IsType<PageResult>(response);
            Assert.NotNull(_pageModel.SupportProject);
            Assert.Equal("Good", _pageModel.SupportProject.QualityOfEducation);
            Assert.Equal("Outstanding", _pageModel.SupportProject.BehaviourAndAttitudes);
            Assert.Equal("Good", _pageModel.SupportProject.PersonalDevelopment);
            Assert.Equal("Good", _pageModel.SupportProject.LeadershipAndManagement);
            Assert.Equal("2022-06-01", _pageModel.SupportProject.LastInspectionDate);
        }

        [Fact]
        public async Task GetSupportProject_ReturnsNotFound_WhenProjectDoesNotExist()
        {
            // Arrange
            var projectId = 1;
            var result = () => Result<SupportProjectDto?>.Failure("Project not found");
            _mockQueryService.Setup(s => s.GetSupportProject(projectId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(result());

            // Act
            var response = await _pageModel.GetSupportProject(projectId, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundResult>(response);
        }
    }

}
