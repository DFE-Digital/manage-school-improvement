using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.ManageSchoolImprovement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddApplicationSettingsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationSettings",
                schema: "RISE",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: "General"),
                    IsEncrypted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationSettings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationSettings_Category",
                schema: "RISE",
                table: "ApplicationSettings",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationSettings_Key",
                schema: "RISE",
                table: "ApplicationSettings",
                column: "Key",
                unique: true);

            // Insert initial SharePoint settings data
            migrationBuilder.InsertData(
                schema: "RISE",
                table: "ApplicationSettings",
                columns: ["Key", "Value", "Description", "Category", "IsEncrypted", "CreatedAt", "UpdatedAt", "IsActive"],
                values: new object[,]
                {
                    { "AssessmentToolOneLink", "#", "Link to Assessment Tool One", "SharePointResources", false, DateTime.UtcNow, DateTime.UtcNow, true },
                    { "AssessmentToolTwoLink", "#", "Link to Assessment Tool Two", "SharePointResources", false, DateTime.UtcNow, DateTime.UtcNow, true },
                    { "AssessmentToolTwoSharePointFolderLink", "#", "SharePoint folder link for Assessment Tool Two", "SharePointResources", false, DateTime.UtcNow, DateTime.UtcNow, true },
                    { "AssessmentToolThreeLink", "#", "Link to Assessment Tool Three", "SharePointResources", false, DateTime.UtcNow, DateTime.UtcNow, true },
                    { "ImprovementPlanTemplateLink", "#", "Link to Improvement Plan Template", "SharePointResources", false , DateTime.UtcNow, DateTime.UtcNow, true},
                    { "EnrolmentLetterTemplate", "#", "Link to Enrolment Letter Template", "SharePointResources", false, DateTime.UtcNow, DateTime.UtcNow, true },
                    { "ConfirmFundingBandLink", "#", "Link to confirm funding band", "SharePointResources", false, DateTime.UtcNow, DateTime.UtcNow, true },
                    { "FundingBandGuidanceLink", "#", "Link to funding band guidance", "SharePointResources", false , DateTime.UtcNow, DateTime.UtcNow, true},
                    { "TargetedInterventionGuidanceLink", "#", "Link to targeted intervention guidance", "SharePointResources", false , DateTime.UtcNow, DateTime.UtcNow, true},
                    { "IEBGuidanceLink", "#", "Link to Interim Executive Boards (IEB) guidance", "SharePointResources", false, DateTime.UtcNow, DateTime.UtcNow, true },
                    { "SOPUCommissioningForm", "#", "Link to SOPU Commissioning Form", "SharePointResources", false, DateTime.UtcNow, DateTime.UtcNow, true },
                    { "PreviousFundingChecksSpreadsheetLink", "#", "Link to Previous Funding Checks Spreadsheet", "SharePointResources", false, DateTime.UtcNow, DateTime.UtcNow, true },
                    { "CheckSupportingOrganisationVendorAccountLink", "#", "Link to check supporting organisation vendor account", "SharePointResources", false, DateTime.UtcNow, DateTime.UtcNow, true },
                    { "SFSOCommissioningFormLink", "#", "Link to SFSO commissioning form", "SharePointResources", false, DateTime.UtcNow, DateTime.UtcNow, true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationSettings",
                schema: "RISE");
        }
    }
}
