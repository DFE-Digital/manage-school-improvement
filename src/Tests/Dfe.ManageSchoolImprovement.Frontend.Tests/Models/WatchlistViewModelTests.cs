using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.Models;

public class WatchlistViewModelTests
{
    [Fact]
    public void WatchlistViewModel_ShouldHaveDefaultValues()
    {
        // Arrange & Act
        var model = new WatchlistViewModel();

        // Assert
        Assert.Equal(Guid.Empty, model.WatchlistId);
        Assert.Equal(0, model.ReadableId);
        Assert.Null(model.SupportProjectId);
        Assert.Null(model.User);
        Assert.Null(model.SchoolName);
        Assert.Null(model.DateAdded);
        Assert.Null(model.AssignedTo);
        Assert.Null(model.SupportingOrganisationName);
        Assert.Null(model.Status);
        Assert.Null(model.CurrentDeliveryMilestone);
    }

    [Fact]
    public void WatchlistViewModel_ShouldSetAndGetValues()
    {
        // Arrange
        var expectedWatchlistId = Guid.NewGuid();
        const int expectedReadableId = 42;
        const int expectedSupportProjectId = 123;
        const string expectedUser = "user@education.gov.uk";
        const string expectedSchoolName = "Test School";
        var expectedDateAdded = new DateTime(2024, 6, 15, 10, 30, 0, DateTimeKind.Utc);
        const string expectedAssignedTo = "Delivery Officer";
        const string expectedSupportingOrganisationName = "Support Org Ltd";
        const ProjectStatusValue expectedStatus = ProjectStatusValue.InProgress;
        const Milestone expectedCurrentDeliveryMilestone = Milestone.MatchingComplete;

        // Act
        var model = new WatchlistViewModel
        {
            WatchlistId = expectedWatchlistId,
            ReadableId = expectedReadableId,
            SupportProjectId = expectedSupportProjectId,
            User = expectedUser,
            SchoolName = expectedSchoolName,
            DateAdded = expectedDateAdded,
            AssignedTo = expectedAssignedTo,
            SupportingOrganisationName = expectedSupportingOrganisationName,
            Status = expectedStatus,
            CurrentDeliveryMilestone = expectedCurrentDeliveryMilestone
        };

        // Assert
        Assert.Equal(expectedWatchlistId, model.WatchlistId);
        Assert.Equal(expectedReadableId, model.ReadableId);
        Assert.Equal(expectedSupportProjectId, model.SupportProjectId);
        Assert.Equal(expectedUser, model.User);
        Assert.Equal(expectedSchoolName, model.SchoolName);
        Assert.Equal(expectedDateAdded, model.DateAdded);
        Assert.Equal(expectedAssignedTo, model.AssignedTo);
        Assert.Equal(expectedSupportingOrganisationName, model.SupportingOrganisationName);
        Assert.Equal(expectedStatus, model.Status);
        Assert.Equal(expectedCurrentDeliveryMilestone, model.CurrentDeliveryMilestone);
    }

    [Fact]
    public void WatchlistViewModel_ShouldHandleNullableProperties()
    {
        // Arrange & Act
        var model = new WatchlistViewModel
        {
            WatchlistId = Guid.NewGuid(),
            ReadableId = 1,
            SupportProjectId = null,
            User = null,
            SchoolName = null,
            DateAdded = null,
            AssignedTo = null,
            SupportingOrganisationName = null,
            Status = null,
            CurrentDeliveryMilestone = null
        };

        // Assert
        Assert.NotEqual(Guid.Empty, model.WatchlistId);
        Assert.Equal(1, model.ReadableId);
        Assert.Null(model.SupportProjectId);
        Assert.Null(model.User);
        Assert.Null(model.SchoolName);
        Assert.Null(model.DateAdded);
        Assert.Null(model.AssignedTo);
        Assert.Null(model.SupportingOrganisationName);
        Assert.Null(model.Status);
        Assert.Null(model.CurrentDeliveryMilestone);
    }

    [Theory]
    [InlineData(ProjectStatusValue.InProgress)]
    [InlineData(ProjectStatusValue.Paused)]
    [InlineData(ProjectStatusValue.Stopped)]
    public void WatchlistViewModel_ShouldAcceptProjectStatusValues(ProjectStatusValue status)
    {
        // Arrange & Act
        var model = new WatchlistViewModel { Status = status };

        // Assert
        Assert.Equal(status, model.Status);
    }

    [Theory]
    [InlineData(Milestone.FormallyInformResponsibleBody)]
    [InlineData(Milestone.FirstRiseMeeting)]
    [InlineData(Milestone.InitialDiagnosis)]
    [InlineData(Milestone.MatchingComplete)]
    [InlineData(Milestone.PlansApproved)]
    [InlineData(Milestone.ImprovementGrantOfferLetterRequested)]
    [InlineData(Milestone.ImplementationAndTermlyReviews)]
    [InlineData(Milestone.TermlyReviews)]
    public void WatchlistViewModel_ShouldAcceptMilestoneValues(Milestone milestone)
    {
        // Arrange & Act
        var model = new WatchlistViewModel { CurrentDeliveryMilestone = milestone };

        // Assert
        Assert.Equal(milestone, model.CurrentDeliveryMilestone);
    }

    [Fact]
    public void WatchlistViewModel_ShouldAllowIndependentPropertyModification()
    {
        // Arrange
        var model = new WatchlistViewModel
        {
            SchoolName = "Original School",
            User = "original@education.gov.uk"
        };

        // Act
        model.SchoolName = "Updated School";

        // Assert
        Assert.Equal("Updated School", model.SchoolName);
        Assert.Equal("original@education.gov.uk", model.User);
    }
}
