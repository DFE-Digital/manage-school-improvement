using System.ComponentModel.DataAnnotations;

namespace Dfe.ManageSchoolImprovement.Domain.ValueObjects
{
    public enum RolesIds
    {
        // local authority? trust? governing body?
        [Display(Name = "Director of Education")]
        DirectorOfEducation = 0,
        // school
        [Display(Name = "Headteacher")]
        Headteacher = 1,
        // school
        [Display(Name = "Chair of governors")]
        ChairOfGovernors = 2,
        // trust
        [Display(Name = "Trust relationship manager")]
        TrustRelationshipManager = 3,
        // trust
        [Display(Name = "Trust CEO")]
        TrustCEO = 4,
        // trust
        [Display(Name = "Trust accounting officer")]
        TrustAccountingOfficer = 5,
        // how do we sort these out?
        [Display(Name = "Other role")]
        Other = 6
    }
}
