using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dfe.ManageSchoolImprovement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class insertwatchlistlinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "RISE",
                table: "ApplicationSettings",
                columns: ["Key", "Value", "Description", "Category", "CreatedAt", "UpdatedAt", "IsActive"],
                values: new object[,]
                {
                    { "RISEDeliveryDashboard", "#", "Link to RISE Delivery Dashboard", "BIReports", DateTime.UtcNow, DateTime.UtcNow, true },
                    { "RISEDataTables", "#", "Link to RISE Data Tables", "BIReports", DateTime.UtcNow, DateTime.UtcNow, true },
                    { "RISEMonitoringReports", "#", "Link to RISE Monitoring Reports", "BIReports", DateTime.UtcNow, DateTime.UtcNow, true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
