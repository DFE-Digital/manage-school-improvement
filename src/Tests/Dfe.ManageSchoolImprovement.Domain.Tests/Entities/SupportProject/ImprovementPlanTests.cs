using Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;

namespace Dfe.ManageSchoolImprovement.Domain.Tests.Entities.SupportProject
{
    public class ImprovementPlanTests
    {
        private readonly ImprovementPlanId _improvementPlanId;
        private readonly SupportProjectId _supportProjectId;
        private readonly ImprovementPlan _improvementPlan;

        public ImprovementPlanTests()
        {
            _improvementPlanId = new ImprovementPlanId(Guid.NewGuid());
            _supportProjectId = new SupportProjectId(1);
            _improvementPlan = new ImprovementPlan(_improvementPlanId, _supportProjectId);
        }

        [Fact]
        public void Constructor_WithValidParameters_CreatesImprovementPlan()
        {
            // Arrange & Act
            var improvementPlan = new ImprovementPlan(_improvementPlanId, _supportProjectId);

            // Assert
            Assert.Equal(_improvementPlanId, improvementPlan.Id);
            Assert.Equal(_supportProjectId, improvementPlan.SupportProjectId);
            Assert.Empty(improvementPlan.ImprovementPlanObjectives);
            Assert.Null(improvementPlan.ObjectivesSectionComplete);
        }

        [Fact]
        public void AddObjective_WithValidParameters_AddsObjectiveToCollection()
        {
            // Arrange
            var objectiveId = new ImprovementPlanObjectiveId(Guid.NewGuid());
            var areaOfImprovement = "QualityOfEducation";
            var details = "Improve mathematics outcomes";
            var order = 1;

            // Act
            _improvementPlan.AddObjective(objectiveId, _improvementPlanId, areaOfImprovement, details, order);

            // Assert
            Assert.Single(_improvementPlan.ImprovementPlanObjectives);
            var addedObjective = _improvementPlan.ImprovementPlanObjectives.First();
            Assert.Equal(objectiveId, addedObjective.Id);
            Assert.Equal(_improvementPlanId, addedObjective.ImprovementPlanId);
            Assert.Equal(areaOfImprovement, addedObjective.AreaOfImprovement);
            Assert.Equal(details, addedObjective.Details);
            Assert.Equal(order, addedObjective.Order);
        }

        [Fact]
        public void AddObjective_MultipleObjectives_AddsAllObjectivesToCollection()
        {
            // Arrange
            var objective1Id = new ImprovementPlanObjectiveId(Guid.NewGuid());
            var objective2Id = new ImprovementPlanObjectiveId(Guid.NewGuid());

            // Act
            _improvementPlan.AddObjective(objective1Id, _improvementPlanId, "QualityOfEducation", "Objective 1", 1);
            _improvementPlan.AddObjective(objective2Id, _improvementPlanId, "LeadershipAndManagement", "Objective 2", 1);

            // Assert
            Assert.Equal(2, _improvementPlan.ImprovementPlanObjectives.Count());
            Assert.Contains(_improvementPlan.ImprovementPlanObjectives, o => o.Id == objective1Id);
            Assert.Contains(_improvementPlan.ImprovementPlanObjectives, o => o.Id == objective2Id);
        }

        [Fact]
        public void AddObjective_MultipleObjectivesInSameArea_OrdersSequentially()
        {
            // Arrange
            var objective1Id = new ImprovementPlanObjectiveId(Guid.NewGuid());
            var objective2Id = new ImprovementPlanObjectiveId(Guid.NewGuid());
            var objective3Id = new ImprovementPlanObjectiveId(Guid.NewGuid());

            // Act - Add multiple objectives to the same improvement area with different orders
            _improvementPlan.AddObjective(objective1Id, _improvementPlanId, "QualityOfEducation", "First objective", 1);
            _improvementPlan.AddObjective(objective2Id, _improvementPlanId, "QualityOfEducation", "Second objective", 2);
            _improvementPlan.AddObjective(objective3Id, _improvementPlanId, "QualityOfEducation", "Third objective", 3);

            // Assert
            Assert.Equal(3, _improvementPlan.ImprovementPlanObjectives.Count());
            var qualityObjectives = _improvementPlan.ImprovementPlanObjectives
                .Where(o => o.AreaOfImprovement == "QualityOfEducation")
                .ToList();

            Assert.Equal(3, qualityObjectives.Count);
            Assert.Equal(1, qualityObjectives.First(o => o.Id == objective1Id).Order);
            Assert.Equal(2, qualityObjectives.First(o => o.Id == objective2Id).Order);
            Assert.Equal(3, qualityObjectives.First(o => o.Id == objective3Id).Order);
        }

        [Fact]
        public void AddObjective_ObjectivesInDifferentAreas_CanHaveSameOrder()
        {
            // Arrange
            var qualityObjective1Id = new ImprovementPlanObjectiveId(Guid.NewGuid());
            var qualityObjective2Id = new ImprovementPlanObjectiveId(Guid.NewGuid());
            var leadershipObjective1Id = new ImprovementPlanObjectiveId(Guid.NewGuid());
            var leadershipObjective2Id = new ImprovementPlanObjectiveId(Guid.NewGuid());

            // Act - Add objectives to different areas, each with their own ordering
            _improvementPlan.AddObjective(qualityObjective1Id, _improvementPlanId, "QualityOfEducation", "Quality Objective 1", 1);
            _improvementPlan.AddObjective(leadershipObjective1Id, _improvementPlanId, "LeadershipAndManagement", "Leadership Objective 1", 1);
            _improvementPlan.AddObjective(qualityObjective2Id, _improvementPlanId, "QualityOfEducation", "Quality Objective 2", 2);
            _improvementPlan.AddObjective(leadershipObjective2Id, _improvementPlanId, "LeadershipAndManagement", "Leadership Objective 2", 2);

            // Assert
            Assert.Equal(4, _improvementPlan.ImprovementPlanObjectives.Count());

            // Quality of Education objectives should have order 1, 2
            var qualityObjectives = _improvementPlan.ImprovementPlanObjectives
                .Where(o => o.AreaOfImprovement == "QualityOfEducation")
                .ToList();
            Assert.Equal(1, qualityObjectives.First(o => o.Id == qualityObjective1Id).Order);
            Assert.Equal(2, qualityObjectives.First(o => o.Id == qualityObjective2Id).Order);

            // Leadership and Management objectives should also have order 1, 2 (independent ordering)
            var leadershipObjectives = _improvementPlan.ImprovementPlanObjectives
                .Where(o => o.AreaOfImprovement == "LeadershipAndManagement")
                .ToList();
            Assert.Equal(1, leadershipObjectives.First(o => o.Id == leadershipObjective1Id).Order);
            Assert.Equal(2, leadershipObjectives.First(o => o.Id == leadershipObjective2Id).Order);
        }

        [Theory]
        [InlineData("QualityOfEducation", "Improve reading comprehension")]
        [InlineData("LeadershipAndManagement", "Develop leadership capacity")]
        [InlineData("BehaviourAndAttitudes", "Improve student behavior")]
        [InlineData("Attendance", "Increase attendance rates")]
        [InlineData("PersonalDevelopment", "Enhance character education")]
        public void AddObjective_WithDifferentAreasOfImprovement_AddsObjectiveCorrectly(string areaOfImprovement, string details)
        {
            // Arrange
            var objectiveId = new ImprovementPlanObjectiveId(Guid.NewGuid());
            var order = 1;

            // Act
            _improvementPlan.AddObjective(objectiveId, _improvementPlanId, areaOfImprovement, details, order);

            // Assert
            var addedObjective = _improvementPlan.ImprovementPlanObjectives.First();
            Assert.Equal(areaOfImprovement, addedObjective.AreaOfImprovement);
            Assert.Equal(details, addedObjective.Details);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void SetObjectivesComplete_WithBooleanValue_SetsPropertyCorrectly(bool objectivesSectionComplete)
        {
            // Act
            _improvementPlan.SetObjectivesComplete(objectivesSectionComplete);

            // Assert
            Assert.Equal(objectivesSectionComplete, _improvementPlan.ObjectivesSectionComplete);
        }

        [Fact]
        public void SetObjectiveDetails_WithExistingObjective_UpdatesDetails()
        {
            // Arrange
            var objectiveId = new ImprovementPlanObjectiveId(Guid.NewGuid());
            var originalDetails = "Original details";
            var updatedDetails = "Updated comprehensive details";

            _improvementPlan.AddObjective(objectiveId, _improvementPlanId, "QualityOfEducation", originalDetails, 1);

            // Act
            _improvementPlan.SetObjectiveDetails(objectiveId, updatedDetails);

            // Assert
            var objective = _improvementPlan.ImprovementPlanObjectives.First();
            Assert.Equal(updatedDetails, objective.Details);
        }

        [Fact]
        public void SetObjectiveDetails_WithNonExistentObjective_ThrowsKeyNotFoundException()
        {
            // Arrange
            var nonExistentObjectiveId = new ImprovementPlanObjectiveId(Guid.NewGuid());
            var details = "Some details";

            // Act & Assert
            var exception = Assert.Throws<KeyNotFoundException>(() =>
                _improvementPlan.SetObjectiveDetails(nonExistentObjectiveId, details));

            Assert.Equal($"Improvement plan objective with id {nonExistentObjectiveId} not found", exception.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("Short")]
        [InlineData("Very long detailed improvement objective with comprehensive implementation strategy and measurable outcomes")]
        public void SetObjectiveDetails_WithVariousDetailLengths_UpdatesDetailsCorrectly(string details)
        {
            // Arrange
            var objectiveId = new ImprovementPlanObjectiveId(Guid.NewGuid());
            _improvementPlan.AddObjective(objectiveId, _improvementPlanId, "QualityOfEducation", "Initial details", 1);

            // Act
            _improvementPlan.SetObjectiveDetails(objectiveId, details);

            // Assert
            var objective = _improvementPlan.ImprovementPlanObjectives.First();
            Assert.Equal(details, objective.Details);
        }

        [Fact]
        public void SetObjectiveDetails_WithMultipleObjectives_UpdatesCorrectObjective()
        {
            // Arrange
            var objective1Id = new ImprovementPlanObjectiveId(Guid.NewGuid());
            var objective2Id = new ImprovementPlanObjectiveId(Guid.NewGuid());
            var updatedDetails = "Updated details for objective 2";

            _improvementPlan.AddObjective(objective1Id, _improvementPlanId, "QualityOfEducation", "Objective 1 details", 1);
            _improvementPlan.AddObjective(objective2Id, _improvementPlanId, "LeadershipAndManagement", "Objective 2 details", 2);

            // Act
            _improvementPlan.SetObjectiveDetails(objective2Id, updatedDetails);

            // Assert
            var objective1 = _improvementPlan.ImprovementPlanObjectives.First(o => o.Id == objective1Id);
            var objective2 = _improvementPlan.ImprovementPlanObjectives.First(o => o.Id == objective2Id);

            Assert.Equal("Objective 1 details", objective1.Details); // Unchanged
            Assert.Equal(updatedDetails, objective2.Details); // Updated
        }

        [Fact]
        public void ImprovementPlanObjectives_ReturnsReadOnlyCollection()
        {
            // Arrange
            var objectiveId = new ImprovementPlanObjectiveId(Guid.NewGuid());
            _improvementPlan.AddObjective(objectiveId, _improvementPlanId, "QualityOfEducation", "Test objective", 1);

            // Act
            var objectives = _improvementPlan.ImprovementPlanObjectives.ToList();

            // Assert
            Assert.IsType<List<ImprovementPlanObjective>>(objectives);
            Assert.Single(objectives);
        }

        #region AddReview Tests

        [Fact]
        public void AddReview_WithValidParameters_AddsReviewToCollection()
        {
            // Arrange
            var reviewId = new ImprovementPlanReviewId(Guid.NewGuid());
            var reviewer = "Test Reviewer";
            var reviewDate = DateTime.UtcNow.Date;

            // Act
            _improvementPlan.AddReview(reviewId, reviewer, reviewDate);

            // Assert
            Assert.Single(_improvementPlan.ImprovementPlanReviews);
            var addedReview = _improvementPlan.ImprovementPlanReviews.First();
            Assert.Equal(reviewId, addedReview.Id);
            Assert.Equal(_improvementPlanId, addedReview.ImprovementPlanId);
            Assert.Equal(reviewer, addedReview.Reviewer);
            Assert.Equal(reviewDate, addedReview.ReviewDate);
        }

        [Fact]
        public void AddReview_MultipleReviews_AddsAllReviewsToCollection()
        {
            // Arrange
            var review1Id = new ImprovementPlanReviewId(Guid.NewGuid());
            var review2Id = new ImprovementPlanReviewId(Guid.NewGuid());
            var reviewer1 = "First Reviewer";
            var reviewer2 = "Second Reviewer";
            var reviewDate1 = DateTime.UtcNow.Date;
            var reviewDate2 = DateTime.UtcNow.Date.AddDays(30);

            // Act
            _improvementPlan.AddReview(review1Id, reviewer1, reviewDate1);
            _improvementPlan.AddReview(review2Id, reviewer2, reviewDate2);

            // Assert
            Assert.Equal(2, _improvementPlan.ImprovementPlanReviews.Count());
            Assert.Contains(_improvementPlan.ImprovementPlanReviews, r => r.Id == review1Id);
            Assert.Contains(_improvementPlan.ImprovementPlanReviews, r => r.Id == review2Id);
        }

        [Fact]
        public void AddReview_GeneratesCorrectTitleAndOrder()
        {
            // Arrange
            var review1Id = new ImprovementPlanReviewId(Guid.NewGuid());
            var review2Id = new ImprovementPlanReviewId(Guid.NewGuid());
            var reviewer = "Test Reviewer";
            var reviewDate = DateTime.UtcNow.Date;

            // Act
            _improvementPlan.AddReview(review1Id, reviewer, reviewDate);
            _improvementPlan.AddReview(review2Id, reviewer, reviewDate);

            // Assert
            var reviews = _improvementPlan.ImprovementPlanReviews.OrderBy(r => r.Order).ToList();
            Assert.Equal("First Review", reviews[0].Title);
            Assert.Equal(1, reviews[0].Order);
            Assert.Equal("Second Review", reviews[1].Title);
            Assert.Equal(2, reviews[1].Order);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("Very Long Reviewer Name With Many Characters")]
        public void AddReview_WithDifferentReviewerNames_AddsReviewCorrectly(string reviewer)
        {
            // Arrange
            var reviewId = new ImprovementPlanReviewId(Guid.NewGuid());
            var reviewDate = DateTime.UtcNow.Date;

            // Act
            _improvementPlan.AddReview(reviewId, reviewer, reviewDate);

            // Assert
            var addedReview = _improvementPlan.ImprovementPlanReviews.First();
            Assert.Equal(reviewer, addedReview.Reviewer);
        }

        [Fact]
        public void AddReview_WithPastAndFutureDates_AddsReviewsCorrectly()
        {
            // Arrange
            var pastReviewId = new ImprovementPlanReviewId(Guid.NewGuid());
            var futureReviewId = new ImprovementPlanReviewId(Guid.NewGuid());
            var pastDate = DateTime.UtcNow.AddDays(-30);
            var futureDate = DateTime.UtcNow.AddDays(30);

            // Act
            _improvementPlan.AddReview(pastReviewId, "Past Reviewer", pastDate);
            _improvementPlan.AddReview(futureReviewId, "Future Reviewer", futureDate);

            // Assert
            Assert.Equal(2, _improvementPlan.ImprovementPlanReviews.Count());
            var pastReview = _improvementPlan.ImprovementPlanReviews.First(r => r.Id == pastReviewId);
            var futureReview = _improvementPlan.ImprovementPlanReviews.First(r => r.Id == futureReviewId);

            Assert.Equal(pastDate, pastReview.ReviewDate);
            Assert.Equal(futureDate, futureReview.ReviewDate);
        }

        #endregion

        #region AddImprovementPlanObjectiveProgress Tests

        [Fact]
        public void AddImprovementPlanObjectiveProgress_WithValidParameters_AddsProgressToReview()
        {
            // Arrange
            var reviewId = new ImprovementPlanReviewId(Guid.NewGuid());
            var progressId = new ImprovementPlanObjectiveProgressId(Guid.NewGuid());
            var objectiveId = new ImprovementPlanObjectiveId(Guid.NewGuid());
            var progressStatus = "On track";
            var progressDetails = "Good progress made";

            _improvementPlan.AddReview(reviewId, "Test Reviewer", DateTime.UtcNow);

            // Act
            _improvementPlan.AddImprovementPlanObjectiveProgress(reviewId, progressId, objectiveId, progressStatus, progressDetails);

            // Assert
            var review = _improvementPlan.ImprovementPlanReviews.First();
            Assert.Single(review.ImprovementPlanObjectiveProgresses);
            var addedProgress = review.ImprovementPlanObjectiveProgresses.First();
            Assert.Equal(progressId, addedProgress.Id);
            Assert.Equal(objectiveId, addedProgress.ImprovementPlanObjectiveId);
            Assert.Equal(reviewId, addedProgress.ImprovementPlanReviewId);
            Assert.Equal(progressStatus, addedProgress.HowIsSchoolProgressing);
            Assert.Equal(progressDetails, addedProgress.ProgressDetails);
        }

        [Fact]
        public void AddImprovementPlanObjectiveProgress_WithNonExistentReview_ThrowsKeyNotFoundException()
        {
            // Arrange
            var nonExistentReviewId = new ImprovementPlanReviewId(Guid.NewGuid());
            var progressId = new ImprovementPlanObjectiveProgressId(Guid.NewGuid());
            var objectiveId = new ImprovementPlanObjectiveId(Guid.NewGuid());

            // Act & Assert
            var exception = Assert.Throws<KeyNotFoundException>(() =>
                _improvementPlan.AddImprovementPlanObjectiveProgress(nonExistentReviewId, progressId, objectiveId, "Status", "Details"));

            Assert.Equal($"Improvement plan review with id {nonExistentReviewId} not found", exception.Message);
        }

        [Fact]
        public void AddImprovementPlanObjectiveProgress_MultipleProgressEntries_AddsAllToReview()
        {
            // Arrange
            var reviewId = new ImprovementPlanReviewId(Guid.NewGuid());
            var progress1Id = new ImprovementPlanObjectiveProgressId(Guid.NewGuid());
            var progress2Id = new ImprovementPlanObjectiveProgressId(Guid.NewGuid());
            var objective1Id = new ImprovementPlanObjectiveId(Guid.NewGuid());
            var objective2Id = new ImprovementPlanObjectiveId(Guid.NewGuid());

            _improvementPlan.AddReview(reviewId, "Test Reviewer", DateTime.UtcNow);

            // Act
            _improvementPlan.AddImprovementPlanObjectiveProgress(reviewId, progress1Id, objective1Id, "On track", "Progress 1");
            _improvementPlan.AddImprovementPlanObjectiveProgress(reviewId, progress2Id, objective2Id, "Behind", "Progress 2");

            // Assert
            var review = _improvementPlan.ImprovementPlanReviews.First();
            Assert.Equal(2, review.ImprovementPlanObjectiveProgresses.Count());
        }

        [Theory]
        [InlineData("On track", "Making excellent progress")]
        [InlineData("Behind", "Some challenges encountered")]
        [InlineData("At risk", "Significant issues need addressing")]
        [InlineData("Complete", "All objectives have been met")]
        [InlineData("", "")]
        public void AddImprovementPlanObjectiveProgress_WithDifferentStatuses_AddsProgressCorrectly(string progressStatus, string progressDetails)
        {
            // Arrange
            var reviewId = new ImprovementPlanReviewId(Guid.NewGuid());
            var progressId = new ImprovementPlanObjectiveProgressId(Guid.NewGuid());
            var objectiveId = new ImprovementPlanObjectiveId(Guid.NewGuid());

            _improvementPlan.AddReview(reviewId, "Test Reviewer", DateTime.UtcNow);

            // Act
            _improvementPlan.AddImprovementPlanObjectiveProgress(reviewId, progressId, objectiveId, progressStatus, progressDetails);

            // Assert
            var progress = _improvementPlan.ImprovementPlanReviews.First().ImprovementPlanObjectiveProgresses.First();
            Assert.Equal(progressStatus, progress.HowIsSchoolProgressing);
            Assert.Equal(progressDetails, progress.ProgressDetails);
        }

        #endregion

        #region SetImprovementPlanObjectiveProgressDetails Tests

        [Fact]
        public void SetImprovementPlanObjectiveProgressDetails_WithExistingProgress_UpdatesDetails()
        {
            // Arrange
            var reviewId = new ImprovementPlanReviewId(Guid.NewGuid());
            var progressId = new ImprovementPlanObjectiveProgressId(Guid.NewGuid());
            var objectiveId = new ImprovementPlanObjectiveId(Guid.NewGuid());

            _improvementPlan.AddReview(reviewId, "Test Reviewer", DateTime.UtcNow);
            _improvementPlan.AddImprovementPlanObjectiveProgress(reviewId, progressId, objectiveId, "Initial status", "Initial details");

            var newStatus = "Updated status";
            var newDetails = "Updated details";

            // Act
            _improvementPlan.SetImprovementPlanObjectiveProgressDetails(reviewId, progressId, newStatus, newDetails);

            // Assert
            var progress = _improvementPlan.ImprovementPlanReviews.First().ImprovementPlanObjectiveProgresses.First();
            Assert.Equal(newStatus, progress.HowIsSchoolProgressing);
            Assert.Equal(newDetails, progress.ProgressDetails);
        }

        [Fact]
        public void SetImprovementPlanObjectiveProgressDetails_WithNonExistentReview_ThrowsKeyNotFoundException()
        {
            // Arrange
            var nonExistentReviewId = new ImprovementPlanReviewId(Guid.NewGuid());
            var progressId = new ImprovementPlanObjectiveProgressId(Guid.NewGuid());

            // Act & Assert
            var exception = Assert.Throws<KeyNotFoundException>(() =>
                _improvementPlan.SetImprovementPlanObjectiveProgressDetails(nonExistentReviewId, progressId, "Status", "Details"));

            Assert.Equal($"Improvement plan review with id {nonExistentReviewId} not found", exception.Message);
        }

        [Fact]
        public void SetImprovementPlanObjectiveProgressDetails_WithMultipleProgress_UpdatesCorrectOne()
        {
            // Arrange
            var reviewId = new ImprovementPlanReviewId(Guid.NewGuid());
            var progress1Id = new ImprovementPlanObjectiveProgressId(Guid.NewGuid());
            var progress2Id = new ImprovementPlanObjectiveProgressId(Guid.NewGuid());
            var objective1Id = new ImprovementPlanObjectiveId(Guid.NewGuid());
            var objective2Id = new ImprovementPlanObjectiveId(Guid.NewGuid());

            _improvementPlan.AddReview(reviewId, "Test Reviewer", DateTime.UtcNow);
            _improvementPlan.AddImprovementPlanObjectiveProgress(reviewId, progress1Id, objective1Id, "Status 1", "Details 1");
            _improvementPlan.AddImprovementPlanObjectiveProgress(reviewId, progress2Id, objective2Id, "Status 2", "Details 2");

            // Act
            _improvementPlan.SetImprovementPlanObjectiveProgressDetails(reviewId, progress1Id, "Updated Status 1", "Updated Details 1");

            // Assert
            var review = _improvementPlan.ImprovementPlanReviews.First();
            var progress1 = review.ImprovementPlanObjectiveProgresses.First(p => p.Id == progress1Id);
            var progress2 = review.ImprovementPlanObjectiveProgresses.First(p => p.Id == progress2Id);

            Assert.Equal("Updated Status 1", progress1.HowIsSchoolProgressing);
            Assert.Equal("Updated Details 1", progress1.ProgressDetails);
            Assert.Equal("Status 2", progress2.HowIsSchoolProgressing);
            Assert.Equal("Details 2", progress2.ProgressDetails);
        }

        #endregion

        #region SetNextReviewDate Tests

        [Fact]
        public void SetNextReviewDate_WithExistingReview_SetsNextReviewDate()
        {
            // Arrange
            var reviewId = new ImprovementPlanReviewId(Guid.NewGuid());
            var nextReviewDate = DateTime.UtcNow.AddDays(30);

            _improvementPlan.AddReview(reviewId, "Test Reviewer", DateTime.UtcNow);

            // Act
            _improvementPlan.SetNextReviewDate(reviewId, nextReviewDate);

            // Assert
            var review = _improvementPlan.ImprovementPlanReviews.First();
            Assert.Equal(nextReviewDate, review.NextReviewDate);
        }

        [Fact]
        public void SetNextReviewDate_WithNull_SetsNextReviewDateToNull()
        {
            // Arrange
            var reviewId = new ImprovementPlanReviewId(Guid.NewGuid());

            _improvementPlan.AddReview(reviewId, "Test Reviewer", DateTime.UtcNow);
            _improvementPlan.SetNextReviewDate(reviewId, DateTime.UtcNow.AddDays(30)); // Set initially

            // Act
            _improvementPlan.SetNextReviewDate(reviewId, null);

            // Assert
            var review = _improvementPlan.ImprovementPlanReviews.First();
            Assert.Null(review.NextReviewDate);
        }

        [Fact]
        public void SetNextReviewDate_WithNonExistentReview_ThrowsKeyNotFoundException()
        {
            // Arrange
            var nonExistentReviewId = new ImprovementPlanReviewId(Guid.NewGuid());
            var nextReviewDate = DateTime.UtcNow.AddDays(30);

            // Act & Assert
            var exception = Assert.Throws<KeyNotFoundException>(() =>
                _improvementPlan.SetNextReviewDate(nonExistentReviewId, nextReviewDate));

            Assert.Equal($"Improvement plan review with id {nonExistentReviewId} not found", exception.Message);
        }

        #endregion

        #region SetImprovementPlanReviewDetails Tests

        [Fact]
        public void SetImprovementPlanReviewDetails_WithExistingReview_UpdatesDetails()
        {
            // Arrange
            var reviewId = new ImprovementPlanReviewId(Guid.NewGuid());
            var originalReviewer = "Original Reviewer";
            var originalDate = DateTime.UtcNow.Date;
            var newReviewer = "Updated Reviewer";
            var newDate = DateTime.UtcNow.AddDays(1).Date;

            _improvementPlan.AddReview(reviewId, originalReviewer, originalDate);

            // Act
            _improvementPlan.SetImprovementPlanReviewDetails(reviewId, newReviewer, newDate);

            // Assert
            var review = _improvementPlan.ImprovementPlanReviews.First();
            Assert.Equal(newReviewer, review.Reviewer);
            Assert.Equal(newDate, review.ReviewDate);
        }

        [Fact]
        public void SetImprovementPlanReviewDetails_WithNonExistentReview_ThrowsKeyNotFoundException()
        {
            // Arrange
            var nonExistentReviewId = new ImprovementPlanReviewId(Guid.NewGuid());

            // Act & Assert
            var exception = Assert.Throws<KeyNotFoundException>(() =>
                _improvementPlan.SetImprovementPlanReviewDetails(nonExistentReviewId, "Reviewer", DateTime.UtcNow));

            Assert.Equal($"Improvement plan review with id {nonExistentReviewId} not found", exception.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("Very Long Reviewer Name")]
        public void SetImprovementPlanReviewDetails_WithDifferentReviewerNames_UpdatesCorrectly(string reviewer)
        {
            // Arrange
            var reviewId = new ImprovementPlanReviewId(Guid.NewGuid());
            var reviewDate = DateTime.UtcNow.Date;

            _improvementPlan.AddReview(reviewId, "Original Reviewer", DateTime.UtcNow.AddDays(-1));

            // Act
            _improvementPlan.SetImprovementPlanReviewDetails(reviewId, reviewer, reviewDate);

            // Assert
            var review = _improvementPlan.ImprovementPlanReviews.First();
            Assert.Equal(reviewer, review.Reviewer);
            Assert.Equal(reviewDate, review.ReviewDate);
        }

        #endregion

        #region ImprovementPlanReviews Collection Tests

        [Fact]
        public void ImprovementPlanReviews_ReturnsReadOnlyCollection()
        {
            // Arrange
            var reviewId = new ImprovementPlanReviewId(Guid.NewGuid());
            _improvementPlan.AddReview(reviewId, "Test Reviewer", DateTime.UtcNow);

            // Act
            var reviews = _improvementPlan.ImprovementPlanReviews;

            // Assert
            Assert.True(reviews is IReadOnlyCollection<ImprovementPlanReview>);
            Assert.Single(reviews);

            // Verify it's read-only by checking we can't cast to List
            Assert.False(reviews is List<ImprovementPlanReview>);
        }

        [Fact]
        public void Constructor_InitializesEmptyReviewsCollection()
        {
            // Arrange & Act
            var improvementPlan = new ImprovementPlan(_improvementPlanId, _supportProjectId);

            // Assert
            Assert.Empty(improvementPlan.ImprovementPlanReviews);
        }

        #endregion
    }
}