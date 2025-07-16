using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject.SetAdviserDetails;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.AllocateAdviser;

public class AllocateAdviser(ISupportProjectQueryService supportProjectQueryService, IUserRepository userRepository, ErrorService errorService, IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService), IDateValidationMessageProvider
{
    [BindProperty(Name = "adviser-email-address")]
    [RiseAdviserEmail]
    public string? AdviserEmailAddress { get; set; }

    [BindProperty(Name = "date-adviser-allocated", BinderType = typeof(DateInputModelBinder))]
    [DateValidation(DateRangeValidationService.DateRange.PastOrToday)]
    [Display(Name = "date adviser was allocated")]
    public DateTime? DateAdviserAllocated { get; set; }
    public IEnumerable<User> RiseAdvisers { get; private set; }
    public string? Referrer { get; set; }


    public bool ShowError { get; set; }

    public bool AdviserEmailAddressError
    {
        get
        {
            return ModelState.Any(x => x.Key == "adviser-email-address");
        }
    }

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

        AdviserEmailAddress = SupportProject.AdviserEmailAddress;

        DateAdviserAllocated = SupportProject.DateAdviserAllocated;

        RiseAdvisers = await userRepository.GetAllRiseAdvisers();

        Referrer = TempData["AssignmentReferrer"] == null ? @Links.TaskList.Index.Page : TempData["AssignmentReferrer"].ToString();

        return Page();
    }

    public async Task<IActionResult> OnPost(int id, string selectedName, string referrer, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            _errorService.AddErrors(Request.Form.Keys, ModelState);
            ShowError = true;
            Referrer = referrer;
            return await base.GetSupportProject(id, cancellationToken);
        }

        if (!string.IsNullOrEmpty(selectedName))
        {
            RiseAdvisers = await userRepository.GetAllRiseAdvisers();
            // Find the selected adviser by email address
            var selectedAdviser = RiseAdvisers.FirstOrDefault(u => u.EmailAddress == selectedName);
            if (selectedAdviser != null)
            {
                AdviserEmailAddress = selectedAdviser.EmailAddress;
            }
        }

        var request = new SetAdviserDetailsCommand(new SupportProjectId(id), DateAdviserAllocated, AdviserEmailAddress);

        var result = await mediator.Send(request, cancellationToken);

        if (!result)
        {
            _errorService.AddApiError();
            return await base.GetSupportProject(id, cancellationToken);
        }

        TaskUpdated = true;
        return RedirectToPage(referrer, new { id });
    }

}
