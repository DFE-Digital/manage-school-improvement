using AutoFixture;
using Dfe.RegionalImprovementForStandardsAndExcellence.Domain.Entities.Schools;
using Dfe.RegionalImprovementForStandardsAndExcellence.Domain.ValueObjects;

namespace Dfe.RegionalImprovementForStandardsAndExcellence.Tests.Common.Customizations.Entities
{
    public class SchoolCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<School>(composer => composer.FromFactory(() =>
            {
                var constituencyId = fixture.Create<SchoolId>();
                var principalId = fixture.Create<PrincipalId>();
                var nameDetails = new NameDetails(
                    "Doe, John",
                    "John Doe",
                    "Mr. John Doe MP"
                );

                return new School(
                    constituencyId,
                    principalId,
                    fixture.Create<string>(),
                    nameDetails,
                    fixture.Create<DateTime>(),
                    DateOnly.FromDateTime(fixture.Create<DateTime>().Date),
                    fixture.Create<PrincipalDetails>()
                );
            }));
        }
    }
}
