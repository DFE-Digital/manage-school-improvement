using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.ManageSchoolImprovement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class renamerecordschoolresponsefield : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HasAcceptedTargetedSupport",
                schema: "RISE",
                table: "SupportProject",
                newName: "HasAcknowledgedAndWillEngage")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SupportProjectHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "RISE")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HasAcknowledgedAndWillEngage",
                schema: "RISE",
                table: "SupportProject",
                newName: "HasAcceptedTargetedSupport")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SupportProjectHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "RISE")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");
        }
    }
}
