using Dfe.ManageSchoolImprovement.Domain.Common;
using Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;

namespace Dfe.ManageSchoolImprovement.Domain.Tests.Entities.SupportProject
{
    public class ImprovementPlanObjectiveProgressTests
    {
        private readonly ImprovementPlanObjectiveProgressId _progressId;
        private readonly ImprovementPlanObjectiveId _objectiveId;
        private readonly ImprovementPlanReviewId _reviewId;
        private readonly string _howIsSchoolProgressing;
        private readonly string _progressDetails;

        public ImprovementPlanObjectiveProgressTests()
        {
            _progressId = new ImprovementPlanObjectiveProgressId(Guid.NewGuid());
            _objectiveId = new ImprovementPlanObjectiveId(Guid.NewGuid());
            _reviewId = new ImprovementPlanReviewId(Guid.NewGuid());
            _howIsSchoolProgressing = "On track";
            _progressDetails = "Good progress has been made";
        }

        #region Constructor Tests

        [Fact]
        public void Constructor_WithValidParameters_SetsPropertiesCorrectly()
        {
            // Act
            var progress = new ImprovementPlanObjectiveProgress(
                _progressId,
                _objectiveId,
                _reviewId,
                _howIsSchoolProgressing,
                _progressDetails);

            // Assert
            Assert.Equal(_progressId, progress.Id);
            Assert.Equal(_objectiveId, progress.ImprovementPlanObjectiveId);
            Assert.Equal(_reviewId, progress.ImprovementPlanReviewId);
            Assert.Equal(_howIsSchoolProgressing, progress.HowIsSchoolProgressing);
            Assert.Equal(_progressDetails, progress.ProgressDetails);
        }

        [Theory]
        [InlineData("On track", "Making excellent progress")]
        [InlineData("Behind", "Some challenges encountered")]
        [InlineData("At risk", "Significant issues need addressing")]
        [InlineData("Complete", "All objectives have been met")]
        [InlineData("Not started", "Objective not yet begun")]
        public void Constructor_WithDifferentProgressStatuses_SetsPropertiesCorrectly(string progressStatus, string details)
        {
            // Act
            var progress = new ImprovementPlanObjectiveProgress(
                _progressId,
                _objectiveId,
                _reviewId,
                progressStatus,
                details);

            // Assert
            Assert.Equal(progressStatus, progress.HowIsSchoolProgressing);
            Assert.Equal(details, progress.ProgressDetails);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("", "Some details")]
        [InlineData("Status", "")]
        [InlineData(null, null)]
        public void Constructor_WithEmptyOrNullValues_SetsPropertiesCorrectly(string progressStatus, string details)
        {
            // Act
            var progress = new ImprovementPlanObjectiveProgress(
                _progressId,
                _objectiveId,
                _reviewId,
                progressStatus,
                details);

            // Assert
            Assert.Equal(progressStatus, progress.HowIsSchoolProgressing);
            Assert.Equal(details, progress.ProgressDetails);
        }

        #endregion

        #region SetProgress Tests

        [Fact]
        public void SetProgress_WithValidParameters_UpdatesProperties()
        {
            // Arrange
            var progress = CreateProgress();
            var newStatus = "Updated status";
            var newDetails = "Updated progress details";

            // Act
            progress.SetProgress(newStatus, newDetails);

            // Assert
            Assert.Equal(newStatus, progress.HowIsSchoolProgressing);
            Assert.Equal(newDetails, progress.ProgressDetails);
        }

        [Theory]
        [InlineData("On track", "Good progress")]
        [InlineData("Behind", "Needs improvement")]
        [InlineData("At risk", "Requires immediate attention")]
        [InlineData("Complete", "Successfully completed")]
        [InlineData("Excellent", "Exceeding expectations")]
        public void SetProgress_WithDifferentStatuses_UpdatesProgressStatus(string newStatus, string newDetails)
        {
            // Arrange
            var progress = CreateProgress();

            // Act
            progress.SetProgress(newStatus, newDetails);

            // Assert
            Assert.Equal(newStatus, progress.HowIsSchoolProgressing);
            Assert.Equal(newDetails, progress.ProgressDetails);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("", "Some details")]
        [InlineData("Status", "")]
        [InlineData(null, null)]
        public void SetProgress_WithEmptyOrNullValues_UpdatesProperties(string newStatus, string newDetails)
        {
            // Arrange
            var progress = CreateProgress();

            // Act
            progress.SetProgress(newStatus, newDetails);

            // Assert
            Assert.Equal(newStatus, progress.HowIsSchoolProgressing);
            Assert.Equal(newDetails, progress.ProgressDetails);
        }

        [Fact]
        public void SetProgress_WithVeryLongDetails_UpdatesProgressDetails()
        {
            // Arrange
            var progress = CreateProgress();
            var longDetails = new string('A', 2000); // Very long string
            var newStatus = "In progress";

            // Act
            progress.SetProgress(newStatus, longDetails);

            // Assert
            Assert.Equal(newStatus, progress.HowIsSchoolProgressing);
            Assert.Equal(longDetails, progress.ProgressDetails);
        }

        [Fact]
        public void SetProgress_CalledMultipleTimes_UpdatesEachTime()
        {
            // Arrange
            var progress = CreateProgress();

            // Act & Assert - First update
            progress.SetProgress("First update", "First details");
            Assert.Equal("First update", progress.HowIsSchoolProgressing);
            Assert.Equal("First details", progress.ProgressDetails);

            // Act & Assert - Second update
            progress.SetProgress("Second update", "Second details");
            Assert.Equal("Second update", progress.HowIsSchoolProgressing);
            Assert.Equal("Second details", progress.ProgressDetails);

            // Act & Assert - Third update
            progress.SetProgress("Final update", "Final details");
            Assert.Equal("Final update", progress.HowIsSchoolProgressing);
            Assert.Equal("Final details", progress.ProgressDetails);
        }

        [Fact]
        public void SetProgress_OverwritesPreviousValues_DoesNotRetainOldValues()
        {
            // Arrange
            var progress = CreateProgress();
            var originalStatus = progress.HowIsSchoolProgressing;
            var originalDetails = progress.ProgressDetails;

            // Act
            progress.SetProgress("New status", "New details");

            // Assert
            Assert.NotEqual(originalStatus, progress.HowIsSchoolProgressing);
            Assert.NotEqual(originalDetails, progress.ProgressDetails);
            Assert.Equal("New status", progress.HowIsSchoolProgressing);
            Assert.Equal("New details", progress.ProgressDetails);
        }

        #endregion

        #region Property Tests

        [Fact]
        public void Id_SetInConstructor_RemainsUnchanged()
        {
            // Arrange & Act
            var progress = CreateProgress();

            // Assert
            Assert.Equal(_progressId, progress.Id);
            // Verify Id is read-only by checking it doesn't have a public setter
            var idProperty = typeof(ImprovementPlanObjectiveProgress).GetProperty(nameof(progress.Id));
            Assert.True(idProperty?.SetMethod?.IsPrivate);
        }

        [Fact]
        public void ImprovementPlanObjectiveId_SetInConstructor_RemainsUnchanged()
        {
            // Arrange & Act
            var progress = CreateProgress();

            // Assert
            Assert.Equal(_objectiveId, progress.ImprovementPlanObjectiveId);
            // Verify property is read-only
            var property = typeof(ImprovementPlanObjectiveProgress).GetProperty(nameof(progress.ImprovementPlanObjectiveId));
            Assert.True(property?.SetMethod?.IsPrivate);
        }

        [Fact]
        public void ImprovementPlanReviewId_SetInConstructor_RemainsUnchanged()
        {
            // Arrange & Act
            var progress = CreateProgress();

            // Assert
            Assert.Equal(_reviewId, progress.ImprovementPlanReviewId);
            // Verify property is read-only
            var property = typeof(ImprovementPlanObjectiveProgress).GetProperty(nameof(progress.ImprovementPlanReviewId));
            Assert.True(property?.SetMethod?.IsPrivate);
        }

        [Fact]
        public void HowIsSchoolProgressing_InitiallySetInConstructor_CanBeUpdatedViaSetProgress()
        {
            // Arrange
            var progress = CreateProgress();
            var initialStatus = progress.HowIsSchoolProgressing;

            // Act
            progress.SetProgress("Updated status", "Updated details");

            // Assert
            Assert.Equal(_howIsSchoolProgressing, initialStatus);
            Assert.Equal("Updated status", progress.HowIsSchoolProgressing);
        }

        [Fact]
        public void ProgressDetails_InitiallySetInConstructor_CanBeUpdatedViaSetProgress()
        {
            // Arrange
            var progress = CreateProgress();
            var initialDetails = progress.ProgressDetails;

            // Act
            progress.SetProgress("Updated status", "Updated details");

            // Assert
            Assert.Equal(_progressDetails, initialDetails);
            Assert.Equal("Updated details", progress.ProgressDetails);
        }

        [Fact]
        public void AuditableProperties_AreInitializedCorrectly()
        {
            // Arrange & Act
            var progress = CreateProgress();

            // Assert
            Assert.Equal(default(DateTime), progress.CreatedOn);
            Assert.Equal(string.Empty, progress.CreatedBy);
            Assert.Null(progress.LastModifiedOn);
            Assert.Null(progress.LastModifiedBy);
        }

        [Fact]
        public void AuditableProperties_CanBeSet()
        {
            // Arrange
            var progress = CreateProgress();
            var createdDate = DateTime.UtcNow;
            var modifiedDate = DateTime.UtcNow.AddHours(1);

            // Act
            progress.CreatedOn = createdDate;
            progress.CreatedBy = "Test User";
            progress.LastModifiedOn = modifiedDate;
            progress.LastModifiedBy = "Modifier User";

            // Assert
            Assert.Equal(createdDate, progress.CreatedOn);
            Assert.Equal("Test User", progress.CreatedBy);
            Assert.Equal(modifiedDate, progress.LastModifiedOn);
            Assert.Equal("Modifier User", progress.LastModifiedBy);
        }

        #endregion

        #region Integration Tests

        [Fact]
        public void ImprovementPlanObjectiveProgress_ImplementsIEntity()
        {
            // Arrange & Act
            var progress = CreateProgress();

            // Assert
            Assert.IsAssignableFrom<IEntity<ImprovementPlanObjectiveProgressId>>(progress);
        }

        [Fact]
        public void ImprovementPlanObjectiveProgress_WithDifferentIds_AreNotEqual()
        {
            // Arrange
            var progress1 = CreateProgress();
            var progress2 = new ImprovementPlanObjectiveProgress(
                new ImprovementPlanObjectiveProgressId(Guid.NewGuid()),
                _objectiveId,
                _reviewId,
                _howIsSchoolProgressing,
                _progressDetails);

            // Act & Assert
            Assert.NotEqual(progress1.Id, progress2.Id);
        }

        [Fact]
        public void ImprovementPlanObjectiveProgress_WithSameConstructorParameters_HaveSamePropertyValues()
        {
            // Arrange & Act
            var progress1 = CreateProgress();
            var progress2 = new ImprovementPlanObjectiveProgress(
                _progressId,
                _objectiveId,
                _reviewId,
                _howIsSchoolProgressing,
                _progressDetails);

            // Assert
            Assert.Equal(progress1.Id, progress2.Id);
            Assert.Equal(progress1.ImprovementPlanObjectiveId, progress2.ImprovementPlanObjectiveId);
            Assert.Equal(progress1.ImprovementPlanReviewId, progress2.ImprovementPlanReviewId);
            Assert.Equal(progress1.HowIsSchoolProgressing, progress2.HowIsSchoolProgressing);
            Assert.Equal(progress1.ProgressDetails, progress2.ProgressDetails);
        }

        #endregion

        #region Edge Case Tests

        [Fact]
        public void Constructor_WithSpecialCharacters_SetsPropertiesCorrectly()
        {
            // Arrange
            var statusWithSpecialChars = "Status with special chars: !@#$%^&*()";
            var detailsWithSpecialChars = "Details with special chars: <>&\"'";

            // Act
            var progress = new ImprovementPlanObjectiveProgress(
                _progressId,
                _objectiveId,
                _reviewId,
                statusWithSpecialChars,
                detailsWithSpecialChars);

            // Assert
            Assert.Equal(statusWithSpecialChars, progress.HowIsSchoolProgressing);
            Assert.Equal(detailsWithSpecialChars, progress.ProgressDetails);
        }

        [Fact]
        public void SetProgress_WithSpecialCharacters_UpdatesPropertiesCorrectly()
        {
            // Arrange
            var progress = CreateProgress();
            var statusWithSpecialChars = "Updated status with chars: !@#$%";
            var detailsWithSpecialChars = "Updated details with chars: <>\"'&";

            // Act
            progress.SetProgress(statusWithSpecialChars, detailsWithSpecialChars);

            // Assert
            Assert.Equal(statusWithSpecialChars, progress.HowIsSchoolProgressing);
            Assert.Equal(detailsWithSpecialChars, progress.ProgressDetails);
        }

        #endregion

        #region Helper Methods

        private ImprovementPlanObjectiveProgress CreateProgress()
        {
            return new ImprovementPlanObjectiveProgress(
                _progressId,
                _objectiveId,
                _reviewId,
                _howIsSchoolProgressing,
                _progressDetails);
        }

        #endregion
    }
}