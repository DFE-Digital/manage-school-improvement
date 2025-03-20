using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels; 
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.Contacts
{
    public class AddContactModel(ISupportProjectQueryService supportProjectQueryService, ErrorService errorService) : BaseSupportProjectPageModel(supportProjectQueryService, errorService)
    {
        [BindProperty(Name = "roleId")]
        public int? RoleId { get; set; }

        [BindProperty(Name = "otherRole")]
        public string? OtherRole { get; set; } 
        public string? ErrorMessage { get; set; }    
        public required IList<RadioButtonsLabelViewModel> RadioButtons { get; set; }
          
        public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
        {
            ProjectListFilters.ClearFiltersFrom(TempData);
            await base.GetSupportProject(id, cancellationToken);
            if (TempData["RoleId"] != null)
            {
                RoleId = (int?)TempData["RoleId"];
                OtherRole = (string?)TempData["OtherRole"];
                TempData["RoleId"] = null;
                TempData["OtherRole"] = null;
            }
            RadioButtons = ContactsUtil.GetRadioButtons(OtherRole);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken)
        {
            if (!RoleId.HasValue)
            {
                RadioButtons = ContactsUtil.GetRadioButtons(OtherRole);
                ErrorMessage = "You must select a role"; 
                await base.GetSupportProject(id, cancellationToken);
                return Page();
            }
            var hasOtherRoleName = ContactsUtil.IsOtherRoleFieldValidation(RoleId, OtherRole);
            if (!hasOtherRoleName && RoleId == RolesIds.Other.GetHashCode())
            { 
                ErrorMessage = "You must enter a role";
                RadioButtons = ContactsUtil.GetRadioButtons(OtherRole, hasOtherRoleName);
                await base.GetSupportProject(id, cancellationToken);
                return Page();
            }
            if(RoleId != RolesIds.Other.GetHashCode())
            {
                OtherRole = null!;
            } 
            return RedirectToPage(@Links.Contacts.AddContactDetail.Page, new { id, RoleId, OtherRole });
        } 
    }
}
