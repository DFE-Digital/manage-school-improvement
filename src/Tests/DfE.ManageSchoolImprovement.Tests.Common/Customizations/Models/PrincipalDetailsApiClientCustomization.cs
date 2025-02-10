using AutoFixture;

namespace DfE.ManageSchoolImprovement.Tests.Common.Customizations.Models
{
    public class PrincipalDetailsApiClientCustomization : ICustomization
    {
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public int? TypeId { get; set; }

        public void Customize(IFixture fixture)
        {
            //fixture.Customize<PrincipalDetailsModel>(composer => composer
            //    .With(x => x.Email, Email ?? fixture.Create<MailAddress>().Address)
            //    .With(x => x.Phone, Phone ?? fixture.Create<int>().ToString())
            //    .With(x => x.TypeId, TypeId ?? fixture.Create<int>()));
        }
    }
}
