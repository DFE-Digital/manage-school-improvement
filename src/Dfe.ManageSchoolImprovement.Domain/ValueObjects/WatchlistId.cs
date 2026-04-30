using Dfe.ManageSchoolImprovement.Domain.Common;

namespace Dfe.ManageSchoolImprovement.Domain.ValueObjects
{
    public record WatchlistId(Guid Value) : IStronglyTypedId;
}