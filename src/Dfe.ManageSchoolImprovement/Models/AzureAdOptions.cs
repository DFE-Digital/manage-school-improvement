using System.Globalization;

namespace Dfe.ManageSchoolImprovement.Frontend.Models;

public class AzureAdOptions
{
    public Guid ClientId { get; set; }
    public string ClientSecret { get; set; } = string.Empty;
    public Guid TenantId { get; set; }
    public Guid GroupId { get; set; }
    public Guid RiseAdviserGroupId { get; set; }
    public string ApiUrl { get; set; } = "https://graph.microsoft.com/";
    public string Authority => string.Format(CultureInfo.InvariantCulture, "https://login.microsoftonline.com/{0}", TenantId);
    public IEnumerable<string> Scopes => [$"{ApiUrl}.default"];
}
