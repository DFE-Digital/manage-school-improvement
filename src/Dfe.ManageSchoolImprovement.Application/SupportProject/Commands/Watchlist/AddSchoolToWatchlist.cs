using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.Watchlist;

public class AddSchoolToWatchlist
{
    public record AddSchoolToWatchlistCommand(
        SupportProjectId SupportProjectId,
        string User
        ): IRequest<bool>;

    public class AddSchoolToWatchlistCommandHandler(IWatchlistRepository watchlistRepository)
    : IRequestHandler<AddSchoolToWatchlistCommand, bool>
    {
        public async Task<bool> Handle(AddSchoolToWatchlistCommand request, CancellationToken cancellationToken)
        {
            var watchlistId = new WatchlistId(Guid.NewGuid());

            var watchlistRecord = new Domain.Entities.SupportProject.Watchlist(
                watchlistId,
                request.SupportProjectId,
                request.User
            );
            
            await watchlistRepository.AddAsync(watchlistRecord, cancellationToken);

            return true;
        }
    }
}