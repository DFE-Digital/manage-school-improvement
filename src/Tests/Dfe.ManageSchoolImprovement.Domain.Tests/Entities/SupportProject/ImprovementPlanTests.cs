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
    }
}