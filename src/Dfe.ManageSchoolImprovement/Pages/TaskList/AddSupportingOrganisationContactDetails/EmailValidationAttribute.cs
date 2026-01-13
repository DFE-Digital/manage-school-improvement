using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.TaskList.AddSupportingOrganisationContactDetails;

[AttributeUsage(AttributeTargets.Property)]
public class EmailValidationAttribute : ValidationAttribute
{
    private static readonly Regex EmailFormatRegex = new(@"^[^@\s]+@[A-Za-z0-9-]+(\.[A-Za-z0-9-]+)*\.[A-Za-z]{2,}$", RegexOptions.Compiled, TimeSpan.FromMilliseconds(100));

    // Constructor that sets the default error message
    public EmailValidationAttribute() : base("Email address must be in the correct format")
    {
    }

    // Override the simple IsValid method (returns bool)
    public override bool IsValid(object? value)
    {
        // Return true for null or non-string values (let other attributes handle required validation)
        if (value == null || value is not string email)
        {
            return true;
        }

        // Validate the email format
        return EmailFormatRegex.IsMatch(email.Trim());
    }

    // Override the complex IsValid method (returns ValidationResult)
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is string email && !EmailFormatRegex.IsMatch(email.Trim()))
        {
            // Use FormatErrorMessage to get the properly formatted error message
            // This will use the ErrorMessage property if it was set, or the default message
            var errorMessage = FormatErrorMessage(validationContext.DisplayName);
            return new ValidationResult(errorMessage, new[] { validationContext.MemberName ?? string.Empty });
        }

        return ValidationResult.Success;
    }
}