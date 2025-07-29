using Dfe.ManageSchoolImprovement.Domain.Common;

namespace Dfe.ManageSchoolImprovement.Domain.ValueObjects
{
    public record ImprovementPlanId(Guid Value) : IStronglyTypedId;
}
