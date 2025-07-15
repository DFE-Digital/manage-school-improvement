using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.ManageSchoolImprovement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initialdiagnosismatchingdecision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InitialDiagnosisMatchingDecision",
                schema: "RISE",
                table: "SupportProject",
                type: "nvarchar(max)",
                nullable: true)
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SupportProjectHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "RISE")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.AddColumn<string>(
                name: "InitialDiagnosisMatchingDecisionNotes",
                schema: "RISE",
                table: "SupportProject",
                type: "nvarchar(max)",
                nullable: true)
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SupportProjectHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "RISE")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");


            // Migrate data from old column boolean values to new column string values
            migrationBuilder.Sql(@"
                UPDATE RISE.SupportProject
                SET InitialDiagnosisMatchingDecision = CASE 
                    WHEN HasSchoolMatchedWithSupportingOrganisation = 1 THEN 'Match with a supporting organisation'
                    WHEN HasSchoolMatchedWithSupportingOrganisation = 0 THEN 'Review school''s progress'
                    ELSE NULL
                END
            ");

            // Migrate Notes
            migrationBuilder.Sql(@"
                UPDATE RISE.SupportProject
                SET InitialDiagnosisMatchingDecisionNotes = NotMatchingSchoolWithSupportingOrgNotes
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InitialDiagnosisMatchingDecision",
                schema: "RISE",
                table: "SupportProject")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SupportProjectHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "RISE")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.DropColumn(
                name: "InitialDiagnosisMatchingDecisionNotes",
                schema: "RISE",
                table: "SupportProject")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SupportProjectHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "RISE")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");
        }
    }
}
