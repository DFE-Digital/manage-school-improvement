using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.ManageSchoolImprovement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class engagementconcerntable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EngagementConcerns",
                schema: "RISE",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "EngagementConcernsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "RISE")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    SupportProjectId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "EngagementConcernsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "RISE")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "EngagementConcernsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "RISE")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "EngagementConcernsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "RISE")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "EngagementConcernsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "RISE")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "EngagementConcernsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "RISE")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    EngagementConcernRecorded = table.Column<bool>(type: "bit", nullable: true)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "EngagementConcernsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "RISE")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    EngagementConcernDetails = table.Column<string>(type: "nvarchar(max)", nullable: true)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "EngagementConcernsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "RISE")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    EngagementConcernRaisedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "EngagementConcernsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "RISE")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    EngagementConcernResolved = table.Column<bool>(type: "bit", nullable: true)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "EngagementConcernsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "RISE")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    EngagementConcernResolvedDetails = table.Column<string>(type: "nvarchar(max)", nullable: true)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "EngagementConcernsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "RISE")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    EngagementConcernResolvedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "EngagementConcernsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "RISE")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    EngagementConcernEscalationConfirmStepsTaken = table.Column<bool>(type: "bit", nullable: true)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "EngagementConcernsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "RISE")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    EngagementConcernEscalationPrimaryReason = table.Column<string>(type: "nvarchar(max)", nullable: true)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "EngagementConcernsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "RISE")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    EngagementConcernEscalationDetails = table.Column<string>(type: "nvarchar(max)", nullable: true)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "EngagementConcernsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "RISE")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    EngagementConcernEscalationDateOfDecision = table.Column<DateTime>(type: "datetime2", nullable: true)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "EngagementConcernsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "RISE")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    EngagementConcernEscalationWarningNotice = table.Column<string>(type: "nvarchar(max)", nullable: true)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "EngagementConcernsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "RISE")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "EngagementConcernsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "RISE")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart"),
                    PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false)
                        .Annotation("SqlServer:IsTemporal", true)
                        .Annotation("SqlServer:TemporalHistoryTableName", "EngagementConcernsHistory")
                        .Annotation("SqlServer:TemporalHistoryTableSchema", "RISE")
                        .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                        .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EngagementConcerns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EngagementConcerns_SupportProject_SupportProjectId",
                        column: x => x.SupportProjectId,
                        principalSchema: "RISE",
                        principalTable: "SupportProject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "EngagementConcernsHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "RISE")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.CreateIndex(
                name: "IX_EngagementConcerns_SupportProjectId",
                schema: "RISE",
                table: "EngagementConcerns",
                column: "SupportProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EngagementConcerns",
                schema: "RISE")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "EngagementConcernsHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "RISE")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");
        }
    }
}
