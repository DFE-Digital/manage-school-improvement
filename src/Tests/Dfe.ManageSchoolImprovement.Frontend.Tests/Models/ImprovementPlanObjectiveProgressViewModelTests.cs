using Dfe.ManageSchoolImprovement.Application.SupportProject.Models;
using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.Models
{
    public class ImprovementPlanObjectiveProgressViewModelTests
    {
        [Fact]
        public void Create_ShouldReturnImprovementPlanObjectiveProgressViewModel_WithCorrectProperties()
        {
            // Arrange
            var progressDto = new ImprovementPlanObjectiveProgressDto(
                id: Guid.NewGuid(),
                readableId: 1,
                improvementPlanReviewId: Guid.NewGuid(),
                improvementPlanObjectiveId: Guid.NewGuid(),
                howIsSchoolProgressing: "On track",
                progressDetails: "Good progress has been made"
            );

            // Act
            var viewModel = ImprovementPlanObjectiveProgressViewModel.Create(progressDto);

            // Assert
            Assert.Equal(progressDto.id, viewModel.Id);
            Assert.Equal(progressDto.readableId, viewModel.ReadableId);
            Assert.Equal(progressDto.improvementPlanReviewId, viewModel.ImprovementPlanReviewId);
            Assert.Equal(progressDto.improvementPlanObjectiveId, viewModel.ImprovementPlanObjectiveId);
            Assert.Equal(progressDto.howIsSchoolProgressing, viewModel.HowIsSchoolProgressing);
            Assert.Equal(progressDto.progressDetails, viewModel.ProgressDetails);
        }

        [Theory]
        [InlineData("On track", "Making excellent progress")]
        [InlineData("Behind", "Some challenges encountered")]
        [InlineData("At risk", "Significant issues need addressing")]
        [InlineData("Complete", "All objectives have been met")]
        [InlineData("Not started", "Objective not yet begun")]
        [InlineData("Excellent", "Exceeding all expectations")]
        public void Create_WithDifferentProgressStatuses_ShouldSetCorrectValues(string progressStatus, string progressDetails)
        {
            // Arrange
            var progressDto = new ImprovementPlanObjectiveProgressDto(
                id: Guid.NewGuid(),
                readableId: 1,
                improvementPlanReviewId: Guid.NewGuid(),
                improvementPlanObjectiveId: Guid.NewGuid(),
                howIsSchoolProgressing: progressStatus,
                progressDetails: progressDetails
            );

            // Act
            var viewModel = ImprovementPlanObjectiveProgressViewModel.Create(progressDto);

            // Assert
            Assert.Equal(progressStatus, viewModel.HowIsSchoolProgressing);
            Assert.Equal(progressDetails, viewModel.ProgressDetails);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("", "Some details")]
        [InlineData("Status", "")]
        [InlineData("   ", "   ")]
        public void Create_WithEmptyOrWhitespaceValues_ShouldSetCorrectValues(string progressStatus, string progressDetails)
        {
            // Arrange
            var progressDto = new ImprovementPlanObjectiveProgressDto(
                id: Guid.NewGuid(),
                readableId: 1,
                improvementPlanReviewId: Guid.NewGuid(),
                improvementPlanObjectiveId: Guid.NewGuid(),
                howIsSchoolProgressing: progressStatus,
                progressDetails: progressDetails
            );

            // Act
            var viewModel = ImprovementPlanObjectiveProgressViewModel.Create(progressDto);

            // Assert
            Assert.Equal(progressStatus, viewModel.HowIsSchoolProgressing);
            Assert.Equal(progressDetails, viewModel.ProgressDetails);
        }

        [Fact]
        public void Create_WithVeryLongProgressDetails_ShouldSetCorrectValues()
        {
            // Arrange
            var longDetails = new string('A', 2000); // Very long string
            var progressDto = new ImprovementPlanObjectiveProgressDto(
                id: Guid.NewGuid(),
                readableId: 1,
                improvementPlanReviewId: Guid.NewGuid(),
                improvementPlanObjectiveId: Guid.NewGuid(),
                howIsSchoolProgressing: "On track",
                progressDetails: longDetails
            );

            // Act
            var viewModel = ImprovementPlanObjectiveProgressViewModel.Create(progressDto);

            // Assert
            Assert.Equal(longDetails, viewModel.ProgressDetails);
            Assert.Equal("On track", viewModel.HowIsSchoolProgressing);
        }

        [Fact]
        public void Create_WithSpecialCharacters_ShouldSetCorrectValues()
        {
            // Arrange
            var progressDto = new ImprovementPlanObjectiveProgressDto(
                id: Guid.NewGuid(),
                readableId: 1,
                improvementPlanReviewId: Guid.NewGuid(),
                improvementPlanObjectiveId: Guid.NewGuid(),
                howIsSchoolProgressing: "Status with special chars: !@#$%^&*()",
                progressDetails: "Details with special chars: <>&\"'"
            );

            // Act
            var viewModel = ImprovementPlanObjectiveProgressViewModel.Create(progressDto);

            // Assert
            Assert.Equal("Status with special chars: !@#$%^&*()", viewModel.HowIsSchoolProgressing);
            Assert.Equal("Details with special chars: <>&\"'", viewModel.ProgressDetails);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(999)]
        [InlineData(int.MaxValue)]
        public void Create_WithDifferentReadableIds_ShouldSetCorrectReadableId(int readableId)
        {
            // Arrange
            var progressDto = new ImprovementPlanObjectiveProgressDto(
                id: Guid.NewGuid(),
                readableId: readableId,
                improvementPlanReviewId: Guid.NewGuid(),
                improvementPlanObjectiveId: Guid.NewGuid(),
                howIsSchoolProgressing: "On track",
                progressDetails: "Good progress"
            );

            // Act
            var viewModel = ImprovementPlanObjectiveProgressViewModel.Create(progressDto);

            // Assert
            Assert.Equal(readableId, viewModel.ReadableId);
        }

        [Fact]
        public void Create_WithDifferentGuids_ShouldSetCorrectGuids()
        {
            // Arrange
            var id = Guid.NewGuid();
            var reviewId = Guid.NewGuid();
            var objectiveId = Guid.NewGuid();

            var progressDto = new ImprovementPlanObjectiveProgressDto(
                id: id,
                readableId: 1,
                improvementPlanReviewId: reviewId,
                improvementPlanObjectiveId: objectiveId,
                howIsSchoolProgressing: "On track",
                progressDetails: "Good progress"
            );

            // Act
            var viewModel = ImprovementPlanObjectiveProgressViewModel.Create(progressDto);

            // Assert
            Assert.Equal(id, viewModel.Id);
            Assert.Equal(reviewId, viewModel.ImprovementPlanReviewId);
            Assert.Equal(objectiveId, viewModel.ImprovementPlanObjectiveId);
        }

        [Fact]
        public void Create_WithEmptyGuids_ShouldSetEmptyGuids()
        {
            // Arrange
            var progressDto = new ImprovementPlanObjectiveProgressDto(
                id: Guid.Empty,
                readableId: 1,
                improvementPlanReviewId: Guid.Empty,
                improvementPlanObjectiveId: Guid.Empty,
                howIsSchoolProgressing: "On track",
                progressDetails: "Good progress"
            );

            // Act
            var viewModel = ImprovementPlanObjectiveProgressViewModel.Create(progressDto);

            // Assert
            Assert.Equal(Guid.Empty, viewModel.Id);
            Assert.Equal(Guid.Empty, viewModel.ImprovementPlanReviewId);
            Assert.Equal(Guid.Empty, viewModel.ImprovementPlanObjectiveId);
        }

        [Fact]
        public void Constructor_ShouldInitializePropertiesWithDefaultValues()
        {
            // Act
            var viewModel = new ImprovementPlanObjectiveProgressViewModel();

            // Assert
            Assert.Equal(Guid.Empty, viewModel.Id);
            Assert.Equal(0, viewModel.ReadableId);
            Assert.Equal(Guid.Empty, viewModel.ImprovementPlanReviewId);
            Assert.Equal(Guid.Empty, viewModel.ImprovementPlanObjectiveId);
            Assert.Equal(string.Empty, viewModel.HowIsSchoolProgressing);
            Assert.Equal(string.Empty, viewModel.ProgressDetails);
        }

        [Fact]
        public void Create_WithMultilineProgressDetails_ShouldSetCorrectValues()
        {
            // Arrange
            var multilineDetails = "Line 1\nLine 2\r\nLine 3\n\rLine 4";
            var progressDto = new ImprovementPlanObjectiveProgressDto(
                id: Guid.NewGuid(),
                readableId: 1,
                improvementPlanReviewId: Guid.NewGuid(),
                improvementPlanObjectiveId: Guid.NewGuid(),
                howIsSchoolProgressing: "On track",
                progressDetails: multilineDetails
            );

            // Act
            var viewModel = ImprovementPlanObjectiveProgressViewModel.Create(progressDto);

            // Assert
            Assert.Equal(multilineDetails, viewModel.ProgressDetails);
        }

        [Fact]
        public void Create_WithUnicodeCharacters_ShouldSetCorrectValues()
        {
            // Arrange
            var progressDto = new ImprovementPlanObjectiveProgressDto(
                id: Guid.NewGuid(),
                readableId: 1,
                improvementPlanReviewId: Guid.NewGuid(),
                improvementPlanObjectiveId: Guid.NewGuid(),
                howIsSchoolProgressing: "Progreso excelente ??",
                progressDetails: "Detalles del progreso: ραινσϊ ??"
            );

            // Act
            var viewModel = ImprovementPlanObjectiveProgressViewModel.Create(progressDto);

            // Assert
            Assert.Equal("Progreso excelente ??", viewModel.HowIsSchoolProgressing);
            Assert.Equal("Detalles del progreso: ραινσϊ ??", viewModel.ProgressDetails);
        }

        [Fact]
        public void Create_MultipleInstancesWithSameData_ShouldCreateIdenticalViewModels()
        {
            // Arrange
            var progressDto = new ImprovementPlanObjectiveProgressDto(
                id: Guid.NewGuid(),
                readableId: 1,
                improvementPlanReviewId: Guid.NewGuid(),
                improvementPlanObjectiveId: Guid.NewGuid(),
                howIsSchoolProgressing: "On track",
                progressDetails: "Good progress"
            );

            // Act
            var viewModel1 = ImprovementPlanObjectiveProgressViewModel.Create(progressDto);
            var viewModel2 = ImprovementPlanObjectiveProgressViewModel.Create(progressDto);

            // Assert
            Assert.Equal(viewModel1.Id, viewModel2.Id);
            Assert.Equal(viewModel1.ReadableId, viewModel2.ReadableId);
            Assert.Equal(viewModel1.ImprovementPlanReviewId, viewModel2.ImprovementPlanReviewId);
            Assert.Equal(viewModel1.ImprovementPlanObjectiveId, viewModel2.ImprovementPlanObjectiveId);
            Assert.Equal(viewModel1.HowIsSchoolProgressing, viewModel2.HowIsSchoolProgressing);
            Assert.Equal(viewModel1.ProgressDetails, viewModel2.ProgressDetails);
        }
    }
}