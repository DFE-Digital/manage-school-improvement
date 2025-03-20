using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.Contacts
{
    public class PhoneValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return ValidationResult.Success;
            }

            string phoneNumber = value.ToString()!;

            string mobilePattern = @"^(\+44\s?|0)7\d{9}$";

            string landlinePattern = @"^(\+44\s?|0)(1\d{8,9}|2\d{8,9}|3\d{8,9}|5\d{8,9})$";

            if (Regex.IsMatch(phoneNumber!, mobilePattern) || Regex.IsMatch(phoneNumber!, landlinePattern))
            {
                return ValidationResult.Success!;
            }

            return new ValidationResult("Enter a valid phone number");
        }
    }
}
