using System;
using Xunit;
using Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;

namespace Dfe.ManageSchoolImprovement.Domain.Tests.Entities.SupportProject
{
    public class EngagementConcernTests
    {
        private readonly EngagementConcernId _testId;
        private readonly SupportProjectId _supportProjectId;

        public EngagementConcernTests()
        {
            _testId = new EngagementConcernId(Guid.NewGuid());
            _supportProjectId = new SupportProjectId(1);
        }

        [Fact]
        public void Constructor_ShouldInitializePropertiesCorrectly()
        {
            // Arrange
            var engagementConcernDetails = new EngagementConcernDetails()
            {
                Details = "Test concern details",
                Summary = "Test concern summary",
                RaisedDate = DateTime.Now,
                Resolved = true,
                ResolvedDetails = "Test resolved details",
                ResolvedDate = DateTime.Now,
            };

            // Act
            var concern = new EngagementConcern(
                _testId,
                _supportProjectId,
                engagementConcernDetails
            );

            // Assert
            Assert.Equal(_testId, concern.Id);
            Assert.Equal(_supportProjectId, concern.SupportProjectId);
            Assert.Equal(engagementConcernDetails.Details, concern.EngagementConcernDetails);
            Assert.Equal(engagementConcernDetails.RaisedDate, concern.EngagementConcernRaisedDate);
            Assert.Equal(engagementConcernDetails.Resolved, concern.EngagementConcernResolved);
            Assert.Equal(engagementConcernDetails.ResolvedDetails, concern.EngagementConcernResolvedDetails);
            Assert.Equal(engagementConcernDetails.ResolvedDate, concern.EngagementConcernResolvedDate);
        }

        [Fact]
        public void SetEngagementConcernDetails_ShouldUpdateProperties()
        {
            // Arrange
            var concern = new EngagementConcern(_testId, _supportProjectId, new EngagementConcernDetails());
            var details = "Updated concern details";
            var summary = "Updated concern summary";
            var raisedDate = DateTime.Now;

            // Act
            concern.SetEngagementConcernDetails(details, summary, raisedDate);

            // Assert
            Assert.Equal(details, concern.EngagementConcernDetails);
            Assert.Equal(raisedDate, concern.EngagementConcernRaisedDate);
        }

        [Fact]
        public void SetEngagementConcernResolvedDetails_ShouldUpdateProperties()
        {
            // Arrange
            var concern = new EngagementConcern(_testId, _supportProjectId, new EngagementConcernDetails());
            var resolved = true;
            var resolvedDetails = "Resolution details";
            var resolvedDate = DateTime.Now;

            // Act
            concern.SetEngagementConcernResolvedDetails(resolved, resolvedDetails, resolvedDate);

            // Assert
            Assert.Equal(resolved, concern.EngagementConcernResolved);
            Assert.Equal(resolvedDetails, concern.EngagementConcernResolvedDetails);
            Assert.Equal(resolvedDate, concern.EngagementConcernResolvedDate);
        }

        [Fact]
        public void SetEngagementConcernEscalationDetails_ShouldUpdateProperties()
        {
            // Arrange
            var concern = new EngagementConcern(_testId, _supportProjectId, new EngagementConcernDetails());
            var confirmStepsTaken = true;
            var primaryReason = "Primary reason";
            var escalationDetails = "Escalation details";
            var dateOfDecision = DateTime.Now;
            var warningNotice = "Warning notice";

            // Act
            concern.SetEngagementConcernEscalationDetails(
                confirmStepsTaken,
                primaryReason,
                escalationDetails,
                dateOfDecision,
                warningNotice
            );
            // Assert
            Assert.Equal(confirmStepsTaken, concern.EngagementConcernEscalationConfirmStepsTaken);
            Assert.Equal(primaryReason, concern.EngagementConcernEscalationPrimaryReason);
            Assert.Equal(escalationDetails, concern.EngagementConcernEscalationDetails);
            Assert.Equal(dateOfDecision, concern.EngagementConcernEscalationDateOfDecision);
            Assert.Equal(warningNotice, concern.EngagementConcernEscalationWarningNotice);
        }
        [Fact]
        public void Constructor_WithNullValues_ShouldCreateValidInstance()
        {
            // Arrange & Act
            var concern = new EngagementConcern(_testId, _supportProjectId, new EngagementConcernDetails());

            // Assert
            Assert.NotNull(concern);
            Assert.Equal(_testId, concern.Id);
            Assert.Equal(_supportProjectId, concern.SupportProjectId);
            Assert.Null(concern.EngagementConcernDetails);
            Assert.Null(concern.EngagementConcernRaisedDate);
            Assert.Null(concern.EngagementConcernResolved);
            Assert.Null(concern.EngagementConcernResolvedDetails);
            Assert.Null(concern.EngagementConcernResolvedDate);
        }
    }
}