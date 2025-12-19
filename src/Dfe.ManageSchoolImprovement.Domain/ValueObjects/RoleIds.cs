using System.ComponentModel.DataAnnotations;

namespace Dfe.ManageSchoolImprovement.Domain.ValueObjects
{
    public enum RolesIds
    {
        [Display(Name = "Director of Education")]
        DirectorOfEducation = 0,
        [Display(Name = "Headteacher")]
        Headteacher = 1,
        [Display(Name = "Chair of governors")]
        ChairOfGovernors = 2,
        [Display(Name = "Trust relationship manager")]
        TrustRelationshipManager = 3,
        [Display(Name = "Trust CEO")]
        TrustCEO = 4,
        [Display(Name = "Trust accounting officer")]
        TrustAccountingOfficer = 5,
        [Display(Name = "Other role")]
        Other = 6
    }
}
