using Mediator;

namespace Dfe.ManageSchoolImprovement.Domain.Common
{
    public interface IDomainEvent : INotification
    {
        DateTime OccurredOn { get; }
    }
}
