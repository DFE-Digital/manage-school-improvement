using DfE.CoreLibs.Testing.AutoFixture.Attributes;
using Dfe.RegionalImprovementForStandardsAndExcellence.Domain.Entities.Schools;
using Dfe.RegionalImprovementForStandardsAndExcellence.Domain.ValueObjects;

namespace Dfe.RegionalImprovementForStandardsAndExcellence.Domain.Tests.Aggregates
{
    public class PrincipalDetailsTests
    {
        [Theory]
        [CustomAutoData]
        public void Constructor_ShouldThrowArgumentNullException_WhenPrincipalIdIsNull(
            int typeId,
            string? email,
            string? phone)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new PrincipalDetails(null!, typeId, email, phone));

            Assert.Equal("principalId", exception.ParamName);
        }

        [Theory]
        [CustomAutoData]
        public void Constructor_ShouldThrowArgumentException_WhenTypeIdIsNotPositive(
            PrincipalId principalId,
            string? email,
            string? phone)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                new PrincipalDetails(principalId, -1, email, phone));

            Assert.Contains("TypeId must be positive", exception.Message);
            Assert.Equal("typeId", exception.ParamName);
        }

        [Theory]
        [CustomAutoData]
        public void Constructor_ShouldThrowArgumentException_WhenTypeIdIsZero(
            PrincipalId principalId,
            string? email,
            string? phone)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                new PrincipalDetails(principalId, 0, email, phone));

            Assert.Contains("TypeId must be positive", exception.Message);
            Assert.Equal("typeId", exception.ParamName);
        }
    }
}
