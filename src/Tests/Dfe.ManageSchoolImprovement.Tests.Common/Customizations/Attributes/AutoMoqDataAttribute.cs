using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace Dfe.ManageSchoolImprovement.Tests.Common.Customizations.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute()
           : base(() => new Fixture().Customize(new CompositeCustomization(new AutoMoqCustomization())))
        {
        }
    }
}
