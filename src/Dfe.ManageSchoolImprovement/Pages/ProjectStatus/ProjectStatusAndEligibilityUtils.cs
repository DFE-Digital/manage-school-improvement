using Dfe.ManageSchoolImprovement.Domain.ValueObjects;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.ProjectStatus;

public static class ProjectStatusAndEligibilityUtils
{
    public static bool? MapEligibilityStatusToBool(SupportProjectEligibilityStatus? status)
    {
        if (status == SupportProjectEligibilityStatus.EligibleForSupport)
            return true;
    
        if (status == SupportProjectEligibilityStatus.NotEligibleForSupport)
            return false;

        return null; 
    }

        public static string ToGovUkTagClass(ProjectStatusValue status)
        {
            return status switch
            {
                ProjectStatusValue.InProgress => "govuk-tag govuk-tag--green",
                ProjectStatusValue.Paused => "govuk-tag govuk-tag--yellow",
                ProjectStatusValue.Stopped => "govuk-tag govuk-tag--red",
                _ => "govuk-tag"
            };
        }
       
       
    
}