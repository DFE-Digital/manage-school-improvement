using Dfe.ManageSchoolImprovement.Frontend.Services;
using FluentAssertions;
using GovUK.Dfe.CoreLibs.ApplicationSettings.Interfaces;
using Moq;

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.Services;

public class SharePointResourceServiceTests
{
    private readonly Mock<IApplicationSettingsService> _mockSettingsService;
    private readonly SharePointResourceService _sharePointResourceService;
    private readonly CancellationToken _cancellationToken;

    public SharePointResourceServiceTests()
    {
        _mockSettingsService = new Mock<IApplicationSettingsService>();
        _sharePointResourceService = new SharePointResourceService(_mockSettingsService.Object);
        _cancellationToken = CancellationToken.None;
    }

    #region Constructor Tests

    [Fact]
    public void Constructor_WithValidSettingsService_ShouldCreateInstance()
    {
        // Act & Assert
        _sharePointResourceService.Should().NotBeNull();
        _sharePointResourceService.Should().BeAssignableTo<ISharePointResourceService>();
    }

    #endregion

    #region Assessment Tool Tests

    [Fact]
    public async Task GetAssessmentToolOneLinkAsync_ShouldCallExtensionMethodAndReturnResult()
    {
        // Arrange
        const string expectedLink = "https://sharepoint.com/assessment-tool-one";
        _mockSettingsService.Setup(x => x.GetSettingAsync("AssessmentToolOneLink", _cancellationToken))
            .ReturnsAsync(expectedLink);

        // Act
        var result = await _sharePointResourceService.GetAssessmentToolOneLinkAsync(_cancellationToken);

        // Assert
        result.Should().Be(expectedLink);
        _mockSettingsService.Verify(x => x.GetSettingAsync("AssessmentToolOneLink", _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task GetAssessmentToolOneLinkAsync_WhenSettingIsNull_ShouldReturnNull()
    {
        // Arrange
        _mockSettingsService.Setup(x => x.GetSettingAsync("AssessmentToolOneLink", _cancellationToken))
            .ReturnsAsync((string?)null);

        // Act
        var result = await _sharePointResourceService.GetAssessmentToolOneLinkAsync(_cancellationToken);

        // Assert
        result.Should().BeNull();
        _mockSettingsService.Verify(x => x.GetSettingAsync("AssessmentToolOneLink", _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task GetAssessmentToolTwoLinkAsync_ShouldCallExtensionMethodAndReturnResult()
    {
        // Arrange
        const string expectedLink = "https://sharepoint.com/assessment-tool-two";
        _mockSettingsService.Setup(x => x.GetSettingAsync("AssessmentToolTwoLink", _cancellationToken))
            .ReturnsAsync(expectedLink);

        // Act
        var result = await _sharePointResourceService.GetAssessmentToolTwoLinkAsync(_cancellationToken);

        // Assert
        result.Should().Be(expectedLink);
        _mockSettingsService.Verify(x => x.GetSettingAsync("AssessmentToolTwoLink", _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task GetAssessmentToolTwoLinkAsync_WhenSettingIsNull_ShouldReturnNull()
    {
        // Arrange
        _mockSettingsService.Setup(x => x.GetSettingAsync("AssessmentToolTwoLink", _cancellationToken))
            .ReturnsAsync((string?)null);

        // Act
        var result = await _sharePointResourceService.GetAssessmentToolTwoLinkAsync(_cancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAssessmentToolTwoSharePointFolderLinkAsync_ShouldCallExtensionMethodAndReturnResult()
    {
        // Arrange
        const string expectedLink = "https://sharepoint.com/assessment-tool-two/folder";
        _mockSettingsService.Setup(x => x.GetSettingAsync("AssessmentToolTwoSharePointFolderLink", _cancellationToken))
            .ReturnsAsync(expectedLink);

        // Act
        var result = await _sharePointResourceService.GetAssessmentToolTwoSharePointFolderLinkAsync(_cancellationToken);

        // Assert
        result.Should().Be(expectedLink);
        _mockSettingsService.Verify(x => x.GetSettingAsync("AssessmentToolTwoSharePointFolderLink", _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task GetAssessmentToolTwoSharePointFolderLinkAsync_WhenSettingIsNull_ShouldReturnNull()
    {
        // Arrange
        _mockSettingsService.Setup(x => x.GetSettingAsync("AssessmentToolTwoSharePointFolderLink", _cancellationToken))
            .ReturnsAsync((string?)null);

        // Act
        var result = await _sharePointResourceService.GetAssessmentToolTwoSharePointFolderLinkAsync(_cancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAssessmentToolThreeLinkAsync_ShouldCallExtensionMethodAndReturnResult()
    {
        // Arrange
        const string expectedLink = "https://sharepoint.com/assessment-tool-three";
        _mockSettingsService.Setup(x => x.GetSettingAsync("AssessmentToolThreeLink", _cancellationToken))
            .ReturnsAsync(expectedLink);

        // Act
        var result = await _sharePointResourceService.GetAssessmentToolThreeLinkAsync(_cancellationToken);

        // Assert
        result.Should().Be(expectedLink);
        _mockSettingsService.Verify(x => x.GetSettingAsync("AssessmentToolThreeLink", _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task GetAssessmentToolThreeLinkAsync_WhenSettingIsNull_ShouldReturnNull()
    {
        // Arrange
        _mockSettingsService.Setup(x => x.GetSettingAsync("AssessmentToolThreeLink", _cancellationToken))
            .ReturnsAsync((string?)null);

        // Act
        var result = await _sharePointResourceService.GetAssessmentToolThreeLinkAsync(_cancellationToken);

        // Assert
        result.Should().BeNull();
    }

    #endregion

    #region Template Tests

    [Fact]
    public async Task GetImprovementPlanTemplateLinkAsync_ShouldCallExtensionMethodAndReturnResult()
    {
        // Arrange
        const string expectedLink = "https://sharepoint.com/improvement-plan-template";
        _mockSettingsService.Setup(x => x.GetSettingAsync("ImprovementPlanTemplateLink", _cancellationToken))
            .ReturnsAsync(expectedLink);

        // Act
        var result = await _sharePointResourceService.GetImprovementPlanTemplateLinkAsync(_cancellationToken);

        // Assert
        result.Should().Be(expectedLink);
        _mockSettingsService.Verify(x => x.GetSettingAsync("ImprovementPlanTemplateLink", _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task GetImprovementPlanTemplateLinkAsync_WhenSettingIsNull_ShouldReturnNull()
    {
        // Arrange
        _mockSettingsService.Setup(x => x.GetSettingAsync("ImprovementPlanTemplateLink", _cancellationToken))
            .ReturnsAsync((string?)null);

        // Act
        var result = await _sharePointResourceService.GetImprovementPlanTemplateLinkAsync(_cancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetEnrolmentLetterTemplateLinkAsync_ShouldCallExtensionMethodAndReturnResult()
    {
        // Arrange
        const string expectedLink = "https://sharepoint.com/enrolment-letter-template";
        _mockSettingsService.Setup(x => x.GetSettingAsync("EnrolmentLetterTemplate", _cancellationToken))
            .ReturnsAsync(expectedLink);

        // Act
        var result = await _sharePointResourceService.GetEnrolmentLetterTemplateLinkAsync(_cancellationToken);

        // Assert
        result.Should().Be(expectedLink);
        _mockSettingsService.Verify(x => x.GetSettingAsync("EnrolmentLetterTemplate", _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task GetEnrolmentLetterTemplateLinkAsync_WhenSettingIsNull_ShouldReturnNull()
    {
        // Arrange
        _mockSettingsService.Setup(x => x.GetSettingAsync("EnrolmentLetterTemplate", _cancellationToken))
            .ReturnsAsync((string?)null);

        // Act
        var result = await _sharePointResourceService.GetEnrolmentLetterTemplateLinkAsync(_cancellationToken);

        // Assert
        result.Should().BeNull();
    }

    #endregion

    #region Guidance Link Tests

    [Fact]
    public async Task GetConfirmFundingBandLinkAsync_ShouldCallExtensionMethodAndReturnResult()
    {
        // Arrange
        const string expectedLink = "https://sharepoint.com/confirm-funding-band";
        _mockSettingsService.Setup(x => x.GetSettingAsync("ConfirmFundingBandLink", _cancellationToken))
            .ReturnsAsync(expectedLink);

        // Act
        var result = await _sharePointResourceService.GetConfirmFundingBandLinkAsync(_cancellationToken);

        // Assert
        result.Should().Be(expectedLink);
        _mockSettingsService.Verify(x => x.GetSettingAsync("ConfirmFundingBandLink", _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task GetConfirmFundingBandLinkAsync_WhenSettingIsNull_ShouldReturnNull()
    {
        // Arrange
        _mockSettingsService.Setup(x => x.GetSettingAsync("ConfirmFundingBandLink", _cancellationToken))
            .ReturnsAsync((string?)null);

        // Act
        var result = await _sharePointResourceService.GetConfirmFundingBandLinkAsync(_cancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetFundingBandGuidanceLinkAsync_ShouldCallExtensionMethodAndReturnResult()
    {
        // Arrange
        const string expectedLink = "https://sharepoint.com/funding-band-guidance";
        _mockSettingsService.Setup(x => x.GetSettingAsync("FundingBandGuidanceLink", _cancellationToken))
            .ReturnsAsync(expectedLink);

        // Act
        var result = await _sharePointResourceService.GetFundingBandGuidanceLinkAsync(_cancellationToken);

        // Assert
        result.Should().Be(expectedLink);
        _mockSettingsService.Verify(x => x.GetSettingAsync("FundingBandGuidanceLink", _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task GetFundingBandGuidanceLinkAsync_WhenSettingIsNull_ShouldReturnNull()
    {
        // Arrange
        _mockSettingsService.Setup(x => x.GetSettingAsync("FundingBandGuidanceLink", _cancellationToken))
            .ReturnsAsync((string?)null);

        // Act
        var result = await _sharePointResourceService.GetFundingBandGuidanceLinkAsync(_cancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetTargetedInterventionGuidanceLinkAsync_ShouldCallExtensionMethodAndReturnResult()
    {
        // Arrange
        const string expectedLink = "https://sharepoint.com/targeted-intervention-guidance";
        _mockSettingsService.Setup(x => x.GetSettingAsync("TargetedInterventionGuidanceLink", _cancellationToken))
            .ReturnsAsync(expectedLink);

        // Act
        var result = await _sharePointResourceService.GetTargetedInterventionGuidanceLinkAsync(_cancellationToken);

        // Assert
        result.Should().Be(expectedLink);
        _mockSettingsService.Verify(x => x.GetSettingAsync("TargetedInterventionGuidanceLink", _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task GetTargetedInterventionGuidanceLinkAsync_WhenSettingIsNull_ShouldReturnNull()
    {
        // Arrange
        _mockSettingsService.Setup(x => x.GetSettingAsync("TargetedInterventionGuidanceLink", _cancellationToken))
            .ReturnsAsync((string?)null);

        // Act
        var result = await _sharePointResourceService.GetTargetedInterventionGuidanceLinkAsync(_cancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetIEBGuidanceLinkAsync_ShouldCallExtensionMethodAndReturnResult()
    {
        // Arrange
        const string expectedLink = "https://sharepoint.com/ieb-guidance";
        _mockSettingsService.Setup(x => x.GetSettingAsync("IEBGuidanceLink", _cancellationToken))
            .ReturnsAsync(expectedLink);

        // Act
        var result = await _sharePointResourceService.GetIEBGuidanceLinkAsync(_cancellationToken);

        // Assert
        result.Should().Be(expectedLink);
        _mockSettingsService.Verify(x => x.GetSettingAsync("IEBGuidanceLink", _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task GetIEBGuidanceLinkAsync_WhenSettingIsNull_ShouldReturnNull()
    {
        // Arrange
        _mockSettingsService.Setup(x => x.GetSettingAsync("IEBGuidanceLink", _cancellationToken))
            .ReturnsAsync((string?)null);

        // Act
        var result = await _sharePointResourceService.GetIEBGuidanceLinkAsync(_cancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetSOPUCommissioningFormLinkAsync_ShouldCallExtensionMethodAndReturnResult()
    {
        // Arrange
        const string expectedLink = "https://sharepoint.com/sopu-commissioning-form";
        _mockSettingsService.Setup(x => x.GetSettingAsync("SOPUCommissioningForm", _cancellationToken))
            .ReturnsAsync(expectedLink);

        // Act
        var result = await _sharePointResourceService.GetSOPUCommissioningFormLinkAsync(_cancellationToken);

        // Assert
        result.Should().Be(expectedLink);
        _mockSettingsService.Verify(x => x.GetSettingAsync("SOPUCommissioningForm", _cancellationToken), Times.Once);
    }

    [Fact]
    public async Task GetSOPUCommissioningFormLinkAsync_WhenSettingIsNull_ShouldReturnNull()
    {
        // Arrange
        _mockSettingsService.Setup(x => x.GetSettingAsync("SOPUCommissioningForm", _cancellationToken))
            .ReturnsAsync((string?)null);

        // Act
        var result = await _sharePointResourceService.GetSOPUCommissioningFormLinkAsync(_cancellationToken);

        // Assert
        result.Should().BeNull();
    }

    #endregion

    #region Default CancellationToken Tests

    [Fact]
    public async Task GetAssessmentToolOneLinkAsync_WithoutCancellationToken_ShouldUseDefaultCancellationToken()
    {
        // Arrange
        const string expectedLink = "https://sharepoint.com/assessment-tool-one";
        _mockSettingsService.Setup(x => x.GetSettingAsync("AssessmentToolOneLink", It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedLink);

        // Act
        var result = await _sharePointResourceService.GetAssessmentToolOneLinkAsync();

        // Assert
        result.Should().Be(expectedLink);
        _mockSettingsService.Verify(x => x.GetSettingAsync("AssessmentToolOneLink", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetImprovementPlanTemplateLinkAsync_WithoutCancellationToken_ShouldUseDefaultCancellationToken()
    {
        // Arrange
        const string expectedLink = "https://sharepoint.com/improvement-plan-template";
        _mockSettingsService.Setup(x => x.GetSettingAsync("ImprovementPlanTemplateLink", It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedLink);

        // Act
        var result = await _sharePointResourceService.GetImprovementPlanTemplateLinkAsync();

        // Assert
        result.Should().Be(expectedLink);
        _mockSettingsService.Verify(x => x.GetSettingAsync("ImprovementPlanTemplateLink", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetConfirmFundingBandLinkAsync_WithoutCancellationToken_ShouldUseDefaultCancellationToken()
    {
        // Arrange
        const string expectedLink = "https://sharepoint.com/confirm-funding-band";
        _mockSettingsService.Setup(x => x.GetSettingAsync("ConfirmFundingBandLink", It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedLink);

        // Act
        var result = await _sharePointResourceService.GetConfirmFundingBandLinkAsync();

        // Assert
        result.Should().Be(expectedLink);
        _mockSettingsService.Verify(x => x.GetSettingAsync("ConfirmFundingBandLink", It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region Integration-Style Tests

    [Theory]
    [InlineData("GetAssessmentToolOneLinkAsync", "AssessmentToolOneLink", "https://sharepoint.com/assessment-tool-one")]
    [InlineData("GetAssessmentToolTwoLinkAsync", "AssessmentToolTwoLink", "https://sharepoint.com/assessment-tool-two")]
    [InlineData("GetAssessmentToolTwoSharePointFolderLinkAsync", "AssessmentToolTwoSharePointFolderLink", "https://sharepoint.com/assessment-tool-two/folder")]
    [InlineData("GetAssessmentToolThreeLinkAsync", "AssessmentToolThreeLink", "https://sharepoint.com/assessment-tool-three")]
    [InlineData("GetImprovementPlanTemplateLinkAsync", "ImprovementPlanTemplateLink", "https://sharepoint.com/improvement-plan-template")]
    [InlineData("GetEnrolmentLetterTemplateLinkAsync", "EnrolmentLetterTemplate", "https://sharepoint.com/enrolment-letter-template")]
    [InlineData("GetConfirmFundingBandLinkAsync", "ConfirmFundingBandLink", "https://sharepoint.com/confirm-funding-band")]
    [InlineData("GetFundingBandGuidanceLinkAsync", "FundingBandGuidanceLink", "https://sharepoint.com/funding-band-guidance")]
    [InlineData("GetTargetedInterventionGuidanceLinkAsync", "TargetedInterventionGuidanceLink", "https://sharepoint.com/targeted-intervention-guidance")]
    [InlineData("GetIEBGuidanceLinkAsync", "IEBGuidanceLink", "https://sharepoint.com/ieb-guidance")]
    [InlineData("GetSOPUCommissioningFormLinkAsync", "SOPUCommissioningForm", "https://sharepoint.com/sopu-commissioning-form")]
    public async Task ServiceMethods_ShouldDelegateToExtensionMethodsCorrectly(string methodName, string expectedSettingKey, string expectedValue)
    {
        // Arrange
        _mockSettingsService.Setup(x => x.GetSettingAsync(expectedSettingKey, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedValue);

        // Act
        var method = typeof(SharePointResourceService).GetMethod(methodName);
        method.Should().NotBeNull($"Method {methodName} should exist");

        var task = (Task<string?>)method!.Invoke(_sharePointResourceService, new object[] { _cancellationToken })!;
        var result = await task;

        // Assert
        result.Should().Be(expectedValue);
        _mockSettingsService.Verify(x => x.GetSettingAsync(expectedSettingKey, _cancellationToken), Times.Once);
    }

    #endregion

    #region Exception Handling Tests

    [Fact]
    public async Task GetAssessmentToolOneLinkAsync_WhenExtensionMethodThrows_ShouldPropagateException()
    {
        // Arrange
        _mockSettingsService.Setup(x => x.GetSettingAsync("AssessmentToolOneLink", _cancellationToken))
            .ThrowsAsync(new InvalidOperationException("Test exception"));

        // Act & Assert
        var act = async () => await _sharePointResourceService.GetAssessmentToolOneLinkAsync(_cancellationToken);
        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Test exception");
    }

    [Fact]
    public async Task GetImprovementPlanTemplateLinkAsync_WhenExtensionMethodThrows_ShouldPropagateException()
    {
        // Arrange
        _mockSettingsService.Setup(x => x.GetSettingAsync("ImprovementPlanTemplateLink", _cancellationToken))
            .ThrowsAsync(new ArgumentException("Test argument exception"));

        // Act & Assert
        var act = async () => await _sharePointResourceService.GetImprovementPlanTemplateLinkAsync(_cancellationToken);
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Test argument exception");
    }

    #endregion

    #region Async Behavior Tests

    [Fact]
    public async Task AllMethods_ShouldBeAsyncAndReturnTask()
    {
        // Arrange
        const string expectedLink = "https://sharepoint.com/test-link";
        _mockSettingsService.Setup(x => x.GetSettingAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedLink);

        // Act & Assert
        var methods = typeof(ISharePointResourceService).GetMethods()
            .Where(m => m.ReturnType == typeof(Task<string?>))
            .ToList();

        foreach (var method in methods)
        {
            var task = (Task<string?>)method.Invoke(_sharePointResourceService, new object[] { _cancellationToken })!;
            var result = await task;
            result.Should().Be(expectedLink);
        }
    }

    #endregion
}