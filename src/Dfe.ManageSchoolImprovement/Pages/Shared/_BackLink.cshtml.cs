namespace Dfe.ManageSchoolImprovement.Frontend.Pages.Shared;

public class BackLink(string linkPage, Dictionary<string, string> linkRouteValues, string linkText = "Back")
{
    public string LinkPage { get; set; } = linkPage;
    public Dictionary<string, string> LinkRouteValues { get; set; } = linkRouteValues;
    public string LinkText { get; set; } = linkText;
}
