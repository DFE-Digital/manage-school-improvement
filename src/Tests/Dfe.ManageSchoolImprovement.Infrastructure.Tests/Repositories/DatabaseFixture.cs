using Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
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
            Context = new RegionalImprovementForStandardsAndExcellenceContext(options, Mock.Of<IConfiguration>(),
                Mock.Of<IMediator>(), mockUserService);

            // Seed data
            SeedData();
        }

        private void SeedData()
        {
            Context.SupportProjects.AddRange(
                SupportProject.Create(
                    projectStatus: ProjectStatus.InProgress,
                    new SchoolDetails
                    {
                        SchoolName = "School A",
                        SchoolUrn = "100001",
                        LocalAuthority = "Authority1",
                        Region = "Region1"
                    },
                    trustDetails: new TrustDetails()
                    {
                        TrustName = "Trust A",
                        TrustReferenceNumber = "TR001"
                    }
                ),
                SupportProject.Create(
                    projectStatus: ProjectStatus.Paused,
                    new SchoolDetails
                    {
                        SchoolName = "School B",
                        SchoolUrn = "100002",
                        LocalAuthority = "Authority2",
                        Region = "Region2"
                    },
                    trustDetails: new TrustDetails()
                    {
                        TrustName = "Trust B",
                        TrustReferenceNumber = "TR002"
                    }
                ),
                SupportProject.Create(
                    projectStatus: ProjectStatus.Stopped,
                    new SchoolDetails
                    {
                        SchoolName = "School C",
                        SchoolUrn = "100003",
                        LocalAuthority = "Authority3",
                        Region = "Region2"
                    },
                    trustDetails: null
                ),

                SupportProject.Create(
                    projectStatus: ProjectStatus.InProgress,
                    new SchoolDetails
                    {
                        SchoolName = "School D",
                        SchoolUrn = "100004",
                        LocalAuthority = "Authority5",
                        Region = "Region3"
                    },
                    trustDetails: new TrustDetails()
                    {
                        TrustName = "Trust D",
                        TrustReferenceNumber = "TR004"
                    },
                    deletedAt: DateTime.Now // This makes it a soft-deleted record
                )
            );

            Context.SaveChanges();

            // Set the delivery officer on one school to prove filter is working
            var school = Context.SupportProjects.Single(x => x.SchoolName == "School A");
            school.SetDeliveryOfficer("User1", "User1");
            school.SetAdviserDetails("Adviser@adviser.com", DateTime.Now, "Test Adviser");

            school.CreatedOn = new DateTime(2025, 10, 1);

            Context.Update(school);
            Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}