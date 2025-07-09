using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.AllocateAdviser;

public partial class RiseAdviserEmailAttribute : ValidationAttribute
{
    private const string EmailPattern = @"^[a-zA-Z]+(?:-[a-zA-Z]+)?\.[a-zA-Z]+(?:-[a-zA-Z]+)?-rise@education\.gov\.uk$"; 

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value is string email && ValidationRegex().IsMatch(email))
        {
            return ValidationResult.Success!;
        }

        return new ValidationResult("Email must be in the format: firstname.lastname-rise@education.gov.uk");
    }

    [GeneratedRegex(EmailPattern, RegexOptions.IgnoreCase, "en-GB")]
    private static partial Regex ValidationRegex();
}
