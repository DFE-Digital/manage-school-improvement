using Dfe.ManageSchoolImprovement.Domain.Common;

namespace Dfe.ManageSchoolImprovement.Domain.ValueObjects
{
    public record EngagementConcernId(Guid Value) : IStronglyTypedId;
}