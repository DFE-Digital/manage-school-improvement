using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.AddSupportingOrganisationContactDetails;

[AttributeUsage(AttributeTargets.Property)]
public class PostcodeValidationAttribute : ValidationAttribute
{
    private static readonly Regex PostcodeRegex = new(
        @"^(GIR\s?0AA|                
        (?:[A-PR-UWYZ][0-9][0-9]?     
        |[A-PR-UWYZ][A-HK-Y][0-9][0-9]? 
        |[A-PR-UWYZ][0-9][A-HJKPSTUW]  
        |[A-PR-UWYZ][A-HK-Y][0-9][ABEHMNPRVWXY]) 
        \s?[0-9][ABD-HJLNP-UW-Z]{2})$",
        RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
    
    public PostcodeValidationAttribute() : base("Postcode must be in the correct format")
    {
    }
    
    public override bool IsValid(object? value)
    {
        if (value is not string postcode)
        {
            return true;
        }
        
        return PostcodeRegex.IsMatch(postcode.Trim());
    }
    
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is string postcode && !PostcodeRegex.IsMatch(postcode.Trim()))
        {
            var errorMessage = FormatErrorMessage(validationContext.DisplayName);
            return new ValidationResult(errorMessage, new[] { validationContext.MemberName ?? string.Empty });
        }

        return ValidationResult.Success;
    }
}