using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.ManageSchoolImprovement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class renameformalnotificationsentfields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmailTheResponsibleBody",
                schema: "RISE",
                table: "SupportProject",
                newName: "FormalNotificationSent")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SupportProjectHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "RISE")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.RenameColumn(
                name: "ContactedTheResponsibleBodyDate",
                schema: "RISE",
                table: "SupportProject",
                newName: "DateFormalNotificationSent")
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
                name: "FormalNotificationSent",
                schema: "RISE",
                table: "SupportProject",
                newName: "EmailTheResponsibleBody")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SupportProjectHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "RISE")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.RenameColumn(
                name: "DateFormalNotificationSent",
                schema: "RISE",
                table: "SupportProject",
                newName: "ContactedTheResponsibleBodyDate")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SupportProjectHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "RISE")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");
        }
    }
}
