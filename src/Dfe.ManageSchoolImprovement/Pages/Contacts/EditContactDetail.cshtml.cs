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
    public class EditContactDetailModel(ISupportProjectQueryService supportProjectQueryService, ErrorService errorService, IMediator mediator) : BaseSupportProjectPageModel(supportProjectQueryService, errorService)
    {
        public string ReturnPage { get; set; }
        public bool ShowError { get; set; }
        [BindProperty(Name = "name")]
        [Required(ErrorMessage = "You must enter a name")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "You must enter an organsiation")]
        [BindProperty(Name = "organisation")]
        public string Organisation { get; set; } = null!;

        [Required(ErrorMessage = "You must enter an email address")]
        [EmailValidation(ErrorMessage = "Email address must be in correct format")]
        [BindProperty(Name = "email-address")] 
        public string EmailAddress { get; set; }

        [PhoneValidation]
        [BindProperty(Name = "phone")]
        public string? Phone { get; set; }
        public Guid ContactId { get; set; }
        public int RoleId { get; set; }
        public string? OtherRole { get; set; }
        public async Task<IActionResult> OnGetAsync(int id, Guid contactId, int roleId, string? otherRole, CancellationToken cancellationToken)
        {
            ReturnPage = Links.Contacts.EditContact.Page;
            ContactId = contactId;
            await base.GetSupportProject(id, cancellationToken);
            var contact = SupportProject.Contacts.FirstOrDefault(a => a.Id.Value == contactId);
            if (contact != null)
            {
                Name = contact.Name;
                Organisation = contact.Organisation;
                EmailAddress = contact.Email;
                Phone = contact.Phone; 
            }
            RoleId = roleId;
            OtherRole = otherRole;
            TempData["RoleId"] = roleId;
            TempData["OtherRole"] = otherRole;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id, Guid contactId, int roleId, string? otherRole, CancellationToken cancellationToken)
        {
            if (EmailAddress != null && EmailAddress.Any(char.IsWhiteSpace))
            {
                ModelState.AddModelError("email-address", "Email address must not contain spaces");
            } 
            //exclude phone validation only if no value
            if (!ModelState.IsValid)
            {
                _errorService.AddErrors(Request.Form.Keys, ModelState);
                ShowError = true;
                return await base.GetSupportProject(id, cancellationToken);
            } 
               
            var supportProjectContactId = new SupportProjectContactId(contactId);
            var request = new UpdateSupportProjectContactCommand(new SupportProjectId(id), new SupportProjectContactId(contactId), Name, (RolesIds)roleId, otherRole!, Organisation, EmailAddress!, Phone, User.GetDisplayName()!);

            var result = await mediator.Send(request, cancellationToken);

            if (result.Value != contactId)
            {
                _errorService.AddApiError();
                return await base.GetSupportProject(id, cancellationToken);
            }
           
            TempData["RoleId"] = null;
            TempData["OtherRole"] = null;
            TempData["contactAddedOrUpdated"] = "updated";
            return RedirectToPage(@Links.Contacts.Index.Page, new { id });
        }
    }
}
