using Dfe.ManageSchoolImprovement.Application.Common.Models;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Models;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Frontend.Pages.EngagementConcern.EscalateEngagementConcern;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using static Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.EngagementConcern.SetSupportProjectEngagementConcernEscalation;

namespace Dfe.ManageSchoolImprovement.Tests.Pages.EngagementConcern.EscalateEngagementConcern
{
    public class BaseEngagementConcernPageModelTests
    {
        private readonly Mock<ISupportProjectQueryService> _mockSupportProjectQueryService;
        private readonly Mock<ErrorService> _mockErrorService;
        private readonly Mock<IMediator> _mockMediator;
        private readonly TestEngagementConcernPageModel _sut;

        public BaseEngagementConcernPageModelTests()
        {
            _mockSupportProjectQueryService = new Mock<ISupportProjectQueryService>();
            _mockErrorService = new Mock<ErrorService>();
            _mockMediator = new Mock<IMediator>();
            _sut = new TestEngagementConcernPageModel(
                _mockSupportProjectQueryService.Object,
                _mockErrorService.Object,
                _mockMediator.Object);
        }

        [Fact]
        public async Task HandleEscalationPost_WhenSuccessful_RedirectsToDefaultPage()
        {
            // Arrange
            const int id = 1;
            var engagementConcernId = Guid.NewGuid();
            var confirmStepsTaken = true;
            var primaryReason = "Test Reason";
            var escalationDetails = "Test Details";
            var dateOfDecision = DateTime.Today;

            _mockSupportProjectQueryService
                .Setup(x => x.GetSupportProject(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<SupportProjectDto?>.Success(new SupportProjectDto(Id: id, CreatedOn: DateTime.Now, LastModifiedOn: DateTime.Now)));

            _mockMediator
                .Setup(x => x.Send(It.IsAny<SetSupportProjectEngagementConcernEscalationCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _sut.HandleEscalationPost(
                engagementConcernId,
                id,
                new EngagementConcernEscalationDetails
                {
                    ConfirmStepsTaken = confirmStepsTaken,
                    PrimaryReason = primaryReason,
                    Details = escalationDetails,
                    DateOfDecision = dateOfDecision
                },
                false);

            // Assert
            Assert.IsType<RedirectToPageResult>(result);
            var redirectResult = (RedirectToPageResult)result;
            Assert.Equal("DefaultPage", redirectResult.PageName);
        }

        [Fact]
        public async Task HandleEscalationPost_WhenApiCallFails_ReturnsPageWithError()
        {
            // Arrange
            const int id = 1;
            var engagementConcernId = Guid.NewGuid();
            _mockSupportProjectQueryService
                .Setup(x => x.GetSupportProject(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<SupportProjectDto?>.Success(new SupportProjectDto(Id: id, CreatedOn: DateTime.Now, LastModifiedOn: DateTime.Now)));

            _mockMediator
                .Setup(x => x.Send(It.IsAny<SetSupportProjectEngagementConcernEscalationCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var result = await _sut.HandleEscalationPost(engagementConcernId,
                id,
                new EngagementConcernEscalationDetails
                {
                    ConfirmStepsTaken = true,
                    PrimaryReason = "reason",
                    Details = "details",
                    DateOfDecision = DateTime.Today
                },
                false);

            // Assert
            Assert.IsType<PageResult>(result);
        }

        [Fact]
        public async Task HandleEscalationPost_UsesExistingValuesWhenParametersAreNull()
        {
            // Arrange
            const int id = 1;
            var engagementConcernId = Guid.NewGuid();
            var engagementConcernDetails = "some details";
            var engagementConcernSummary = "some summary";
            var engagementConcernRaisedDate = DateTime.UtcNow.AddDays(-5);
            var existingConfirmStepsTaken = true;
            var existingPrimaryReason = "Existing Reason";
            var existingDetails = "Existing Details";
            var existingDate = DateTime.Today.AddDays(-1);
            var existingWarningNotice = "Existing Warning Notice";

            var engagementConcernDto = new EngagementConcernDto(engagementConcernId, 
                1, 
                id,
                engagementConcernDetails,
                engagementConcernSummary,
                engagementConcernRaisedDate,
                false,
                null,
                null,
                EngagementConcernEscalationConfirmStepsTaken: existingConfirmStepsTaken,
                EngagementConcernEscalationPrimaryReason: existingPrimaryReason,
                EngagementConcernEscalationDetails: existingDetails,
                EngagementConcernEscalationDateOfDecision: existingDate,
                EngagementConcernEscalationWarningNotice: existingWarningNotice);

            _mockSupportProjectQueryService
                .Setup(x => x.GetSupportProject(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result<SupportProjectDto?>.Success(new SupportProjectDto(Id: id, CreatedOn: DateTime.Now, LastModifiedOn: DateTime.Now, EngagementConcerns: new[] { engagementConcernDto })));

            _mockMediator
                 .Setup(x => x.Send(It.IsAny<SetSupportProjectEngagementConcernEscalationCommand>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(true);

            // Act
            var result = await _sut.HandleEscalationPost(engagementConcernId,
                id,
                new EngagementConcernEscalationDetails
                {
                    ConfirmStepsTaken = null,
                    PrimaryReason = null,
                    Details = null,
                    DateOfDecision = null
                }, false);

            // Assert
            _mockMediator.Verify(x => x.Send(
                It.Is<SetSupportProjectEngagementConcernEscalationCommand>(cmd =>
                    cmd.EngagementConcernEscalationConfirmStepsTaken == existingConfirmStepsTaken &&
                    cmd.EngagementConcernEscalationPrimaryReason == existingPrimaryReason &&
                    cmd.EngagementConcernEscalationDetails == existingDetails &&
                    cmd.EngagementConcernEscalationDateOfDecision == existingDate),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        private class TestEngagementConcernPageModel : BaseEngagementConcernPageModel
        {
            public TestEngagementConcernPageModel(
                ISupportProjectQueryService supportProjectQueryService,
                ErrorService errorService,
                IMediator mediator)
                : base(supportProjectQueryService, errorService, mediator)
            {
            }

            protected internal override IActionResult GetDefaultRedirect(int id, object? routeValues = null)
            {
                return RedirectToPage("DefaultPage", new { id });
            }
        }
    }
}
