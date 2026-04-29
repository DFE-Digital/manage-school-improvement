namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Models
{
    public record WatchlistDto(
        Guid Id,
        int ReadableId,
        int SupportProjectId,
        string? User
    )
    {
    }
}