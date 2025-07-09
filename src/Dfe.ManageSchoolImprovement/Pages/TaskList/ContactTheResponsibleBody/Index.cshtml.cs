using System.ComponentModel.DataAnnotations;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.ContactTheResponsibleBody;

public class ContactTheResponsibleBodyModel(ISupportProjectQueryService supportProjectQueryService,ErrorService errorService, IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService,errorService),IDateValidationMessageProvider
{
    
    [BindProperty(Name = "responsible-body-contacted-date", BinderType = typeof(DateInputModelBinder))]
    [DateValidation(Dfe.ManageSchoolImprovement.Frontend.Services.DateRangeValidationService.DateRange.PastOrToday)]
    [Display(Name = "Enter the date of contact")]
    public DateTime? ResponsibleBodyContactedDate  { get; set; }
    
    [BindProperty(Name = "discuss-best-approach")]
    public bool? DiscussBestApproach{ get; set; }
    
    [BindProperty(Name = "email-responsible-body")]
    
    public bool? EmailResponsibleBody { get; set; }
    
  
    
    public bool ShowError { get; set; }
    
    string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts)
    {
        return $"Date must include a {string.Join(" and ", missingParts)}";
    }

    string IDateValidationMessageProvider.AllMissing(string displayName)
    {
        return $"Enter the school contacted date";
    }
    
    public async Task<IActionResult> OnGet(int id, CancellationToken cancellationToken)
    {
        
        await base.GetSupportProject(id, cancellationToken);
        
        ResponsibleBodyContactedDate = SupportProject.ContactedTheResponsibleBodyDate ?? null;

        DiscussBestApproach = SupportProject.DiscussTheBestApproach;

        EmailResponsibleBody = SupportProject.EmailTheResponsibleBody;
        
        return Page(); 
    }

    public async Task<IActionResult> OnPost(int id,CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            _errorService.AddErrors(Request.Form.Keys, ModelState);
            ShowError = true;
            return await base.GetSupportProject(id, cancellationToken);
        }
        
        var request = new SetContactTheResponsibleBodyDetailsCommand(new SupportProjectId(id),  DiscussBestApproach ,EmailResponsibleBody,ResponsibleBodyContactedDate );

        var result = await mediator.Send(request, cancellationToken);
       
        if (!result)
        {
            _errorService.AddApiError();
            return await base.GetSupportProject(id, cancellationToken);
        }
        
        TaskUpdated = true;
        return RedirectToPage(@Links.TaskList.Index.Page, new { id });
    }

}
