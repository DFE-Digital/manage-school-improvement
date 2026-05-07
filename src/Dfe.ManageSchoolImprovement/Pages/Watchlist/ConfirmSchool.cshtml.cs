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

public class ConfirmSchoolModel(
    ISupportProjectQueryService supportProjectQueryService,
    ErrorService errorService,
    IMediator mediator) : PageModel
{
    public string ReturnPage { get; set; }
    
    public SupportProjectViewModel? SelectedSchool { get; set; }
    
    [BindProperty]
    public int SelectedSchoolId { get; set; }

    public bool ShowError { get; set; }


    public async Task<IActionResult> OnGetAsync(int expectedSupportProjectId, CancellationToken cancellationToken)
    {
        ReturnPage = @Links.Watchlist.SelectSchool.Page;
        
        var expectedSchool = await supportProjectQueryService.GetSupportProject(expectedSupportProjectId, cancellationToken);
        if (expectedSchool.Value != null) SelectedSchool = SupportProjectViewModel.Create(expectedSchool.Value);
        
        SelectedSchoolId = expectedSupportProjectId;

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
        
        var currentUser = User.Identity?.Name;
        var selectedSchoolId = new SupportProjectId(SelectedSchoolId);
        
        var request = new AddSchoolToWatchlist.AddSchoolToWatchlistCommand(selectedSchoolId, currentUser!);
        
        var result = await mediator.Send(request, cancellationToken);
        
        if (!result)
        {
            errorService.AddApiError();
            return Page();
        }
        
        // Get the action from the button clicked  
        var action = Request.Form["action"].ToString();

        // Determine where to redirect based on button clicked  
        if (action == "add-another")
        {
            return RedirectToPage(@Links.Watchlist.SelectSchool.Page);
        }

        return RedirectToPage(@Links.Watchlist.Index.Page);
        

    }
}