using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels; 
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.Contacts
{
    public class EditContactModel(ISupportProjectQueryService supportProjectQueryService, ErrorService errorService) : BaseSupportProjectPageModel(supportProjectQueryService, errorService)
    {
        [BindProperty(Name = "roleId")]
        public int? RoleId { get; set; }

        [BindProperty(Name = "otherRole")]
        public string OtherRole { get; set; }

        public string? ErrorMessage { get; set; }   
        public required IList<RadioButtonsLabelViewModel> RadioButtons { get; set; }

        public Guid ContactId { get; set; }

        public async Task<IActionResult> OnGetAsync(int id, Guid contactId, CancellationToken cancellationToken)
        {
            ProjectListFilters.ClearFiltersFrom(TempData);
            await base.GetSupportProject(id, cancellationToken);
            ContactId = contactId;
            if (TempData["RoleId"] != null)
            {
                RoleId = (int?)TempData["RoleId"];
                OtherRole = (string?)TempData["OtherRole"];
                TempData["RoleId"] = null;
                TempData["OtherRole"] = null;
            }
            else
            {
                var contact = SupportProject.Contacts.FirstOrDefault(a => a.Id.Value == contactId);
                if (contact != null)
                {
                    RoleId = contact.RoleId.GetHashCode();
                    OtherRole = contact.OtherRoleName;
                }
            }

            RadioButtons = ContactsUtil.GetRadioButtons(OtherRole);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id, Guid contactId, CancellationToken cancellationToken)
        {
            var hasOtherRoleName = ContactsUtil.IsOtherRoleFieldValidation(RoleId, OtherRole);
            if (!hasOtherRoleName && RoleId == RolesIds.Other.GetHashCode())
            {
                ErrorMessage = "You must enter a role"; 
                RadioButtons = ContactsUtil.GetRadioButtons(OtherRole, hasOtherRoleName);
                await base.GetSupportProject(id, cancellationToken);
                return Page();
            }
            if (RoleId != RolesIds.Other.GetHashCode())
            {
                OtherRole = null!;
            }
            return RedirectToPage(@Links.Contacts.EditContactDetail.Page, new { id, contactId, RoleId, OtherRole });
        } 
    }
}
