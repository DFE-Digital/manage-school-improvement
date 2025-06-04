using FluentAssertions;
using Dfe.ManageSchoolImprovement.Infrastructure.Repositories;

namespace Dfe.ManageSchoolImprovement.Infrastructure.Tests.Repositories
{
    public class SupportProjectRepositoryTests(DatabaseFixture fixture) : IClassFixture<DatabaseFixture>
    {
        [Fact]
        public async Task SearchForSupportProjects_ShouldReturnFilteredAndPagedResults()
        {
            // Arrange
            var service = new SupportProjectRepository(fixture.Context);

            // Act 
            var (projects, totalCount) = await service.SearchForSupportProjects(
                title: "School",
                states: [],
                assignedUsers: ["User1"],
                regions: ["Region1"],
                localAuthorities: ["Authority1"],
                page: 1,
                count: 2,
                cancellationToken: CancellationToken.None
            );

            // Assert
            totalCount.Should().Be(1); // Assert total count
            projects.Should().HaveCount(1);              // Assert paged results
            projects.First().SchoolName.Should().Be("School A");
        }

        [Fact]
        public async Task SearchForSupportProjectsWithSchoolNameLikeSchool_ShouldReturnFilteredAndPagedResults()
        {
            // Arrange
            var service = new SupportProjectRepository(fixture.Context);

            var (projects, totalCount) = await service.SearchForSupportProjects(
                title: "School",
                states: [],
                assignedUsers: [],
                regions: [],
                localAuthorities: [],
                page: 1,
                count: 10,
                cancellationToken: CancellationToken.None
            );

            // Assert
            totalCount.Should().Be(3); // Assert total count
            projects.Should().HaveCount(3);              // Assert paged results
            projects.First().SchoolName.Should().Be("School C");
        }

        [Fact]
        public async Task SearchForSupportProjectsWithUrn_ShouldReturnFilteredAndPagedResults()
        {
            // Arrange
            var service = new SupportProjectRepository(fixture.Context);

            var (projects, totalCount) = await service.SearchForSupportProjects(
                title: "100001",
                states: [],
                assignedUsers: [],
                regions: [],
                localAuthorities: [],
                page: 1,
                count: 10,
                cancellationToken: CancellationToken.None
            );

            // Assert
            totalCount.Should().Be(1); // Assert total count
            projects.Should().HaveCount(1);              // Assert paged results
            projects.First().SchoolName.Should().Be("School A");
        }

        public static readonly TheoryData<string?, string[], string[]> DeleteProjectCases = new()
        {
            { "School D", [], []},
            { "100004", [], []},
            { null, ["Region3"], []},
            { null, [], ["Authority5"]},
        };

        [Theory, MemberData(nameof(DeleteProjectCases))]
        public async Task SearchForSupportProjects_ShouldReturnNoSoftDeletedProjects(string? title, string[] regions, string[] localAuthorities)
        {
            // Arrange
            var service = new SupportProjectRepository(fixture.Context);

            // Act 
            var (projects, totalCount) = await service.SearchForSupportProjects(
                title: title,
                states: [],
                assignedUsers: [],
                regions: regions,
                localAuthorities: localAuthorities,
                page: 1,
                count: 2,
                cancellationToken: CancellationToken.None
            );

            // Assert
            totalCount.Should().Be(0);
            projects.Should().HaveCount(0);
        }
    }
}

