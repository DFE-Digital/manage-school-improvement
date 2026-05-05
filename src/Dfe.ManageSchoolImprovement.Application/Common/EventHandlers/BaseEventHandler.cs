using Dfe.ManageSchoolImprovement.Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Dfe.ManageSchoolImprovement.Application.Common.EventHandlers
{
#pragma warning disable S2629, S2139
    public abstract class BaseEventHandler<TEvent>(ILogger<BaseEventHandler<TEvent>> logger)
        : INotificationHandler<TEvent>
        where TEvent : IDomainEvent
    {
        public virtual async Task Handle(TEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                await HandleEvent(notification, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error handling event: {typeof(TEvent).Name}");
                throw;
            }
        }

        protected virtual Task HandleEvent(TEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
#pragma warning restore S2629, S2139
}
