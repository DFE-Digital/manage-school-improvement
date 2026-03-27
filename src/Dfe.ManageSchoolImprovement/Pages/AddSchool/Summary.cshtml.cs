using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.CreateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.AddSchool;

public class SummaryModel(IGetEstablishment getEstablishment, ISupportProjectQueryService supportProjectQueryService, IMediator mediator) : PageModel
{
    public string? ReturnPage { get; set; }
    public GovUK.Dfe.CoreLibs.Contracts.Academies.V4.Establishments.EstablishmentDto? Establishment { get; set; }

    [BindProperty]
    public string? TrustName { get; set; }

    [BindProperty]
    public string? TrustReferenceNumber { get; set; }
    
    public async Task<IActionResult> OnGetAsync(string urn, bool? backLinkClicked, CancellationToken cancellationToken)
    {
        ReturnPage = @Links.AddSchool.SelectSchool.Page;
        
        Establishment = await getEstablishment.GetEstablishmentByUrn(urn);

        var existingSupportProjects = await supportProjectQueryService.GetAllSupportProjects(cancellationToken);

        if (existingSupportProjects.Value != null && existingSupportProjects.Value.Any(a => a.SchoolUrn == Establishment.Urn) && backLinkClicked != true)
        {
            // if we are in the position the user must have selected back so navigate them to the summary page
            return RedirectToPage(Links.SchoolList.Index.Page);
        }

        //Get Trust
        var trust = await getEstablishment.GetEstablishmentTrust(urn);

        //if there is one set trust name
        if (trust != null)
        {
            TrustName = trust.Name;
            TrustReferenceNumber = trust.ReferenceNumber;
        }

        if (backLinkClicked == true)
        {
            var supportProjectId  = existingSupportProjects.Value?.First(a => a.SchoolUrn == Establishment.Urn).Id;

            if (supportProjectId.HasValue)
            {
                var request = new SetSoftDeletedCommand(new SupportProjectId((int)supportProjectId), User.Identity?.Name!);
                await mediator.Send(request);
                
                TempData.Remove("schoolAdded");
            }
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string urn, CancellationToken cancellationToken)
    {
        GovUK.Dfe.CoreLibs.Contracts.Academies.V4.Establishments.EstablishmentDto establishment = await getEstablishment.GetEstablishmentByUrn(urn);

        var request = new CreateSupportProjectCommand(establishment.Name, establishment.Urn, establishment.LocalAuthorityName, establishment.Gor.Name, TrustName, TrustReferenceNumber, establishment.Address);

        await mediator.Send(request);
        
        var existingSupportProjects = await supportProjectQueryService.GetAllSupportProjects(cancellationToken);
        
        var supportProjectId  = existingSupportProjects.Value?.First(a => a.SchoolUrn == establishment.Urn).Id;

        TempData["schoolAdded"] = true;
        
        return RedirectToPage(Links.AddSchool.ConfirmStartingEligibility.Page, new {id = supportProjectId});
    }

}
