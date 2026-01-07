using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.ManageSchoolImprovement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addcontactorgtype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrganisationType",
                schema: "RISE",
                table: "SupportProjectContacts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SupportProjectContactsHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "RISE")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.AddColumn<string>(
                name: "OrganisationTypeSubCategory",
                schema: "RISE",
                table: "SupportProjectContacts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SupportProjectContactsHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "RISE")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.AddColumn<string>(
                name: "OrganisationTypeSubCategoryOther",
                schema: "RISE",
                table: "SupportProjectContacts",
                type: "nvarchar(max)",
                nullable: true)
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SupportProjectContactsHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "RISE")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrganisationType",
                schema: "RISE",
                table: "SupportProjectContacts")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SupportProjectContactsHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "RISE")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.DropColumn(
                name: "OrganisationTypeSubCategory",
                schema: "RISE",
                table: "SupportProjectContacts")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SupportProjectContactsHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "RISE")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");

            migrationBuilder.DropColumn(
                name: "OrganisationTypeSubCategoryOther",
                schema: "RISE",
                table: "SupportProjectContacts")
                .Annotation("SqlServer:IsTemporal", true)
                .Annotation("SqlServer:TemporalHistoryTableName", "SupportProjectContactsHistory")
                .Annotation("SqlServer:TemporalHistoryTableSchema", "RISE")
                .Annotation("SqlServer:TemporalPeriodEndColumnName", "PeriodEnd")
                .Annotation("SqlServer:TemporalPeriodStartColumnName", "PeriodStart");
        }
    }
}
