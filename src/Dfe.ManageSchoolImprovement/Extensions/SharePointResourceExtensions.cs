using Dfe.ManageSchoolImprovement.Frontend.Constants;
using GovUK.Dfe.CoreLibs.ApplicationSettings.Interfaces;

namespace Dfe.ManageSchoolImprovement.Frontend.Extensions;

public static class SharePointResourceExtensions
{
    // Assessment Tools
    public static async Task<string?> GetAssessmentToolOneLinkAsync(this IApplicationSettingsService service, CancellationToken cancellationToken = default)
        => await service.GetSettingAsync(SettingKeys.SharePointResources.AssessmentToolOneLink, cancellationToken);

    public static async Task<string?> GetAssessmentToolTwoLinkAsync(this IApplicationSettingsService service, CancellationToken cancellationToken = default)
        => await service.GetSettingAsync(SettingKeys.SharePointResources.AssessmentToolTwoLink, cancellationToken);

    public static async Task<string?> GetAssessmentToolTwoSharePointFolderLinkAsync(this IApplicationSettingsService service, CancellationToken cancellationToken = default)
        => await service.GetSettingAsync(SettingKeys.SharePointResources.AssessmentToolTwoSharePointFolderLink, cancellationToken);

    public static async Task<string?> GetAssessmentToolThreeLinkAsync(this IApplicationSettingsService service, CancellationToken cancellationToken = default)
        => await service.GetSettingAsync(SettingKeys.SharePointResources.AssessmentToolThreeLink, cancellationToken);

    // Templates
    public static async Task<string?> GetImprovementPlanTemplateLinkAsync(this IApplicationSettingsService service, CancellationToken cancellationToken = default)
        => await service.GetSettingAsync(SettingKeys.SharePointResources.ImprovementPlanTemplateLink, cancellationToken);

    public static async Task<string?> GetEnrolmentLetterTemplateLinkAsync(this IApplicationSettingsService service, CancellationToken cancellationToken = default)
        => await service.GetSettingAsync(SettingKeys.SharePointResources.EnrolmentLetterTemplate, cancellationToken);

    // Guidance Links
    public static async Task<string?> GetConfirmFundingBandLinkAsync(this IApplicationSettingsService service, CancellationToken cancellationToken = default)
        => await service.GetSettingAsync(SettingKeys.SharePointResources.ConfirmFundingBandLink, cancellationToken);

    public static async Task<string?> GetFundingBandGuidanceLinkAsync(this IApplicationSettingsService service, CancellationToken cancellationToken = default)
        => await service.GetSettingAsync(SettingKeys.SharePointResources.FundingBandGuidanceLink, cancellationToken);

    public static async Task<string?> GetTargetedInterventionGuidanceLinkAsync(this IApplicationSettingsService service, CancellationToken cancellationToken = default)
        => await service.GetSettingAsync(SettingKeys.SharePointResources.TargetedInterventionGuidanceLink, cancellationToken);

    public static async Task<string?> GetIEBGuidanceLinkAsync(this IApplicationSettingsService service, CancellationToken cancellationToken = default)
        => await service.GetSettingAsync(SettingKeys.SharePointResources.IEBGuidanceLink, cancellationToken);

    public static async Task<string?> GetSOPUCommissioningFormLinkAsync(this IApplicationSettingsService service, CancellationToken cancellationToken = default)
        => await service.GetSettingAsync(SettingKeys.SharePointResources.SOPUCommissioningForm, cancellationToken);

    public static async Task<string?> GetPreviousFundingChecksSpreadsheetLink(this IApplicationSettingsService service, CancellationToken cancellationToken = default)
        => await service.GetSettingAsync(SettingKeys.SharePointResources.PreviousFundingChecksSpreadsheetLink, cancellationToken);

    public static async Task<string?> GetCheckSupportingOrganisationVendorAccountLink(this IApplicationSettingsService service, CancellationToken cancellationToken = default)
    => await service.GetSettingAsync(SettingKeys.SharePointResources.CheckSupportingOrganisationVendorAccountLink, cancellationToken);

    public static async Task<string?> GetSFSOCommissioningFormLink(this IApplicationSettingsService service, CancellationToken cancellationToken = default)
    => await service.GetSettingAsync(SettingKeys.SharePointResources.SFSOCommissioningFormLink, cancellationToken);
}