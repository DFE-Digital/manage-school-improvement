using Dfe.ManageSchoolImprovement.Application.SupportProject.Models;
using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.Models;

public class SupportProjectSummaryViewModelTests
{
    [Fact]
    public void Create_ShouldReturnSupportProjectSummaryViewModel_WithCorrectProperties()
    {
        // Arrange
        var supportProjectSummaryDto = new SupportProjectSummaryDto(Id: 1, SchoolName: "Test School");

        // Act
        var viewModel = SupportProjectSummaryViewModel.Create(supportProjectSummaryDto);

        // Assert
        Assert.Equal(supportProjectSummaryDto.Id, viewModel.Id);
        Assert.Equal(supportProjectSummaryDto.SchoolName, viewModel.SchoolName);
    }
}