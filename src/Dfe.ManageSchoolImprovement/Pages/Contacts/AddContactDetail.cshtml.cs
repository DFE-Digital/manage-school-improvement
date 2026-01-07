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
    public class AddContactDetailModel(ISupportProjectQueryService supportProjectQueryService, ErrorService errorService, IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService)
    {
        public string ReturnPage { get; set; }
        public bool ShowError { get; set; }
        [BindProperty(Name = "name")]
        [Required(ErrorMessage = "Enter a name")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Enter an organisation")]
        [BindProperty(Name = "organisation")]
        public string Organisation { get; set; } = null!;

        [Required(ErrorMessage = "Enter an email address")]
        [EmailValidation(ErrorMessage = "Email address must be in correct format")]
        [BindProperty(Name = "email-address")]
        public string EmailAddress { get; set; }

        [PhoneValidation]
        [BindProperty(Name = "phone")]
        public string? Phone { get; set; }
        public string OrganisationTypeSubCategory { get; set; }
        public string? OrganisationTypeSubCategoryOther { get; set; }
        public string OrganisationType { get; set; }
        public async Task<IActionResult> OnGetAsync(int id, string organisationType, string organisationTypeSubCategory, string? organisationTypeSubCategoryOther, CancellationToken cancellationToken)
        {
            ReturnPage = Links.Contacts.AddContact.Page;
            OrganisationTypeSubCategory = organisationTypeSubCategory;
            OrganisationTypeSubCategoryOther = organisationTypeSubCategoryOther;
            OrganisationType = organisationType;
            TempData["OrganisationTypeSubCategory"] = organisationTypeSubCategory;
            TempData["OrganisationTypeSubCategoryOther"] = organisationTypeSubCategoryOther;
            await base.GetSupportProject(id, cancellationToken);
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int id, string organisationType, string organisationTypeSubCategory, string? organisationTypeSubCategoryOther, CancellationToken cancellationToken)
        {
            if (EmailAddress != null && EmailAddress.Any(char.IsWhiteSpace))
            {
                ModelState.AddModelError("email-address", "Email address must not contain spaces");
            }
            if (!ModelState.IsValid)
            {
                _errorService.AddErrors(Request.Form.Keys, ModelState);
                ShowError = true;
                return await base.GetSupportProject(id, cancellationToken);
            }
            var request = new CreateSupportProjectContactCommand(new SupportProjectId(id), Name, organisationTypeSubCategory, organisationTypeSubCategoryOther!, organisationType, EmailAddress!, Phone!, User.GetDisplayName()!);

            var result = await mediator.Send(request, cancellationToken);

            if (result == null)
            {
                _errorService.AddApiError();
                return await base.GetSupportProject(id, cancellationToken);
            }

            TempData["OrganisationTypeSubCategory"] = null;
            TempData["OtherRole"] = null;
            TempData["OrganisationTypeSubCategoryOther"] = "added";
            return RedirectToPage(@Links.Contacts.Index.Page, new { id });
        }
    }
}
