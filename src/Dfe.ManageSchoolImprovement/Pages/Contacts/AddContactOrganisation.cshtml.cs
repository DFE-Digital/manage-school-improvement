using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.Contacts
{
    public class AddContactOrganisationModel(ISupportProjectQueryService supportProjectQueryService, ErrorService errorService) : BaseSupportProjectPageModel(supportProjectQueryService, errorService)
    {
        [BindProperty(Name = "organisationType")]
        public string? OrganisationType { get; set; }

        public string? ErrorMessage { get; set; }

        public bool ShowError { get; set; }
        public required IList<RadioButtonsLabelViewModel> RadioButtons { get; set; }

        public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
        {
            await base.GetSupportProject(id, cancellationToken);

            if (TempData["OrganisationType"] != null)
            {
                OrganisationType = (string?)TempData["OrganisationType"];
                TempData["OrganisationType"] = null;
            }
            RadioButtons = CreateOrganisationTypeOptions();
            return Page();
        }

        private static IList<RadioButtonsLabelViewModel> CreateOrganisationTypeOptions()
        {
            return new List<RadioButtonsLabelViewModel>
            {
                new ()
                {
                    Id = "organisation-type-school",
                    Name = OrganisationTypes.School,
                    Value = OrganisationTypes.School
    },
                new ()
                {
                    Id = "organisation-type-supporting-organisation",
                    Name = OrganisationTypes.SupportingOrganisation,
                    Value = OrganisationTypes.SupportingOrganisation
},
                new()
                {
                    Id = "organisation-type-governance-bodies",
                    Name = OrganisationTypes.GovernanceBodies,
                    Value = OrganisationTypes.GovernanceBodies
                }
            };
        }

        public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(OrganisationType))
            {
                RadioButtons = CreateOrganisationTypeOptions();
                ErrorMessage = "Select an organisation type";
                ShowError = true;
                _errorService.AddError("-hint", ErrorMessage);
                await base.GetSupportProject(id, cancellationToken);
                return Page();
            }
            RadioButtons = CreateOrganisationTypeOptions();
            return RedirectToPage(@Links.Contacts.AddContact.Page, new { id, OrganisationType });
        }
    }
}
