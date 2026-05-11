using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.Watchlist;

public record RemoveSchoolFromWatchlistCommand(
    Guid WatchlistId
): IRequest<bool>;

public class RemoveSchoolFromWatchlist
{
    public class RemoveSchoolFromWatchlistCommandHandler(IWatchlistRepository watchlistRepository)
        : IRequestHandler<RemoveSchoolFromWatchlistCommand, bool>
    {
        public async Task<bool> Handle(RemoveSchoolFromWatchlistCommand request, CancellationToken cancellationToken)
        {
            var id = new WatchlistId(request.WatchlistId);
            var watchlistItem = await watchlistRepository.GetAsync(id, cancellationToken);
            
            await watchlistRepository.RemoveAsync(watchlistItem, cancellationToken);

            return true;
        }
    }
}