using Dfe.ManageSchoolImprovement.Domain.ValueObjects;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Models
{
    public record ProgressReviewDto(
        Guid id,
        int readableId,
        int supportProjectId,
        DateTime reviewDate,
        DateTime? nextReviewDate,
        string reviewer,
        int order,
        string title
    )
    {
    }
}