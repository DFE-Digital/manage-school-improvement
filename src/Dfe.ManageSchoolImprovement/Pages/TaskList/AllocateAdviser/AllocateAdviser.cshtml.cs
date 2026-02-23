using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject.SetAdviserDetails;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.AllocateAdviser;

public class AllocateAdviser(ISupportProjectQueryService supportProjectQueryService, IUserRepository userRepository, ErrorService errorService, IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService), IDateValidationMessageProvider
{
    [Display(Name = "Adviser email address")]
    public string? AdviserEmailAddress { get; set; }

    [BindProperty(Name = "date-adviser-allocated", BinderType = typeof(DateInputModelBinder))]
    [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
    [Display(Name = "Date adviser was allocated")]
    public DateTime? DateAdviserAllocated { get; set; }
    public IEnumerable<User> RiseAdvisers { get; private set; }
    public string? Referrer { get; set; }

    public TaskListStatus? TaskListStatus { get; set; }
    public ProjectStatusValue? ProjectStatus { get; set; }
    
    [Display(Name = "Adviser name")]
    public string? AdviserFullName { get; set; }

    public bool ShowError { get; set; }

    public bool AdviserError => ModelState.ContainsKey("adviser") &&
                                ModelState["adviser"]?.Errors.Count > 0;

    string IDateValidationMessageProvider.SomeMissing(string displayName, IEnumerable<string> missingParts)
    {
        return $"Date must include a {string.Join(" and ", missingParts)}";
    }
    
    string IDateValidationMessageProvider.AllMissing => "Enter a date";

    public async Task<IActionResult> OnGet(int id, CancellationToken cancellationToken)
    {
        await base.GetSupportProject(id, cancellationToken);

        AdviserEmailAddress = SupportProject.AdviserEmailAddress;

        DateAdviserAllocated = SupportProject.DateAdviserAllocated;

        RiseAdvisers = await userRepository.GetAllRiseAdvisers();
        
        TaskListStatus = TaskStatusViewModel.CheckAllocateAdviserTaskListStatus(SupportProject);
        ProjectStatus = SupportProject?.ProjectStatus;
        AdviserFullName = SupportProject?.AdviserFullName;

        Referrer = TempData["AssignmentReferrer"] == null ? @Links.TaskList.Index.Page : TempData["AssignmentReferrer"].ToString();

        return Page();
    }

    public async Task<IActionResult> OnPost(int id, string selectedName, string referrer, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(selectedName))
        {
           ModelState.AddModelError("adviser", "Enter the name or email address of the adviser");
           _errorService.AddError("adviser", "Enter the name or email address of the adviser");
        }
        
        if (ModelState.ContainsKey("selectedName"))
        {
            ModelState.Remove("selectedName");
        }
        
        if (!ModelState.IsValid)
        {
            _errorService.AddErrors(Request.Form.Keys, ModelState);
            ShowError = true;
            Referrer = referrer;
            RiseAdvisers = await userRepository.GetAllRiseAdvisers();

            return await base.GetSupportProject(id, cancellationToken);
        }

        if (!string.IsNullOrEmpty(selectedName))
        {
            RiseAdvisers = await userRepository.GetAllRiseAdvisers();
            // Find the selected adviser by email address
            var selectedAdviser = RiseAdvisers.FirstOrDefault(u => u.EmailAddress == selectedName);

            if (selectedAdviser != null)
            {
                var request = new SetAdviserDetailsCommand(new SupportProjectId(id), DateAdviserAllocated, selectedAdviser.EmailAddress, selectedAdviser.FullName);
                var result = await mediator.Send(request, cancellationToken);

                if (!result)
                {
                    _errorService.AddApiError();
                    return await base.GetSupportProject(id, cancellationToken);
                }

                TaskUpdated = true;

            }
            else
            {
                _errorService.AddError("AdviserInput", "Selected adviser not found.");
                ShowError = true;
                Referrer = referrer;
                return await base.GetSupportProject(id, cancellationToken);
            }
        }

        return RedirectToPage(referrer, new { id });
    }

}
