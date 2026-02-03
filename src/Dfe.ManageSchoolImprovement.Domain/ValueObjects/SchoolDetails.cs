using GovUK.Dfe.CoreLibs.Contracts.Academies.Base;

namespace Dfe.ManageSchoolImprovement.Domain.ValueObjects;

public class SchoolDetails
{
    public string SchoolName { get; set; }
    public string SchoolUrn { get; set; }
    public string LocalAuthority { get; set; }
    public string Region { get; set; }
    public string? Address { get; set; }
}