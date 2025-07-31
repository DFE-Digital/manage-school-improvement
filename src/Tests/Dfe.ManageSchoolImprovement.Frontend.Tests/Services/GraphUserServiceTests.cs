using AutoFixture;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services.AzureAd;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Moq;
using User = Microsoft.Graph.User;

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.Services;

public class GraphUserServiceTests
{
    private readonly Fixture _fixture = new();
    private readonly Mock<IGraphClientFactory> _mockGraphClientFactory = new();
    private readonly Mock<IOptions<AzureAdOptions>> _mockOptions = new();
    private readonly AzureAdOptions _azureAdOptions;

    public GraphUserServiceTests()
    {
        _azureAdOptions = new AzureAdOptions
        {
            GroupId = Guid.NewGuid(),
            RiseAdviserGroupId = Guid.NewGuid()
        };

        _mockOptions.Setup(x => x.Value).Returns(_azureAdOptions);
    }

    [Fact]
    public void Constructor_DoesNotThrow_WithValidParameters()
    {
        // Arrange - Setup the factory to return null since we won't actually use the client
        _mockGraphClientFactory.Setup(x => x.Create()).Returns((GraphServiceClient)null!);

        // Act & Assert - Should not throw during construction
        var exception = Record.Exception(() =>
            new GraphUserService(_mockGraphClientFactory.Object, _mockOptions.Object));

        Assert.Null(exception);
    }

    [Fact]
    public void Constructor_StoresConfiguration_Correctly()
    {
        // Arrange
        var specificGroupId = Guid.NewGuid();
        var specificRiseAdviserGroupId = Guid.NewGuid();

        var specificOptions = new AzureAdOptions
        {
            GroupId = specificGroupId,
            RiseAdviserGroupId = specificRiseAdviserGroupId
        };

        var mockSpecificOptions = new Mock<IOptions<AzureAdOptions>>();
        mockSpecificOptions.Setup(x => x.Value).Returns(specificOptions);

        _mockGraphClientFactory.Setup(x => x.Create()).Returns((GraphServiceClient)null!);

        // Act
        var service = new GraphUserService(_mockGraphClientFactory.Object, mockSpecificOptions.Object);

        // Assert
        Assert.NotNull(service);
        // Verify the options were accessed during construction
        mockSpecificOptions.Verify(x => x.Value, Times.AtLeastOnce);
    }

    [Theory]
    [InlineData("00000000-0000-0000-0000-000000000000")]
    [InlineData("12345678-1234-1234-1234-123456789012")]
    [InlineData("ffffffff-ffff-ffff-ffff-ffffffffffff")]
    public void Constructor_AcceptsValidGroupIds(string groupIdString)
    {
        // Arrange
        var groupId = Guid.Parse(groupIdString);
        var riseAdviserGroupId = Guid.NewGuid();

        var options = new AzureAdOptions
        {
            GroupId = groupId,
            RiseAdviserGroupId = riseAdviserGroupId
        };

        var mockOptions = new Mock<IOptions<AzureAdOptions>>();
        mockOptions.Setup(x => x.Value).Returns(options);

        _mockGraphClientFactory.Setup(x => x.Create()).Returns((GraphServiceClient)null!);

        // Act & Assert
        var exception = Record.Exception(() =>
            new GraphUserService(_mockGraphClientFactory.Object, mockOptions.Object));

        Assert.Null(exception);
    }

    [Fact]
    public void GraphUserService_ImplementsIGraphUserService()
    {
        // Arrange
        _mockGraphClientFactory.Setup(x => x.Create()).Returns((GraphServiceClient)null!);

        // Act
        var service = new GraphUserService(_mockGraphClientFactory.Object, _mockOptions.Object);

        // Assert
        Assert.IsAssignableFrom<IGraphUserService>(service);
    }

    [Fact]
    public void GraphUserService_HasCorrectMethodSignatures()
    {
        // Arrange
        _mockGraphClientFactory.Setup(x => x.Create()).Returns((GraphServiceClient)null!);
        var service = new GraphUserService(_mockGraphClientFactory.Object, _mockOptions.Object);

        // Act & Assert
        var getAllUsersMethod = typeof(GraphUserService).GetMethod(nameof(GraphUserService.GetAllUsers));
        var getAllRiseAdvisersMethod = typeof(GraphUserService).GetMethod(nameof(GraphUserService.GetAllRiseAdvisers));

        Assert.NotNull(getAllUsersMethod);
        Assert.NotNull(getAllRiseAdvisersMethod);

        Assert.Equal(typeof(Task<IEnumerable<User>>), getAllUsersMethod.ReturnType);
        Assert.Equal(typeof(Task<IEnumerable<User>>), getAllRiseAdvisersMethod.ReturnType);

        Assert.Empty(getAllUsersMethod.GetParameters());
        Assert.Empty(getAllRiseAdvisersMethod.GetParameters());
    }

    [Fact]
    public void Constructor_CallsGraphClientFactory_OnConstruction()
    {
        // Arrange
        _mockGraphClientFactory.Setup(x => x.Create()).Returns((GraphServiceClient)null!);

        // Act
        var service = new GraphUserService(_mockGraphClientFactory.Object, _mockOptions.Object);

        // Assert
        Assert.NotNull(service);
        _mockGraphClientFactory.Verify(x => x.Create(), Times.Once);
    }

    [Fact]
    public async Task Constructor_ThrowsException_WhenGraphClientFactoryFails()
    {
        // Arrange
        _mockGraphClientFactory.Setup(x => x.Create())
            .Throws(new InvalidOperationException("Failed to create graph client"));

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => new GraphUserService(_mockGraphClientFactory.Object, _mockOptions.Object));
        Assert.Contains("Failed to create graph client", exception.Message);
    }

    [Fact]
    public void Constructor_WithNullGraphClient_StillConstructsSuccessfully()
    {
        // Arrange - This tests the scenario where the factory returns null
        _mockGraphClientFactory.Setup(x => x.Create()).Returns((GraphServiceClient)null!);

        // Act
        var service = new GraphUserService(_mockGraphClientFactory.Object, _mockOptions.Object);

        // Assert
        Assert.NotNull(service);
        Assert.IsAssignableFrom<IGraphUserService>(service);
    }

    [Fact]
    public void Options_Configuration_IsAccessedDuringConstruction()
    {
        // Arrange
        var mockOptionsAccessor = new Mock<IOptions<AzureAdOptions>>();
        mockOptionsAccessor.Setup(x => x.Value).Returns(_azureAdOptions);
        _mockGraphClientFactory.Setup(x => x.Create()).Returns((GraphServiceClient)null!);

        // Act
        var service = new GraphUserService(_mockGraphClientFactory.Object, mockOptionsAccessor.Object);

        // Assert
        Assert.NotNull(service);
        mockOptionsAccessor.Verify(x => x.Value, Times.AtLeastOnce);
    }
}