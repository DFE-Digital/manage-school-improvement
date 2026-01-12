using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using Dfe.ManageSchoolImprovement.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.Contacts
{
    public class AddContactModel(ISupportProjectQueryService supportProjectQueryService, ErrorService errorService) : BaseSupportProjectPageModel(supportProjectQueryService, errorService)
    {
        [BindProperty(Name = "organisationTypeSubCategory")]
        public string? OrganisationTypeSubCategory { get; set; }

        [BindProperty(Name = "organisationTypeSubCategoryOther")]
        public string? OrganisationTypeSubCategoryOther { get; set; }
        public string? ErrorMessage { get; set; }

        public bool ShowError { get; set; }

        public required IList<RadioButtonsLabelViewModel> SchoolRadioButtons { get; set; }

        public required IList<RadioButtonsLabelViewModel> SupportingOrganisationRadioButtons { get; set; }

        public required IList<RadioButtonsLabelViewModel> GoverningBodyRadioButtons { get; set; }


        [BindProperty]
        public string? OrganisationType { get; set; }
        public async Task<IActionResult> OnGetAsync(int id, string organisationType, CancellationToken cancellationToken)
        {
            ProjectListFilters.ClearFiltersFrom(TempData);
            await base.GetSupportProject(id, cancellationToken);
            OrganisationType = organisationType;
            TempData["OrganisationType"] = organisationType;

            if (TempData["OrganisationTypeSubCategory"] != null)
            {
                OrganisationTypeSubCategory = (string?)TempData["OrganisationTypeSubCategory"];
                OrganisationTypeSubCategoryOther = (string?)TempData["OrganisationTypeSubCategoryOther"];
                TempData["OrganisationTypeSubCategory"] = null;
                TempData["OrganisationTypeSubCategoryOther"] = null;
            }

            SchoolRadioButtons = ContactsUtil.GetSchoolRadioButtons(OrganisationTypeSubCategoryOther);
            SupportingOrganisationRadioButtons = ContactsUtil.GetSupportingOrganisationRadioButtons(OrganisationTypeSubCategoryOther);
            GoverningBodyRadioButtons = ContactsUtil.GetGoverningBodyRadioButtons(OrganisationTypeSubCategoryOther);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(OrganisationTypeSubCategory))
            {
                SchoolRadioButtons = ContactsUtil.GetSchoolRadioButtons(OrganisationTypeSubCategoryOther);
                SupportingOrganisationRadioButtons = ContactsUtil.GetSupportingOrganisationRadioButtons(OrganisationTypeSubCategoryOther);
                GoverningBodyRadioButtons = ContactsUtil.GetGoverningBodyRadioButtons(OrganisationTypeSubCategoryOther);

                ErrorMessage = "Select a role";
                ShowError = true;
                _errorService.AddError("-hint", ErrorMessage);
                await base.GetSupportProject(id, cancellationToken);
                return Page();
            }

            var IsOtherCategoryValid = RadioButtonOtherOptionInputIsValid();

            if (!IsOtherCategoryValid)
            {
                if (OrganisationTypeSubCategory == SchoolOrginisationTypes.Other.GetDisplayName() ||
                    OrganisationTypeSubCategory == SchoolOrginisationTypes.Other.GetDisplayName())
                {
                    ModelState.AddModelError("organisationTypeSubCategoryOther", "Enter a job title");

                }
                else
                {
                    ModelState.AddModelError("organisationTypeSubCategoryOther", "Enter a governance body");

                }

                _errorService.AddErrors(Request.Form.Keys, ModelState);

                ShowError = true;

                SchoolRadioButtons = ContactsUtil.GetSchoolRadioButtons(OrganisationTypeSubCategoryOther, IsOtherCategoryValid);
                SupportingOrganisationRadioButtons = ContactsUtil.GetSupportingOrganisationRadioButtons(OrganisationTypeSubCategoryOther, IsOtherCategoryValid);
                GoverningBodyRadioButtons = ContactsUtil.GetGoverningBodyRadioButtons(OrganisationTypeSubCategoryOther, IsOtherCategoryValid);
                await base.GetSupportProject(id, cancellationToken);
                return Page();
            }

            return RedirectToPage(@Links.Contacts.AddContactDetail.Page, new { id, OrganisationType, OrganisationTypeSubCategory, OrganisationTypeSubCategoryOther });
        }

        private bool RadioButtonOtherOptionInputIsValid()
        {
            if (OrganisationType == OrganisationTypes.School)
            {
                if (OrganisationTypeSubCategory == SchoolOrginisationTypes.Other.GetDisplayName() && string.IsNullOrEmpty(OrganisationTypeSubCategoryOther))
                {
                    return false;
                }
            }

            if (OrganisationType == OrganisationTypes.SupportingOrganisation)
            {
                if (OrganisationTypeSubCategory == SupportOrganisationTypes.Other.GetDisplayName() && string.IsNullOrEmpty(OrganisationTypeSubCategoryOther))
                {
                    return false;
                }
            }

            if (OrganisationType == OrganisationTypes.GovernanceBodies)
            {
                if (OrganisationTypeSubCategory == GovernanceBodyTypes.Other.GetDisplayName() && string.IsNullOrEmpty(OrganisationTypeSubCategoryOther))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
