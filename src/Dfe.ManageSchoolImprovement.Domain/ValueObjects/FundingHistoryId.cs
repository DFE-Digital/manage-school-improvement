using Dfe.ManageSchoolImprovement.Domain.Common;

namespace Dfe.ManageSchoolImprovement.Domain.ValueObjects
{
    public record FundingHistoryId(Guid Value) : IStronglyTypedId;
}
