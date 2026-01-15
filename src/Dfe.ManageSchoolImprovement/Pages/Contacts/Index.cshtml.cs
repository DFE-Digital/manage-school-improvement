using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Extensions; // Add this using statement
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Utils;
using GovUK.Dfe.PersonsApi.Client.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
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
        public ContactViewModel? SupportingOrganisationAccountingOfficer { get; set; } = new();
        public ContactViewModel? SupportingOrganisationHeadteacher { get; set; } = new();

        public IEnumerable<GovernanceContactsGroup> GovernanceContacts { get; private set; } =
            Enumerable.Empty<GovernanceContactsGroup>();

        public IEnumerable<ContactViewModel> OtherSchoolContacts { get; set; } = new List<ContactViewModel>();

        public IEnumerable<ContactViewModel> OtherSupportingOrganisationContacts { get; set; } =
            new List<ContactViewModel>();

        public IEnumerable<ContactViewModel> OtherContacts { get; set; } = new List<ContactViewModel>();

        public string? SchoolAddress { get; set; }
        public string? SupportingOrganisationAddress { get; set; }

        public const string ChairOfGovernorsRole = "Chair of Governors";
        public const string AccountingOfficerRole = "Accounting Officer";
        public const string HeadteacherRole = "Headteacher";

        public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
        {
            ProjectListFilters.ClearFiltersFrom(TempData);
            ReturnPage = @Links.SchoolList.Index.Page;
            TempData["RoleId"] = null;
            TempData["OtherRole"] = null;
            await base.GetSupportProject(id, cancellationToken);

            await SetSchoolContacts(id, cancellationToken);

            if (SupportProject?.SupportOrganisationType is "School" or "Trust"
                && SupportProject.SupportOrganisationIdNumber != null)
            {
                await SetSupportingOrganisationContacts();
            }

            SetOtherSupportingOrganisationContacts();

            var governanceBodiesContacts = SupportProject?.Contacts?
                .Where(c => c.OrganisationType == "Governance bodies")
                .OrderBy(c => c.CreatedOn);

            GovernanceContacts = BuildGovernanceContacts(governanceBodiesContacts);

            var otherContacts = SupportProject?.Contacts?
                .Where(c => string.IsNullOrEmpty(c.OrganisationType))
                .OrderBy(c => c.CreatedOn)
                .ToList();

            if (otherContacts != null && otherContacts.Any())
            {
                OtherContacts = otherContacts.Select(contact =>
                {
                    var fallbackRolename = "Not available";
                    if (contact.RoleId != null && contact.RoleId != RolesIds.Other)
                    {
                        fallbackRolename = contact.RoleId.GetDisplayName();
                    }
                    else if (contact.RoleId == RolesIds.Other)
                    {
                        fallbackRolename = contact.OtherRoleName;
                    }

                    return new ContactViewModel
                    {
                        Name = contact.Name,
                        Email = contact.Email,
                        Phone = contact.Phone,
                        RoleName = (!string.IsNullOrEmpty(contact.OrganisationTypeSubCategory)
                            ? contact.OrganisationTypeSubCategory
                            : fallbackRolename)!,
                        ManuallyAdded = true,
                        SupportProjectId = SupportProject?.Id,
                        ContactId = contact.Id,
                        LastModifiedOn = contact.LastModifiedOn
                    };
                }).ToList();
            }

            return Page();
        }

        private void SetOtherSupportingOrganisationContacts()
        {
            var otherSupportingOrganisationContacts = SupportProject?.Contacts?
    .Where(c => c.OrganisationType == "Supporting organisation")
    .OrderBy(c => c.CreatedOn);

            if (otherSupportingOrganisationContacts != null && otherSupportingOrganisationContacts.Any())
            {
                OtherSupportingOrganisationContacts = otherSupportingOrganisationContacts.Select(contact =>
                    new ContactViewModel
                    {
                        Name = contact.Name,
                        Email = contact.Email,
                        Phone = contact.Phone,
                        RoleName = contact.OrganisationTypeSubCategory,
                        ManuallyAdded = true,
                        SupportProjectId = SupportProject?.Id,
                        ContactId = contact.Id,
                        LastModifiedOn = contact.LastModifiedOn
                    }).ToList();
            }
        }

        private async Task SetSchoolContacts(int id, CancellationToken cancellationToken)
        {
            SchoolAddress = SupportProject?.Address;

            if (string.IsNullOrEmpty(SchoolAddress))
            {
                await GetAndSaveSchoolAddress(id, cancellationToken);
            }

            Headteacher = new ContactViewModel
            {
                Name = SupportProject?.HeadteacherName ?? "",
                RoleName = SupportProject?.HeadteacherPreferredJobTitle ?? HeadteacherRole,
                Phone = SupportProject?.SchoolMainPhone ?? "",
                LastModifiedOn = SupportProject?.LastModifiedOn
            };

            if (!int.TryParse(SupportProject?.SchoolUrn, out var schoolUrn))
            {
                schoolUrn = 0; // Default value if parsing fails
            }

            var establishmentContacts = await establishmentsClient
                .GetAllPersonsAssociatedWithAcademyByUrnAsync(schoolUrn, cancellationToken).ConfigureAwait(false);

            if (establishmentContacts != null && establishmentContacts.Count > 0)
            {
                var chairOfGovernors = establishmentContacts
                    .Where(c => c.Roles.Contains(ChairOfGovernorsRole) && c.IsCurrent(dateTimeProvider))
                    .OrderByDescending(x => x.UpdatedAt)
                    .FirstOrDefault();

                if (chairOfGovernors != null)
                {
                    ChairOfGovernors = new ContactViewModel
                    {
                        Name = chairOfGovernors.DisplayName,
                        Email = chairOfGovernors.Email,
                        Phone = chairOfGovernors.Phone,
                        RoleName = ChairOfGovernorsRole,
                        LastModifiedOn = DateTime.Today
                    };
                }
            }

            if (!string.IsNullOrEmpty(SupportProject?.TrustName))
            {
                var trustContacts = await trustClient
                    .GetAllPersonsAssociatedWithTrustByTrnOrUkPrnAsync(SupportProject.TrustReferenceNumber,
                        cancellationToken).ConfigureAwait(false);

                if (trustContacts != null && trustContacts.Count > 0)
                {
                    var accountingOfficer = trustContacts
                        .FirstOrDefault(c => c.Roles.Contains(AccountingOfficerRole) && c.IsCurrent(dateTimeProvider));

                    if (accountingOfficer != null)
                    {
                        AccountingOfficer = new ContactViewModel
                        {
                            Name = accountingOfficer.DisplayName,
                            Email = accountingOfficer.Email,
                            Phone = accountingOfficer.Phone,
                            RoleName = AccountingOfficerRole,
                            LastModifiedOn = DateTime.Today
                        };
                    }
                }
            }

            var otherSchoolContacts = SupportProject?.Contacts?
                .Where(c => c.OrganisationType == "School")
                .OrderBy(c => c.CreatedOn);

            if (otherSchoolContacts != null && otherSchoolContacts.Any())
            {
                OtherSchoolContacts = otherSchoolContacts.Select(contact => new ContactViewModel
                {
                    Name = contact.Name,
                    Email = contact.Email,
                    Phone = contact.Phone,
                    RoleName = contact.OrganisationTypeSubCategory,
                    ManuallyAdded = true,
                    SupportProjectId = SupportProject?.Id,
                    ContactId = contact.Id,
                    LastModifiedOn = contact.LastModifiedOn
                }).ToList();
            }
        }

        private async Task SetSupportingOrganisationContacts()
        {
            SupportingOrganisationAddress = SupportProject?.SupportingOrganisationContactAddress;

            if (SupportProject is { SupportOrganisationType: "School", SupportOrganisationIdNumber: not null })
            {
                var supportingSchoolIsAcademy =
                    await getEstablishment.GetEstablishmentTrust(SupportProject.SupportOrganisationIdNumber) ?? null;

                if (supportingSchoolIsAcademy != null)
                {
                    var supportingSchool =
                        await getEstablishment.GetEstablishmentByUrn(SupportProject.SupportOrganisationIdNumber);
                    var headteacherName = supportingSchool.HeadteacherFirstName + " " +
                                          supportingSchool.HeadteacherLastName;

                    SupportingOrganisationHeadteacher = new ContactViewModel
                    {
                        Name = headteacherName,
                        Phone = supportingSchool.MainPhone,
                        RoleName = !string.IsNullOrEmpty(supportingSchool.HeadteacherPreferredJobTitle)
                            ? supportingSchool.HeadteacherPreferredJobTitle
                            : HeadteacherRole,
                        LastModifiedOn = Convert.ToDateTime(supportingSchool.GiasLastChangedDate,
                            new CultureInfo("en-GB"))
                    };

                    if (SupportProject?.SupportingOrganisationContactName != null)
                    {
                        SupportingOrganisationAccountingOfficer = new ContactViewModel
                        {
                            Name = SupportProject.SupportingOrganisationContactName ?? "",
                            RoleName = AccountingOfficerRole,
                            Email = SupportProject.SupportingOrganisationContactEmailAddress ?? "",
                            Phone = SupportProject.SupportingOrganisationContactPhone ?? "",
                            Address = SupportProject.SupportingOrganisationContactAddress ?? "",
                            LastModifiedOn = SupportProject.DateSupportingOrganisationContactDetailsAdded
                        };
                    }
                }
                else
                {
                    SupportingOrganisationHeadteacher = new ContactViewModel
                    {
                        Name = SupportProject?.SupportingOrganisationContactName ?? "",
                        RoleName = HeadteacherRole,
                        Email = SupportProject?.SupportingOrganisationContactEmailAddress ?? "",
                        Phone = SupportProject?.SupportingOrganisationContactPhone ?? "",
                        Address = SupportProject?.SupportingOrganisationContactAddress ?? "",
                        LastModifiedOn = SupportProject?.DateSupportingOrganisationContactDetailsAdded
                    };
                }
            }

            if (SupportProject?.SupportOrganisationType == "Trust" &&
                SupportProject.SupportingOrganisationContactName != null)
            {
                SupportingOrganisationAccountingOfficer = new ContactViewModel
                {
                    Name = SupportProject.SupportingOrganisationContactName ?? "",
                    RoleName = AccountingOfficerRole,
                    Email = SupportProject.SupportingOrganisationContactEmailAddress ?? "",
                    Phone = SupportProject.SupportingOrganisationContactPhone ?? "",
                    Address = SupportProject.SupportingOrganisationContactAddress ?? "",
                    LastModifiedOn = SupportProject.DateSupportingOrganisationContactDetailsAdded
                };
            }
        }

        // Method is here because we were not saving school addresses prior to 12/25 - could be deleted in future once no
        // longer needed (from 12/25 address saved at the point school was added)
        private async Task GetAndSaveSchoolAddress(int id, CancellationToken cancellationToken)
        {
            if (SupportProject?.SchoolUrn != null)
            {
                var establishment = await _getEstablishment.GetEstablishmentByUrn(SupportProject.SchoolUrn);

                SchoolAddress = string.Join(", ", new[]
                {
                    establishment.Address.Street,
                    establishment.Address.Locality,
                    establishment.Address.Town,
                    establishment.Address.County,
                    establishment.Address.Postcode
                }.Where(x => !string.IsNullOrWhiteSpace(x)));
            }

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

        private IEnumerable<ContactViewModel> GetGovernanceBodiesContacts(
            IEnumerable<SupportProjectContact> governanceBodiesContacts, GovernanceBodyTypes governanceBodiesType)
        {
            return governanceBodiesContacts
                .Where(c => c.OrganisationTypeSubCategory == governanceBodiesType.GetDisplayName())
                .Select(contact =>
                {
                    if (contact.JobTitle != null)
                        return new ContactViewModel
                        {
                            Name = contact.Name,
                            Email = contact.Email,
                            Phone = contact.Phone,
                            RoleName = contact.JobTitle,
                            ManuallyAdded = true,
                            SupportProjectId = SupportProject?.Id,
                            ContactId = contact.Id,
                            LastModifiedOn = contact.LastModifiedOn
                        };
                    return new ContactViewModel();
                }).ToList();
        }

        private IEnumerable<GovernanceContactsGroup> BuildGovernanceContacts(
            IEnumerable<SupportProjectContact>? governanceBodiesContacts)
        {
            if (governanceBodiesContacts == null)
            {
                return Enumerable.Empty<GovernanceContactsGroup>();
            }

            var orderedTypes = new[]
            {
                GovernanceBodyTypes.Trust,
                GovernanceBodyTypes.LocalAuthority,
                GovernanceBodyTypes.Diocese,
                GovernanceBodyTypes.Foundation,
                GovernanceBodyTypes.Federation,
                GovernanceBodyTypes.Other
            };

            return orderedTypes
                .Select(type => new GovernanceContactsGroup(type,
                    GetGovernanceBodiesContacts(governanceBodiesContacts, type)))
                .Where(group => group.Contacts.Any())
                .ToList();
        }
    }

    public record GovernanceContactsGroup(GovernanceBodyTypes Type, IEnumerable<ContactViewModel> Contacts);
}
