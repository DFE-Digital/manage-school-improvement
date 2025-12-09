using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics.CodeAnalysis;

namespace Dfe.ManageSchoolImprovement.Frontend.Routing
{
    [ExcludeFromCodeCoverage]
    public class MaintenancePageFilter : IAsyncPageFilter
    {
        private readonly IConfiguration _config;

        public MaintenancePageFilter(IConfiguration config)
        {
            _config = config;
        }

        public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {
            return Task.CompletedTask;
        }

        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context,
                                                      PageHandlerExecutionDelegate next)
        {
            string? maintenanceModeValue = _config["MaintenanceMode"];

            if (bool.TryParse(maintenanceModeValue, out bool maintenanceMode) && maintenanceMode &&
                !string.IsNullOrEmpty(context.ActionDescriptor.DisplayName) &&
                !context.ActionDescriptor.DisplayName.Contains("Maintenance"))
            {
                context.Result = new RedirectToPageResult("/public/maintenance");
                return;
            }

            // Do post work.
            await next.Invoke();
        }
    }
}
