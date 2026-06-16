using Dfe.ManageSchoolImprovement.Application.SupportProject.Models;
using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.Models
{
    public class ProgressReviewViewModelTests
    {
        [Fact]
        public void Create_ShouldReturnProgressReviewViewModel_WithCorrectProperties()
        {
            // Arrange
            var id = Guid.NewGuid();
            var reviewDate = DateTime.Now.AddDays(-7);
            var nextReviewDate = DateTime.Now.AddDays(30);

            var reviewDto = new ProgressReviewDto(
                id: id,
                readableId: 42,
                supportProjectId: 1,
                reviewDate: reviewDate,
                nextReviewDate: nextReviewDate,
                nextSteps: "Complete training programme",
                additionalDetails: "School is making steady progress",
                reviewer: "Jane Smith",
                order: 2,
                title: "Second review"
            );

            // Act
            var viewModel = ProgressReviewViewModel.Create(reviewDto);

            // Assert
            Assert.Equal(id, viewModel.Id);
            Assert.Equal(42, viewModel.ReadableId);
            Assert.Equal(1, viewModel.SupportProjectId);
            Assert.Equal(reviewDate, viewModel.ReviewDate);
            Assert.Equal("Jane Smith", viewModel.Reviewer);
            Assert.Equal(nextReviewDate, viewModel.NextReviewDate);
            Assert.Equal("Complete training programme", viewModel.NextSteps);
            Assert.Equal("School is making steady progress", viewModel.AdditionalDetails);
            Assert.Equal(ProgressReviewViewModel.ProgressStatusRecorded, viewModel.ProgressStatus);
            Assert.Equal("govuk-tag--green", viewModel.ProgressStatusClass);
            Assert.Equal(2, viewModel.Order);
            Assert.Equal("Second review", viewModel.Title);
        }

        [Fact]
        public void Create_WithNullNextSteps_ShouldSetProgressStatusNotRecorded()
        {
            // Arrange
            var reviewDto = new ProgressReviewDto(
                id: Guid.NewGuid(),
                readableId: 1,
                supportProjectId: 1,
                reviewDate: DateTime.Now,
                nextReviewDate: null,
                nextSteps: null,
                additionalDetails: null,
                reviewer: "Test Reviewer",
                order: 1,
                title: "First review"
            );

            // Act
            var viewModel = ProgressReviewViewModel.Create(reviewDto);

            // Assert
            Assert.Equal(ProgressReviewViewModel.ProgressStatusNotRecorded, viewModel.ProgressStatus);
            Assert.Equal("govuk-tag--blue", viewModel.ProgressStatusClass);
            Assert.Null(viewModel.NextSteps);
            Assert.Null(viewModel.AdditionalDetails);
        }

        [Fact]
        public void Create_WithNextSteps_ShouldSetProgressStatusRecorded()
        {
            // Arrange
            var reviewDto = new ProgressReviewDto(
                id: Guid.NewGuid(),
                readableId: 1,
                supportProjectId: 1,
                reviewDate: DateTime.Now,
                nextReviewDate: null,
                nextSteps: "Follow up with headteacher",
                additionalDetails: "Progress on track",
                reviewer: "Test Reviewer",
                order: 1,
                title: "First review"
            );

            // Act
            var viewModel = ProgressReviewViewModel.Create(reviewDto);

            // Assert
            Assert.Equal(ProgressReviewViewModel.ProgressStatusRecorded, viewModel.ProgressStatus);
            Assert.Equal("govuk-tag--green", viewModel.ProgressStatusClass);
            Assert.Equal("Follow up with headteacher", viewModel.NextSteps);
            Assert.Equal("Progress on track", viewModel.AdditionalDetails);
        }

        [Fact]
        public void Create_WithEmptyNextSteps_ShouldSetProgressStatusRecorded()
        {
            // Arrange
            var reviewDto = new ProgressReviewDto(
                id: Guid.NewGuid(),
                readableId: 1,
                supportProjectId: 1,
                reviewDate: DateTime.Now,
                nextReviewDate: null,
                nextSteps: "",
                additionalDetails: null,
                reviewer: "Test Reviewer",
                order: 1,
                title: "First review"
            );

            // Act
            var viewModel = ProgressReviewViewModel.Create(reviewDto);

            // Assert
            Assert.Equal(ProgressReviewViewModel.ProgressStatusRecorded, viewModel.ProgressStatus);
            Assert.Equal("govuk-tag--green", viewModel.ProgressStatusClass);
            Assert.Equal("", viewModel.NextSteps);
        }

        [Fact]
        public void Create_WithNullNextReviewDate_ShouldSetNullNextReviewDate()
        {
            // Arrange
            var reviewDto = new ProgressReviewDto(
                id: Guid.NewGuid(),
                readableId: 1,
                supportProjectId: 1,
                reviewDate: DateTime.Now,
                nextReviewDate: null,
                nextSteps: null,
                additionalDetails: null,
                reviewer: "Test Reviewer",
                order: 1,
                title: "First review"
            );

            // Act
            var viewModel = ProgressReviewViewModel.Create(reviewDto);

            // Assert
            Assert.Null(viewModel.NextReviewDate);
        }

        [Fact]
        public void Create_WithEmptyReviewer_ShouldSetEmptyReviewer()
        {
            // Arrange
            var reviewDto = new ProgressReviewDto(
                id: Guid.NewGuid(),
                readableId: 1,
                supportProjectId: 1,
                reviewDate: DateTime.Now,
                nextReviewDate: null,
                nextSteps: null,
                additionalDetails: null,
                reviewer: "",
                order: 1,
                title: "First review"
            );

            // Act
            var viewModel = ProgressReviewViewModel.Create(reviewDto);

            // Assert
            Assert.Equal("", viewModel.Reviewer);
        }

        [Theory]
        [InlineData("First review", 1)]
        [InlineData("Second review", 2)]
        [InlineData("Third review", 3)]
        [InlineData("Final review", 10)]
        public void Create_WithDifferentTitlesAndOrders_ShouldSetCorrectTitleAndOrder(string title, int order)
        {
            // Arrange
            var reviewDto = new ProgressReviewDto(
                id: Guid.NewGuid(),
                readableId: 1,
                supportProjectId: 1,
                reviewDate: DateTime.Now,
                nextReviewDate: null,
                nextSteps: null,
                additionalDetails: null,
                reviewer: "Test Reviewer",
                order: order,
                title: title
            );

            // Act
            var viewModel = ProgressReviewViewModel.Create(reviewDto);

            // Assert
            Assert.Equal(title, viewModel.Title);
            Assert.Equal(order, viewModel.Order);
        }

        [Fact]
        public void Create_WithPastAndFutureReviewDates_ShouldSetCorrectDates()
        {
            // Arrange
            var pastDate = DateTime.Now.AddDays(-30);
            var futureDate = DateTime.Now.AddDays(30);

            var reviewDto = new ProgressReviewDto(
                id: Guid.NewGuid(),
                readableId: 1,
                supportProjectId: 1,
                reviewDate: pastDate,
                nextReviewDate: futureDate,
                nextSteps: null,
                additionalDetails: null,
                reviewer: "Test Reviewer",
                order: 1,
                title: "Past review"
            );

            // Act
            var viewModel = ProgressReviewViewModel.Create(reviewDto);

            // Assert
            Assert.Equal(pastDate, viewModel.ReviewDate);
            Assert.Equal(futureDate, viewModel.NextReviewDate);
        }

        [Fact]
        public void ProgressStatusConstants_ShouldHaveCorrectValues()
        {
            // Assert
            Assert.Equal("Progress not recorded", ProgressReviewViewModel.ProgressStatusNotRecorded);
            Assert.Equal("Progress recorded", ProgressReviewViewModel.ProgressStatusRecorded);
        }

        [Fact]
        public void Constructor_ShouldInitializePropertiesWithDefaultValues()
        {
            // Act
            var viewModel = new ProgressReviewViewModel();

            // Assert
            Assert.Equal(Guid.Empty, viewModel.Id);
            Assert.Equal(0, viewModel.ReadableId);
            Assert.Equal(0, viewModel.SupportProjectId);
            Assert.Equal(default(DateTime), viewModel.ReviewDate);
            Assert.Equal(string.Empty, viewModel.Reviewer);
            Assert.Null(viewModel.NextReviewDate);
            Assert.Null(viewModel.NextSteps);
            Assert.Null(viewModel.AdditionalDetails);
            Assert.Equal(string.Empty, viewModel.ProgressStatusClass);
            Assert.Equal(string.Empty, viewModel.ProgressStatus);
            Assert.Equal(0, viewModel.Order);
            Assert.Null(viewModel.Title);
        }

        [Theory]
        [InlineData("Schedule follow-up meeting", "Detailed notes about school progress")]
        [InlineData("Review governance arrangements", "Some concerns remain")]
        [InlineData("Complete action plan", "")]
        [InlineData("", "No next steps recorded yet")]
        public void Create_WithDifferentNextStepsAndAdditionalDetails_ShouldSetCorrectValues(string nextSteps, string additionalDetails)
        {
            // Arrange
            var reviewDto = new ProgressReviewDto(
                id: Guid.NewGuid(),
                readableId: 1,
                supportProjectId: 1,
                reviewDate: DateTime.Now,
                nextReviewDate: null,
                nextSteps: nextSteps,
                additionalDetails: additionalDetails,
                reviewer: "Test Reviewer",
                order: 1,
                title: "Test review"
            );

            // Act
            var viewModel = ProgressReviewViewModel.Create(reviewDto);

            // Assert
            Assert.Equal(nextSteps, viewModel.NextSteps);
            Assert.Equal(additionalDetails, viewModel.AdditionalDetails);
        }
    }
}
