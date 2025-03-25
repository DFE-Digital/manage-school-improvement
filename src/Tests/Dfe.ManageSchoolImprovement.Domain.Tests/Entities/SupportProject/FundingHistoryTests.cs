using Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;

namespace Dfe.ManageSchoolImprovement.Domain.Tests.Entities.SupportProject
{
    public class FundingHistoryTests
    {
        [Fact]
        public void SetValues_ShouldUpdateFundingHistoryAndModifyProperties()
        {
            // Arrange
            var id = new FundingHistoryId(Guid.NewGuid());
            var fundingType = "funding type";
            var fundingAmount = (decimal)100.10;
            var financialYear = "financial year";
            var fundingRounds = 10;
            var comments = "comments";
            var supportProjectId = new SupportProjectId(1);

            var fundingHistory = new FundingHistory(id, supportProjectId, fundingType, fundingAmount, financialYear, fundingRounds, comments);

            // Act
            fundingType = "funding type 1";
            fundingAmount = (decimal)111.10;
            financialYear = "financial year 1";
            fundingRounds = 11;
            comments = "comments 1";

            fundingHistory.SetValues(fundingType, fundingAmount, financialYear, fundingRounds, comments);

            // Assert
            Assert.Equal(fundingHistory.FundingType, fundingType);
            Assert.Equal(fundingHistory.FundingAmount, fundingAmount);
            Assert.Equal(fundingHistory.FinancialYear, financialYear);
            Assert.Equal(fundingHistory.FundingRounds, fundingRounds);
            Assert.Equal(fundingHistory.Comments, comments);
        }
    }

}
