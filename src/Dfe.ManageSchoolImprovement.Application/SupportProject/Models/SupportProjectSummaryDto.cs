namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Models;

public record SupportProjectSummaryDto(
    int Id,
    string SchoolName,
    string SchoolUrn,
    string LocalAuthority,
    string Region
);