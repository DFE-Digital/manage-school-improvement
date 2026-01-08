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

    public enum SchoolOrginisationTypes
    {
        [Display(Name = "Headteacher (permanent)")]
        PermanentHeadteacher = 1,
        [Display(Name = "Headteacher (interim)")]
        InterimHeadteacher = 2,
        [Display(Name = "Chair of governors")]
        ChairOfGovernors = 3,
        [Display(Name = "Other job title")]
        Other = 4
    }

    public enum SupportOrganisationTypes
    {
        [Display(Name = "Accounting officer")]
        ProjectLead = 1,
        [Display(Name = "Headteacher")]
        Headteacher = 2,
        [Display(Name = "Other job title")]
        Other = 3
    }

    public enum GovernanceBodyTypes
    {
        [Display(Name = "Trust")]
        Trust = 1,
        [Display(Name = "Local authority")]
        LocalAuthority = 2,
        [Display(Name = "Diocese")]
        Diocese = 3,
        [Display(Name = "Foundation")]
        Foundation = 4,
        [Display(Name = "Federation")]
        Federation = 5,
        [Display(Name = "Other body")]
        Other = 6,
    }
}
