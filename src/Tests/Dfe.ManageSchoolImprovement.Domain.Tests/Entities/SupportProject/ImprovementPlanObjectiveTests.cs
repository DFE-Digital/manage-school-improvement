using Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;

namespace Dfe.ManageSchoolImprovement.Domain.Tests.Entities.SupportProject
{
    public class ImprovementPlanObjectiveTests
    {
        private readonly ImprovementPlanObjectiveId _objectiveId;
        private readonly ImprovementPlanId _improvementPlanId;
        private readonly string _areaOfImprovement;
        private readonly string _details;
        private readonly int _order;

        public ImprovementPlanObjectiveTests()
        {
            _objectiveId = new ImprovementPlanObjectiveId(Guid.NewGuid());
            _improvementPlanId = new ImprovementPlanId(Guid.NewGuid());
            _areaOfImprovement = "QualityOfEducation";
            _details = "Improve student outcomes in mathematics";
            _order = 1;
        }

        [Fact]
        public void Constructor_WithValidParameters_CreatesImprovementPlanObjective()
        {
            // Act
            var objective = new ImprovementPlanObjective(_objectiveId, _improvementPlanId, _areaOfImprovement, _details, _order);

            // Assert
            Assert.Equal(_objectiveId, objective.Id);
            Assert.Equal(_improvementPlanId, objective.ImprovementPlanId);
            Assert.Equal(_areaOfImprovement, objective.AreaOfImprovement);
            Assert.Equal(_details, objective.Details);
            Assert.Equal(_order, objective.Order);
            Assert.Equal(string.Empty, objective.CreatedBy);
            Assert.Null(objective.LastModifiedOn);
            Assert.Null(objective.LastModifiedBy);
        }

        [Theory]
        [InlineData("QualityOfEducation")]
        [InlineData("LeadershipAndManagement")]
        [InlineData("BehaviourAndAttitudes")]
        [InlineData("Attendance")]
        [InlineData("PersonalDevelopment")]
        public void Constructor_WithDifferentAreasOfImprovement_SetsAreaCorrectly(string areaOfImprovement)
        {
            // Act
            var objective = new ImprovementPlanObjective(_objectiveId, _improvementPlanId, areaOfImprovement, _details, _order);

            // Assert
            Assert.Equal(areaOfImprovement, objective.AreaOfImprovement);
        }

        [Theory]
        [InlineData("")]
        [InlineData("Short details")]
        [InlineData("Very comprehensive and detailed improvement objective with specific implementation strategies and measurable outcomes")]
        [InlineData("   ")]
        public void Constructor_WithDifferentDetails_SetsDetailsCorrectly(string details)
        {
            // Act
            var objective = new ImprovementPlanObjective(_objectiveId, _improvementPlanId, _areaOfImprovement, details, _order);

            // Assert
            Assert.Equal(details, objective.Details);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        [InlineData(10)]
        public void Constructor_WithDifferentOrders_SetsOrderCorrectly(int order)
        {
            // Act
            var objective = new ImprovementPlanObjective(_objectiveId, _improvementPlanId, _areaOfImprovement, _details, order);

            // Assert
            Assert.Equal(order, objective.Order);
        }

        [Fact]
        public void SetDetails_WithNewDetails_UpdatesDetailsProperty()
        {
            // Arrange
            var objective = new ImprovementPlanObjective(_objectiveId, _improvementPlanId, _areaOfImprovement, _details, _order);
            var newDetails = "Updated comprehensive details with new strategies";

            // Act
            objective.SetDetails(newDetails);

            // Assert
            Assert.Equal(newDetails, objective.Details);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("Short update")]
        [InlineData("Extremely detailed and comprehensive update with multiple implementation phases and measurable outcomes")]
        public void SetDetails_WithVariousDetailLengths_UpdatesDetailsCorrectly(string newDetails)
        {
            // Arrange
            var objective = new ImprovementPlanObjective(_objectiveId, _improvementPlanId, _areaOfImprovement, _details, _order);

            // Act
            objective.SetDetails(newDetails);

            // Assert
            Assert.Equal(newDetails, objective.Details);
        }

        [Fact]
        public void SetDetails_MultipleUpdates_UpdatesDetailsEachTime()
        {
            // Arrange
            var objective = new ImprovementPlanObjective(_objectiveId, _improvementPlanId, _areaOfImprovement, _details, _order);
            var firstUpdate = "First update";
            var secondUpdate = "Second update";
            var thirdUpdate = "Third update";

            // Act & Assert
            objective.SetDetails(firstUpdate);
            Assert.Equal(firstUpdate, objective.Details);

            objective.SetDetails(secondUpdate);
            Assert.Equal(secondUpdate, objective.Details);

            objective.SetDetails(thirdUpdate);
            Assert.Equal(thirdUpdate, objective.Details);
        }

        [Fact]
        public void SetDetails_DoesNotAffectOtherProperties()
        {
            // Arrange
            var objective = new ImprovementPlanObjective(_objectiveId, _improvementPlanId, _areaOfImprovement, _details, _order);
            var newDetails = "Updated details";

            // Act
            objective.SetDetails(newDetails);

            // Assert - Verify other properties remain unchanged
            Assert.Equal(_objectiveId, objective.Id);
            Assert.Equal(_improvementPlanId, objective.ImprovementPlanId);
            Assert.Equal(_areaOfImprovement, objective.AreaOfImprovement);
            Assert.Equal(_order, objective.Order);
        }

        [Fact]
        public void AreaOfImprovement_CanBeModifiedDirectly()
        {
            // Arrange
            var objective = new ImprovementPlanObjective(_objectiveId, _improvementPlanId, _areaOfImprovement, _details, _order);
            var newAreaOfImprovement = "LeadershipAndManagement";

            // Act
            objective.AreaOfImprovement = newAreaOfImprovement;

            // Assert
            Assert.Equal(newAreaOfImprovement, objective.AreaOfImprovement);
        }

        [Fact]
        public void Details_CanBeModifiedDirectly()
        {
            // Arrange
            var objective = new ImprovementPlanObjective(_objectiveId, _improvementPlanId, _areaOfImprovement, _details, _order);
            var newDetails = "Directly modified details";

            // Act
            objective.Details = newDetails;

            // Assert
            Assert.Equal(newDetails, objective.Details);
        }

        [Fact]
        public void CreatedBy_CanBeSetDirectly()
        {
            // Arrange
            var objective = new ImprovementPlanObjective(_objectiveId, _improvementPlanId, _areaOfImprovement, _details, _order);
            var createdBy = "test.user@example.com";

            // Act
            objective.CreatedBy = createdBy;

            // Assert
            Assert.Equal(createdBy, objective.CreatedBy);
        }

        [Fact]
        public void LastModifiedBy_CanBeSetDirectly()
        {
            // Arrange
            var objective = new ImprovementPlanObjective(_objectiveId, _improvementPlanId, _areaOfImprovement, _details, _order);
            var lastModifiedBy = "modifier.user@example.com";

            // Act
            objective.LastModifiedBy = lastModifiedBy;

            // Assert
            Assert.Equal(lastModifiedBy, objective.LastModifiedBy);
        }

        [Fact]
        public void CreatedOn_CanBeSetDirectly()
        {
            // Arrange
            var objective = new ImprovementPlanObjective(_objectiveId, _improvementPlanId, _areaOfImprovement, _details, _order);
            var createdOn = DateTime.UtcNow;

            // Act
            objective.CreatedOn = createdOn;

            // Assert
            Assert.Equal(createdOn, objective.CreatedOn);
        }

        [Fact]
        public void LastModifiedOn_CanBeSetDirectly()
        {
            // Arrange
            var objective = new ImprovementPlanObjective(_objectiveId, _improvementPlanId, _areaOfImprovement, _details, _order);
            var lastModifiedOn = DateTime.UtcNow;

            // Act
            objective.LastModifiedOn = lastModifiedOn;

            // Assert
            Assert.Equal(lastModifiedOn, objective.LastModifiedOn);
        }
    }
} 