namespace Dfe.ManageSchoolImprovement.Frontend.Models;

public class AutoCompleteSearchModel(string label, string searchQuery, string searchEndpoint, bool error = false)
{
    public string Label { get; set; } = label;

    public string SearchQuery { get; set; } = searchQuery?.Replace("'", "\\'")!;

    public string SearchEndpoint { get; set; } = searchEndpoint;
    
    public bool Error { get; set; } = error;
}
