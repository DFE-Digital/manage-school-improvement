using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.OfstedReports
{
    public class IndexModel(ISupportProjectQueryService supportProjectQueryService, IGetEstablishment getEstablishment, ErrorService errorService) : BaseSupportProjectEstablishmentPageModel(supportProjectQueryService, getEstablishment, errorService)
    {
        public string ReturnPage { get; set; }
        public string PreviousQualityOfEducation { get; private set; }
        public string PreviousInspectionDate { get; private set; }
        public string PreviousBehaviourAndAttitudes { get; private set; }
        public string PreviousPersonalDevelopment { get; private set; }
        public string PreviousLeadershipAndManagement { get; private set; }

        public void SetErrorPage(string errorPage)
        {
            TempData["ErrorPage"] = errorPage;
        }
        public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
        {
            ReturnPage = @Links.SchoolList.Index.Page;
            await base.GetSupportProject(id, cancellationToken);

            var establishmentOfstedData = await _getEstablishment.GetEstablishmentOfstedDataByUrn(SupportProject.SchoolUrn);

            PreviousQualityOfEducation = establishmentOfstedData.PreviousQualityOfEducation;
            PreviousInspectionDate = establishmentOfstedData.PreviousInspectionStartDate;
            PreviousBehaviourAndAttitudes = establishmentOfstedData.PreviousBehaviourAndAttitudes;
            PreviousPersonalDevelopment = establishmentOfstedData.PreviousPersonalDevelopment;
            PreviousLeadershipAndManagement = establishmentOfstedData.PreviousEffectivenessOfLeadershipAndManagement;

            return Page();
        }
    }
}
