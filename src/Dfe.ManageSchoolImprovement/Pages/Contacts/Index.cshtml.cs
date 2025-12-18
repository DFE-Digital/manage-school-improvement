using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Extensions; // Add this using statement
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Utils;
using GovUK.Dfe.PersonsApi.Client.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject.SetSchoolAddress;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.Contacts
{
    public class IndexModel(
        ISupportProjectQueryService supportProjectQueryService,
        IGetEstablishment getEstablishment,
        ITrustsClient trustClient,
        IEstablishmentsClient establishmentsClient,
        IDateTimeProvider dateTimeProvider,
        ErrorService errorService,
        IMediator mediator)
        : BaseSupportProjectEstablishmentPageModel(supportProjectQueryService, getEstablishment, errorService)
    {
        public string ReturnPage { get; set; } = null!;

        public ContactViewModel? ChairOfGovernors { get; set; }
        public ContactViewModel? Headteacher { get; set; }
        public ContactViewModel? AccountingOfficer { get; set; }
        public ContactViewModel? SupportingOrganisationContact { get; set; }
        
        public IEnumerable<ContactViewModel> OtherContacts { get; set; }
        public IEnumerable<ContactViewModel> OtherSchoolContacts { get; set; }

        public string? SchoolAddress { get; set; }
        public string? SupportingOrganisationAddress { get; set; }

        public const string ChairOfGovernorsRole = "Chair of Governors";
        public const string AccountingOfficerRole = "Accounting Officer";

        public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
        {
            ProjectListFilters.ClearFiltersFrom(TempData);
            ReturnPage = @Links.SchoolList.Index.Page;
            TempData["RoleId"] = null;
            TempData["OtherRole"] = null;
            await base.GetSupportProject(id, cancellationToken);


            await SetSchoolContacts(id, cancellationToken);
            await SetSupportingOrganisationContacts(id, cancellationToken);
            
            var otherContacts = SupportProject?.Contacts
                .Where(c => c.RoleId != RolesIds.Headteacher
                && c.RoleId != RolesIds.ChairOfGovernors
                && c.RoleId != RolesIds.TrustAccountingOfficer)
                .OrderBy(c => c.RoleId);

            if (otherContacts.Any())
            {
                OtherContacts = otherContacts.Select(contact => new ContactViewModel
                {
                    Name = contact.Name,
                    Email = contact.Email,
                    Phone = contact.Phone,
                    RoleName = contact.RoleId == RolesIds.Other ? contact.OtherRoleName : contact.RoleId.GetDisplayName(),
                    ManuallyAdded = true,
                    SupportProjectId = SupportProject?.Id,
                    ContactId = contact.Id,
                    LastModifiedOn = contact.LastModifiedOn
                }).ToList();
            }


            return Page();
        }

        public async Task SetSchoolContacts(int id, CancellationToken cancellationToken)
        {
            if (!int.TryParse(SupportProject?.SchoolUrn, out var schoolUrn))
            {
                schoolUrn = 0; // Default value if parsing fails
            }


            var establishmentContacts = await establishmentsClient
                .GetAllPersonsAssociatedWithAcademyByUrnAsync(schoolUrn, cancellationToken).ConfigureAwait(false);

            if (establishmentContacts != null && establishmentContacts.Count > 0)
            {
                // Get current (non-historical) chair of governors
                var chairOfGovernors = establishmentContacts
                    .Where(c => c.Roles.Contains(ChairOfGovernorsRole) && c.IsCurrent(dateTimeProvider))
                    .OrderByDescending(x => x.UpdatedAt)
                    .FirstOrDefault();

                if (chairOfGovernors != null)
                {
                    ChairOfGovernors = new ContactViewModel()
                    {
                        Name = chairOfGovernors.DisplayName,
                        Email = chairOfGovernors.Email,
                        Phone = chairOfGovernors.Phone,
                        RoleName = ChairOfGovernorsRole,
                        LastModifiedOn = chairOfGovernors.UpdatedAt
                    };
                }
            }

            SchoolAddress = SupportProject?.Address;

            if (string.IsNullOrEmpty(SchoolAddress))
            {
                var establishment = await _getEstablishment.GetEstablishmentByUrn(SupportProject?.SchoolUrn);
                SchoolAddress = string.Join(", ", new[]
                {
                    establishment.Address.Street,
                    establishment.Address.Locality,
                    establishment.Address.Town,
                    establishment.Address.County,
                    establishment.Address.Postcode
                }.Where(x => !string.IsNullOrWhiteSpace(x)));

                var command = new SetSchoolAddressCommand(
                    new SupportProjectId(id),
                    SchoolAddress);

                var result = await mediator.Send(command, cancellationToken);

                // Early return for API error
                if (!result)
                {
                    _errorService.AddApiError();
                }
            }

            Headteacher = new ContactViewModel()
            {
                Name = SupportProject?.HeadteacherName ?? "",
                RoleName = SupportProject?.HeadteacherPreferredJobTitle ?? "",
                Phone = SupportProject?.SchoolMainPhone ?? "",
                LastModifiedOn = SupportProject?.LastModifiedOn
            };
            
            var otherschoolContacts = SupportProject?.Contacts
                .Where(c => c.RoleId == RolesIds.Headteacher
                            || c.RoleId == RolesIds.ChairOfGovernors
                            || c.RoleId == RolesIds.TrustAccountingOfficer)
                .OrderBy(c => c.RoleId);

            if (otherschoolContacts.Any())
            {
                OtherSchoolContacts = otherschoolContacts.Select(contact => new ContactViewModel
                {
                    Name = contact.Name,
                    Email = contact.Email,
                    Phone = contact.Phone,
                    RoleName = contact.RoleId == RolesIds.Other ? contact.OtherRoleName : contact.RoleId.GetDisplayName(),
                    ManuallyAdded = true,
                    SupportProjectId = SupportProject?.Id,
                    ContactId = contact.Id,
                    LastModifiedOn = contact.LastModifiedOn
                }).ToList();
            }

            if (!string.IsNullOrEmpty(SupportProject?.TrustName))
            {
                var trustContacts = await trustClient
                    .GetAllPersonsAssociatedWithTrustByTrnOrUkPrnAsync(SupportProject.TrustReferenceNumber,
                        cancellationToken).ConfigureAwait(false);

                if (trustContacts != null && trustContacts.Count > 0)
                {
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
                            RoleName = AccountingOfficerRole,
                            LastModifiedOn = accountingOfficer.UpdatedAt
                        };
                    }
                }
            }
        }

        public async Task SetSupportingOrganisationContacts(int id,
            CancellationToken cancellationToken)
        {
            var schoolIsAcademy = await getEstablishment.GetEstablishmentTrust(SupportProject?.SupportOrganisationIdNumber) ?? null;

            var contactRoleName = SupportProject?.SupportOrganisationType == "Trust" || schoolIsAcademy != null
                ? AccountingOfficerRole
                : SupportProject?.HeadteacherPreferredJobTitle;

            SupportingOrganisationAddress = SupportProject?.SupportingOrganisationContactAddress;
            SupportingOrganisationContact = new()
            {
                Name = SupportProject?.SupportingOrganisationContactName ?? "",
                RoleName = contactRoleName ?? "",
                Email = SupportProject?.SupportingOrganisationContactEmailAddress ?? "",
                Phone = SupportProject?.SupportingOrganisationContactPhone ?? "",
                Address = SupportProject?.SupportingOrganisationContactAddress ?? "",
                LastModifiedOn = SupportProject?.DateSupportingOrganisationContactDetailsAdded
            };
        }
    }
}