using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.Contacts
{
    public class EditContactOrganisationModel(ISupportProjectQueryService supportProjectQueryService, ErrorService errorService) : BaseSupportProjectPageModel(supportProjectQueryService, errorService)
    {
        [BindProperty(Name = "organisationType")]
        public string? OrganisationType { get; set; }

        public string? ErrorMessage { get; set; }

        public bool ShowError { get; set; }
        public required IList<RadioButtonsLabelViewModel> RadioButtons { get; set; }
        [BindProperty]
        public Guid ContactId { get; set; }
        public async Task<IActionResult> OnGetAsync(int id, Guid contactId, CancellationToken cancellationToken)
        {
            await base.GetSupportProject(id, cancellationToken);
            ContactId = contactId;

            if (TempData["OrganisationType"] != null)
            {
                OrganisationType = (string?)TempData["OrganisationType"];
                TempData["OrganisationType"] = null;
            }
            else
            {
                var contact = SupportProject!.Contacts!.FirstOrDefault(a => a.Id!.Value == contactId);
                if (contact != null)
                {
                    OrganisationType = contact.OrganisationType;
                }
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

            return RedirectToPage(@Links.Contacts.EditContact.Page, new { id, ContactId, OrganisationType });
        }
    }
}
