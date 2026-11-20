using Dfe.ManageSchoolImprovement.Application.MappingProfiles;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Models;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Mapster;

namespace Dfe.ManageSchoolImprovement.Application.Tests.MappingProfiles
{
    public class RiseProfileTests
    {
        private readonly TypeAdapterConfig _config;

        public RiseProfileTests()
        {
            _config = new TypeAdapterConfig();
            _config.Default.MaxDepth(64);
            _config.Default.MapToConstructor(true);
            new RiseProfile().Register(_config);
            _config.Compile();
        }

        [Fact]
        public void Should_Map_SupportProjectId_To_Int()
        {
            // Arrange
            var supportProjectId = new SupportProjectId(123);

            // Act
            var result = supportProjectId.Adapt<int>(_config);

            // Assert
            Assert.Equal(123, result);
        }

        [Fact]
        public void Should_Map_Int_To_SupportProjectId()
        {
            // Arrange
            var value = 123;

            // Act
            var result = value.Adapt<SupportProjectId>(_config);

            // Assert
            Assert.Equal(value, result.Value);
        }

        [Fact]
        public void Should_Map_SupportProject_To_SupportProjectDto()
        {
            // Arrange
            var supportProject = new Domain.Entities.SupportProject.SupportProject(new SupportProjectId(123), "School Name", "Urn", "Local Authority", "Region");

            // Act
            var result = supportProject.Adapt<SupportProjectDto>(_config);

            // Assert
            Assert.Equal(123, result.Id);
        }

        [Fact]
        public void Should_Map_SupportProjectDto_Id_To_SupportProjectId()
        {
            // Arrange
            var dto = new SupportProjectDto(123, DateTime.UtcNow, null);

            // Act
            var result = dto.Adapt<Domain.Entities.SupportProject.SupportProject>(_config);

            // Assert
            Assert.Equal(123, result.Id.Value);
        }

        [Fact]
        public void Should_Map_Nested_ImprovementPlans()
        {
            // Arrange
            var supportProjectId = new SupportProjectId(123);
            var improvementPlanId = new ImprovementPlanId(Guid.NewGuid());
            var supportProject = new Domain.Entities.SupportProject.SupportProject(
                supportProjectId,
                "School Name",
                "Urn",
                "Local Authority",
                "Region");
            supportProject.AddImprovementPlan(improvementPlanId, supportProjectId);

            // Act
            var result = supportProject.Adapt<SupportProjectDto>(_config);

            // Assert
            var improvementPlan = Assert.Single(result.ImprovementPlans);
            Assert.Equal(improvementPlanId.Value, improvementPlan.id);
            Assert.Equal(supportProjectId.Value, improvementPlan.supportProjectId);
        }
    }
}
