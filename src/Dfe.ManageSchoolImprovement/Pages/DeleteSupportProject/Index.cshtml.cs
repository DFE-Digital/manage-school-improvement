using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.Watchlist;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.DeleteSupportProject
{
    public class IndexModel(ISupportProjectQueryService supportProjectQueryService, IWatchlistQueryService watchlistQueryService, ErrorService errorService, IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService)
    {
        protected readonly IWatchlistQueryService _watchlistQueryService = watchlistQueryService;

        [BindProperty]
        public bool IsSchoolDeleted { get; private set; }

        public async Task<IActionResult> OnGet(int id, CancellationToken cancellationToken)
        {
            await base.GetSupportProject(id, cancellationToken);
            IsSchoolDeleted = false;
            return Page();
        }

        public async Task<IActionResult> OnPost(int id, bool isSchoolDeleted, CancellationToken cancellationToken)
        {
            if (isSchoolDeleted)
            {
                var request = new SetSoftDeletedCommand(new SupportProjectId(id), User?.Identity?.Name!);

                var result = await mediator.Send(request, cancellationToken);

                if (!result)
                {
                    _errorService.AddApiError();
                    return await base.GetSupportProject(id, cancellationToken);
                }
                
                var watchlists = await _watchlistQueryService.GetAllWatchlistsForSchool(new SupportProjectId(id), cancellationToken);
                
                foreach (var item in watchlists.Value)
                {
                    var removefromWatchlistRequest = new RemoveSchoolFromWatchlistCommand(item.Id.Value);

                    await mediator.Send(removefromWatchlistRequest, cancellationToken);
                }
                
                return RedirectToPage(@Links.SchoolList.Index.Page);
            }
            return RedirectToPage(@Links.TaskList.Index.Page, new { id });
        }
    }
}
