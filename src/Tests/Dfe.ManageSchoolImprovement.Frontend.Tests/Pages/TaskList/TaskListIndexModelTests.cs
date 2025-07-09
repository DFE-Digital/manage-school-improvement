using Dfe.ManageSchoolImprovement.Application.Common.Models;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Models;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using DfE.CoreLibs.Contracts.Academies.V4.Establishments;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.Pages.TaskList
{
    public class TaskListIndexModelTests
    {
        private readonly Mock<ISupportProjectQueryService> _mockSupportProjectQueryService;
        private readonly Mock<IGetEstablishment> _mockGetEstablishment;
        private readonly Mock<ErrorService> _mockErrorService;
        private readonly IndexModel _indexModel;
        private readonly string _urn;

        public TaskListIndexModelTests()
        {
            _urn = "URN234";
            _mockSupportProjectQueryService = new Mock<ISupportProjectQueryService>();
            _mockGetEstablishment = new Mock<IGetEstablishment>();
            _mockErrorService = new Mock<ErrorService>();

            // Create the IndexModel with the mocked dependencies
            _indexModel = new IndexModel(
                _mockSupportProjectQueryService.Object,
                _mockGetEstablishment.Object,
                _mockErrorService.Object
            )
            {
                TempData = (new Mock<ITempDataDictionary>()).Object
            };
            _mockGetEstablishment.Setup(x => x.GetEstablishmentByUrn(_urn)).ReturnsAsync(new DfE.CoreLibs.Contracts.Academies.V4.Establishments.EstablishmentDto
            {
                EstablishmentType = new NameAndCodeDto() { Name = "School" },
                Diocese = new NameAndCodeDto()
                {
                    Name = "TestDiocese",
                    Code = "1"
                },

                PhaseOfEducation = new NameAndCodeDto()
                {
                    Name = "TestPhase",
                    Code = "2"
                },

                ReligiousCharacter = new NameAndCodeDto()
                {
                    Name = "TestReli",
                    Code = "3"
                },

                Census = new CensusDto()
                {
                    NumberOfPupils = "1234"
                },

                MISEstablishment = new DfE.CoreLibs.Contracts.Academies.V4.Establishments.MisEstablishmentDto
                {
                    QualityOfEducation = "Good",
                    BehaviourAndAttitudes = "Good",
                    PersonalDevelopment = "Good",
                    EffectivenessOfLeadershipAndManagement = "Good"
                },
                OfstedLastInspection = DateTime.Now.ToString()
            });
        }

        [Fact]
        public async Task OnGetAsync_ShouldSetTaskListStatusesAndReturnPage()
        {
            // Arrange
            int projectId = 1;
            var cancellationToken = CancellationToken.None;
            var mockProject = () => Result<SupportProjectDto?>.Success(new SupportProjectDto(projectId, DateTime.Now, "schoolName", _urn, "local Authority", "Region"));
            _mockSupportProjectQueryService.Setup(service => service.GetSupportProject(projectId, cancellationToken))
                .ReturnsAsync(mockProject);

            // Act
            var result = await _indexModel.OnGetAsync(projectId, cancellationToken);

            // Assert
            Assert.Equal(TaskListStatus.NotStarted, _indexModel.ContactTheResponsibleBodyTaskListStatus);
            Assert.Equal(TaskListStatus.NotStarted, _indexModel.RecordTheSchoolResponseTaskListStatus);
            Assert.Equal(TaskListStatus.NotStarted, _indexModel.CheckThePotentialAdviserConflictsOfInterestTaskListStatus);
            Assert.Equal(TaskListStatus.NotStarted, _indexModel.AllocateAdviserTaskListStatus);
            Assert.Equal(TaskListStatus.NotStarted, _indexModel.SendIntroductoryEmailTaskListStatus);
            Assert.Equal(TaskListStatus.NotStarted, _indexModel.ArrangeAdvisersFirstFaceToFaceVisitTaskListStatus);
            Assert.Equal(TaskListStatus.NotStarted, _indexModel.CompleteAndSaveInitialDiagnosisTemplateTaskListStatus);
            Assert.Equal(TaskListStatus.NotStarted, _indexModel.NoteOfVisitTaskListStatus);
            Assert.Equal(TaskListStatus.NotStarted, _indexModel.RecordVisitDateToVisitSchoolTaskListStatus);
            Assert.Equal(TaskListStatus.NotStarted, _indexModel.ChosePreferredSupportingOrganisationTaskListStatus);
            Assert.Equal(TaskListStatus.NotStarted, _indexModel.RecordSupportDecisionTaskListStatus);
            Assert.Equal(TaskListStatus.NotStarted, _indexModel.DueDiligenceOnPreferredSupportingOrganisationTaskListStatus);
            Assert.Equal(TaskListStatus.NotStarted, _indexModel.SetRecordSupportingOrganisationAppointment);
            Assert.Equal(TaskListStatus.NotStarted, _indexModel.SupportingOrganisationContactDetailsTaskListStatus);
            Assert.Equal(TaskListStatus.NotStarted, _indexModel.ShareTheIndicativeFundingBandAndTheImprovementPlanTemplateTaskListStatus);
            Assert.Equal(TaskListStatus.NotStarted, _indexModel.RecordImprovementPlanDecisionTaskListStatus);
            Assert.Equal(TaskListStatus.NotStarted, _indexModel.SendAgreedImprovementPlanForApprovalTaskListStatus);
            Assert.Equal(TaskListStatus.NotStarted, _indexModel.RequestPlanningGrantOfferLetterTaskListStatus);
            Assert.Equal(TaskListStatus.NotStarted, _indexModel.ConfirmPlanningGrantOfferLetterTaskListStatus);
            Assert.Equal(TaskListStatus.NotStarted, _indexModel.ReviewTheImprovementPlanTaskListStatus);
            Assert.Equal(TaskListStatus.NotStarted, _indexModel.RequestImprovementGrantOfferLetterTaskListStatus);
            Assert.Equal(TaskListStatus.NotStarted, _indexModel.ConfirmImprovementGrantOfferLetterTaskListStatus);

            // Verify the return page
            Assert.Equal("/SchoolList/Index", _indexModel.ReturnPage);

            // Ensure the action result is a PageResult
            Assert.IsType<PageResult>(result);
        }
    }
}
