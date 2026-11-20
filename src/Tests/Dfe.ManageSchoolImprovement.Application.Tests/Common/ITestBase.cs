using Mediator;

namespace Dfe.ManageSchoolImprovement.Application.Tests.Common
{
    public interface ITestBase
    {
    }

    public class TestRequest : ITestBase, IMessage { }
    public class TestResponse : ITestBase
    {
    }
}