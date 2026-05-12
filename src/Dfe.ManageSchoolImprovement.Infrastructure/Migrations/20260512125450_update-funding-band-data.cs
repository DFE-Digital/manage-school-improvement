using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.ManageSchoolImprovement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatefundingbanddata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE RISE.SupportProject 
                SET FundingBand = 'Up to £100,000' 
                WHERE FundingBand = 'Up to £120,000'
                AND (Cohort = 'Autumn 25' 
                         OR Cohort = 'Spring 26' 
                         OR Cohort = 'Flow')
                ");
            
            migrationBuilder.Sql(@"
                UPDATE RISE.SupportProject 
                SET IndicativeFundingBand = 'Up to £100,000' 
                WHERE IndicativeFundingBand = 'Up to £120,000'
                AND (Cohort = 'Autumn 25' 
                         OR Cohort = 'Spring 26' 
                         OR Cohort = 'Flow')
                ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
