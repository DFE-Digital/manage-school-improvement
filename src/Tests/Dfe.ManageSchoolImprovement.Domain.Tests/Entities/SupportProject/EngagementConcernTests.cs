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

        [Fact]
        public void SetInformationPowersDetails_ShouldUpdateProperties()
        {
            // Arrange
            var concern = new EngagementConcern(_testId, _supportProjectId, new EngagementConcernDetails());
            var informationPowersInUse = true;
            var informationPowersDetails = "Information powers were used to obtain necessary data";
            var powersUsedDate = DateTime.Now;

            // Act
            concern.SetInformationPowersDetails(informationPowersInUse, informationPowersDetails, powersUsedDate);

            // Assert
            Assert.Equal(informationPowersInUse, concern.InformationPowersInUse);
            Assert.Equal(informationPowersDetails, concern.InformationPowersDetails);
            Assert.Equal(powersUsedDate, concern.PowersUsedDate);
        }

        [Fact]
        public void SetInformationPowersDetails_WithNullValues_ShouldUpdateProperties()
        {
            // Arrange
            var concern = new EngagementConcern(_testId, _supportProjectId, new EngagementConcernDetails());

            // Act
            concern.SetInformationPowersDetails(null, null, null);

            // Assert
            Assert.Null(concern.InformationPowersInUse);
            Assert.Null(concern.InformationPowersDetails);
            Assert.Null(concern.PowersUsedDate);
        }

        [Fact]
        public void SetInformationPowersDetails_WithFalseValue_ShouldUpdateProperties()
        {
            // Arrange
            var concern = new EngagementConcern(_testId, _supportProjectId, new EngagementConcernDetails());
            var informationPowersInUse = false;
            var informationPowersDetails = "Information powers were not required";
            var powersUsedDate = DateTime.Now;

            // Act
            concern.SetInformationPowersDetails(informationPowersInUse, informationPowersDetails, powersUsedDate);

            // Assert
            Assert.Equal(informationPowersInUse, concern.InformationPowersInUse);
            Assert.Equal(informationPowersDetails, concern.InformationPowersDetails);
            Assert.Equal(powersUsedDate, concern.PowersUsedDate);
        }

        [Fact]
        public void SetInterimExecutiveBoardCreated_ShouldUpdateProperties()
        {
            // Arrange
            var concern = new EngagementConcern(_testId, _supportProjectId, new EngagementConcernDetails());
            var interimExecutiveBoardCreated = true;
            var interimExecutiveBoardCreatedDetails = "Interim Executive Board was established to oversee school operations";
            var interimExecutiveBoardCreatedDate = DateTime.Now;

            // Act
            concern.SetInterimExecutiveBoardCreated(
                interimExecutiveBoardCreated,
                interimExecutiveBoardCreatedDetails,
                interimExecutiveBoardCreatedDate);

            // Assert
            Assert.Equal(interimExecutiveBoardCreated, concern.InterimExecutiveBoardCreated);
            Assert.Equal(interimExecutiveBoardCreatedDetails, concern.InterimExecutiveBoardCreatedDetails);
            Assert.Equal(interimExecutiveBoardCreatedDate, concern.InterimExecutiveBoardCreatedDate);
        }

        [Fact]
        public void SetInterimExecutiveBoardCreated_WithNullValues_ShouldUpdateProperties()
        {
            // Arrange
            var concern = new EngagementConcern(_testId, _supportProjectId, new EngagementConcernDetails());

            // Act
            concern.SetInterimExecutiveBoardCreated(null, null, null);

            // Assert
            Assert.Null(concern.InterimExecutiveBoardCreated);
            Assert.Null(concern.InterimExecutiveBoardCreatedDetails);
            Assert.Null(concern.InterimExecutiveBoardCreatedDate);
        }

        [Fact]
        public void SetInterimExecutiveBoardCreated_WithFalseValue_ShouldUpdateProperties()
        {
            // Arrange
            var concern = new EngagementConcern(_testId, _supportProjectId, new EngagementConcernDetails());
            var interimExecutiveBoardCreated = false;
            var interimExecutiveBoardCreatedDetails = "Interim Executive Board was not required";
            var interimExecutiveBoardCreatedDate = DateTime.Now;

            // Act
            concern.SetInterimExecutiveBoardCreated(
                interimExecutiveBoardCreated,
                interimExecutiveBoardCreatedDetails,
                interimExecutiveBoardCreatedDate);

            // Assert
            Assert.Equal(interimExecutiveBoardCreated, concern.InterimExecutiveBoardCreated);
            Assert.Equal(interimExecutiveBoardCreatedDetails, concern.InterimExecutiveBoardCreatedDetails);
            Assert.Equal(interimExecutiveBoardCreatedDate, concern.InterimExecutiveBoardCreatedDate);
        }

        [Fact]
        public void SetInformationPowersDetails_CalledMultipleTimes_ShouldUpdateEachTime()
        {
            // Arrange
            var concern = new EngagementConcern(_testId, _supportProjectId, new EngagementConcernDetails());

            // First call
            concern.SetInformationPowersDetails(true, "Initial details", DateTime.Now.AddDays(-1));

            var newPowersInUse = false;
            var newDetails = "Updated information powers details";
            var newDate = DateTime.Now;

            // Act - Second call
            concern.SetInformationPowersDetails(newPowersInUse, newDetails, newDate);

            // Assert
            Assert.Equal(newPowersInUse, concern.InformationPowersInUse);
            Assert.Equal(newDetails, concern.InformationPowersDetails);
            Assert.Equal(newDate, concern.PowersUsedDate);
        }

        [Fact]
        public void SetInterimExecutiveBoardCreated_CalledMultipleTimes_ShouldUpdateEachTime()
        {
            // Arrange
            var concern = new EngagementConcern(_testId, _supportProjectId, new EngagementConcernDetails());

            // First call
            concern.SetInterimExecutiveBoardCreated(true, "Initial IEB details", DateTime.Now.AddDays(-1));

            var newIebCreated = false;
            var newDetails = "Updated IEB details";
            var newDate = DateTime.Now;

            // Act - Second call
            concern.SetInterimExecutiveBoardCreated(newIebCreated, newDetails, newDate);

            // Assert
            Assert.Equal(newIebCreated, concern.InterimExecutiveBoardCreated);
            Assert.Equal(newDetails, concern.InterimExecutiveBoardCreatedDetails);
            Assert.Equal(newDate, concern.InterimExecutiveBoardCreatedDate);
        }

        [Fact]
        public void SetInformationPowersDetails_DoesNotAffectOtherProperties()
        {
            // Arrange
            var concern = new EngagementConcern(_testId, _supportProjectId, new EngagementConcernDetails());
            concern.SetEngagementConcernDetails("Original details", "Summary", DateTime.Now.AddDays(-2));
            concern.SetInterimExecutiveBoardCreated(true, "IEB details", DateTime.Now.AddDays(-1));

            var originalDetails = concern.EngagementConcernDetails;
            var originalIebCreated = concern.InterimExecutiveBoardCreated;

            // Act
            concern.SetInformationPowersDetails(true, "Information powers details", DateTime.Now);

            // Assert - Other properties remain unchanged
            Assert.Equal(originalDetails, concern.EngagementConcernDetails);
            Assert.Equal(originalIebCreated, concern.InterimExecutiveBoardCreated);
        }

        [Fact]
        public void SetInterimExecutiveBoardCreated_DoesNotAffectOtherProperties()
        {
            // Arrange
            var concern = new EngagementConcern(_testId, _supportProjectId, new EngagementConcernDetails());
            concern.SetEngagementConcernDetails("Original details", "Summary", DateTime.Now.AddDays(-2));
            concern.SetInformationPowersDetails(true, "Powers details", DateTime.Now.AddDays(-1));

            var originalDetails = concern.EngagementConcernDetails;
            var originalPowersInUse = concern.InformationPowersInUse;

            // Act
            concern.SetInterimExecutiveBoardCreated(true, "IEB details", DateTime.Now);

            // Assert - Other properties remain unchanged
            Assert.Equal(originalDetails, concern.EngagementConcernDetails);
            Assert.Equal(originalPowersInUse, concern.InformationPowersInUse);
        }
    }
}