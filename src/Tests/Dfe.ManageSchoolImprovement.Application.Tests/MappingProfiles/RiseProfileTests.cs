using AutoMapper;
using Dfe.ManageSchoolImprovement.Application.MappingProfiles;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Models;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;

namespace Dfe.ManageSchoolImprovement.Application.Tests.MappingProfiles
{
    public class RiseProfileTests
    {
        private readonly IMapper _mapper;

        public RiseProfileTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<RiseProfile>();
            });
            _mapper = config.CreateMapper();
        }

        [Fact]
        public void Should_Map_SupportProjectId_To_Int()
        {
            // Arrange
            var supportProjectId = new SupportProjectId(123);

            // Act
            var result = _mapper.Map<int>(supportProjectId);

            // Assert
            Assert.Equal(123, result);
        }

        [Fact]
        public void Should_Map_Int_To_SupportProjectId()
        {
            // Arrange
            var value = 123;

            // Act
            var result = _mapper.Map<SupportProjectId>(value);

            // Assert
            Assert.Equal(value, result.Value);
        }

        [Fact]
        public void Should_Map_SupportProject_To_SupportProjectDto()
        {
            // Arrange
            var supportProject = new Domain.Entities.SupportProject.SupportProject(new SupportProjectId(123), "School Name", "Urn", "Local Authority", "Region");

            // Act
            var result = _mapper.Map<SupportProjectDto>(supportProject);

            // Assert
            Assert.Equal(123, result.Id);
        }
    }
}
