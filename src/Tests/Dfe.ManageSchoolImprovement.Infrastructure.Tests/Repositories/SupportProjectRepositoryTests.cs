using Dfe.ManageSchoolImprovement.Infrastructure.Repositories;
using FluentAssertions;

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
                assignedAdvisers: ["Test Adviser"],
                regions: ["Region1"],
                localAuthorities: ["Authority1"],
                trusts: [], // Add the missing trusts parameter
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
                assignedAdvisers: [],
                regions: [],
                localAuthorities: [],
                trusts: [], // Add the missing trusts parameter
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
                assignedAdvisers: [],
                regions: [],
                localAuthorities: [],
                trusts: [], // Add the missing trusts parameter
                page: 1,
                count: 10,
                cancellationToken: CancellationToken.None
            );

            // Assert
            totalCount.Should().Be(1); // Assert total count
            projects.Should().HaveCount(1);              // Assert paged results
            projects.First().SchoolName.Should().Be("School A");
        }

        [Fact]
        public async Task SearchForSupportProjects_WithNoUserAssigned_ShouldReturnFilteredAndPagedResults()
        {
            // Arrange
            var service = new SupportProjectRepository(fixture.Context);

            // Act 
            var (projects, totalCount) = await service.SearchForSupportProjects(
                title: null,
                states: [],
                assignedUsers: ["not assigned"],
                assignedAdvisers: [],
                regions: [],
                localAuthorities: [],
                trusts: [], // Add the missing trusts parameter
                page: 1,
                count: 3,
                cancellationToken: CancellationToken.None
            );

            // Assert
            totalCount.Should().Be(2); // Assert total count
            projects.Should().HaveCount(2);              // Assert paged results
            projects.Should().Contain(x => x.SchoolName == "School B");
            projects.Should().Contain(x => x.SchoolName == "School C");
        }

        [Fact]
        public async Task SearchForSupportProjects_WithNoAdviserAssigned_ShouldReturnFilteredAndPagedResults()
        {
            // Arrange
            var service = new SupportProjectRepository(fixture.Context);

            // Act 
            var (projects, totalCount) = await service.SearchForSupportProjects(
                title: null,
                states: [],
                assignedUsers: [],
                assignedAdvisers: ["not assigned"],
                regions: [],
                localAuthorities: [],
                trusts: [], // Add the missing trusts parameter
                page: 1,
                count: 3,
                cancellationToken: CancellationToken.None
            );

            // Assert
            totalCount.Should().Be(2); // Assert total count
            projects.Should().HaveCount(2);              // Assert paged results
            projects.Should().Contain(x => x.SchoolName == "School B");
            projects.Should().Contain(x => x.SchoolName == "School C");
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
                assignedAdvisers: [],
                regions: regions,
                localAuthorities: localAuthorities,
                trusts: [], // Add the missing trusts parameter
                page: 1,
                count: 2,
                cancellationToken: CancellationToken.None
            );

            // Assert
            totalCount.Should().Be(0);
            projects.Should().HaveCount(0);
        }

        [Fact]
        public async Task SearchForSupportProjects_WithTrustFilter_ShouldReturnFilteredResults()
        {
            // Arrange
            var service = new SupportProjectRepository(fixture.Context);

            // Act 
            var (projects, totalCount) = await service.SearchForSupportProjects(
                title: null,
                states: [],
                assignedUsers: [],
                assignedAdvisers: [],
                regions: [],
                localAuthorities: [],
                trusts: ["Trust A"],
                page: 1,
                count: 10,
                cancellationToken: CancellationToken.None
            );

            // Assert
            totalCount.Should().Be(1);
            projects.Should().HaveCount(1);
            projects.First().SchoolName.Should().Be("School A");
            projects.First().TrustName.Should().Be("Trust A");
        }

        [Fact]
        public async Task SearchForSupportProjects_WithMultipleTrusts_ShouldReturnFilteredResults()
        {
            // Arrange
            var service = new SupportProjectRepository(fixture.Context);

            // Act 
            var (projects, totalCount) = await service.SearchForSupportProjects(
                title: null,
                states: [],
                assignedUsers: [],
                assignedAdvisers: [],
                regions: [],
                localAuthorities: [],
                trusts: ["Trust A", "Trust B"],
                page: 1,
                count: 10,
                cancellationToken: CancellationToken.None
            );

            // Assert
            totalCount.Should().Be(2);
            projects.Should().HaveCount(2);
            projects.Should().Contain(x => x.SchoolName == "School A");
            projects.Should().Contain(x => x.SchoolName == "School B");
        }

        [Fact]
        public async Task SearchForSupportProjects_WithPagination_ShouldReturnCorrectPage()
        {
            // Arrange
            var service = new SupportProjectRepository(fixture.Context);

            // Act - Get page 2 with 1 item per page
            var (projects, totalCount) = await service.SearchForSupportProjects(
                title: "School",
                states: [],
                assignedUsers: [],
                assignedAdvisers: [],
                regions: [],
                localAuthorities: [],
                trusts: [],
                page: 2,
                count: 1,
                cancellationToken: CancellationToken.None
            );

            // Assert
            totalCount.Should().Be(3); // Total count should remain the same
            projects.Should().HaveCount(1); // Only 1 item on page 2
        }

        [Fact]
        public async Task GetAllProjectRegions_ShouldReturnDistinctRegions()
        {
            // Arrange
            var service = new SupportProjectRepository(fixture.Context);

            // Act
            var regions = await service.GetAllProjectRegions(CancellationToken.None);

            // Assert
            regions.Should().HaveCount(2); // Region1, Region2 (Region3 is soft deleted)
            regions.Should().Contain("Region1");
            regions.Should().Contain("Region2");
            regions.Should().NotContain("Region3"); // Soft deleted
        }

        [Fact]
        public async Task GetAllProjectLocalAuthorities_ShouldReturnDistinctAuthorities()
        {
            // Arrange
            var service = new SupportProjectRepository(fixture.Context);

            // Act
            var authorities = await service.GetAllProjectLocalAuthorities(CancellationToken.None);

            // Assert
            authorities.Should().HaveCount(3); // Authority1, Authority2, Authority3 (Authority5 is soft deleted)
            authorities.Should().Contain("Authority1");
            authorities.Should().Contain("Authority2");
            authorities.Should().Contain("Authority3");
            authorities.Should().NotContain("Authority5"); // Soft deleted
        }

        [Fact]
        public async Task GetAllProjectAssignedUsers_ShouldReturnDistinctUsers()
        {
            // Arrange
            var service = new SupportProjectRepository(fixture.Context);

            // Act
            var users = await service.GetAllProjectAssignedUsers(CancellationToken.None);

            // Assert
            users.Should().HaveCount(1); // Only User1 is assigned
            users.Should().Contain("User1");
        }

        [Fact]
        public async Task GetAllProjectAssignedAdvisers_ShouldReturnDistinctAdvisers()
        {
            // Arrange
            var service = new SupportProjectRepository(fixture.Context);

            // Act
            var advisers = await service.GetAllProjectAssignedAdvisers(CancellationToken.None);

            // Assert
            advisers.Should().HaveCount(1); // Only one adviser assigned
            advisers.Should().Contain("Test Adviser");
        }

        [Fact]
        public async Task GetAllProjectTrusts_ShouldReturnDistinctTrusts()
        {
            // Arrange
            var service = new SupportProjectRepository(fixture.Context);

            // Act
            var trusts = await service.GetAllProjectTrusts(CancellationToken.None);

            // Assert
            trusts.Should().HaveCount(2); // Trust A, Trust B (School C has no trust, Trust D is soft deleted)
            trusts.Should().Contain("Trust A");
            trusts.Should().Contain("Trust B");
            trusts.Should().NotContain("Trust D"); // Soft deleted
        }

        [Fact]
        public async Task GetSupportProjectById_WithValidId_ShouldReturnProject()
        {
            // Arrange
            var service = new SupportProjectRepository(fixture.Context);
            var existingProject = fixture.Context.SupportProjects.First(x => x.SchoolName == "School A");

            // Act
            var result = await service.GetSupportProjectById(existingProject.Id, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result!.SchoolName.Should().Be("School A");
            result.SchoolUrn.Should().Be("100001");
            result.Notes.Should().NotBeNull(); // Verify includes are working
            result.Contacts.Should().NotBeNull();
            result.FundingHistories.Should().NotBeNull();
        }

        [Fact]
        public async Task GetSupportProjectById_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            var service = new SupportProjectRepository(fixture.Context);
            var nonExistentId = new Domain.ValueObjects.SupportProjectId(999);

            // Act
            var result = await service.GetSupportProjectById(nonExistentId, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task SearchForSupportProjects_WithCombinedFilters_ShouldReturnCorrectResults()
        {
            // Arrange
            var service = new SupportProjectRepository(fixture.Context);

            // Act 
            var (projects, totalCount) = await service.SearchForSupportProjects(
                title: "School",
                states: [],
                assignedUsers: ["User1"],
                assignedAdvisers: ["Test Adviser"],
                regions: ["Region1"],
                localAuthorities: ["Authority1"],
                trusts: ["Trust A"],
                page: 1,
                count: 10,
                cancellationToken: CancellationToken.None
            );

            // Assert
            totalCount.Should().Be(1);
            projects.Should().HaveCount(1);
            projects.First().SchoolName.Should().Be("School A");
        }

        [Fact]
        public async Task SearchForSupportProjects_WithNoMatches_ShouldReturnEmptyResults()
        {
            // Arrange
            var service = new SupportProjectRepository(fixture.Context);

            // Act 
            var (projects, totalCount) = await service.SearchForSupportProjects(
                title: "NonExistentSchool",
                states: [],
                assignedUsers: [],
                assignedAdvisers: [],
                regions: [],
                localAuthorities: [],
                trusts: [],
                page: 1,
                count: 10,
                cancellationToken: CancellationToken.None
            );

            // Assert
            totalCount.Should().Be(0);
            projects.Should().BeEmpty();
        }

        [Fact]
        public async Task SearchForSupportProjects_WithAllEmptyFilters_ShouldReturnAllProjects()
        {
            // Arrange
            var service = new SupportProjectRepository(fixture.Context);

            // Act 
            var (projects, totalCount) = await service.SearchForSupportProjects(
                title: null,
                states: [],
                assignedUsers: [],
                assignedAdvisers: [],
                regions: [],
                localAuthorities: [],
                trusts: [],
                page: 1,
                count: 10,
                cancellationToken: CancellationToken.None
            );

            // Assert
            totalCount.Should().Be(3); // All non-deleted projects
            projects.Should().HaveCount(3);
            projects.Should().Contain(x => x.SchoolName == "School A");
            projects.Should().Contain(x => x.SchoolName == "School B");
            projects.Should().Contain(x => x.SchoolName == "School C");
            projects.Should().NotContain(x => x.SchoolName == "School D"); // Soft deleted
        }

        [Fact]
        public async Task SearchForSupportProjects_WithCaseInsensitiveSearch_ShouldReturnResults()
        {
            // Arrange
            var service = new SupportProjectRepository(fixture.Context);

            // Act 
            var (projects, totalCount) = await service.SearchForSupportProjects(
                title: "school a", // lowercase
                states: [],
                assignedUsers: [],
                assignedAdvisers: [],
                regions: [],
                localAuthorities: [],
                trusts: [],
                page: 1,
                count: 10,
                cancellationToken: CancellationToken.None
            );

            // Assert
            totalCount.Should().Be(1);
            projects.Should().HaveCount(1);
            projects.First().SchoolName.Should().Be("School A");
        }

        [Fact]
        public async Task SearchForSupportProjects_OrdersByCreatedOnDescending_ShouldReturnCorrectOrder()
        {
            // Arrange
            var service = new SupportProjectRepository(fixture.Context);

            // Act 
            var (projects, totalCount) = await service.SearchForSupportProjects(
                title: null,
                states: [],
                assignedUsers: [],
                assignedAdvisers: [],
                regions: [],
                localAuthorities: [],
                trusts: [],
                page: 1,
                count: 10,
                cancellationToken: CancellationToken.None
            );

            // Assert
            totalCount.Should().Be(3);
            projects.Should().HaveCount(3);
            // Verify ordering by CreatedOn descending
            var projectList = projects.ToList();
            for (int i = 0; i < projectList.Count - 1; i++)
            {
                projectList[i].CreatedOn.Should().BeOnOrAfter(projectList[i + 1].CreatedOn);
            }
        }
    }
}

