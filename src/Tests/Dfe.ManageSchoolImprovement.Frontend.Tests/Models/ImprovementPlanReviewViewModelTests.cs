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
                nextReviewDate: DateTime.Now.AddDays(30),
                reviewer: "Test Reviewer",
                title: "First review",
                order: 1,
                howIsTheSchoolProgressingOverall: "The school is making good progress",
                overallProgressDetails: "Significant improvements in teaching quality",
                ImprovementPlanObjectiveProgresses: new List<ImprovementPlanObjectiveProgressDto>()
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
            Assert.Equal(reviewDto.id, viewModel.Id);
            Assert.Equal(reviewDto.readableId, viewModel.ReadableId);
            Assert.Equal(reviewDto.improvementPlanId, viewModel.ImprovementPlanId);
            Assert.Equal(reviewDto.reviewDate, viewModel.ReviewDate);
            Assert.Equal(reviewDto.reviewer, viewModel.Reviewer);
            Assert.Equal(reviewDto.title, viewModel.Title);
            Assert.Equal(reviewDto.order, viewModel.Order);
            Assert.Equal(reviewDto.nextReviewDate, viewModel.NextReviewDate);
            Assert.Equal(reviewDto.howIsTheSchoolProgressingOverall, viewModel.HowIsTheSchoolProgressingOverall);
            Assert.Equal(reviewDto.overallProgressDetails, viewModel.OverallProgressDetails);
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
                nextReviewDate: null,
                reviewer: "Test Reviewer",
                title: "First review",
                order: 1,
                howIsTheSchoolProgressingOverall: "On track",
                overallProgressDetails: "Good progress overall",
                ImprovementPlanObjectiveProgresses: new List<ImprovementPlanObjectiveProgressDto>()
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
                nextReviewDate: null,
                reviewer: "Test Reviewer",
                title: "First Review",
                order: 1,
                howIsTheSchoolProgressingOverall: "Making progress",
                overallProgressDetails: "Some areas improving faster than others",
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
                nextReviewDate: null,
                reviewer: "Test Reviewer",
                title: "First Review",
                order: 1,
                howIsTheSchoolProgressingOverall: "Excellent progress",
                overallProgressDetails: "All objectives are being met successfully",
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
                readableId: 1,
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
                nextReviewDate: null,
                reviewer: "Test Reviewer",
                title: "First Review",
                order: 1,
                howIsTheSchoolProgressingOverall: "Not yet assessed",
                overallProgressDetails: "Overall progress assessment pending",
                ImprovementPlanObjectiveProgresses: Array.Empty<ImprovementPlanObjectiveProgressDto>()
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
                nextReviewDate: null,
                reviewer: "",
                title: "First Review",
                order: 1,
                howIsTheSchoolProgressingOverall: "",
                overallProgressDetails: "",
                ImprovementPlanObjectiveProgresses: new List<ImprovementPlanObjectiveProgressDto>()
            );

            var improvementPlanDto = new ImprovementPlanDto(
                id: reviewDto.improvementPlanId,
                readableId: 1,
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
                nextReviewDate: null,
                reviewer: "Test Reviewer",
                title: "First Review",
                order: 1,
                howIsTheSchoolProgressingOverall: "Steady progress",
                overallProgressDetails: "Consistent improvement across most areas",
                ImprovementPlanObjectiveProgresses: new List<ImprovementPlanObjectiveProgressDto>()
            );

            var improvementPlanDto = new ImprovementPlanDto(
                id: reviewDto.improvementPlanId,
                readableId: 1,
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
                nextReviewDate: null,
                reviewer: "Test Reviewer",
                title: title,
                order: order,
                howIsTheSchoolProgressingOverall: "Variable progress",
                overallProgressDetails: "Progress varies across different areas",
                ImprovementPlanObjectiveProgresses: new List<ImprovementPlanObjectiveProgressDto>()
            );

            var improvementPlanDto = new ImprovementPlanDto(
                id: reviewDto.improvementPlanId,
                readableId: 1,
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
                nextReviewDate: futureDate,
                reviewer: "Test Reviewer",
                title: "Past Review",
                order: 1,
                howIsTheSchoolProgressingOverall: "Historical progress",
                overallProgressDetails: "Review of past performance",
                ImprovementPlanObjectiveProgresses: new List<ImprovementPlanObjectiveProgressDto>()
            );

            var improvementPlanDto = new ImprovementPlanDto(
                id: reviewDto.improvementPlanId,
                readableId: 1,
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
            Assert.Equal(string.Empty, viewModel.HowIsTheSchoolProgressingOverall);
            Assert.Equal(string.Empty, viewModel.OverallProgressDetails);
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
                nextReviewDate: null,
                reviewer: "Test Reviewer",
                title: "First Review",
                order: 1,
                howIsTheSchoolProgressingOverall: "Exceptional progress",
                overallProgressDetails: "Progress exceeds expectations in all areas",
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
            Assert.Equal(ImprovementPlanReviewViewModel.ProgressStatusRecorded, viewModel.ProgressStatus);
            Assert.Equal("govuk-tag--green", viewModel.ProgressStatusClass);
        }

        [Theory]
        [InlineData("On track", "Good progress being made")]
        [InlineData("Behind schedule", "Some areas need improvement")]
        [InlineData("Exceeding expectations", "Outstanding progress in all areas")]
        [InlineData("At risk", "Serious concerns about current trajectory")]
        [InlineData("", "No details recorded yet")]
        [InlineData("Complete", "All objectives successfully met")]
        public void Create_WithDifferentOverallProgressValues_ShouldSetCorrectOverallProgress(string progressStatus, string progressDetails)
        {
            // Arrange
            var reviewDto = new ImprovementPlanReviewDto(
                id: Guid.NewGuid(),
                readableId: 1,
                improvementPlanId: Guid.NewGuid(),
                reviewDate: DateTime.Now,
                nextReviewDate: null,
                reviewer: "Test Reviewer",
                title: "Test Review",
                order: 1,
                howIsTheSchoolProgressingOverall: progressStatus,
                overallProgressDetails: progressDetails,
                ImprovementPlanObjectiveProgresses: new List<ImprovementPlanObjectiveProgressDto>()
            );

            var improvementPlanDto = new ImprovementPlanDto(
                id: reviewDto.improvementPlanId,
                readableId: 1,
                supportProjectId: 1,
                objectivesSectionComplete: true,
                ImprovementPlanObjectives: new List<ImprovementPlanObjectiveDto>(),
                ImprovementPlanReviews: new List<ImprovementPlanReviewDto>()
            );

            // Act
            var viewModel = ImprovementPlanReviewViewModel.Create(reviewDto, improvementPlanDto);

            // Assert
            Assert.Equal(progressStatus, viewModel.HowIsTheSchoolProgressingOverall);
            Assert.Equal(progressDetails, viewModel.OverallProgressDetails);
        }

        [Fact]
        public void Create_WithNullOverallProgressValues_ShouldHandleNullValues()
        {
            // Arrange
            var reviewDto = new ImprovementPlanReviewDto(
                id: Guid.NewGuid(),
                readableId: 1,
                improvementPlanId: Guid.NewGuid(),
                reviewDate: DateTime.Now,
                nextReviewDate: null,
                reviewer: "Test Reviewer",
                title: "Test Review",
                order: 1,
                howIsTheSchoolProgressingOverall: null!,
                overallProgressDetails: null!,
                ImprovementPlanObjectiveProgresses: new List<ImprovementPlanObjectiveProgressDto>()
            );

            var improvementPlanDto = new ImprovementPlanDto(
                id: reviewDto.improvementPlanId,
                readableId: 1,
                supportProjectId: 1,
                objectivesSectionComplete: true,
                ImprovementPlanObjectives: new List<ImprovementPlanObjectiveDto>(),
                ImprovementPlanReviews: new List<ImprovementPlanReviewDto>()
            );

            // Act
            var viewModel = ImprovementPlanReviewViewModel.Create(reviewDto, improvementPlanDto);

            // Assert
            Assert.Null(viewModel.HowIsTheSchoolProgressingOverall);
            Assert.Equal("No details recorded yet", viewModel.OverallProgressDetails);
        }
    }
}