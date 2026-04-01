using System.ComponentModel.DataAnnotations;

namespace Dfe.ManageSchoolImprovement.Domain.ValueObjects;

public enum SupportProjectEligibilityStatus
{
    [Display(Name = "Eligible")]
    EligibleForSupport = 0,
    [Display(Name = "Not eligible")]
    NotEligibleForSupport = 1,
    [Display(Name = "Not eligible")]
    NotEligibleForSupportMidIntervention = 2
}