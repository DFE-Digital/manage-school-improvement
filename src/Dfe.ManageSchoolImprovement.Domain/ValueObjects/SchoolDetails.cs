using GovUK.Dfe.CoreLibs.Contracts.Academies.Base;

namespace Dfe.ManageSchoolImprovement.Domain.ValueObjects;

public class SchoolDetails
{
    public required string SchoolName { get; set; }
    public required string SchoolUrn { get; set; }
    public required string LocalAuthority { get; set; }
    public required string Region { get; set; }
    public string? Address { get; set; }
}
