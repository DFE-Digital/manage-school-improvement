using Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Infrastructure.Database;
using Dfe.ManageSchoolImprovement.Infrastructure.Repositories;
using Dfe.ManageSchoolImprovement.Infrastructure.Security;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Dfe.ManageSchoolImprovement.Infrastructure.Tests.Repositories
{
    public class WatchlistRepositoryTests
    {
        private static RegionalImprovementForStandardsAndExcellenceContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<RegionalImprovementForStandardsAndExcellenceContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var mockUserService = Mock.Of<IUserContextService>();
            Mock.Get(mockUserService).Setup(x => x.GetCurrentUsername()).Returns("test@user.com");

            return new RegionalImprovementForStandardsAndExcellenceContext(
                options,
                Mock.Of<IConfiguration>(),
                Mock.Of<IMediator>(),
                mockUserService);
        }

        private static SupportProject CreateSupportProject(string schoolName, string urn) =>
            SupportProject.Create(
                ProjectStatusValue.InProgress,
                new SchoolDetails
                {
                    SchoolName = schoolName,
                    SchoolUrn = urn,
                    LocalAuthority = "Authority",
                    Region = "Region"
                },
                trustDetails: null);

        [Fact]
        public async Task GetAllSchoolsForUser_WhenUserIsNull_ReturnsEmpty()
        {
            await using var context = CreateContext();
            var sut = new WatchlistRepository(context);

            var result = await sut.GetAllSchoolsForUser(null!, CancellationToken.None);

            result.Should().BeEmpty();
            result.Should().BeAssignableTo<IEnumerable<int>>();
        }

        [Fact]
        public async Task GetAllSchoolsForUser_WhenUserIsEmpty_ReturnsEmpty()
        {
            await using var context = CreateContext();
            var sut = new WatchlistRepository(context);

            var result = await sut.GetAllSchoolsForUser(string.Empty, CancellationToken.None);

            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllSchoolsForUser_WhenNoMatchingWatchlists_ReturnsEmpty()
        {
            await using var context = CreateContext();
            var project = CreateSupportProject("School X", "900001");
            context.SupportProjects.Add(project);
            await context.SaveChangesAsync();

            context.Set<Watchlist>().Add(
                new Watchlist(new WatchlistId(Guid.NewGuid()), project.Id!, "other.user@test.gov.uk"));
            await context.SaveChangesAsync();

            var sut = new WatchlistRepository(context);

            var result = await sut.GetAllSchoolsForUser("me@test.gov.uk", CancellationToken.None);

            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllSchoolsForUser_ReturnsSupportProjectIdsForThatUserOnly()
        {
            await using var context = CreateContext();
            var project1 = CreateSupportProject("School A", "900001");
            var project2 = CreateSupportProject("School B", "900002");
            context.SupportProjects.AddRange(project1, project2);
            await context.SaveChangesAsync();

            var user = "delivery.officer@test.gov.uk";
            context.Set<Watchlist>().AddRange(
                new Watchlist(new WatchlistId(Guid.NewGuid()), project1.Id!, user),
                new Watchlist(new WatchlistId(Guid.NewGuid()), project2.Id!, user));
            await context.SaveChangesAsync();

            var sut = new WatchlistRepository(context);

            var result = (await sut.GetAllSchoolsForUser(user, CancellationToken.None)).OrderBy(x => x).ToList();

            result.Should().Equal(project1.Id!.Value, project2.Id!.Value);
        }

        [Fact]
        public async Task GetAllSchoolsForUser_WhenSameSchoolWatchlistedTwice_ReturnsDistinctProjectIds()
        {
            await using var context = CreateContext();
            var project = CreateSupportProject("School A", "900001");
            context.SupportProjects.Add(project);
            await context.SaveChangesAsync();

            var user = "user@test.gov.uk";
            context.Set<Watchlist>().AddRange(
                new Watchlist(new WatchlistId(Guid.NewGuid()), project.Id!, user),
                new Watchlist(new WatchlistId(Guid.NewGuid()), project.Id!, user));
            await context.SaveChangesAsync();

            var sut = new WatchlistRepository(context);

            var result = await sut.GetAllSchoolsForUser(user, CancellationToken.None);

            result.Should().ContainSingle().Which.Should().Be(project.Id!.Value);
        }

        [Fact]
        public async Task GetAllSchoolsForUser_DoesNotReturnRowsForOtherUsers()
        {
            await using var context = CreateContext();
            var mine = CreateSupportProject("Mine", "900001");
            var theirs = CreateSupportProject("Theirs", "900002");
            context.SupportProjects.AddRange(mine, theirs);
            await context.SaveChangesAsync();

            context.Set<Watchlist>().AddRange(
                new Watchlist(new WatchlistId(Guid.NewGuid()), mine.Id!, "me@test.gov.uk"),
                new Watchlist(new WatchlistId(Guid.NewGuid()), theirs.Id!, "them@test.gov.uk"));
            await context.SaveChangesAsync();

            var sut = new WatchlistRepository(context);

            var result = (await sut.GetAllSchoolsForUser("me@test.gov.uk", CancellationToken.None)).ToList();

            result.Should().ContainSingle().Which.Should().Be(mine.Id!.Value);
        }
    }
}
