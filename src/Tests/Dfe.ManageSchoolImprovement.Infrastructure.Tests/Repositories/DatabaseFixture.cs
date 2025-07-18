using Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject;
using Dfe.ManageSchoolImprovement.Infrastructure.Database;
using Dfe.ManageSchoolImprovement.Infrastructure.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Dfe.ManageSchoolImprovement.Infrastructure.Tests.Repositories
{
    public class DatabaseFixture : IDisposable
    {
        public RegionalImprovementForStandardsAndExcellenceContext Context { get; private set; }

        public DatabaseFixture()
        {
            // Configure in-memory database options
            var options = new DbContextOptionsBuilder<RegionalImprovementForStandardsAndExcellenceContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var mockUserService = Mock.Of<IUserContextService>();
            Mock.Get(mockUserService).Setup(x => x.GetCurrentUsername()).Returns("test@user.com");

            // Initialize the context
            Context = new RegionalImprovementForStandardsAndExcellenceContext(options, Mock.Of<IConfiguration>(), Mock.Of<IMediator>(), mockUserService);

            // Seed data
            SeedData();
        }

        private void SeedData()
        {
            Context.SupportProjects.AddRange(
                SupportProject.Create(
                    schoolName: "School A",
                    schoolUrn: "100001",
                    localAuthority: "Authority1",
                    region: "Region1",
                    trustName: "Trust A",
                    trustReferenceNumber: "TR001"
                ),
                SupportProject.Create(
                    schoolName: "School B",
                    schoolUrn: "100002",
                    localAuthority: "Authority2",
                    region: "Region2",
                    trustName: "Trust B",
                    trustReferenceNumber: "TR002"
                ),
                SupportProject.Create(
                    schoolName: "School C",
                    schoolUrn: "100003",
                    localAuthority: "Authority3",
                    region: "Region2",
                    trustName: null,
                    trustReferenceNumber: null
                ),
                SupportProject.Create(
                    schoolName: "School D",
                    schoolUrn: "100004",
                    localAuthority: "Authority5",
                    region: "Region3",
                    trustName: "Trust D",
                    trustReferenceNumber: "TR004",
                    deletedAt: DateTime.Now // This makes it a soft-deleted record
                )
            );

            Context.SaveChanges();

            // Set the delivery officer on one school to prove filter is working
            var school = Context.SupportProjects.Single(x => x.SchoolName == "School A");
            school.SetDeliveryOfficer("User1", "User1");
            school.SetAdviserDetails("Adviser@adviser.com", DateTime.Now);

            Context.Update(school);
            Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
