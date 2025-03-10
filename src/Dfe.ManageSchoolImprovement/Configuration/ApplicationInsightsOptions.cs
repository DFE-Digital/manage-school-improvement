namespace Dfe.ManageSchoolImprovement.Frontend.Configuration;

public class ApplicationInsightsOptions
{
    public const string ConfigurationSection = "ApplicationInsights";
    public string ConnectionString { get; init; } = string.Empty;
    public string InstrumentationKey { get; init; } = string.Empty;
    public string EnableBrowserAnalytics { get; init; } = string.Empty;
}
