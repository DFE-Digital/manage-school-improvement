using Dfe.ManageSchoolImprovement.Frontend.Models.SupportProject;

namespace Dfe.ManageSchoolImprovement.Frontend.Tests.Models
{
    public class FundingHistoryViewModelTests
    {
        [Fact]
        public void FundingHistoryViewModel_ShouldHaveDefaultValues()
        {
            // Arrange & Act
            var model = new FundingHistoryViewModel();

            // Assert
            Assert.Equal(Guid.Empty, model.Id);
            Assert.Equal(0, model.SupportProjectId);
            Assert.Equal(0, model.ReadableId);
            Assert.Equal(string.Empty, model.FundingType);
            Assert.Equal(0, model.FundingAmount);
            Assert.Equal(string.Empty, model.FinancialYear);
            Assert.Equal(0, model.FundingRounds);
            Assert.Equal(string.Empty, model.Comments);
        }

        [Fact]
        public void FundingHistoryViewModel_ShouldSetAndGetValues()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var model = new FundingHistoryViewModel()
            {
                Id = guid,
                SupportProjectId = 456,
                ReadableId = 789,
                FundingType = "Type of funding",
                FundingAmount = 100000,
                FinancialYear = "2025/6",
                FundingRounds = 1,
                Comments = "This is a comment"
            };

            // Act & Assert
            Assert.Equal(guid, model.Id);
            Assert.Equal(456, model.SupportProjectId);
            Assert.Equal(789, model.ReadableId);
            Assert.Equal("Type of funding", model.FundingType);
            Assert.Equal(100000, model.FundingAmount);
            Assert.Equal("2025/6", model.FinancialYear);
            Assert.Equal(1, model.FundingRounds);
            Assert.Equal("This is a comment", model.Comments);
        }
    }
}
