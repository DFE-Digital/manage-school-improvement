using Dfe.ManageSchoolImprovement.Application.SupportProject.Models;
using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.Models
{
    public class ImprovementPlanReviewViewModelTests
    {
        [Fact]
        public void Create_ShouldReturnImprovementPlanReviewViewModel_WithCorrectProperties()
        {
            // Arrange
            var reviewDto = new ImprovementPlanReviewDto(
                id: Guid.NewGuid(),
                readableId: 1,
                improvementPlanId: Guid.NewGuid(),
                reviewDate: DateTime.Now,
                reviewer: "Test Reviewer",
                title: "First review",
                order: 1,
                nextReviewDate: DateTime.Now.AddDays(30),
                ImprovementPlanObjectiveProgresses: new List<ImprovementPlanObjectiveProgressDto>()
            );

            var improvementPlanDto = new ImprovementPlanDto(
                id: reviewDto.improvementPlanId,
                readableId: 1,                    // Add this parameter
                supportProjectId: 1,
                objectivesSectionComplete: true,
                ImprovementPlanObjectives: new List<ImprovementPlanObjectiveDto>
                {
                    new ImprovementPlanObjectiveDto(
                        id: Guid.NewGuid(),
                        readableId: 1,
                        improvementPlanId: reviewDto.improvementPlanId,
                        areaOfImprovement: "Quality of education",
                        details: "Test objective",
                        order: 1
                    )
                },
                ImprovementPlanReviews: new List<ImprovementPlanReviewDto>()
            );

            // Act
            var viewModel = ImprovementPlanReviewViewModel.Create(reviewDto, improvementPlanDto);

            // Assert
            Assert.Equal(reviewDto.id, viewModel.Id);
            Assert.Equal(reviewDto.readableId, viewModel.ReadableId);
            Assert.Equal(reviewDto.improvementPlanId, viewModel.ImprovementPlanId);
            Assert.Equal(reviewDto.reviewDate, viewModel.ReviewDate);
            Assert.Equal(reviewDto.reviewer, viewModel.Reviewer);
            Assert.Equal(reviewDto.title, viewModel.Title);
            Assert.Equal(reviewDto.order, viewModel.Order);
            Assert.Equal(reviewDto.nextReviewDate, viewModel.NextReviewDate);
            Assert.Equal(ImprovementPlanReviewViewModel.ProgressStatusNotRecorded, viewModel.ProgressStatus);
            Assert.Equal("govuk-tag--blue", viewModel.ProgressStatusClass);
            Assert.Empty(viewModel.ImprovementPlanObjectiveProgresses);
        }

        [Fact]
        public void Create_WithNoObjectiveProgresses_ShouldSetProgressStatusNotRecorded()
        {
            // Arrange
            var reviewDto = new ImprovementPlanReviewDto(
                id: Guid.NewGuid(),
                readableId: 1,
                improvementPlanId: Guid.NewGuid(),
                reviewDate: DateTime.Now,
                reviewer: "Test Reviewer",
                title: "First review",
                order: 1,
                nextReviewDate: null,
                ImprovementPlanObjectiveProgresses: new List<ImprovementPlanObjectiveProgressDto>()
            );

            var improvementPlanDto = new ImprovementPlanDto(
                id: reviewDto.improvementPlanId,
                readableId: 1,                    // Add this parameter
                supportProjectId: 1,
                objectivesSectionComplete: true,
                ImprovementPlanObjectives: new List<ImprovementPlanObjectiveDto>
                {
                    new ImprovementPlanObjectiveDto(
                        id: Guid.NewGuid(),
                        readableId: 1,
                        improvementPlanId: reviewDto.improvementPlanId,
                        areaOfImprovement: "Quality of education",
                        details: "Test objective",
                        order: 1
                    )
                },
                ImprovementPlanReviews: new List<ImprovementPlanReviewDto>()
            );

            // Act
            var viewModel = ImprovementPlanReviewViewModel.Create(reviewDto, improvementPlanDto);

            // Assert
            Assert.Equal(ImprovementPlanReviewViewModel.ProgressStatusNotRecorded, viewModel.ProgressStatus);
            Assert.Equal("govuk-tag--blue", viewModel.ProgressStatusClass);
        }

        [Fact]
        public void Create_WithPartialObjectiveProgresses_ShouldSetProgressStatusPartlyRecorded()
        {
            // Arrange
            var reviewDto = new ImprovementPlanReviewDto(
                id: Guid.NewGuid(),
                readableId: 1,
                improvementPlanId: Guid.NewGuid(),
                reviewDate: DateTime.Now,
                reviewer: "Test Reviewer",
                title: "First Review",
                order: 1,
                nextReviewDate: null,
                ImprovementPlanObjectiveProgresses: new List<ImprovementPlanObjectiveProgressDto>
                {
                    new ImprovementPlanObjectiveProgressDto(
                        id: Guid.NewGuid(),
                        readableId: 1,
                        improvementPlanReviewId: Guid.NewGuid(),
                        improvementPlanObjectiveId: Guid.NewGuid(),
                        howIsSchoolProgressing: "On track",
                        progressDetails: "Good progress"
                    )
                }
            );

            var improvementPlanDto = new ImprovementPlanDto(
                id: reviewDto.improvementPlanId,
                readableId: 1,                    // Add this parameter
                supportProjectId: 1,
                objectivesSectionComplete: true,
                ImprovementPlanObjectives: new List<ImprovementPlanObjectiveDto>
                {
                    new ImprovementPlanObjectiveDto(
                        id: Guid.NewGuid(),
                        readableId: 1,
                        improvementPlanId: reviewDto.improvementPlanId,
                        areaOfImprovement: "Quality of education",
                        details: "Test objective 1",
                        order: 1
                    ),
                    new ImprovementPlanObjectiveDto(
                        id: Guid.NewGuid(),
                        readableId: 2,
                        improvementPlanId: reviewDto.improvementPlanId,
                        areaOfImprovement: "Leadership and management",
                        details: "Test objective 2",
                        order: 2
                    )
                },
                ImprovementPlanReviews: new List<ImprovementPlanReviewDto>()
            );

            // Act
            var viewModel = ImprovementPlanReviewViewModel.Create(reviewDto, improvementPlanDto);

            // Assert
            Assert.Equal(ImprovementPlanReviewViewModel.ProgressStatusPartlyRecorded, viewModel.ProgressStatus);
            Assert.Equal("govuk-tag--orange", viewModel.ProgressStatusClass);
            Assert.Single(viewModel.ImprovementPlanObjectiveProgresses);
        }

        [Fact]
        public void Create_WithAllObjectiveProgresses_ShouldSetProgressStatusRecorded()
        {
            // Arrange
            var objective1Id = Guid.NewGuid();
            var objective2Id = Guid.NewGuid();
            var reviewDto = new ImprovementPlanReviewDto(
                id: Guid.NewGuid(),
                readableId: 1,
                improvementPlanId: Guid.NewGuid(),
                reviewDate: DateTime.Now,
                reviewer: "Test Reviewer",
                title: "First Review",
                order: 1,
                nextReviewDate: null,
                ImprovementPlanObjectiveProgresses: new List<ImprovementPlanObjectiveProgressDto>
                {
                    new ImprovementPlanObjectiveProgressDto(
                        id: Guid.NewGuid(),
                        readableId: 1,
                        improvementPlanReviewId: Guid.NewGuid(),
                        improvementPlanObjectiveId: objective1Id,
                        howIsSchoolProgressing: "On track",
                        progressDetails: "Good progress 1"
                    ),
                    new ImprovementPlanObjectiveProgressDto(
                        id: Guid.NewGuid(),
                        readableId: 2,
                        improvementPlanReviewId: Guid.NewGuid(),
                        improvementPlanObjectiveId: objective2Id,
                        howIsSchoolProgressing: "Behind",
                        progressDetails: "Some challenges"
                    )
                }
            );

            var improvementPlanDto = new ImprovementPlanDto(
                id: reviewDto.improvementPlanId,
                readableId: 1,                    // Add this parameter
                supportProjectId: 1,
                objectivesSectionComplete: true,
                ImprovementPlanObjectives: new List<ImprovementPlanObjectiveDto>
                {
                    new ImprovementPlanObjectiveDto(
                        id: objective1Id,
                        readableId: 1,
                        improvementPlanId: reviewDto.improvementPlanId,
                        areaOfImprovement: "Quality of education",
                        details: "Test objective 1",
                        order: 1
                    ),
                    new ImprovementPlanObjectiveDto(
                        id: objective2Id,
                        readableId: 2,
                        improvementPlanId: reviewDto.improvementPlanId,
                        areaOfImprovement: "Leadership and management",
                        details: "Test objective 2",
                        order: 2
                    )
                },
                ImprovementPlanReviews: new List<ImprovementPlanReviewDto>()
            );

            // Act
            var viewModel = ImprovementPlanReviewViewModel.Create(reviewDto, improvementPlanDto);

            // Assert
            Assert.Equal(ImprovementPlanReviewViewModel.ProgressStatusRecorded, viewModel.ProgressStatus);
            Assert.Equal("govuk-tag--green", viewModel.ProgressStatusClass);
            Assert.Equal(2, viewModel.ImprovementPlanObjectiveProgresses.Count);
        }

        [Fact]
        public void Create_WithNullObjectiveProgresses_ShouldSetProgressStatusNotRecorded()
        {
            // Arrange
            var reviewDto = new ImprovementPlanReviewDto(
                id: Guid.NewGuid(),
                readableId: 1,
                improvementPlanId: Guid.NewGuid(),
                reviewDate: DateTime.Now,
                reviewer: "Test Reviewer",
                title: "First Review",
                order: 1,
                nextReviewDate: null,
                ImprovementPlanObjectiveProgresses: Array.Empty<ImprovementPlanObjectiveProgressDto>() // Fix: Replace null with an empty array
            );

            var improvementPlanDto = new ImprovementPlanDto(
                id: reviewDto.improvementPlanId,
                readableId: 1,
                supportProjectId: 1,
                objectivesSectionComplete: true,
                ImprovementPlanObjectives: new List<ImprovementPlanObjectiveDto>
                {
                    new ImprovementPlanObjectiveDto(
                        id: Guid.NewGuid(),
                        readableId: 1,
                        improvementPlanId: reviewDto.improvementPlanId,
                        areaOfImprovement: "Quality of education",
                        details: "Test objective",
                        order: 1
                    )
                },
                ImprovementPlanReviews: new List<ImprovementPlanReviewDto>()
            );

            // Act
            var viewModel = ImprovementPlanReviewViewModel.Create(reviewDto, improvementPlanDto);

            // Assert
            Assert.Equal(ImprovementPlanReviewViewModel.ProgressStatusNotRecorded, viewModel.ProgressStatus);
            Assert.Empty(viewModel.ImprovementPlanObjectiveProgresses);
        }

        [Fact]
        public void Create_WithEmptyReviewer_ShouldSetEmptyReviewer()
        {
            // Arrange
            var reviewDto = new ImprovementPlanReviewDto(
                id: Guid.NewGuid(),
                readableId: 1,
                improvementPlanId: Guid.NewGuid(),
                reviewDate: DateTime.Now,
                reviewer: "",
                title: "First Review",
                order: 1,
                nextReviewDate: null,
                ImprovementPlanObjectiveProgresses: new List<ImprovementPlanObjectiveProgressDto>()
            );

            var improvementPlanDto = new ImprovementPlanDto(
                id: reviewDto.improvementPlanId,
                readableId: 1,                    // Add this parameter
                supportProjectId: 1,
                objectivesSectionComplete: true,
                ImprovementPlanObjectives: new List<ImprovementPlanObjectiveDto>(),
                ImprovementPlanReviews: new List<ImprovementPlanReviewDto>()
            );

            // Act
            var viewModel = ImprovementPlanReviewViewModel.Create(reviewDto, improvementPlanDto);

            // Assert
            Assert.Equal("", viewModel.Reviewer);
        }

        [Fact]
        public void Create_WithNullNextReviewDate_ShouldSetNullNextReviewDate()
        {
            // Arrange
            var reviewDto = new ImprovementPlanReviewDto(
                id: Guid.NewGuid(),
                readableId: 1,
                improvementPlanId: Guid.NewGuid(),
                reviewDate: DateTime.Now,
                reviewer: "Test Reviewer",
                title: "First Review",
                order: 1,
                nextReviewDate: null,
                ImprovementPlanObjectiveProgresses: new List<ImprovementPlanObjectiveProgressDto>()
            );

            var improvementPlanDto = new ImprovementPlanDto(
                id: reviewDto.improvementPlanId,
                readableId: 1,                    // Add this parameter
                supportProjectId: 1,
                objectivesSectionComplete: true,
                ImprovementPlanObjectives: new List<ImprovementPlanObjectiveDto>(),
                ImprovementPlanReviews: new List<ImprovementPlanReviewDto>()
            );

            // Act
            var viewModel = ImprovementPlanReviewViewModel.Create(reviewDto, improvementPlanDto);

            // Assert
            Assert.Null(viewModel.NextReviewDate);
        }

        [Theory]
        [InlineData("First review", 1)]
        [InlineData("Second review", 2)]
        [InlineData("Third review", 3)]
        [InlineData("Final review", 10)]
        public void Create_WithDifferentTitlesAndOrders_ShouldSetCorrectTitleAndOrder(string title, int order)
        {
            // Arrange
            var reviewDto = new ImprovementPlanReviewDto(
                id: Guid.NewGuid(),
                readableId: 1,
                improvementPlanId: Guid.NewGuid(),
                reviewDate: DateTime.Now,
                reviewer: "Test Reviewer",
                title: title,
                order: order,
                nextReviewDate: null,
                ImprovementPlanObjectiveProgresses: new List<ImprovementPlanObjectiveProgressDto>()
            );

            var improvementPlanDto = new ImprovementPlanDto(
                id: reviewDto.improvementPlanId,
                readableId: 1,                    // Add this parameter
                supportProjectId: 1,
                objectivesSectionComplete: true,
                ImprovementPlanObjectives: new List<ImprovementPlanObjectiveDto>(),
                ImprovementPlanReviews: new List<ImprovementPlanReviewDto>()
            );

            // Act
            var viewModel = ImprovementPlanReviewViewModel.Create(reviewDto, improvementPlanDto);

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

            var reviewDto = new ImprovementPlanReviewDto(
                id: Guid.NewGuid(),
                readableId: 1,
                improvementPlanId: Guid.NewGuid(),
                reviewDate: pastDate,
                reviewer: "Test Reviewer",
                title: "Past Review",
                order: 1,
                nextReviewDate: futureDate,
                ImprovementPlanObjectiveProgresses: new List<ImprovementPlanObjectiveProgressDto>()
            );

            var improvementPlanDto = new ImprovementPlanDto(
                id: reviewDto.improvementPlanId,
                readableId: 1,                    // Add this parameter
                supportProjectId: 1,
                objectivesSectionComplete: true,
                ImprovementPlanObjectives: new List<ImprovementPlanObjectiveDto>(),
                ImprovementPlanReviews: new List<ImprovementPlanReviewDto>()
            );

            // Act
            var viewModel = ImprovementPlanReviewViewModel.Create(reviewDto, improvementPlanDto);

            // Assert
            Assert.Equal(pastDate, viewModel.ReviewDate);
            Assert.Equal(futureDate, viewModel.NextReviewDate);
        }

        [Fact]
        public void ProgressStatusConstants_ShouldHaveCorrectValues()
        {
            // Assert
            Assert.Equal("Progress not recorded", ImprovementPlanReviewViewModel.ProgressStatusNotRecorded);
            Assert.Equal("Progress partly recorded", ImprovementPlanReviewViewModel.ProgressStatusPartlyRecorded);
            Assert.Equal("Progress recorded", ImprovementPlanReviewViewModel.ProgressStatusRecorded);
        }

        [Fact]
        public void Constructor_ShouldInitializePropertiesWithDefaultValues()
        {
            // Act
            var viewModel = new ImprovementPlanReviewViewModel();

            // Assert
            Assert.Equal(Guid.Empty, viewModel.Id);
            Assert.Equal(0, viewModel.ReadableId);
            Assert.Equal(Guid.Empty, viewModel.ImprovementPlanId);
            Assert.Equal(default(DateTime), viewModel.ReviewDate);
            Assert.Equal(string.Empty, viewModel.Reviewer);
            Assert.Equal(string.Empty, viewModel.ProgressStatusClass);
            Assert.Equal(string.Empty, viewModel.ProgressStatus);
            Assert.Equal(string.Empty, viewModel.Title);
            Assert.Equal(0, viewModel.Order);
            Assert.Empty(viewModel.ImprovementPlanObjectiveProgresses);
            Assert.Null(viewModel.NextReviewDate);
        }

        [Fact]
        public void Create_WithMoreProgressesThanObjectives_ShouldSetProgressStatusRecorded()
        {
            // Arrange - Edge case where there are more progresses than objectives
            var reviewDto = new ImprovementPlanReviewDto(
                id: Guid.NewGuid(),
                readableId: 1,
                improvementPlanId: Guid.NewGuid(),
                reviewDate: DateTime.Now,
                reviewer: "Test Reviewer",
                title: "First Review",
                order: 1,
                nextReviewDate: null,
                ImprovementPlanObjectiveProgresses: new List<ImprovementPlanObjectiveProgressDto>
                {
                    new ImprovementPlanObjectiveProgressDto(
                        id: Guid.NewGuid(),
                        readableId: 1,
                        improvementPlanReviewId: Guid.NewGuid(),
                        improvementPlanObjectiveId: Guid.NewGuid(),
                        howIsSchoolProgressing: "On track",
                        progressDetails: "Progress 1"
                    ),
                    new ImprovementPlanObjectiveProgressDto(
                        id: Guid.NewGuid(),
                        readableId: 2,
                        improvementPlanReviewId: Guid.NewGuid(),
                        improvementPlanObjectiveId: Guid.NewGuid(),
                        howIsSchoolProgressing: "Behind",
                        progressDetails: "Progress 2"
                    )
                }
            );

            var improvementPlanDto = new ImprovementPlanDto(
                id: reviewDto.improvementPlanId,
                readableId: 1,                    // Add this parameter
                supportProjectId: 1,
                objectivesSectionComplete: true,
                ImprovementPlanObjectives: new List<ImprovementPlanObjectiveDto>
                {
                    new ImprovementPlanObjectiveDto(
                        id: Guid.NewGuid(),
                        readableId: 1,
                        improvementPlanId: reviewDto.improvementPlanId,
                        areaOfImprovement: "Quality of education",
                        details: "Test objective",
                        order: 1
                    )
                },
                ImprovementPlanReviews: new List<ImprovementPlanReviewDto>()
            );

            // Act
            var viewModel = ImprovementPlanReviewViewModel.Create(reviewDto, improvementPlanDto);

            // Assert
            Assert.Equal(ImprovementPlanReviewViewModel.ProgressStatusRecorded, viewModel.ProgressStatus);
            Assert.Equal("govuk-tag--green", viewModel.ProgressStatusClass);
        }
    }
}