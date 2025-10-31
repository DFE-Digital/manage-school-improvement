using Dfe.ManageSchoolImprovement.Frontend.Extensions;
using GovUK.Dfe.CoreLibs.ApplicationSettings.Interfaces;

namespace Dfe.ManageSchoolImprovement.Frontend.Services;

public class SharePointResourceService : ISharePointResourceService
{
    private readonly IApplicationSettingsService _settingsService;

    public SharePointResourceService(IApplicationSettingsService settingsService)
    {
        _settingsService = settingsService;
    }

    public async Task<string?> GetAssessmentToolOneLinkAsync(CancellationToken cancellationToken = default)
        => await _settingsService.GetAssessmentToolOneLinkAsync(cancellationToken);

    public async Task<string?> GetAssessmentToolTwoLinkAsync(CancellationToken cancellationToken = default)
        => await _settingsService.GetAssessmentToolTwoLinkAsync(cancellationToken);

    public async Task<string?> GetAssessmentToolTwoSharePointFolderLinkAsync(CancellationToken cancellationToken = default)
        => await _settingsService.GetAssessmentToolTwoSharePointFolderLinkAsync(cancellationToken);

    public async Task<string?> GetAssessmentToolThreeLinkAsync(CancellationToken cancellationToken = default)
        => await _settingsService.GetAssessmentToolThreeLinkAsync(cancellationToken);

    public async Task<string?> GetImprovementPlanTemplateLinkAsync(CancellationToken cancellationToken = default)
        => await _settingsService.GetImprovementPlanTemplateLinkAsync(cancellationToken);

    public async Task<string?> GetConfirmFundingBandLinkAsync(CancellationToken cancellationToken = default)
        => await _settingsService.GetConfirmFundingBandLinkAsync(cancellationToken);

    public async Task<string?> GetFundingBandGuidanceLinkAsync(CancellationToken cancellationToken = default)
        => await _settingsService.GetFundingBandGuidanceLinkAsync(cancellationToken);

    public async Task<string?> GetTargetedInterventionGuidanceLinkAsync(CancellationToken cancellationToken = default)
        => await _settingsService.GetTargetedInterventionGuidanceLinkAsync(cancellationToken);

    public async Task<string?> GetEnrolmentLetterTemplateLinkAsync(CancellationToken cancellationToken = default)
        => await _settingsService.GetEnrolmentLetterTemplateLinkAsync(cancellationToken);

    public async Task<string?> GetIEBGuidanceLinkAsync(CancellationToken cancellationToken = default)
        => await _settingsService.GetIEBGuidanceLinkAsync(cancellationToken);

    public async Task<string?> GetSOPUCommissioningFormLinkAsync(CancellationToken cancellationToken = default)
        => await _settingsService.GetSOPUCommissioningFormLinkAsync(cancellationToken);

    public async Task<string?> GetPreviousFundingChecksSpreadsheetLink(CancellationToken cancellationToken = default)
        => await _settingsService.GetPreviousFundingChecksSpreadsheetLink(cancellationToken);
    public async Task<string?> GetCheckSupportingOrganisationVendorAccountLink(CancellationToken cancellationToken = default)
    => await _settingsService.GetCheckSupportingOrganisationVendorAccountLink(cancellationToken);

    public async Task<string?> GetSFSOCommissioningFormLink(CancellationToken cancellationToken = default)
    => await _settingsService.GetSFSOCommissioningFormLink(cancellationToken);
}