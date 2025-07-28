using Dfe.ManageSchoolImprovement.Domain.Common;

namespace Dfe.ManageSchoolImprovement.Domain.ValueObjects
{
    public record ImprovementPlanObjectiveId(Guid Value) : IStronglyTypedId;
}
