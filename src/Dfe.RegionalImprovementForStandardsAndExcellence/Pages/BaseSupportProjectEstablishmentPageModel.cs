using Dfe.RegionalImprovementForStandardsAndExcellence.Application.SupportProject.Queries;
using Dfe.RegionalImprovementForStandardsAndExcellence.Frontend.Models.SupportProject;
using Dfe.RegionalImprovementForStandardsAndExcellence.Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dfe.RegionalImprovementForStandardsAndExcellence.Frontend.Pages;

public class BaseSupportProjectEstablishmentPageModel(ISupportProjectQueryService supportProjectQueryService,IGetEstablishment getEstablishment,ErrorService errorService) : PageModel
{
    protected readonly ISupportProjectQueryService _supportProjectQueryService = supportProjectQueryService;
    protected readonly IGetEstablishment _getEstablishment = getEstablishment;
    protected readonly ErrorService _errorService = errorService;
    public SupportProjectViewModel SupportProject { get; set; }
    
    public virtual async Task<IActionResult> GetSupportProject(int id, CancellationToken cancellationToken)
    {
        return await GetProject(id,cancellationToken);
    }
    protected async Task<IActionResult> GetProject(int id, CancellationToken cancellationToken)
    {
        
        var result = await _supportProjectQueryService.GetSupportProject(id, cancellationToken);
        
        if (result.IsSuccess && result.Value != null)
        {
            SupportProject = SupportProjectViewModel.Create(result.Value);
            
            DfE.CoreLibs.Contracts.Academies.V4.Establishments.EstablishmentDto establishment = await _getEstablishment.GetEstablishmentByUrn(result.Value.SchoolUrn);
            
            SupportProject.QualityOfEducation = establishment.MISEstablishment.QualityOfEducation;
            SupportProject.LastInspectionDate = establishment.OfstedLastInspection;
            SupportProject.BehaviourAndAttitudes = establishment.MISEstablishment.BehaviourAndAttitudes;
            SupportProject.PersonalDevelopment = establishment.MISEstablishment.PersonalDevelopment;
            SupportProject.LeadershipAndManagement = establishment.MISEstablishment.EffectivenessOfLeadershipAndManagement;
        }
        
        if (!result.IsSuccess)
        {
            return NotFound();
        }
        return Page();
    }
}