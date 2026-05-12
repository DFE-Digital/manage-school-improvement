using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.Watchlist;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.Watchlist;

public class RemoveSchoolModel(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService,
    IMediator mediator) : PageModel
{
    public string ReturnPage { get; set; }
    
    public SupportProjectViewModel? SelectedSchool { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public Guid WatchlistIdToRemove { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public int ReadableIdToRemove { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public int SelectedSchoolId { get; set; }

    public bool ShowError { get; set; }


    public async Task<IActionResult> OnGetAsync(Guid watchlistIdToRemove, CancellationToken cancellationToken)
    {
        ReturnPage = @Links.Watchlist.Index.Page;
        
        var expectedSchool = await supportProjectQueryService.GetSupportProject(SelectedSchoolId, cancellationToken);
        if (expectedSchool.Value != null) SelectedSchool = SupportProjectViewModel.Create(expectedSchool.Value);

        return Page();
    }
    

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            ShowError = true;
            errorService.AddErrors(Request.Form.Keys, ModelState);
            return Page();
        }

        var request = new RemoveSchoolFromWatchlistCommand(WatchlistIdToRemove);

        var result = await mediator.Send(request, cancellationToken);
        
        if (!result)
        {
            errorService.AddApiError();
            return Page();
        }
        
        return RedirectToPage(@Links.Watchlist.Index.Page);
    }
}
