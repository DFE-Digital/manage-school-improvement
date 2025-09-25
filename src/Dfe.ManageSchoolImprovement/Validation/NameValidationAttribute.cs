using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Dfe.ManageSchoolImprovement.Frontend.Validation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public partial class NameValidationAttribute : ValidationAttribute
{
    // Regex pattern for proper capitalization of first and last names, including double-barrelled names
    private static readonly Regex NameRegex = GeneratedRegex();

    public override bool IsValid(object? value)
    {
        // If the value is null or empty, consider it valid
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            return true;

        string name = (string)value;

        // Check if the name matches the regex
        return NameRegex.IsMatch(name.Trim());
    }

    public override string FormatErrorMessage(string name)
    {
        return "First and last name must start with capital letters and be followed by lowercase letters (e.g., John Smith)";
    }

    [GeneratedRegex(@"^[A-Z][a-z]+(-[A-Z][a-z]+)* [A-Z][a-z]+(-[A-Z][a-z]+)*$", RegexOptions.Compiled)]
    private static partial Regex GeneratedRegex();
}
