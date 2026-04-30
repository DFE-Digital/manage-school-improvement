using Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;

namespace Dfe.ManageSchoolImprovement.Domain.Tests.Entities.SupportProject
{
    public class WatchlistTests
    {
        [Fact]
        public void Constructor_WithValidParameters_SetsIdSupportProjectIdAndUser()
        {
            var id = new WatchlistId(Guid.NewGuid());
            var supportProjectId = new SupportProjectId(42);
            var user = "user@example.com";

            var watchlist = new Watchlist(id, supportProjectId, user);

            Assert.Equal(id, watchlist.Id);
            Assert.Equal(supportProjectId, watchlist.SupportProjectId);
            Assert.Equal(user, watchlist.User);
        }

        [Fact]
        public void Constructor_DoesNotSetReadableId_ToSupportDatabaseGeneratedValue()
        {
            var watchlist = new Watchlist(new WatchlistId(Guid.NewGuid()), new SupportProjectId(1), "user@test.gov.uk");

            Assert.Equal(0, watchlist.ReadableId);
        }

        [Fact]
        public void Constructor_CreatedByDefaultsToEmptyString()
        {
            var watchlist = new Watchlist(new WatchlistId(Guid.NewGuid()), new SupportProjectId(1), "user@test.gov.uk");

            Assert.Equal(string.Empty, watchlist.CreatedBy);
        }

        [Fact]
        public void Properties_WhenAssigned_PersistAuditAndIdentityFields()
        {
            var watchlist = new Watchlist(new WatchlistId(Guid.NewGuid()), new SupportProjectId(1), "original@test.gov.uk");
            var newId = new WatchlistId(Guid.NewGuid());
            var newSupportProjectId = new SupportProjectId(99);
            var createdOn = DateTime.UtcNow;
            var lastModifiedOn = createdOn.AddHours(2);

            watchlist.Id = newId;
            watchlist.SupportProjectId = newSupportProjectId;
            watchlist.User = "updated@test.gov.uk";
            watchlist.CreatedOn = createdOn;
            watchlist.CreatedBy = "creator@test.gov.uk";
            watchlist.LastModifiedOn = lastModifiedOn;
            watchlist.LastModifiedBy = "modifier@test.gov.uk";

            Assert.Equal(newId, watchlist.Id);
            Assert.Equal(newSupportProjectId, watchlist.SupportProjectId);
            Assert.Equal("updated@test.gov.uk", watchlist.User);
            Assert.Equal(createdOn, watchlist.CreatedOn);
            Assert.Equal("creator@test.gov.uk", watchlist.CreatedBy);
            Assert.Equal(lastModifiedOn, watchlist.LastModifiedOn);
            Assert.Equal("modifier@test.gov.uk", watchlist.LastModifiedBy);
        }

        [Fact]
        public void User_CanBeSetToNull()
        {
            var watchlist = new Watchlist(new WatchlistId(Guid.NewGuid()), new SupportProjectId(1), "user@test.gov.uk");

            watchlist.User = null;

            Assert.Null(watchlist.User);
        }

        [Fact]
        public void LastModifiedOnAndLastModifiedBy_CanBeSetToNull()
        {
            var watchlist = new Watchlist(new WatchlistId(Guid.NewGuid()), new SupportProjectId(1), "user@test.gov.uk");
            watchlist.LastModifiedOn = DateTime.UtcNow;
            watchlist.LastModifiedBy = "someone";

            watchlist.LastModifiedOn = null;
            watchlist.LastModifiedBy = null;

            Assert.Null(watchlist.LastModifiedOn);
            Assert.Null(watchlist.LastModifiedBy);
        }
    }
}
