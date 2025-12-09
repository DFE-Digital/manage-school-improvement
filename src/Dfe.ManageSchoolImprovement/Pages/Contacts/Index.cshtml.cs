using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Extensions; // Add this using statement
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Utils;
using GovUK.Dfe.PersonsApi.Client.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.Contacts
{
    public class IndexModel(ISupportProjectQueryService supportProjectQueryService,
        IGetEstablishment getEstablishment,
        ITrustsClient trustClient,
        IEstablishmentsClient establishmentsClient,
        IDateTimeProvider dateTimeProvider,
        IWebHostEnvironment hostingEnvironment,
        ErrorService errorService) : BaseSupportProjectEstablishmentPageModel(supportProjectQueryService, getEstablishment, errorService)
    {
        public string ReturnPage { get; set; } = null!;

        public ContactViewModel? ChairOfGoverningBody { get; set; } = null;
        public ContactViewModel? Headteacher { get; set; } = null;
        public ContactViewModel? ChiefFinancialOfficer { get; set; } = null;
        public ContactViewModel? AccountingOfficer { get; set; } = null;

        public const string ChairOfGoverningBodyRole = "Chair of Local Governing Body";
        public const string ChiefFinancialOfficerRole = "Chief Financial Officer";
        public const string AccountingOfficerRole = "Accounting Officer";

        public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
        {
            ProjectListFilters.ClearFiltersFrom(TempData);
            ReturnPage = @Links.SchoolList.Index.Page;
            TempData["RoleId"] = null;
            TempData["OtherRole"] = null;
            await base.GetSupportProject(id, cancellationToken);

            if (!int.TryParse(SupportProject?.SchoolUrn, out var schoolUrn))
            {
                schoolUrn = 0; // Default value if parsing fails
            }

            var establishmentContacts = await establishmentsClient.GetAllPersonsAssociatedWithAcademyByUrnAsync(schoolUrn, cancellationToken).ConfigureAwait(false);

            if (establishmentContacts != null && establishmentContacts.Count > 0)
            {
                // Get current (non-historical) chair of governors
                var chairOfGovernors = establishmentContacts
                    .Where(c => c.Roles.Contains(ChairOfGoverningBodyRole) && c.IsCurrent(dateTimeProvider))
                    .OrderByDescending(x => x.UpdatedAt)
                    .FirstOrDefault();

                if (chairOfGovernors != null)
                {
                    ChairOfGoverningBody = new ContactViewModel()
                    {
                        Name = chairOfGovernors.DisplayName,
                        Email = chairOfGovernors.Email,
                        Phone = chairOfGovernors.Phone,
                        RoleName = ChairOfGoverningBodyRole
                    };
                }
            }

            if (!string.IsNullOrEmpty(SupportProject?.TrustName))
            {
                var trustContacts = await trustClient.GetAllPersonsAssociatedWithTrustByTrnOrUkPrnAsync(SupportProject.TrustReferenceNumber, cancellationToken).ConfigureAwait(false);

                if (trustContacts != null && trustContacts.Count > 0)
                {
                    // Get current (non-historical) trust contacts
                    var chiefFinancialOfficer = trustContacts
                        .Where(c => c.Roles.Contains(ChiefFinancialOfficerRole) && c.IsCurrent(dateTimeProvider))
                        .FirstOrDefault();

                    if (chiefFinancialOfficer != null)
                    {
                        ChiefFinancialOfficer = new ContactViewModel()
                        {
                            Name = chiefFinancialOfficer.DisplayName,
                            Email = chiefFinancialOfficer.Email,
                            Phone = chiefFinancialOfficer.Phone,
                            RoleName = ChiefFinancialOfficerRole
                        };
                    }

                    var accountingOfficer = trustContacts
                        .Where(c => c.Roles.Contains(AccountingOfficerRole) && c.IsCurrent(dateTimeProvider))
                        .FirstOrDefault();

                    if (accountingOfficer != null)
                    {
                        AccountingOfficer = new ContactViewModel()
                        {
                            Name = accountingOfficer.DisplayName,
                            Email = accountingOfficer.Email,
                            Phone = accountingOfficer.Phone,
                            RoleName = AccountingOfficerRole
                        };
                    }
                }
            }


            return Page();
        }
    }
}