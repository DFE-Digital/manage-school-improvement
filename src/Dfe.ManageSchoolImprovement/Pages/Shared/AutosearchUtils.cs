using Dfe.ManageSchoolImprovement.Application.SupportProject.Models;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using GovUK.Dfe.CoreLibs.Contracts.Academies.Base;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.Shared;

public static class AutosearchUtils
{
    private static readonly char[] BracketSplitSeparators = ['(', ')'];

    public static string[] SplitOnBrackets(string input)
    {
        // return array containing one empty string if input string is null or empty
        if (string.IsNullOrWhiteSpace(input)) return new string[1] { string.Empty };

        return input.Split(BracketSplitSeparators, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
    }

    public static string HighlightSearchMatch(string input, string toReplace, EstablishmentSearchResponse school)
    {
        if (school == null || string.IsNullOrWhiteSpace(school.Name))
            return string.Empty;

        if (string.IsNullOrWhiteSpace(school.Urn) && string.IsNullOrWhiteSpace(school.Ukprn))
            return string.Empty;

        return HighlightMatchedSubstring(input, toReplace);
    }

    public static string HighlightSearchMatch(string input, string toReplace, SupportProjectDto project)
    {
        if (project == null || string.IsNullOrWhiteSpace(project.SchoolName))
            return string.Empty;

        if (string.IsNullOrWhiteSpace(project.SchoolUrn))
            return string.Empty;

        return HighlightMatchedSubstring(input, toReplace);
    }

    public static string HighlightSearchMatch(string input, string toReplace, TrustSearchResponse trust)
    {
        if (trust == null || string.IsNullOrWhiteSpace(trust.Ukprn) || string.IsNullOrWhiteSpace(trust.Name))
            return string.Empty;

        return HighlightMatchedSubstring(input, toReplace);
    }

    public static string HighlightSearchMatch(string input, string toReplace, NameAndCodeDto localAuthority)
    {
        if (localAuthority == null || string.IsNullOrWhiteSpace(localAuthority.Code) ||
            string.IsNullOrWhiteSpace(localAuthority.Name))
            return string.Empty;

        return HighlightMatchedSubstring(input, toReplace);
    }

    private static string HighlightMatchedSubstring(string input, string toReplace)
    {
        if (string.IsNullOrWhiteSpace(toReplace))
            return input;

        int index = input.IndexOf(toReplace, StringComparison.InvariantCultureIgnoreCase);
        if (index < 0)
            return input;

        string correctCaseSearchString = input.Substring(index, toReplace.Length);

        return input.Replace(toReplace, $"<strong>{correctCaseSearchString}</strong>",
            StringComparison.InvariantCultureIgnoreCase);
    }
}
