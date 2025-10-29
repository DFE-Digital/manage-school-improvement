using Dfe.ManageSchoolImprovement.Frontend.Constants;
using Dfe.ManageSchoolImprovement.Frontend.Extensions;
using FluentAssertions;
using GovUK.Dfe.CoreLibs.ApplicationSettings.Interfaces;
using Moq;

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.Extensions;

public class SharePointResourceExtensionsTests
{
    private readonly Mock<IApplicationSettingsService> _mockSettingsService;
    private readonly CancellationToken _cancellationToken;

    public SharePointResourceExtensionsTests()
    {
        _mockSettingsService = new Mock<IApplicationSettingsService>();
        _cancellationToken = CancellationToken.None;
    }

    #region Assessment Tool Tests

    [Fact]
    public async Task GetAssessmentToolOneLinkAsync_ShouldCallGetSettingAsyncWithCorrectKey()
    {
        // Arrange
        const string expectedValue = "https://sharepoint.com/assessment-tool-one";
        _mockSettingsService.Setup(x => x.GetSettingAsync(SettingKeys.SharePointResources.AssessmentToolOneLink, _cancellationToken))
            .ReturnsAsync(expectedValue);

        // Act
        var result = await _mockSettingsService.Object.GetAssessmentToolOneLinkAsync(_cancellationToken);

        // Assert
        result.Should().Be(expectedValue);
        _mockSettingsService.Verify(x => x.GetSettingAsync(SettingKeys.SharePointResources.AssessmentToolOneLink, _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task GetAssessmentToolOneLinkAsync_WhenSettingIsNull_ShouldReturnNull()
    {
        // Arrange
        _mockSettingsService.Setup(x => x.GetSettingAsync(SettingKeys.SharePointResources.AssessmentToolOneLink, _cancellationToken))
            .ReturnsAsync((string?)null);

        // Act
        var result = await _mockSettingsService.Object.GetAssessmentToolOneLinkAsync(_cancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAssessmentToolTwoLinkAsync_ShouldCallGetSettingAsyncWithCorrectKey()
    {
        // Arrange
        const string expectedValue = "https://sharepoint.com/assessment-tool-two";
        _mockSettingsService.Setup(x => x.GetSettingAsync(SettingKeys.SharePointResources.AssessmentToolTwoLink, _cancellationToken))
            .ReturnsAsync(expectedValue);

        // Act
        var result = await _mockSettingsService.Object.GetAssessmentToolTwoLinkAsync(_cancellationToken);

        // Assert
        result.Should().Be(expectedValue);
        _mockSettingsService.Verify(x => x.GetSettingAsync(SettingKeys.SharePointResources.AssessmentToolTwoLink, _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task GetAssessmentToolTwoLinkAsync_WhenSettingIsNull_ShouldReturnNull()
    {
        // Arrange
        _mockSettingsService.Setup(x => x.GetSettingAsync(SettingKeys.SharePointResources.AssessmentToolTwoLink, _cancellationToken))
            .ReturnsAsync((string?)null);

        // Act
        var result = await _mockSettingsService.Object.GetAssessmentToolTwoLinkAsync(_cancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAssessmentToolTwoSharePointFolderLinkAsync_ShouldCallGetSettingAsyncWithCorrectKey()
    {
        // Arrange
        const string expectedValue = "https://sharepoint.com/assessment-tool-two/folder";
        _mockSettingsService.Setup(x => x.GetSettingAsync(SettingKeys.SharePointResources.AssessmentToolTwoSharePointFolderLink, _cancellationToken))
            .ReturnsAsync(expectedValue);

        // Act
        var result = await _mockSettingsService.Object.GetAssessmentToolTwoSharePointFolderLinkAsync(_cancellationToken);

        // Assert
        result.Should().Be(expectedValue);
        _mockSettingsService.Verify(x => x.GetSettingAsync(SettingKeys.SharePointResources.AssessmentToolTwoSharePointFolderLink, _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task GetAssessmentToolTwoSharePointFolderLinkAsync_WhenSettingIsNull_ShouldReturnNull()
    {
        // Arrange
        _mockSettingsService.Setup(x => x.GetSettingAsync(SettingKeys.SharePointResources.AssessmentToolTwoSharePointFolderLink, _cancellationToken))
            .ReturnsAsync((string?)null);

        // Act
        var result = await _mockSettingsService.Object.GetAssessmentToolTwoSharePointFolderLinkAsync(_cancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAssessmentToolThreeLinkAsync_ShouldCallGetSettingAsyncWithCorrectKey()
    {
        // Arrange
        const string expectedValue = "https://sharepoint.com/assessment-tool-three";
        _mockSettingsService.Setup(x => x.GetSettingAsync(SettingKeys.SharePointResources.AssessmentToolThreeLink, _cancellationToken))
            .ReturnsAsync(expectedValue);

        // Act
        var result = await _mockSettingsService.Object.GetAssessmentToolThreeLinkAsync(_cancellationToken);

        // Assert
        result.Should().Be(expectedValue);
        _mockSettingsService.Verify(x => x.GetSettingAsync(SettingKeys.SharePointResources.AssessmentToolThreeLink, _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task GetAssessmentToolThreeLinkAsync_WhenSettingIsNull_ShouldReturnNull()
    {
        // Arrange
        _mockSettingsService.Setup(x => x.GetSettingAsync(SettingKeys.SharePointResources.AssessmentToolThreeLink, _cancellationToken))
            .ReturnsAsync((string?)null);

        // Act
        var result = await _mockSettingsService.Object.GetAssessmentToolThreeLinkAsync(_cancellationToken);

        // Assert
        result.Should().BeNull();
    }

    #endregion

    #region Template Tests

    [Fact]
    public async Task GetImprovementPlanTemplateLinkAsync_ShouldCallGetSettingAsyncWithCorrectKey()
    {
        // Arrange
        const string expectedValue = "https://sharepoint.com/improvement-plan-template";
        _mockSettingsService.Setup(x => x.GetSettingAsync(SettingKeys.SharePointResources.ImprovementPlanTemplateLink, _cancellationToken))
            .ReturnsAsync(expectedValue);

        // Act
        var result = await _mockSettingsService.Object.GetImprovementPlanTemplateLinkAsync(_cancellationToken);

        // Assert
        result.Should().Be(expectedValue);
        _mockSettingsService.Verify(x => x.GetSettingAsync(SettingKeys.SharePointResources.ImprovementPlanTemplateLink, _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task GetImprovementPlanTemplateLinkAsync_WhenSettingIsNull_ShouldReturnNull()
    {
        // Arrange
        _mockSettingsService.Setup(x => x.GetSettingAsync(SettingKeys.SharePointResources.ImprovementPlanTemplateLink, _cancellationToken))
            .ReturnsAsync((string?)null);

        // Act
        var result = await _mockSettingsService.Object.GetImprovementPlanTemplateLinkAsync(_cancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetEnrolmentLetterTemplateLinkAsync_ShouldCallGetSettingAsyncWithCorrectKey()
    {
        // Arrange
        const string expectedValue = "https://sharepoint.com/enrolment-letter-template";
        _mockSettingsService.Setup(x => x.GetSettingAsync(SettingKeys.SharePointResources.EnrolmentLetterTemplate, _cancellationToken))
            .ReturnsAsync(expectedValue);

        // Act
        var result = await _mockSettingsService.Object.GetEnrolmentLetterTemplateLinkAsync(_cancellationToken);

        // Assert
        result.Should().Be(expectedValue);
        _mockSettingsService.Verify(x => x.GetSettingAsync(SettingKeys.SharePointResources.EnrolmentLetterTemplate, _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task GetEnrolmentLetterTemplateLinkAsync_WhenSettingIsNull_ShouldReturnNull()
    {
        // Arrange
        _mockSettingsService.Setup(x => x.GetSettingAsync(SettingKeys.SharePointResources.EnrolmentLetterTemplate, _cancellationToken))
            .ReturnsAsync((string?)null);

        // Act
        var result = await _mockSettingsService.Object.GetEnrolmentLetterTemplateLinkAsync(_cancellationToken);

        // Assert
        result.Should().BeNull();
    }

    #endregion

    #region Guidance Link Tests

    [Fact]
    public async Task GetConfirmFundingBandLinkAsync_ShouldCallGetSettingAsyncWithCorrectKey()
    {
        // Arrange
        const string expectedValue = "https://sharepoint.com/confirm-funding-band";
        _mockSettingsService.Setup(x => x.GetSettingAsync(SettingKeys.SharePointResources.ConfirmFundingBandLink, _cancellationToken))
            .ReturnsAsync(expectedValue);

        // Act
        var result = await _mockSettingsService.Object.GetConfirmFundingBandLinkAsync(_cancellationToken);

        // Assert
        result.Should().Be(expectedValue);
        _mockSettingsService.Verify(x => x.GetSettingAsync(SettingKeys.SharePointResources.ConfirmFundingBandLink, _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task GetConfirmFundingBandLinkAsync_WhenSettingIsNull_ShouldReturnNull()
    {
        // Arrange
        _mockSettingsService.Setup(x => x.GetSettingAsync(SettingKeys.SharePointResources.ConfirmFundingBandLink, _cancellationToken))
            .ReturnsAsync((string?)null);

        // Act
        var result = await _mockSettingsService.Object.GetConfirmFundingBandLinkAsync(_cancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetFundingBandGuidanceLinkAsync_ShouldCallGetSettingAsyncWithCorrectKey()
    {
        // Arrange
        const string expectedValue = "https://sharepoint.com/funding-band-guidance";
        _mockSettingsService.Setup(x => x.GetSettingAsync(SettingKeys.SharePointResources.FundingBandGuidanceLink, _cancellationToken))
            .ReturnsAsync(expectedValue);

        // Act
        var result = await _mockSettingsService.Object.GetFundingBandGuidanceLinkAsync(_cancellationToken);

        // Assert
        result.Should().Be(expectedValue);
        _mockSettingsService.Verify(x => x.GetSettingAsync(SettingKeys.SharePointResources.FundingBandGuidanceLink, _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task GetFundingBandGuidanceLinkAsync_WhenSettingIsNull_ShouldReturnNull()
    {
        // Arrange
        _mockSettingsService.Setup(x => x.GetSettingAsync(SettingKeys.SharePointResources.FundingBandGuidanceLink, _cancellationToken))
            .ReturnsAsync((string?)null);

        // Act
        var result = await _mockSettingsService.Object.GetFundingBandGuidanceLinkAsync(_cancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetTargetedInterventionGuidanceLinkAsync_ShouldCallGetSettingAsyncWithCorrectKey()
    {
        // Arrange
        const string expectedValue = "https://sharepoint.com/targeted-intervention-guidance";
        _mockSettingsService.Setup(x => x.GetSettingAsync(SettingKeys.SharePointResources.TargetedInterventionGuidanceLink, _cancellationToken))
            .ReturnsAsync(expectedValue);

        // Act
        var result = await _mockSettingsService.Object.GetTargetedInterventionGuidanceLinkAsync(_cancellationToken);

        // Assert
        result.Should().Be(expectedValue);
        _mockSettingsService.Verify(x => x.GetSettingAsync(SettingKeys.SharePointResources.TargetedInterventionGuidanceLink, _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task GetTargetedInterventionGuidanceLinkAsync_WhenSettingIsNull_ShouldReturnNull()
    {
        // Arrange
        _mockSettingsService.Setup(x => x.GetSettingAsync(SettingKeys.SharePointResources.TargetedInterventionGuidanceLink, _cancellationToken))
            .ReturnsAsync((string?)null);

        // Act
        var result = await _mockSettingsService.Object.GetTargetedInterventionGuidanceLinkAsync(_cancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetIEBGuidanceLinkAsync_ShouldCallGetSettingAsyncWithCorrectKey()
    {
        // Arrange
        const string expectedValue = "https://sharepoint.com/ieb-guidance";
        _mockSettingsService.Setup(x => x.GetSettingAsync(SettingKeys.SharePointResources.IEBGuidanceLink, _cancellationToken))
            .ReturnsAsync(expectedValue);

        // Act
        var result = await _mockSettingsService.Object.GetIEBGuidanceLinkAsync(_cancellationToken);

        // Assert
        result.Should().Be(expectedValue);
        _mockSettingsService.Verify(x => x.GetSettingAsync(SettingKeys.SharePointResources.IEBGuidanceLink, _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task GetIEBGuidanceLinkAsync_WhenSettingIsNull_ShouldReturnNull()
    {
        // Arrange
        _mockSettingsService.Setup(x => x.GetSettingAsync(SettingKeys.SharePointResources.IEBGuidanceLink, _cancellationToken))
            .ReturnsAsync((string?)null);

        // Act
        var result = await _mockSettingsService.Object.GetIEBGuidanceLinkAsync(_cancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetSOPUCommissioningFormLinkAsync_ShouldCallGetSettingAsyncWithCorrectKey()
    {
        // Arrange
        const string expectedValue = "https://sharepoint.com/sopu-commissioning-form";
        _mockSettingsService.Setup(x => x.GetSettingAsync(SettingKeys.SharePointResources.SOPUCommissioningForm, _cancellationToken))
            .ReturnsAsync(expectedValue);

        // Act
        var result = await _mockSettingsService.Object.GetSOPUCommissioningFormLinkAsync(_cancellationToken);

        // Assert
        result.Should().Be(expectedValue);
        _mockSettingsService.Verify(x => x.GetSettingAsync(SettingKeys.SharePointResources.SOPUCommissioningForm, _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task GetSOPUCommissioningFormLinkAsync_WhenSettingIsNull_ShouldReturnNull()
    {
        // Arrange
        _mockSettingsService.Setup(x => x.GetSettingAsync(SettingKeys.SharePointResources.SOPUCommissioningForm, _cancellationToken))
            .ReturnsAsync((string?)null);

        // Act
        var result = await _mockSettingsService.Object.GetSOPUCommissioningFormLinkAsync(_cancellationToken);

        // Assert
        result.Should().BeNull();
    }

    #endregion

    #region Default CancellationToken Tests

    [Fact]
    public async Task GetAssessmentToolOneLinkAsync_WithoutCancellationToken_ShouldUseDefaultCancellationToken()
    {
        // Arrange
        const string expectedValue = "https://sharepoint.com/assessment-tool-one";
        _mockSettingsService.Setup(x => x.GetSettingAsync(SettingKeys.SharePointResources.AssessmentToolOneLink, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedValue);

        // Act
        var result = await _mockSettingsService.Object.GetAssessmentToolOneLinkAsync();

        // Assert
        result.Should().Be(expectedValue);
        _mockSettingsService.Verify(x => x.GetSettingAsync(SettingKeys.SharePointResources.AssessmentToolOneLink, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetImprovementPlanTemplateLinkAsync_WithoutCancellationToken_ShouldUseDefaultCancellationToken()
    {
        // Arrange
        const string expectedValue = "https://sharepoint.com/improvement-plan-template";
        _mockSettingsService.Setup(x => x.GetSettingAsync(SettingKeys.SharePointResources.ImprovementPlanTemplateLink, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedValue);

        // Act
        var result = await _mockSettingsService.Object.GetImprovementPlanTemplateLinkAsync();

        // Assert
        result.Should().Be(expectedValue);
        _mockSettingsService.Verify(x => x.GetSettingAsync(SettingKeys.SharePointResources.ImprovementPlanTemplateLink, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetConfirmFundingBandLinkAsync_WithoutCancellationToken_ShouldUseDefaultCancellationToken()
    {
        // Arrange
        const string expectedValue = "https://sharepoint.com/confirm-funding-band";
        _mockSettingsService.Setup(x => x.GetSettingAsync(SettingKeys.SharePointResources.ConfirmFundingBandLink, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedValue);

        // Act
        var result = await _mockSettingsService.Object.GetConfirmFundingBandLinkAsync();

        // Assert
        result.Should().Be(expectedValue);
        _mockSettingsService.Verify(x => x.GetSettingAsync(SettingKeys.SharePointResources.ConfirmFundingBandLink, It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region Integration-Style Tests

    [Theory]
    [InlineData("GetAssessmentToolOneLinkAsync", "AssessmentToolOneLink")]
    [InlineData("GetAssessmentToolTwoLinkAsync", "AssessmentToolTwoLink")]
    [InlineData("GetAssessmentToolTwoSharePointFolderLinkAsync", "AssessmentToolTwoSharePointFolderLink")]
    [InlineData("GetAssessmentToolThreeLinkAsync", "AssessmentToolThreeLink")]
    [InlineData("GetImprovementPlanTemplateLinkAsync", "ImprovementPlanTemplateLink")]
    [InlineData("GetEnrolmentLetterTemplateLinkAsync", "EnrolmentLetterTemplate")]
    [InlineData("GetConfirmFundingBandLinkAsync", "ConfirmFundingBandLink")]
    [InlineData("GetFundingBandGuidanceLinkAsync", "FundingBandGuidanceLink")]
    [InlineData("GetTargetedInterventionGuidanceLinkAsync", "TargetedInterventionGuidanceLink")]
    [InlineData("GetIEBGuidanceLinkAsync", "IEBGuidanceLink")]
    [InlineData("GetSOPUCommissioningFormLinkAsync", "SOPUCommissioningForm")]
    public async Task ExtensionMethods_ShouldCallCorrectSettingKey(string methodName, string expectedSettingKey)
    {
        // Arrange
        const string expectedValue = "https://sharepoint.com/test-link";
        _mockSettingsService.Setup(x => x.GetSettingAsync(expectedSettingKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedValue);

        // Act
        var method = typeof(SharePointResourceExtensions).GetMethod(methodName);
        method.Should().NotBeNull($"Method {methodName} should exist");

        var task = (Task<string?>)method!.Invoke(null, new object[] { _mockSettingsService.Object, _cancellationToken })!;
        var result = await task;

        // Assert
        result.Should().Be(expectedValue);
        _mockSettingsService.Verify(x => x.GetSettingAsync(expectedSettingKey, _cancellationToken), Times.Once);
    }

    #endregion
}