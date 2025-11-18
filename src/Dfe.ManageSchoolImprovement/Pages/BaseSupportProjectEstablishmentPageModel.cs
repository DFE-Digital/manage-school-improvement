using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages;

public class BaseSupportProjectEstablishmentPageModel(ISupportProjectQueryService supportProjectQueryService, IGetEstablishment getEstablishment, ErrorService errorService) : PageModel
{
    protected readonly ISupportProjectQueryService _supportProjectQueryService = supportProjectQueryService;
    protected readonly IGetEstablishment _getEstablishment = getEstablishment;
    protected readonly ErrorService _errorService = errorService;
    public SupportProjectViewModel? SupportProject { get; set; }

    public virtual async Task<IActionResult> GetSupportProject(int id, CancellationToken cancellationToken)
    {
        return await GetProject(id, cancellationToken);
    }

    protected async Task<IActionResult> GetProject(int id, CancellationToken cancellationToken)
    {
        var result = await _supportProjectQueryService.GetSupportProject(id, cancellationToken);

        if (result.IsSuccess && result.Value != null)
        {
            SupportProject = SupportProjectViewModel.Create(result.Value);

            var establishment = await _getEstablishment.GetEstablishmentByUrn(result.Value.SchoolUrn);

            SupportProject.QualityOfEducation = establishment.MISEstablishment.QualityOfEducation;
            SupportProject.LastInspectionDate = establishment.OfstedLastInspection;
            SupportProject.BehaviourAndAttitudes = establishment.MISEstablishment.BehaviourAndAttitudes;
            SupportProject.PersonalDevelopment = establishment.MISEstablishment.PersonalDevelopment;
            SupportProject.LeadershipAndManagement = establishment.MISEstablishment.EffectivenessOfLeadershipAndManagement;
            SupportProject.OftedReportWeblink = establishment.MISEstablishment.Weblink;
            SupportProject.Diocese = establishment.Diocese.Name ?? "Not applicable";
            SupportProject.SchoolPhase = establishment.PhaseOfEducation.Name;
            SupportProject.ReligiousCharacter = establishment.ReligiousCharacter.Name;
            SupportProject.NumbersOnRoll = establishment.Census.NumberOfPupils;
            SupportProject.PreviousUrn = establishment.PreviousEstablishment?.Urn ?? "Not applicable";
            SupportProject.SchoolType = establishment.EstablishmentType.Name;
            SupportProject.HeadteacherName = $"{establishment.HeadteacherTitle} {establishment.HeadteacherFirstName} {establishment.HeadteacherLastName}";
            SupportProject.HeadteacherPreferredJobTitle = establishment.HeadteacherPreferredJobTitle;
            SupportProject.SchoolMainPhone = establishment.MainPhone;
        }

        if (!result.IsSuccess)
        {
            return NotFound();
        }
        return Page();
    }
}
