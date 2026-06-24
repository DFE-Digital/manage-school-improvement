using Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;

namespace Dfe.ManageSchoolImprovement.Domain.Tests.Entities.SupportProject;

public class ProgressReviewTests
{
    private readonly ProgressReviewId _reviewId;
    private readonly SupportProjectId _supportProjectId;
    private readonly DateTime _reviewDate;
    private readonly string _reviewer;
    private readonly string _title;
    private readonly int _order;

    public ProgressReviewTests()
    {
        _reviewId = new ProgressReviewId(Guid.NewGuid());
        _supportProjectId = new SupportProjectId(1);
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
        var review = CreateReview();

        // Assert
        Assert.Equal(_reviewId, review.Id);
        Assert.Equal(_supportProjectId, review.SupportProjectId);
        Assert.Equal(_reviewDate, review.ReviewDate);
        Assert.Equal(_reviewer, review.Reviewer);
        Assert.Equal(_title, review.Title);
        Assert.Equal(_order, review.Order);
        Assert.Null(review.NextReviewDate);
        Assert.Null(review.NextSteps);
        Assert.Null(review.AdditionalDetails);
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
        review.SetNextReviewDate(DateTime.UtcNow.AddDays(30));

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
    public void SetDetails_WithValidParameters_UpdatesNextStepsAndAdditionalDetails()
    {
        // Arrange
        var review = CreateReview();
        var nextSteps = "Complete the next phase of improvement";
        var additionalDetails = "Focus on mathematics outcomes";

        // Act
        review.SetDetails(nextSteps, additionalDetails);

        // Assert
        Assert.Equal(nextSteps, review.NextSteps);
        Assert.Equal(additionalDetails, review.AdditionalDetails);
    }

    [Fact]
    public void SetDetails_WithNullAdditionalDetails_SetsAdditionalDetailsToNull()
    {
        // Arrange
        var review = CreateReview();
        var nextSteps = "Complete the next phase of improvement";

        // Act
        review.SetDetails(nextSteps, null);

        // Assert
        Assert.Equal(nextSteps, review.NextSteps);
        Assert.Null(review.AdditionalDetails);
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("", "Some details")]
    [InlineData("Next steps", "")]
    public void SetDetails_WithEmptyStrings_UpdatesProperties(string nextSteps, string additionalDetails)
    {
        // Arrange
        var review = CreateReview();

        // Act
        review.SetDetails(nextSteps, additionalDetails);

        // Assert
        Assert.Equal(nextSteps, review.NextSteps);
        Assert.Equal(additionalDetails, review.AdditionalDetails);
    }

    [Fact]
    public void SetDetails_CalledMultipleTimes_UpdatesEachTime()
    {
        // Arrange
        var review = CreateReview();

        // Act & Assert - First update
        review.SetDetails("First steps", "First details");
        Assert.Equal("First steps", review.NextSteps);
        Assert.Equal("First details", review.AdditionalDetails);

        // Act & Assert - Second update
        review.SetDetails("Second steps", "Second details");
        Assert.Equal("Second steps", review.NextSteps);
        Assert.Equal("Second details", review.AdditionalDetails);
    }

    [Fact]
    public void SetDetails_DoesNotAffectOtherProperties()
    {
        // Arrange
        var review = CreateReview();
        var originalReviewer = review.Reviewer;
        var originalReviewDate = review.ReviewDate;
        var originalTitle = review.Title;
        var originalOrder = review.Order;

        // Act
        review.SetDetails("Next steps", "Additional details");

        // Assert
        Assert.Equal(originalReviewer, review.Reviewer);
        Assert.Equal(originalReviewDate, review.ReviewDate);
        Assert.Equal(originalTitle, review.Title);
        Assert.Equal(originalOrder, review.Order);
    }

    #endregion

    #region DeleteProgress Tests

    [Fact]
    public void DeleteProgress_WithExistingDetails_ClearsNextStepsAndAdditionalDetails()
    {
        // Arrange
        var review = CreateReview();
        review.SetDetails("Complete the next phase of improvement", "Focus on mathematics outcomes");

        // Act
        review.DeleteProgress();

        // Assert
        Assert.Null(review.NextSteps);
        Assert.Null(review.AdditionalDetails);
    }

    [Fact]
    public void DeleteProgress_WithNullAdditionalDetails_ClearsNextSteps()
    {
        // Arrange
        var review = CreateReview();
        review.SetDetails("Complete the next phase of improvement", null);

        // Act
        review.DeleteProgress();

        // Assert
        Assert.Null(review.NextSteps);
        Assert.Null(review.AdditionalDetails);
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("", "Some details")]
    [InlineData("Next steps", "")]
    public void DeleteProgress_WithEmptyStrings_ClearsProperties(string nextSteps, string additionalDetails)
    {
        // Arrange
        var review = CreateReview();
        review.SetDetails(nextSteps, additionalDetails);

        // Act
        review.DeleteProgress();

        // Assert
        Assert.Null(review.NextSteps);
        Assert.Null(review.AdditionalDetails);
    }

    [Fact]
    public void DeleteProgress_WhenAlreadyNull_DoesNotThrow()
    {
        // Arrange
        var review = CreateReview();

        // Act
        var exception = Record.Exception(() => review.DeleteProgress());

        // Assert
        Assert.Null(exception);
        Assert.Null(review.NextSteps);
        Assert.Null(review.AdditionalDetails);
    }

    [Fact]
    public void DeleteProgress_CalledMultipleTimes_RemainsCleared()
    {
        // Arrange
        var review = CreateReview();
        review.SetDetails("First steps", "First details");

        // Act
        review.DeleteProgress();
        review.DeleteProgress();

        // Assert
        Assert.Null(review.NextSteps);
        Assert.Null(review.AdditionalDetails);
    }

    [Fact]
    public void DeleteProgress_DoesNotAffectOtherProperties()
    {
        // Arrange
        var review = CreateReview();
        review.SetDetails("Next steps", "Additional details");
        review.SetNextReviewDate(DateTime.UtcNow.AddDays(30));

        var originalReviewer = review.Reviewer;
        var originalReviewDate = review.ReviewDate;
        var originalTitle = review.Title;
        var originalOrder = review.Order;
        var originalNextReviewDate = review.NextReviewDate;

        // Act
        review.DeleteProgress();

        // Assert
        Assert.Equal(originalReviewer, review.Reviewer);
        Assert.Equal(originalReviewDate, review.ReviewDate);
        Assert.Equal(originalTitle, review.Title);
        Assert.Equal(originalOrder, review.Order);
        Assert.Equal(originalNextReviewDate, review.NextReviewDate);
    }

    #endregion

    #region Property Tests

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

    [Fact]
    public void ReviewDate_CanBeModified()
    {
        // Arrange
        var review = CreateReview();
        var newReviewDate = DateTime.UtcNow.AddDays(7);

        // Act
        review.ReviewDate = newReviewDate;

        // Assert
        Assert.Equal(newReviewDate, review.ReviewDate);
    }

    [Fact]
    public void Reviewer_CanBeModified()
    {
        // Arrange
        var review = CreateReview();
        var newReviewer = "Updated Reviewer";

        // Act
        review.Reviewer = newReviewer;

        // Assert
        Assert.Equal(newReviewer, review.Reviewer);
    }

    #endregion

    #region Helper Methods

    private ProgressReview CreateReview()
    {
        return new ProgressReview(
            _reviewId,
            _supportProjectId,
            _reviewDate,
            _reviewer,
            _order,
            _title);
    }

    #endregion
}
