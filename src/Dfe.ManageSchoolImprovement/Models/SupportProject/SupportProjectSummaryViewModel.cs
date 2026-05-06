using Dfe.ManageSchoolImprovement.Application.SupportProject.Models;
using Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;

namespace Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;

public class SupportProjectSummaryViewModel
{
    public int Id { get; set; }

    public string SchoolName { get; set; } = string.Empty;

    public string SchoolUrn { get; set; } = string.Empty;

    public string LocalAuthority { get; set; } = string.Empty;

    public string Region { get; set; } = string.Empty;
    

    public static SupportProjectSummaryViewModel Create(SupportProjectSummaryDto dto)
    {
        return new SupportProjectSummaryViewModel
        {
            Id = dto.Id,
            SchoolName = dto.SchoolName,
            SchoolUrn = dto.SchoolUrn,
            LocalAuthority = dto.LocalAuthority,
            Region = dto.Region,
            
        };
    }
}