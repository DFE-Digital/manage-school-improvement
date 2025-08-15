using Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;

namespace Dfe.ManageSchoolImprovement.Domain.Tests.Entities.SupportProject;

public class ImprovementPlanReviewTests
{
    private readonly ImprovementPlanReviewId _reviewId;
    private readonly ImprovementPlanId _improvementPlanId;
    private readonly DateTime _reviewDate;
    private readonly string _reviewer;
    private readonly string _title;
    private readonly int _order;

    public ImprovementPlanReviewTests()
    {
        _reviewId = new ImprovementPlanReviewId(Guid.NewGuid());
        _improvementPlanId = new ImprovementPlanId(Guid.NewGuid());
        _reviewDate = DateTime.UtcNow.Date;
        _reviewer = "Test Reviewer";
        _title = "First Review";
        _order = 1;
    }

    #region Constructor Tests

    [Fact]
    public void Constructor_WithValidParameters_SetsPropertiesCorrectly()
    {
        // Act
        var review = new ImprovementPlanReview(
            _reviewId,
            _improvementPlanId,
            _reviewDate,
            _reviewer,
            _title,
            _order);

        // Assert
        Assert.Equal(_reviewId, review.Id);
        Assert.Equal(_improvementPlanId, review.ImprovementPlanId);
        Assert.Equal(_reviewDate, review.ReviewDate);
        Assert.Equal(_reviewer, review.Reviewer);
        Assert.Equal(_title, review.Title);
        Assert.Equal(_order, review.Order);
        Assert.Empty(review.ImprovementPlanObjectiveProgresses);
        Assert.Null(review.NextReviewDate);
    }

    #endregion

    #region AddObjectiveProgress Tests

    [Fact]
    public void AddObjectiveProgress_WithValidParameters_AddsProgressToCollection()
    {
        // Arrange
        var review = CreateReview();
        var progressId = new ImprovementPlanObjectiveProgressId(Guid.NewGuid());
        var objectiveId = new ImprovementPlanObjectiveId(Guid.NewGuid());
        var progressStatus = "On track";
        var progressDetails = "Good progress made";

        // Act
        review.AddObjectiveProgress(progressId, objectiveId, _reviewId, progressStatus, progressDetails);

        // Assert
        Assert.Single(review.ImprovementPlanObjectiveProgresses);
        var addedProgress = review.ImprovementPlanObjectiveProgresses.First();
        Assert.Equal(progressId, addedProgress.Id);
        Assert.Equal(objectiveId, addedProgress.ImprovementPlanObjectiveId);
        Assert.Equal(_reviewId, addedProgress.ImprovementPlanReviewId);
        Assert.Equal(progressStatus, addedProgress.HowIsSchoolProgressing);
        Assert.Equal(progressDetails, addedProgress.ProgressDetails);
    }

    [Fact]
    public void AddObjectiveProgress_WithMultipleProgresses_AddsAllToCollection()
    {
        // Arrange
        var review = CreateReview();
        var progressId1 = new ImprovementPlanObjectiveProgressId(Guid.NewGuid());
        var progressId2 = new ImprovementPlanObjectiveProgressId(Guid.NewGuid());
        var objectiveId1 = new ImprovementPlanObjectiveId(Guid.NewGuid());
        var objectiveId2 = new ImprovementPlanObjectiveId(Guid.NewGuid());

        // Act
        review.AddObjectiveProgress(progressId1, objectiveId1, _reviewId, "On track", "Progress 1");
        review.AddObjectiveProgress(progressId2, objectiveId2, _reviewId, "Behind", "Progress 2");

        // Assert
        Assert.Equal(2, review.ImprovementPlanObjectiveProgresses.Count());
    }

    [Theory]
    [InlineData("", "Details")]
    [InlineData("Status", "")]
    [InlineData("", "")]
    public void AddObjectiveProgress_WithEmptyStrings_StillAddsProgress(string progressStatus, string progressDetails)
    {
        // Arrange
        var review = CreateReview();
        var progressId = new ImprovementPlanObjectiveProgressId(Guid.NewGuid());
        var objectiveId = new ImprovementPlanObjectiveId(Guid.NewGuid());

        // Act
        review.AddObjectiveProgress(progressId, objectiveId, _reviewId, progressStatus, progressDetails);

        // Assert
        Assert.Single(review.ImprovementPlanObjectiveProgresses);
        var addedProgress = review.ImprovementPlanObjectiveProgresses.First();
        Assert.Equal(progressStatus, addedProgress.HowIsSchoolProgressing);
        Assert.Equal(progressDetails, addedProgress.ProgressDetails);
    }

    #endregion

    #region SetImprovementPlanObjectiveProgressDetails Tests

    [Fact]
    public void SetImprovementPlanObjectiveProgressDetails_WithExistingProgress_UpdatesDetails()
    {
        // Arrange
        var review = CreateReview();
        var progressId = new ImprovementPlanObjectiveProgressId(Guid.NewGuid());
        var objectiveId = new ImprovementPlanObjectiveId(Guid.NewGuid());
        
        review.AddObjectiveProgress(progressId, objectiveId, _reviewId, "Initial status", "Initial details");
        
        var newStatus = "Updated status";
        var newDetails = "Updated details";

        // Act
        review.SetImprovementPlanObjectiveProgressDetails(progressId, newStatus, newDetails);

        // Assert
        var updatedProgress = review.ImprovementPlanObjectiveProgresses.First();
        Assert.Equal(newStatus, updatedProgress.HowIsSchoolProgressing);
        Assert.Equal(newDetails, updatedProgress.ProgressDetails);
    }

    [Fact]
    public void SetImprovementPlanObjectiveProgressDetails_WithNonExistentProgressId_ThrowsKeyNotFoundException()
    {
        // Arrange
        var review = CreateReview();
        var nonExistentProgressId = new ImprovementPlanObjectiveProgressId(Guid.NewGuid());

        // Act & Assert
        var exception = Assert.Throws<KeyNotFoundException>(() =>
            review.SetImprovementPlanObjectiveProgressDetails(nonExistentProgressId, "Status", "Details"));
        
        Assert.Contains($"Improvement plan review objective progress with id {nonExistentProgressId} not found", exception.Message);
    }

    [Fact]
    public void SetImprovementPlanObjectiveProgressDetails_WithMultipleProgresses_UpdatesCorrectOne()
    {
        // Arrange
        var review = CreateReview();
        var progressId1 = new ImprovementPlanObjectiveProgressId(Guid.NewGuid());
        var progressId2 = new ImprovementPlanObjectiveProgressId(Guid.NewGuid());
        var objectiveId1 = new ImprovementPlanObjectiveId(Guid.NewGuid());
        var objectiveId2 = new ImprovementPlanObjectiveId(Guid.NewGuid());

        review.AddObjectiveProgress(progressId1, objectiveId1, _reviewId, "Status 1", "Details 1");
        review.AddObjectiveProgress(progressId2, objectiveId2, _reviewId, "Status 2", "Details 2");

        // Act
        review.SetImprovementPlanObjectiveProgressDetails(progressId1, "Updated Status 1", "Updated Details 1");

        // Assert
        var progress1 = review.ImprovementPlanObjectiveProgresses.First(p => p.Id == progressId1);
        var progress2 = review.ImprovementPlanObjectiveProgresses.First(p => p.Id == progressId2);

        Assert.Equal("Updated Status 1", progress1.HowIsSchoolProgressing);
        Assert.Equal("Updated Details 1", progress1.ProgressDetails);
        Assert.Equal("Status 2", progress2.HowIsSchoolProgressing);
        Assert.Equal("Details 2", progress2.ProgressDetails);
    }

    [Theory]
    [InlineData("", "")]
    [InlineData(null, null)]
    public void SetImprovementPlanObjectiveProgressDetails_WithEmptyOrNullValues_UpdatesProgress(string status, string details)
    {
        // Arrange
        var review = CreateReview();
        var progressId = new ImprovementPlanObjectiveProgressId(Guid.NewGuid());
        var objectiveId = new ImprovementPlanObjectiveId(Guid.NewGuid());
        
        review.AddObjectiveProgress(progressId, objectiveId, _reviewId, "Initial status", "Initial details");

        // Act
        review.SetImprovementPlanObjectiveProgressDetails(progressId, status, details);

        // Assert
        var updatedProgress = review.ImprovementPlanObjectiveProgresses.First();
        Assert.Equal(status, updatedProgress.HowIsSchoolProgressing);
        Assert.Equal(details, updatedProgress.ProgressDetails);
    }

    #endregion

    #region SetNextReviewDate Tests

    [Fact]
    public void SetNextReviewDate_WithValidDate_SetsNextReviewDate()
    {
        // Arrange
        var review = CreateReview();
        var nextReviewDate = DateTime.UtcNow.AddDays(30);

        // Act
        review.SetNextReviewDate(nextReviewDate);

        // Assert
        Assert.Equal(nextReviewDate, review.NextReviewDate);
    }

    [Fact]
    public void SetNextReviewDate_WithNull_SetsNextReviewDateToNull()
    {
        // Arrange
        var review = CreateReview();
        review.SetNextReviewDate(DateTime.UtcNow.AddDays(30)); // Set initially

        // Act
        review.SetNextReviewDate(null);

        // Assert
        Assert.Null(review.NextReviewDate);
    }

    [Fact]
    public void SetNextReviewDate_WithPastDate_SetsNextReviewDate()
    {
        // Arrange
        var review = CreateReview();
        var pastDate = DateTime.UtcNow.AddDays(-30);

        // Act
        review.SetNextReviewDate(pastDate);

        // Assert
        Assert.Equal(pastDate, review.NextReviewDate);
    }

    #endregion

    #region SetDetails Tests

    [Fact]
    public void SetDetails_WithValidParameters_UpdatesReviewerAndDate()
    {
        // Arrange
        var review = CreateReview();
        var newReviewer = "Updated Reviewer";
        var newReviewDate = DateTime.UtcNow.AddDays(1);

        // Act
        review.SetDetails(newReviewer, newReviewDate);

        // Assert
        Assert.Equal(newReviewer, review.Reviewer);
        Assert.Equal(newReviewDate, review.ReviewDate);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void SetDetails_WithEmptyOrNullReviewer_UpdatesReviewer(string reviewer)
    {
        // Arrange
        var review = CreateReview();
        var newReviewDate = DateTime.UtcNow.AddDays(1);

        // Act
        review.SetDetails(reviewer, newReviewDate);

        // Assert
        Assert.Equal(reviewer, review.Reviewer);
        Assert.Equal(newReviewDate, review.ReviewDate);
    }

    [Fact]
    public void SetDetails_WithPastDate_UpdatesReviewDate()
    {
        // Arrange
        var review = CreateReview();
        var newReviewer = "Past Date Reviewer";
        var pastDate = DateTime.UtcNow.AddDays(-10);

        // Act
        review.SetDetails(newReviewer, pastDate);

        // Assert
        Assert.Equal(newReviewer, review.Reviewer);
        Assert.Equal(pastDate, review.ReviewDate);
    }

    #endregion

    #region Property Tests

    [Fact]
    public void ImprovementPlanObjectiveProgresses_ReturnsReadOnlyCollection()
    {
        // Arrange
        var review = CreateReview();
        var progressId = new ImprovementPlanObjectiveProgressId(Guid.NewGuid());
        var objectiveId = new ImprovementPlanObjectiveId(Guid.NewGuid());
        
        review.AddObjectiveProgress(progressId, objectiveId, _reviewId, "Status", "Details");

        // Act
        var progresses = review.ImprovementPlanObjectiveProgresses;

        // Assert
        Assert.IsAssignableFrom<IEnumerable<ImprovementPlanObjectiveProgress>>(progresses);
        Assert.Single(progresses);
        
        // Verify it's read-only by checking we can't cast to List
        Assert.False(progresses is List<ImprovementPlanObjectiveProgress>);
    }

    [Fact]
    public void Title_CanBeModified()
    {
        // Arrange
        var review = CreateReview();
        var newTitle = "Updated Title";

        // Act
        review.Title = newTitle;

        // Assert
        Assert.Equal(newTitle, review.Title);
    }

    [Fact]
    public void Order_CanBeModified()
    {
        // Arrange
        var review = CreateReview();
        var newOrder = 5;

        // Act
        review.Order = newOrder;

        // Assert
        Assert.Equal(newOrder, review.Order);
    }

    #endregion

    #region Helper Methods

    private ImprovementPlanReview CreateReview()
    {
        return new ImprovementPlanReview(
            _reviewId,
            _improvementPlanId,
            _reviewDate,
            _reviewer,
            _title,
            _order);
    }

    private ImprovementPlanReview CreateReviewWithProgress()
    {
        var review = CreateReview();
        var progressId = new ImprovementPlanObjectiveProgressId(Guid.NewGuid());
        var objectiveId = new ImprovementPlanObjectiveId(Guid.NewGuid());
        
        review.AddObjectiveProgress(progressId, objectiveId, _reviewId, "Test Status", "Test Details");
        
        return review;
    }

    #endregion
}