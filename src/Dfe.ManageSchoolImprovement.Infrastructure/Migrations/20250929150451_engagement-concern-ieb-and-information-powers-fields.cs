using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.ManageSchoolImprovement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class engagementconcerniebandinformationpowersfields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InformationPowersDetails",
                schema: "RISE",
                table: "EngagementConcerns",
                type: "nvarchar(max)",
                nullable: true)
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "EngagementConcernsHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "RISE")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.AddColumn<bool>(
                name: "InformationPowersInUse",
                schema: "RISE",
                table: "EngagementConcerns",
                type: "bit",
                nullable: true)
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "EngagementConcernsHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "RISE")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.AddColumn<bool>(
                name: "InterimExecutiveBoardCreated",
                schema: "RISE",
                table: "EngagementConcerns",
                type: "bit",
                nullable: true)
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "EngagementConcernsHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "RISE")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.AddColumn<DateTime>(
                name: "InterimExecutiveBoardCreatedDate",
                schema: "RISE",
                table: "EngagementConcerns",
                type: "datetime2",
                nullable: true)
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "EngagementConcernsHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "RISE")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.AddColumn<string>(
                name: "InterimExecutiveBoardCreatedDetails",
                schema: "RISE",
                table: "EngagementConcerns",
                type: "nvarchar(max)",
                nullable: true)
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "EngagementConcernsHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "RISE")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.AddColumn<DateTime>(
                name: "PowersUsedDate",
                schema: "RISE",
                table: "EngagementConcerns",
                type: "datetime2",
                nullable: true)
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "EngagementConcernsHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "RISE")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.Sql(@"
                WITH EarliestConcerns AS (
                    SELECT 
                        Id,
                        SupportProjectId,
                        ROW_NUMBER() OVER (
                            PARTITION BY SupportProjectId 
                            ORDER BY CreatedOn ASC, Id ASC
                        ) as rn
                    FROM RISE.EngagementConcerns
                )
                UPDATE RISE.EngagementConcerns
                SET
                    EngagementConcerns.InformationPowersDetails = SupportProject.InformationPowersDetails,
                    EngagementConcerns.InformationPowersInUse = SupportProject.InformationPowersInUse,
                    EngagementConcerns.PowersUsedDate = SupportProject.PowersUsedDate,
                    EngagementConcerns.InterimExecutiveBoardCreated = SupportProject.InterimExecutiveBoardCreated,
                    EngagementConcerns.InterimExecutiveBoardCreatedDate = SupportProject.InterimExecutiveBoardCreatedDate,                 
                    EngagementConcerns.InterimExecutiveBoardCreatedDetails = SupportProject.InterimExecutiveBoardCreatedDetails

                FROM RISE.EngagementConcerns
                INNER JOIN RISE.SupportProject ON EngagementConcerns.SupportProjectId = SupportProject.Id
                INNER JOIN EarliestConcerns ON EngagementConcerns.Id = EarliestConcerns.Id
                WHERE EarliestConcerns.rn = 1;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InformationPowersDetails",
                schema: "RISE",
                table: "EngagementConcerns")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "EngagementConcernsHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "RISE")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.DropColumn(
                name: "InformationPowersInUse",
                schema: "RISE",
                table: "EngagementConcerns")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "EngagementConcernsHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "RISE")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.DropColumn(
                name: "InterimExecutiveBoardCreated",
                schema: "RISE",
                table: "EngagementConcerns")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "EngagementConcernsHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "RISE")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.DropColumn(
                name: "InterimExecutiveBoardCreatedDate",
                schema: "RISE",
                table: "EngagementConcerns")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "EngagementConcernsHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "RISE")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.DropColumn(
                name: "InterimExecutiveBoardCreatedDetails",
                schema: "RISE",
                table: "EngagementConcerns")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "EngagementConcernsHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "RISE")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.DropColumn(
                name: "PowersUsedDate",
                schema: "RISE",
                table: "EngagementConcerns")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "EngagementConcernsHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "RISE")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");
        }
    }
}
