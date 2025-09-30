using Dfe.ManageSchoolImprovement.Application.SupportProject.Models;
using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.Models
{
    public class EngagementConcernViewModelTests
    {
        [Fact]
        public void Create_WithValidDto_ShouldMapAllProperties()
        {
            // Arrange
            var dto = new EngagementConcernDto
            (
                Id: Guid.NewGuid(),
                ReadableId: 123,
                SupportProjectId: 456,
                EngagementConcernDetails: "Test Details",
                EngagementConcernSummary: "Test Summary",
                EngagementConcernRaisedDate: DateTime.Now,
                EngagementConcernResolved: true,
                EngagementConcernResolvedDetails: "Resolution Details",
                EngagementConcernResolvedDate: DateTime.Now,
                EngagementConcernEscalationConfirmStepsTaken: true,
                EngagementConcernEscalationPrimaryReason: "Primary Reason",
                EngagementConcernEscalationDetails: "Escalation Details",
                EngagementConcernEscalationDateOfDecision: DateTime.Now,
                EngagementConcernEscalationWarningNotice: "Warning Notice"
            );

            // Act
            var viewModel = EngagementConcernViewModel.Create(dto);

            // Assert
            Assert.Equal(dto.Id, viewModel.Id.Value);
            Assert.Equal(dto.ReadableId, viewModel.ReadableId);
            Assert.Equal(dto.SupportProjectId, viewModel.SupportProjectId);
            Assert.Equal(dto.EngagementConcernDetails, viewModel.EngagementConcernDetails);
            Assert.Equal(dto.EngagementConcernSummary, viewModel.EngagementConcernSummary);
            Assert.Equal(dto.EngagementConcernRaisedDate, viewModel.EngagementConcernRaisedDate);
            Assert.Equal(dto.EngagementConcernResolved, viewModel.EngagementConcernResolved);
            Assert.Equal(dto.EngagementConcernResolvedDetails, viewModel.EngagementConcernResolvedDetails);
            Assert.Equal(dto.EngagementConcernResolvedDate, viewModel.EngagementConcernResolvedDate);
            Assert.Equal(dto.EngagementConcernEscalationConfirmStepsTaken, viewModel.EngagementConcernEscalationConfirmStepsTaken);
            Assert.Equal(dto.EngagementConcernEscalationPrimaryReason, viewModel.EngagementConcernEscalationPrimaryReason);
            Assert.Equal(dto.EngagementConcernEscalationDetails, viewModel.EngagementConcernEscalationDetails);
            Assert.Equal(dto.EngagementConcernEscalationDateOfDecision, viewModel.EngagementConcernEscalationDateOfDecision);
            Assert.Equal(dto.EngagementConcernEscalationWarningNotice, viewModel.EngagementConcernEscalationWarningNotice);
        }

        [Fact]
        public void Create_WithNullValues_ShouldMapNullProperties()
        {
            // Arrange
            var dto = new EngagementConcernDto
            (
                Id: Guid.NewGuid(),
                ReadableId: 123,
                SupportProjectId: 456,
                EngagementConcernDetails: null,
                EngagementConcernSummary: null,
                EngagementConcernRaisedDate: null,
                EngagementConcernResolved: null,
                EngagementConcernResolvedDetails: null,
                EngagementConcernResolvedDate: null,
                EngagementConcernEscalationConfirmStepsTaken: null,
                EngagementConcernEscalationPrimaryReason: null,
                EngagementConcernEscalationDetails: null,
                EngagementConcernEscalationDateOfDecision: null,
                EngagementConcernEscalationWarningNotice: null
            );

            // Act
            var viewModel = EngagementConcernViewModel.Create(dto);

            // Assert
            Assert.Equal(dto.Id, viewModel.Id.Value);
            Assert.Equal(dto.ReadableId, viewModel.ReadableId);
            Assert.Equal(dto.SupportProjectId, viewModel.SupportProjectId);
            Assert.Null(viewModel.EngagementConcernDetails);
            Assert.Null(viewModel.EngagementConcernSummary);
            Assert.Null(viewModel.EngagementConcernRaisedDate);
            Assert.Null(viewModel.EngagementConcernResolved);
            Assert.Null(viewModel.EngagementConcernResolvedDetails);
            Assert.Null(viewModel.EngagementConcernResolvedDate);
            Assert.Null(viewModel.EngagementConcernEscalationConfirmStepsTaken);
            Assert.Null(viewModel.EngagementConcernEscalationPrimaryReason);
            Assert.Null(viewModel.EngagementConcernEscalationDetails);
            Assert.Null(viewModel.EngagementConcernEscalationDateOfDecision);
            Assert.Null(viewModel.EngagementConcernEscalationWarningNotice);
        }

        [Fact]
        public void Create_WithMinimumRequiredFields_ShouldMapCorrectly()
        {
            // Arrange
            var dto = new EngagementConcernDto
            (
                Id: Guid.NewGuid(),
                ReadableId: 123,
                SupportProjectId: 456,
                EngagementConcernDetails: null,
                EngagementConcernSummary: null,
                EngagementConcernRaisedDate: null,
                EngagementConcernResolved: null,
                EngagementConcernResolvedDetails: null,
                EngagementConcernResolvedDate: null,
                EngagementConcernEscalationConfirmStepsTaken: null,
                EngagementConcernEscalationPrimaryReason: null,
                EngagementConcernEscalationDetails: null,
                EngagementConcernEscalationDateOfDecision: null,
                EngagementConcernEscalationWarningNotice: null
            );

            // Act
            var viewModel = EngagementConcernViewModel.Create(dto);

            // Assert
            Assert.NotNull(viewModel);
            Assert.Equal(dto.Id, viewModel.Id.Value);
            Assert.Equal(dto.ReadableId, viewModel.ReadableId);
            Assert.Equal(dto.SupportProjectId, viewModel.SupportProjectId);
        }
    }
}