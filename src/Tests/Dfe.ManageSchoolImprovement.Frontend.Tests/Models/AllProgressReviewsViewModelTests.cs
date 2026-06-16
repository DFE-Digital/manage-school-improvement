using Dfe.ManageSchoolImprovement.Application.SupportProject.Models;
using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.Models
{
    public class AllProgressReviewsViewModelTests
    {
        [Fact]
        public void Create_FromProgressReviewViewModel_ShouldMapAllProperties()
        {
            // Arrange
            var id = Guid.NewGuid();
            var reviewDate = DateTime.Now.AddDays(-7);
            var nextReviewDate = DateTime.Now.AddDays(30);

            var progressReview = ProgressReviewViewModel.Create(new ProgressReviewDto(
                id: id,
                readableId: 42,
                supportProjectId: 1,
                reviewDate: reviewDate,
                nextReviewDate: nextReviewDate,
                nextSteps: "Next steps",
                additionalDetails: "Additional details",
                reviewer: "Jane Smith",
                order: 2,
                title: "Second review"
            ));

            const string progressStatusClass = "govuk-tag--green";
            const string progressStatus = "Progress recorded";

            // Act
            var viewModel = AllProgressReviewsViewModel.Create(progressReview, progressStatusClass, progressStatus);

            // Assert
            Assert.Equal(id, viewModel.Id);
            Assert.Equal(42, viewModel.ReadableId);
            Assert.Equal(reviewDate, viewModel.ReviewDate);
            Assert.Equal("Jane Smith", viewModel.Reviewer);
            Assert.Equal(2, viewModel.Order);
            Assert.Equal("Second review", viewModel.Title);
            Assert.Equal(progressStatusClass, viewModel.ProgressStatusClass);
            Assert.Equal(progressStatus, viewModel.ProgressStatus);
            Assert.Equal(nextReviewDate, viewModel.NextReviewDate);
        }

        [Fact]
        public void Create_FromProgressReviewViewModel_WithNullNextReviewDate_ShouldSetNullNextReviewDate()
        {
            // Arrange
            var progressReview = ProgressReviewViewModel.Create(new ProgressReviewDto(
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
            ));

            // Act
            var viewModel = AllProgressReviewsViewModel.Create(progressReview, "govuk-tag--blue", "Progress not recorded");

            // Assert
            Assert.Null(viewModel.NextReviewDate);
        }

        [Fact]
        public void Create_FromImprovementPlanReviewViewModel_ShouldMapAllProperties()
        {
            // Arrange
            var id = Guid.NewGuid();
            var improvementPlanId = Guid.NewGuid();
            var reviewDate = DateTime.Now.AddDays(-14);
            var nextReviewDate = DateTime.Now.AddDays(60);

            var reviewDto = new ImprovementPlanReviewDto(
                id: id,
                readableId: 7,
                improvementPlanId: improvementPlanId,
                reviewDate: reviewDate,
                nextReviewDate: nextReviewDate,
                reviewer: "John Doe",
                title: "Third review",
                order: 3,
                howIsTheSchoolProgressingOverall: "On track",
                overallProgressDetails: "Good progress",
                ImprovementPlanObjectiveProgresses: new List<ImprovementPlanObjectiveProgressDto>()
            );

            var improvementPlanDto = new ImprovementPlanDto(
                id: improvementPlanId,
                readableId: 1,
                supportProjectId: 1,
                objectivesSectionComplete: true,
                ImprovementPlanObjectives: new List<ImprovementPlanObjectiveDto>(),
                ImprovementPlanReviews: new List<ImprovementPlanReviewDto>()
            );

            var improvementPlanReview = ImprovementPlanReviewViewModel.Create(reviewDto, improvementPlanDto);

            const string progressStatusClass = "govuk-tag--orange";
            const string progressStatus = "Progress partly recorded";

            // Act
            var viewModel = AllProgressReviewsViewModel.Create(improvementPlanReview, progressStatusClass, progressStatus);

            // Assert
            Assert.Equal(id, viewModel.Id);
            Assert.Equal(7, viewModel.ReadableId);
            Assert.Equal(reviewDate, viewModel.ReviewDate);
            Assert.Equal("John Doe", viewModel.Reviewer);
            Assert.Equal(3, viewModel.Order);
            Assert.Equal("Third review", viewModel.Title);
            Assert.Equal(progressStatusClass, viewModel.ProgressStatusClass);
            Assert.Equal(progressStatus, viewModel.ProgressStatus);
            Assert.Equal(nextReviewDate, viewModel.NextReviewDate);
        }

        [Fact]
        public void Create_FromImprovementPlanReviewViewModel_WithNullNextReviewDate_ShouldSetNullNextReviewDate()
        {
            // Arrange
            var improvementPlanId = Guid.NewGuid();

            var reviewDto = new ImprovementPlanReviewDto(
                id: Guid.NewGuid(),
                readableId: 1,
                improvementPlanId: improvementPlanId,
                reviewDate: DateTime.Now,
                nextReviewDate: null,
                reviewer: "Test Reviewer",
                title: "First review",
                order: 1,
                howIsTheSchoolProgressingOverall: "On track",
                overallProgressDetails: "Good progress",
                ImprovementPlanObjectiveProgresses: new List<ImprovementPlanObjectiveProgressDto>()
            );

            var improvementPlanDto = new ImprovementPlanDto(
                id: improvementPlanId,
                readableId: 1,
                supportProjectId: 1,
                objectivesSectionComplete: true,
                ImprovementPlanObjectives: new List<ImprovementPlanObjectiveDto>(),
                ImprovementPlanReviews: new List<ImprovementPlanReviewDto>()
            );

            var improvementPlanReview = ImprovementPlanReviewViewModel.Create(reviewDto, improvementPlanDto);

            // Act
            var viewModel = AllProgressReviewsViewModel.Create(improvementPlanReview, "govuk-tag--blue", "Progress not recorded");

            // Assert
            Assert.Null(viewModel.NextReviewDate);
        }

        [Theory]
        [InlineData("govuk-tag--blue", "Progress not recorded")]
        [InlineData("govuk-tag--orange", "Progress partly recorded")]
        [InlineData("govuk-tag--green", "Progress recorded")]
        public void Create_ShouldUseProvidedProgressStatusAndClass(string progressStatusClass, string progressStatus)
        {
            // Arrange
            var progressReview = ProgressReviewViewModel.Create(new ProgressReviewDto(
                id: Guid.NewGuid(),
                readableId: 1,
                supportProjectId: 1,
                reviewDate: DateTime.Now,
                nextReviewDate: null,
                nextSteps: null,
                additionalDetails: null,
                reviewer: "Test Reviewer",
                order: 1,
                title: "Review"
            ));

            // Act
            var viewModel = AllProgressReviewsViewModel.Create(progressReview, progressStatusClass, progressStatus);

            // Assert
            Assert.Equal(progressStatusClass, viewModel.ProgressStatusClass);
            Assert.Equal(progressStatus, viewModel.ProgressStatus);
        }

        [Fact]
        public void Constructor_ShouldInitializePropertiesWithDefaultValues()
        {
            // Act
            var viewModel = new AllProgressReviewsViewModel();

            // Assert
            Assert.Equal(Guid.Empty, viewModel.Id);
            Assert.Equal(0, viewModel.ReadableId);
            Assert.Equal(default(DateTime), viewModel.ReviewDate);
            Assert.Equal(string.Empty, viewModel.Reviewer);
            Assert.Equal(0, viewModel.Order);
            Assert.Equal(string.Empty, viewModel.Title);
            Assert.Equal(string.Empty, viewModel.ProgressStatusClass);
            Assert.Equal(string.Empty, viewModel.ProgressStatus);
            Assert.Null(viewModel.NextReviewDate);
        }
    }
}
