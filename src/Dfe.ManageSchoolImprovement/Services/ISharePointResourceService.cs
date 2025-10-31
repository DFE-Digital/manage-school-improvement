namespace Dfe.ManageSchoolImprovement.Frontend.Services;

public interface ISharePointResourceService
{
    Task<string?> GetAssessmentToolOneLinkAsync(CancellationToken cancellationToken = default);
    Task<string?> GetAssessmentToolTwoLinkAsync(CancellationToken cancellationToken = default);
    Task<string?> GetAssessmentToolTwoSharePointFolderLinkAsync(CancellationToken cancellationToken = default);
    Task<string?> GetAssessmentToolThreeLinkAsync(CancellationToken cancellationToken = default);
    Task<string?> GetImprovementPlanTemplateLinkAsync(CancellationToken cancellationToken = default);
    Task<string?> GetConfirmFundingBandLinkAsync(CancellationToken cancellationToken = default);
    Task<string?> GetFundingBandGuidanceLinkAsync(CancellationToken cancellationToken = default);
    Task<string?> GetTargetedInterventionGuidanceLinkAsync(CancellationToken cancellationToken = default);
    Task<string?> GetEnrolmentLetterTemplateLinkAsync(CancellationToken cancellationToken = default);
    Task<string?> GetIEBGuidanceLinkAsync(CancellationToken cancellationToken = default);
    Task<string?> GetSOPUCommissioningFormLinkAsync(CancellationToken cancellationToken = default);
    Task<string?> GetPreviousFundingChecksSpreadsheetLink(CancellationToken cancellationToken = default);
}