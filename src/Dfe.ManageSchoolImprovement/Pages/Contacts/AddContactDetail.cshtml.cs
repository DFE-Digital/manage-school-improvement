using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.SupportProjectContacts;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.AddSupportingOrganisationContactDetails;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using System.ComponentModel.DataAnnotations;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.Contacts
{
    public class AddContactDetailModel(
        ISupportProjectQueryService supportProjectQueryService,
        ErrorService errorService,
        IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService)
    {
        public string ReturnPage { get; set; } = null!;
        public bool ShowError { get; set; }

        [BindProperty(Name = "name")]
        [Required(ErrorMessage = "Enter a name")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Enter an email address")]
        [EmailValidation(ErrorMessage = "Enter an email address in the correct format, like name@example.com")]
        [BindProperty(Name = "email-address")]
        public string EmailAddress { get; set; } = null!;

        [PhoneValidation]
        [BindProperty(Name = "phone")]
        public string? Phone { get; set; }

        [BindProperty(Name = "JobTitle")] public string? JobTitle { get; set; }

        [BindProperty] public string OrganisationTypeSubCategory { get; set; } = null!;
        [BindProperty] public string? OrganisationTypeSubCategoryOther { get; set; }
        [BindProperty] public string OrganisationType { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(int id, string organisationType, string organisationTypeSubCategory,
            string? organisationTypeSubCategoryOther, CancellationToken cancellationToken)
        {
            await GetSupportProject(id, cancellationToken).ConfigureAwait(false);

            ReturnPage = Links.Contacts.AddContact.Page;
            OrganisationTypeSubCategory = organisationTypeSubCategory;
            OrganisationTypeSubCategoryOther = organisationTypeSubCategoryOther;
            OrganisationType = organisationType;
            TempData["OrganisationTypeSubCategory"] = organisationTypeSubCategory;
            TempData["OrganisationTypeSubCategoryOther"] = organisationTypeSubCategoryOther;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id, string organisationType,
            string organisationTypeSubCategory, string? organisationTypeSubCategoryOther,
            CancellationToken cancellationToken)
        {
            ReturnPage = Links.Contacts.AddContact.Page;

            if (EmailAddress != null && EmailAddress.Any(char.IsWhiteSpace))
            {
                ModelState.AddModelError("email-address", "Email address must not contain spaces");
            }

            if (OrganisationType == OrganisationTypes.GovernanceBodies && string.IsNullOrEmpty(JobTitle))
            {
                ModelState.AddModelError("JobTitle", "Enter job title");
            }

            if (!ModelState.IsValid)
            {
                _errorService.AddErrors(Request.Form.Keys, ModelState);
                ShowError = true;
                return await base.GetSupportProject(id, cancellationToken);
            }

            var request = new CreateSupportProjectContactCommand(new SupportProjectId(id), Name,
                organisationTypeSubCategory, organisationTypeSubCategoryOther!, organisationType, EmailAddress!, Phone!,
                User.GetDisplayName()!, JobTitle);

            var result = await mediator.Send(request, cancellationToken);

            if (result == null)
            {
                _errorService.AddApiError();
                return await base.GetSupportProject(id, cancellationToken);
            }

            TempData["OrganisationType"] = null;
            TempData["OrganisationTypeSubCategory"] = null;
            TempData["OrganisationTypeSubCategoryOther"] = null;
            TempData["contactAddedOrUpdated"] = "added";
            return RedirectToPage(@Links.Contacts.Index.Page, new { id });
        }
    }
}