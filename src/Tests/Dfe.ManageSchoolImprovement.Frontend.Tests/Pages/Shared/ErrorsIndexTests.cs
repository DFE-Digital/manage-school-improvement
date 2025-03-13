using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Dfe.ManageSchoolImprovement.Frontend.Pages.Errors;
using Microsoft.AspNetCore.Routing;

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.Pages.Shared
{
    public class ErrorsIndexTests
    {
        private readonly Mock<HttpContext> _httpContextMock;
        private readonly IndexModel _model;

        public ErrorsIndexTests()
        {
            _httpContextMock = new Mock<HttpContext>();

            ActionContext actionContext = new(_httpContextMock.Object, new RouteData(), new PageActionDescriptor(), new ModelStateDictionary());
            PageContext pageContext = new(actionContext);
            _model = new IndexModel { PageContext = pageContext };
        }

        [Theory]
        [InlineData(404, "Page not found")]
        [InlineData(500, "Sorry, there is a problem with the service")]
        [InlineData(501, "Sorry, there is a problem with the service")]
        [InlineData(99999, "Sorry, there is a problem with the service")]
        public void OnGet_WhenResponseHasAStatusCode_SetsMessageCorrectly(int statusCode, string expectedMessage)
        {
            _model.OnGet(statusCode);

            Assert.Equal(_model.ErrorMessage, expectedMessage);
        }

        [Fact]
        public void OnGet_WhenUnhandledError_HasDefaultMessage()
        {
            Mock<IFeatureCollection> mockIFeatureCollection = new();
            mockIFeatureCollection.Setup(collection => collection.Get<IExceptionHandlerPathFeature>())
               .Returns(new ExceptionHandlerFeature { Error = new Exception() });
            _httpContextMock.Setup(context => context.Features).Returns(mockIFeatureCollection.Object);

            _model.OnGet();

            Assert.Equal("Sorry, there is a problem with the service", _model.ErrorMessage);
        }

        [Fact]
        public void OnGet_WhenUnhandledNoPageNamedError_SetsPageNotFoundMessageAndStatus()
        {
            Mock<IFeatureCollection> mockIFeatureCollection = new();
            mockIFeatureCollection.Setup(collection => collection.Get<IExceptionHandlerPathFeature>())
               .Returns(new ExceptionHandlerFeature { Error = new InvalidOperationException("No page named") });
            _httpContextMock.Setup(context => context.Features).Returns(mockIFeatureCollection.Object);
            _httpContextMock.SetupSet(context => context.Response.StatusCode = 404).Verifiable();

            _model.OnGet();

            Assert.Equal("Page not found", _model.ErrorMessage);
            _httpContextMock.Verify();
        }

        [Theory]
        [InlineData(404, "Page not found")]
        [InlineData(500, "Sorry, there is a problem with the service")]
        [InlineData(501, "Sorry, there is a problem with the service")]
        [InlineData(99999, "Sorry, there is a problem with the service")]
        public void OnPost_WhenResponseHasAStatusCode_SetsMessageCorrectly(int statusCode, string expectedMessage)
        {
            _model.OnPost(statusCode);

            Assert.Equal(_model.ErrorMessage, expectedMessage);
        }

        [Fact]
        public void OnPost_WhenUnhandledError_HasDefaultMessage()
        {
            Mock<IFeatureCollection> mockIFeatureCollection = new();
            mockIFeatureCollection.Setup(collection => collection.Get<IExceptionHandlerPathFeature>())
               .Returns(new ExceptionHandlerFeature { Error = new Exception() });
            _httpContextMock.Setup(context => context.Features).Returns(mockIFeatureCollection.Object);

            _model.OnPost();

            Assert.Equal("Sorry, there is a problem with the service", _model.ErrorMessage);
        }

        [Fact]
        public void OnPost_WhenUnhandledNoPageNamedError_SetsPageNotFoundMessageAndStatus()
        {
            Mock<IFeatureCollection> mockIFeatureCollection = new();
            mockIFeatureCollection.Setup(collection => collection.Get<IExceptionHandlerPathFeature>())
               .Returns(new ExceptionHandlerFeature { Error = new InvalidOperationException("No page named") });
            _httpContextMock.Setup(context => context.Features).Returns(mockIFeatureCollection.Object);
            _httpContextMock.SetupSet(context => context.Response.StatusCode = 404).Verifiable();

            _model.OnPost();

            Assert.Equal("Page not found", _model.ErrorMessage);
            _httpContextMock.Verify();
        }
    }
}
