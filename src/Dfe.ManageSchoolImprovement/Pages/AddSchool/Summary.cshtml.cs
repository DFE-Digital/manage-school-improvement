using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using MediatR;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.CreateSupportProject;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.AddSchool;

public class SummaryModel(IGetEstablishment getEstablishment, IMediator mediator ) : PageModel
{
    public DfE.CoreLibs.Contracts.Academies.V4.Establishments.EstablishmentDto Establishment { get; set; }

    public async Task<IActionResult> OnGetAsync(string urn)
    {
        Establishment = await getEstablishment.GetEstablishmentByUrn(urn);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string urn)
    {
        DfE.CoreLibs.Contracts.Academies.V4.Establishments.EstablishmentDto establishment = await getEstablishment.GetEstablishmentByUrn(urn);

        var request = new CreateSupportProjectCommand(establishment.Name, establishment.Urn, establishment.LocalAuthorityName, establishment.Gor.Name);

        var id = await mediator.Send(request);

        if (id != null)
        {
            TempData["schoolAdded"] = true;
            return RedirectToPage(Links.SchoolList.Index.Page);
        }

        else
        {
            return RedirectToPage(Links.AddSchool.Summary.Page);
        }
    }

}
