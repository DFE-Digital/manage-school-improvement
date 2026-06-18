using Dfe.ManageSchoolImprovement.Domain.Common;

namespace Dfe.ManageSchoolImprovement.Domain.ValueObjects
{
    public record ProgressReviewId(Guid Value) : IStronglyTypedId;
}