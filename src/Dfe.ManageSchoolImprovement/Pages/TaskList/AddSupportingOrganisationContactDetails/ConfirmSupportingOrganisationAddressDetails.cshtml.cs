using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Extensions;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Utils;
using GovUK.Dfe.PersonsApi.Client.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.AddSupportingOrganisationContactDetails;

public class AddSupportingOrganisationAddressDetailsModel(
    ISupportProjectQueryService supportProjectQueryService,
    IGetEstablishment getEstablishment,
    IGetTrust getTrust,
    ErrorService errorService,
    IMediator mediator,
    IDateTimeProvider dateTimeProvider) : BaseSupportProjectPageModel(supportProjectQueryService, errorService),
    IDateValidationMessageProvider
{
    [BindProperty(Name = "address-1")] public string? AddressLine1 { get; set; }

    [BindProperty(Name = "address-2")] public string? AddressLine2 { get; set; }

    [BindProperty(Name = "town")] public string? Town { get; set; }

    [BindProperty(Name = "county")] public string? County { get; set; }

    [PostcodeValidation(ErrorMessage = "Postcode must be in correct format")]
    [BindProperty(Name = "postcode")]
    public string? Postcode { get; set; }

    [BindProperty] public string? SupportingOrganisationName { get; set; }

    [BindProperty] public string? SupportingOrganisationId { get; set; }

    public bool ShowError { get; set; }

    public async Task<IActionResult> OnGet(int id, CancellationToken cancellationToken)
    {
        await base.GetSupportProject(id, cancellationToken);

        if (SupportProject?.SupportOrganisationType == "School")
        {
            var expectedSchool =
                await getEstablishment.GetEstablishmentByUrn(SupportProject.SupportOrganisationIdNumber!);

            var expectedTrust = await getEstablishment.GetEstablishmentTrust(expectedSchool.Urn) ?? null;

            if (expectedTrust != null)
            {
                AddressLine1 = expectedTrust.Address.Street;
                AddressLine2 = expectedTrust.Address.Locality;
                Town = expectedTrust.Address.Town;
                County = expectedTrust.Address.County;
                Postcode = expectedTrust.Address.Postcode;
            }
            else
            {
                AddressLine1 = expectedSchool.Address.Street;
                AddressLine2 = expectedSchool.Address.Locality;
                Town = expectedSchool.Address.Town;
                County = expectedSchool.Address.County;
                Postcode = expectedSchool.Address.Postcode;
            }
        }

        if (SupportProject is { SupportOrganisationType: "Trust", SupportOrganisationIdNumber: not null })
        {
            var expectedTrust = await getTrust.GetTrustByUkprn(SupportProject.SupportOrganisationIdNumber);

            AddressLine1 = expectedTrust.Address.Street;
            AddressLine2 = expectedTrust.Address.Locality;
            Town = expectedTrust.Address.Town;
            County = expectedTrust.Address.County;
            Postcode = expectedTrust.Address.Postcode;
        }

        SupportingOrganisationName = SupportProject?.SupportOrganisationName;
        SupportingOrganisationId = SupportProject?.SupportOrganisationIdNumber;

        return Page();
    }

    public async Task<IActionResult> OnPost(int id, CancellationToken cancellationToken)
    {
        await base.GetSupportProject(id, cancellationToken);

        AddressLine1 = AddressLine1?.Trim();
        AddressLine2 = AddressLine2?.Trim();
        Town = Town?.Trim();
        County = County?.Trim();
        Postcode = Postcode?.Trim().ToUpper();

        var address = string.Join(", ", new[]
        {
            AddressLine1, AddressLine2, Town, County, Postcode
        }.Where(x => !string.IsNullOrWhiteSpace(x)));

        if (string.IsNullOrWhiteSpace(AddressLine1))
        {
            ModelState.AddModelError("address-1", "Enter a street address");
        }

        if (string.IsNullOrWhiteSpace(Town))
        {
            ModelState.AddModelError("town", "Enter a town or city");
        }

        if (string.IsNullOrWhiteSpace(Postcode))
        {
            ModelState.AddModelError("postcode", "Enter a postcode");
        }

        if (!ModelState.IsValid)
        {
            _errorService.AddErrors(Request.Form.Keys, ModelState);
            ShowError = true;
            await base.GetSupportProject(id, cancellationToken);
            return Page();
        }
        
        var request = new SetChoosePreferredSupportingOrganisationCommand(
            new SupportProjectId(id),
            SupportProject?.SupportOrganisationName,
            SupportProject?.SupportOrganisationIdNumber,
            SupportProject?.SupportOrganisationType,
            SupportProject?.DateSupportOrganisationChosen,
            SupportProject?.AssessmentToolTwoCompleted,
            SupportProject?.SupportingOrganisationAddress,
            SupportProject?.SupportingOrganisationContactName,
            SupportProject?.SupportingOrganisationContactEmailAddress,
            SupportProject?.SupportingOrganisationContactPhone,
            address);

        var result = await mediator.Send(request, cancellationToken);

        if (!result)
        {
            _errorService.AddApiError();
            return await base.GetSupportProject(id, cancellationToken);
        }

        TaskUpdated = true;
        return RedirectToPage(@Links.TaskList.Index.Page, new { id });
    }
}