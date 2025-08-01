using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.Services.AzureAd;
using Microsoft.Extensions.Hosting;
using Moq;

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.Services
{
    public class UserRepositoryTests
    {
        private readonly Mock<IGraphUserService> _mockGraphUserService;
        private readonly Mock<IHostEnvironment> _mockHostingEnvironment;
        private readonly UserRepository _userRepository;

        public UserRepositoryTests()
        {
            _mockGraphUserService = new Mock<IGraphUserService>();
            _mockHostingEnvironment = new Mock<IHostEnvironment>();
            _userRepository = new UserRepository(_mockGraphUserService.Object, _mockHostingEnvironment.Object);
        }

        [Fact]
        public async Task GetAllUsers_ReturnsMappedUsers()
        {
            // Arrange
            var graphUsers = new List<Microsoft.Graph.User>
            {
                new() { Id = "1", Mail = "John.Doe@education.gov.uk", GivenName = "John", Surname = "Doe" },
                new() { Id = "2", Mail = "Jane.Smith@education.gov.uk", GivenName = "Jane", Surname = "Smith" }
            };

            _mockGraphUserService.Setup(service => service.GetAllUsers()).ReturnsAsync(graphUsers);

            // Act
            var users = await _userRepository.GetAllUsers();

            // Assert
            Assert.Equal(2, users.Count());
            Assert.Equal("1", users.First().Id);
            Assert.Equal("John.Doe@education.gov.uk", users.First().EmailAddress);
            Assert.Equal("John Doe", users.First().FullName);
            Assert.Equal("2", users.Last().Id);
            Assert.Equal("Jane.Smith@education.gov.uk", users.Last().EmailAddress);
            Assert.Equal("Jane Smith", users.Last().FullName);
        }

        [Fact]
        public async Task GetAllUsers_ReturnsEmptyList_WhenNoUsersReturned()
        {
            // Arrange
            _mockGraphUserService.Setup(service => service.GetAllUsers()).ReturnsAsync(new List<Microsoft.Graph.User>());

            // Act
            var users = await _userRepository.GetAllUsers();

            // Assert
            Assert.Empty(users);
            _mockGraphUserService.Verify(service => service.GetAllUsers(), Times.Once);
        }

        [Fact]
        public async Task GetAllUsers_CallsGraphUserServiceCorrectly()
        {
            // Arrange
            var graphUsers = new List<Microsoft.Graph.User>();
            _mockGraphUserService.Setup(service => service.GetAllUsers()).ReturnsAsync(graphUsers);

            // Act
            await _userRepository.GetAllUsers();

            // Assert
            _mockGraphUserService.Verify(service => service.GetAllUsers(), Times.Once);
        }

        [Fact]
        public async Task GetAllRiseAdvisers_ReturnsMappedUsers()
        {
            // Arrange
            var graphUsers = new List<Microsoft.Graph.User>
            {
                new() { Id = "1", Mail = "John.Doe@education.gov.uk", GivenName = "John", Surname = "Doe" },
                new() { Id = "2", Mail = "Jane.Smith@education.gov.uk", GivenName = "Jane", Surname = "Wilson" }
            };

            _mockGraphUserService.Setup(service => service.GetAllRiseAdvisers()).ReturnsAsync(graphUsers);

            // Act
            var users = await _userRepository.GetAllRiseAdvisers();

            // Assert
            Assert.Equal(2, users.Count());
            Assert.Equal("1", users.First().Id);
            Assert.Equal("John.Doe@education.gov.uk", users.First().EmailAddress);
            Assert.Equal("John Doe", users.First().FullName);
            Assert.Equal("2", users.Last().Id);
            Assert.Equal("Jane.Smith@education.gov.uk", users.Last().EmailAddress);
            Assert.Equal("Jane Wilson", users.Last().FullName);
        }

        [Fact]
        public async Task GetAllRiseAdvisers_RemovesRiseSuffixFromSurname()
        {
            // Arrange
            var graphUsers = new List<Microsoft.Graph.User>
            {
                new() { Id = "1", Mail = "John.Doe-rise@education.gov.uk", GivenName = "John", Surname = "doe-rise" },
                new() { Id = "2", Mail = "Jane.Smith-rise@education.gov.uk", GivenName = "Jane", Surname = "smith-rise" }
            };

            _mockGraphUserService.Setup(service => service.GetAllRiseAdvisers()).ReturnsAsync(graphUsers);

            // Act
            var users = await _userRepository.GetAllRiseAdvisers();

            // Assert
            Assert.Equal(2, users.Count());
            Assert.Equal("John Doe", users.First().FullName);
            Assert.Equal("Jane Smith", users.Last().FullName);
        }

        [Fact]
        public async Task GetAllRiseAdvisers_HandlesCapitalizationCorrectly()
        {
            // Arrange
            var graphUsers = new List<Microsoft.Graph.User>
            {
                new() { Id = "1", Mail = "john.doe@education.gov.uk", GivenName = "john", Surname = "doe" },
                new() { Id = "2", Mail = "jane.smith@education.gov.uk", GivenName = "jane", Surname = "smith-jones" }
            };

            _mockGraphUserService.Setup(service => service.GetAllRiseAdvisers()).ReturnsAsync(graphUsers);

            // Act
            var users = await _userRepository.GetAllRiseAdvisers();

            // Assert
            Assert.Equal(2, users.Count());
            Assert.Equal("john Doe", users.First().FullName);
            Assert.Equal("jane Smith-jones", users.Last().FullName);
        }

        [Fact]
        public async Task GetAllRiseAdvisers_ReturnsEmptyList_WhenNoUsersReturned()
        {
            // Arrange
            _mockGraphUserService.Setup(service => service.GetAllRiseAdvisers()).ReturnsAsync(new List<Microsoft.Graph.User>());

            // Act
            var users = await _userRepository.GetAllRiseAdvisers();

            // Assert
            Assert.Empty(users);
            _mockGraphUserService.Verify(service => service.GetAllRiseAdvisers(), Times.Once);
        }

        [Fact]
        public async Task GetAllRiseAdvisers_CallsGraphUserServiceCorrectly()
        {
            // Arrange
            var graphUsers = new List<Microsoft.Graph.User>();
            _mockGraphUserService.Setup(service => service.GetAllRiseAdvisers()).ReturnsAsync(graphUsers);

            // Act
            await _userRepository.GetAllRiseAdvisers();

            // Assert
            _mockGraphUserService.Verify(service => service.GetAllRiseAdvisers(), Times.Once);
        }

        [Fact]
        public async Task GetAllUsers_PropagatesException_WhenGraphServiceThrows()
        {
            // Arrange
            _mockGraphUserService.Setup(service => service.GetAllUsers())
                .ThrowsAsync(new InvalidOperationException("Graph service error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _userRepository.GetAllUsers());
            Assert.Contains("Graph service error", exception.Message);
        }

        [Fact]
        public async Task GetAllRiseAdvisers_PropagatesException_WhenGraphServiceThrows()
        {
            // Arrange
            _mockGraphUserService.Setup(service => service.GetAllRiseAdvisers())
                .ThrowsAsync(new InvalidOperationException("Graph service error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _userRepository.GetAllRiseAdvisers());
            Assert.Contains("Graph service error", exception.Message);
        }

        [Theory]
        [InlineData("john", "doe-rise", "john Doe")]
        [InlineData("jane", "smith", "jane Smith")]
        [InlineData("bob", "jones-rise-test", "bob Jones-test")]
        public async Task GetAllRiseAdvisers_HandlesDifferentNameCombinations(string givenName, string surname, string expectedFullName)
        {
            // Arrange
            var graphUsers = new List<Microsoft.Graph.User>
            {
                new() { Id = "1", Mail = "test@education.gov.uk", GivenName = givenName, Surname = surname }
            };

            _mockGraphUserService.Setup(service => service.GetAllRiseAdvisers()).ReturnsAsync(graphUsers);

            // Act
            var users = await _userRepository.GetAllRiseAdvisers();

            // Assert
            Assert.Single(users);
            Assert.Equal(expectedFullName, users.First().FullName);
        }
    }
}
