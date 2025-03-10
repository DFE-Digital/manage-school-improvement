using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.DeleteSupportProject
{
    public class IndexModel(ISupportProjectQueryService supportProjectQueryService, ErrorService errorService, IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService)
    {
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
                return RedirectToPage(@Links.SchoolList.Index.Page);
            }
            return RedirectToPage(@Links.TaskList.Index.Page, new { id });
        }
    }
}
