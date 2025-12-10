using Dfe.ManageSchoolImprovement.Frontend.Services;
using GovUK.Dfe.CoreLibs.Contracts.Academies.Base;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.Services
{
    public class GetLocalAuthoritiesCacheDecoratorTests
    {
        private readonly Mock<IGetLocalAuthority> _mockGetLocalAuthority;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly Mock<HttpContext> _mockHttpContext;
        private readonly Dictionary<object, object?> _httpContextItems;

        public GetLocalAuthoritiesCacheDecoratorTests()
        {
            _mockGetLocalAuthority = new Mock<IGetLocalAuthority>();
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _mockHttpContext = new Mock<HttpContext>();
            _httpContextItems = new Dictionary<object, object?>();

            // Setup HttpContext.Items to use our dictionary
            _mockHttpContext.Setup(x => x.Items).Returns(_httpContextItems);
            _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(_mockHttpContext.Object);
        }

        private GetLocalAuthoritiesCacheDecorator CreateGetLocalAuthoritiesCacheDecorator()
        {
            return new GetLocalAuthoritiesCacheDecorator(
                _mockGetLocalAuthority.Object,
                _mockHttpContextAccessor.Object);
        }

        #region GetLocalAuthorityByCode Tests

        [Fact]
        public async Task GetLocalAuthorityByCode_ReturnsFromCache_WhenDataExistsInCache()
        {
            // Arrange
            var code = "123";
            var cachedLocalAuthority = new NameAndCodeDto { Name = "Cached Authority", Code = code };
            var cacheKey = $"local-authorities-{code}";

            // Pre-populate cache
            _httpContextItems[cacheKey] = cachedLocalAuthority;

            var decorator = CreateGetLocalAuthoritiesCacheDecorator();

            // Act
            var result = await decorator.GetLocalAuthorityByCode(code);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(cachedLocalAuthority.Name, result.Name);
            Assert.Equal(cachedLocalAuthority.Code, result.Code);

            // Verify that the underlying service was not called
            _mockGetLocalAuthority.Verify(x => x.GetLocalAuthorityByCode(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task GetLocalAuthorityByCode_CallsUnderlyingServiceAndCaches_WhenNotInCache()
        {
            // Arrange
            var code = "456";
            var localAuthority = new NameAndCodeDto { Name = "New Authority", Code = code };
            var cacheKey = $"local-authorities-{code}";

            _mockGetLocalAuthority.Setup(x => x.GetLocalAuthorityByCode(code))
                .ReturnsAsync(localAuthority);

            var decorator = CreateGetLocalAuthoritiesCacheDecorator();

            // Act
            var result = await decorator.GetLocalAuthorityByCode(code);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(localAuthority.Name, result.Name);
            Assert.Equal(localAuthority.Code, result.Code);

            // Verify that the underlying service was called
            _mockGetLocalAuthority.Verify(x => x.GetLocalAuthorityByCode(code), Times.Once);

            // Verify that the result was cached
            Assert.True(_httpContextItems.ContainsKey(cacheKey));
            Assert.Equal(localAuthority, _httpContextItems[cacheKey]);
        }

        [Fact]
        public async Task GetLocalAuthorityByCode_CallsUnderlyingService_WhenCacheContainsWrongType()
        {
            // Arrange
            var code = "789";
            var localAuthority = new NameAndCodeDto { Name = "Authority", Code = code };
            var cacheKey = $"local-authorities-{code}";

            // Put wrong type in cache
            _httpContextItems[cacheKey] = "wrong type";

            _mockGetLocalAuthority.Setup(x => x.GetLocalAuthorityByCode(code))
                .ReturnsAsync(localAuthority);

            var decorator = CreateGetLocalAuthoritiesCacheDecorator();

            // Act
            var result = await decorator.GetLocalAuthorityByCode(code);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(localAuthority.Name, result.Name);

            // Verify that the underlying service was called because cache contained wrong type
            _mockGetLocalAuthority.Verify(x => x.GetLocalAuthorityByCode(code), Times.Once);

            // Verify the correct value is now cached
            Assert.Equal(localAuthority, _httpContextItems[cacheKey]);
        }

        [Fact]
        public async Task GetLocalAuthorityByCode_CallsUnderlyingService_WhenCacheContainsNull()
        {
            // Arrange
            var code = "null-test";
            var localAuthority = new NameAndCodeDto { Name = "Authority", Code = code };
            var cacheKey = $"local-authorities-{code}";

            // Put null in cache
            _httpContextItems[cacheKey] = null;

            _mockGetLocalAuthority.Setup(x => x.GetLocalAuthorityByCode(code))
                .ReturnsAsync(localAuthority);

            var decorator = CreateGetLocalAuthoritiesCacheDecorator();

            // Act
            var result = await decorator.GetLocalAuthorityByCode(code);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(localAuthority.Name, result.Name);

            // Verify that the underlying service was called because cache contained null
            _mockGetLocalAuthority.Verify(x => x.GetLocalAuthorityByCode(code), Times.Once);
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("test-code")]
        [InlineData("123")]
        [InlineData("special&chars")]
        public async Task GetLocalAuthorityByCode_WorksWithVariousCodeFormats(string code)
        {
            // Arrange
            var localAuthority = new NameAndCodeDto { Name = $"Authority for {code}", Code = code };

            _mockGetLocalAuthority.Setup(x => x.GetLocalAuthorityByCode(code))
                .ReturnsAsync(localAuthority);

            var decorator = CreateGetLocalAuthoritiesCacheDecorator();

            // Act
            var result = await decorator.GetLocalAuthorityByCode(code);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(localAuthority.Name, result.Name);
            Assert.Equal(localAuthority.Code, result.Code);

            // Verify caching works for different code formats
            var cacheKey = $"local-authorities-{code}";
            Assert.True(_httpContextItems.ContainsKey(cacheKey));
        }

        #endregion

        #region SearchLocalAuthorities Tests

        [Fact]
        public async Task SearchLocalAuthorities_ReturnsFromCache_WhenDataExistsInCache()
        {
            // Arrange
            var searchQuery = "Birmingham";
            var cachedResults = new List<NameAndCodeDto>
            {
                new() { Name = "Birmingham Council", Code = "330" },
                new() { Name = "Birmingham Education", Code = "331" }
            }.AsEnumerable();
            var cacheKey = $"local-authorities-{searchQuery}";

            // Pre-populate cache with IEnumerable<NameAndCodeDto>
            _httpContextItems[cacheKey] = cachedResults;

            var decorator = CreateGetLocalAuthoritiesCacheDecorator();

            // Act
            var result = await decorator.SearchLocalAuthorities(searchQuery);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal(cachedResults, result);

            // Verify that the underlying service was not called
            _mockGetLocalAuthority.Verify(x => x.SearchLocalAuthorities(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task SearchLocalAuthorities_CallsUnderlyingServiceAndCaches_WhenNotInCache()
        {
            // Arrange
            var searchQuery = "London";
            var searchResults = new List<NameAndCodeDto>
            {
                new() { Name = "London Borough of Camden", Code = "202" },
                new() { Name = "London Borough of Westminster", Code = "213" }
            };
            var cacheKey = $"local-authorities-{searchQuery}";

            _mockGetLocalAuthority.Setup(x => x.SearchLocalAuthorities(searchQuery))
                .ReturnsAsync(searchResults);

            var decorator = CreateGetLocalAuthoritiesCacheDecorator();

            // Act
            var result = await decorator.SearchLocalAuthorities(searchQuery);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());

            // Verify that the underlying service was called
            _mockGetLocalAuthority.Verify(x => x.SearchLocalAuthorities(searchQuery), Times.Once);

            // Verify that the Task was cached (as per implementation)
            Assert.True(_httpContextItems.ContainsKey(cacheKey));
            await Assert.IsType<Task<IEnumerable<NameAndCodeDto>>>(_httpContextItems[cacheKey]);
        }

        [Fact]
        public async Task SearchLocalAuthorities_CallsUnderlyingService_WhenCacheContainsWrongType()
        {
            // Arrange
            var searchQuery = "Manchester";
            var searchResults = new List<NameAndCodeDto>
            {
                new() { Name = "Manchester City Council", Code = "352" }
            };
            var cacheKey = $"local-authorities-{searchQuery}";

            // Put wrong type in cache
            _httpContextItems[cacheKey] = "wrong type";

            _mockGetLocalAuthority.Setup(x => x.SearchLocalAuthorities(searchQuery))
                .ReturnsAsync(searchResults);

            var decorator = CreateGetLocalAuthoritiesCacheDecorator();

            // Act
            var result = await decorator.SearchLocalAuthorities(searchQuery);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);

            // Verify that the underlying service was called because cache contained wrong type
            _mockGetLocalAuthority.Verify(x => x.SearchLocalAuthorities(searchQuery), Times.Once);
        }

        [Fact]
        public async Task SearchLocalAuthorities_ReturnsEmptyCollection_WhenUnderlyingServiceReturnsEmpty()
        {
            // Arrange
            var searchQuery = "NonExistent";
            var emptyResults = new List<NameAndCodeDto>();

            _mockGetLocalAuthority.Setup(x => x.SearchLocalAuthorities(searchQuery))
                .ReturnsAsync(emptyResults);

            var decorator = CreateGetLocalAuthoritiesCacheDecorator();

            // Act
            var result = await decorator.SearchLocalAuthorities(searchQuery);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);

            // Verify that the underlying service was called
            _mockGetLocalAuthority.Verify(x => x.SearchLocalAuthorities(searchQuery), Times.Once);
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("test query")]
        [InlineData("123")]
        [InlineData("special&chars")]
        public async Task SearchLocalAuthorities_WorksWithVariousSearchQueries(string searchQuery)
        {
            // Arrange
            var searchResults = new List<NameAndCodeDto>
            {
                new() { Name = $"Result for '{searchQuery}'", Code = "999" }
            };

            _mockGetLocalAuthority.Setup(x => x.SearchLocalAuthorities(searchQuery))
                .ReturnsAsync(searchResults);

            var decorator = CreateGetLocalAuthoritiesCacheDecorator();

            // Act
            var result = await decorator.SearchLocalAuthorities(searchQuery);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);

            // Verify caching works for different search query formats
            var cacheKey = $"local-authorities-{searchQuery}";
            Assert.True(_httpContextItems.ContainsKey(cacheKey));
        }

        #endregion

        #region Cache Behavior Tests

        [Fact]
        public async Task GetLocalAuthorityByCode_UsesSeparateCacheKeys_ForDifferentCodes()
        {
            // Arrange
            var code1 = "123";
            var code2 = "456";
            var authority1 = new NameAndCodeDto { Name = "Authority 1", Code = code1 };
            var authority2 = new NameAndCodeDto { Name = "Authority 2", Code = code2 };

            _mockGetLocalAuthority.Setup(x => x.GetLocalAuthorityByCode(code1)).ReturnsAsync(authority1);
            _mockGetLocalAuthority.Setup(x => x.GetLocalAuthorityByCode(code2)).ReturnsAsync(authority2);

            var decorator = CreateGetLocalAuthoritiesCacheDecorator();

            // Act
            var result1 = await decorator.GetLocalAuthorityByCode(code1);
            var result2 = await decorator.GetLocalAuthorityByCode(code2);

            // Assert
            Assert.Equal(authority1.Name, result1.Name);
            Assert.Equal(authority2.Name, result2.Name);

            // Verify both items are cached separately
            Assert.True(_httpContextItems.ContainsKey($"local-authorities-{code1}"));
            Assert.True(_httpContextItems.ContainsKey($"local-authorities-{code2}"));
            Assert.Equal(2, _httpContextItems.Count);
        }

        [Fact]
        public async Task SearchLocalAuthorities_UsesSeparateCacheKeys_ForDifferentQueries()
        {
            // Arrange
            var query1 = "Birmingham";
            var query2 = "London";
            var results1 = new List<NameAndCodeDto> { new() { Name = "Birmingham Council", Code = "330" } };
            var results2 = new List<NameAndCodeDto> { new() { Name = "London Council", Code = "202" } };

            _mockGetLocalAuthority.Setup(x => x.SearchLocalAuthorities(query1)).ReturnsAsync(results1);
            _mockGetLocalAuthority.Setup(x => x.SearchLocalAuthorities(query2)).ReturnsAsync(results2);

            var decorator = CreateGetLocalAuthoritiesCacheDecorator();

            // Act
            var result1 = await decorator.SearchLocalAuthorities(query1);
            var result2 = await decorator.SearchLocalAuthorities(query2);

            // Assert
            Assert.Single(result1);
            Assert.Single(result2);

            // Verify both items are cached separately
            Assert.True(_httpContextItems.ContainsKey($"local-authorities-{query1}"));
            Assert.True(_httpContextItems.ContainsKey($"local-authorities-{query2}"));
            Assert.Equal(2, _httpContextItems.Count);
        }

        [Fact]
        public async Task GetLocalAuthorityByCode_ReusesCache_OnSubsequentCalls()
        {
            // Arrange
            var code = "cached-test";
            var localAuthority = new NameAndCodeDto { Name = "Cached Authority", Code = code };

            _mockGetLocalAuthority.Setup(x => x.GetLocalAuthorityByCode(code))
                .ReturnsAsync(localAuthority);

            var decorator = CreateGetLocalAuthoritiesCacheDecorator();

            // Act - Call twice
            var result1 = await decorator.GetLocalAuthorityByCode(code);
            var result2 = await decorator.GetLocalAuthorityByCode(code);

            // Assert
            Assert.Equal(localAuthority.Name, result1.Name);
            Assert.Equal(localAuthority.Name, result2.Name);
            Assert.Same(result1, result2); // Should return same cached instance

            // Verify underlying service was only called once
            _mockGetLocalAuthority.Verify(x => x.GetLocalAuthorityByCode(code), Times.Once);
        }

        #endregion
    }
}