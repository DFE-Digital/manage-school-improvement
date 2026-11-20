using Mediator;

namespace Dfe.ManageSchoolImprovement.Application.Common.Mediation;

public class MediatorAppMediator(IMediator mediator) : IAppMediator
{
    public ValueTask<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        return mediator.Send(request, cancellationToken);
    }

    public ValueTask Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
        where TNotification : INotification
    {
        return mediator.Publish(notification, cancellationToken);
    }
}
