using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using Dfe.ManageSchoolImprovement.Utils;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.Contacts
{
    public static class ContactsUtil
    {
        // Configuration records for different radio button types
        private sealed record RadioButtonConfig(
            string InputId,
            string ValidationMessage,
            string Paragraph,
            bool DisplayAsOr = false,
            bool UseHashCodeValue = false
        );

        // Static configurations for different organization types
        private static readonly Dictionary<Type, (RadioButtonConfig Config, Func<Enum, bool> IsOther)> _configurations = new()
        {
            [typeof(SchoolOrginisationTypes)] = (
                new RadioButtonConfig("organisationTypeSubCategoryOther", "Enter a job title", "Name of job title", DisplayAsOr: true),
                e => e.Equals(SchoolOrginisationTypes.Other)
            ),
            [typeof(SupportOrganisationTypes)] = (
                new RadioButtonConfig("organisationTypeSubCategoryOther", "Enter a job title", "Name of job title", DisplayAsOr: true),
                e => e.Equals(SupportOrganisationTypes.Other)
            ),
            [typeof(GovernanceBodyTypes)] = (
                new RadioButtonConfig("organisationTypeSubCategoryOther", "Enter a governance body", "Name of governance body", DisplayAsOr: true),
                e => e.Equals(GovernanceBodyTypes.Other)
            )
        };

        /// <summary>
        /// Generic method to create radio buttons for any enum type with consistent behavior
        /// </summary>
        private static List<RadioButtonsLabelViewModel> CreateRadioButtons<TEnum>(string? otherRole, bool isValid = true)
            where TEnum : struct, Enum
        {
            var enumType = typeof(TEnum);
            if (!_configurations.TryGetValue(enumType, out var configTuple))
                throw new ArgumentException($"No configuration found for enum type {enumType.Name}");

            var (config, isOtherPredicate) = configTuple;

            return Enum.GetValues<TEnum>()
                .Select(enumValue => CreateRadioButton(enumValue, config, isOtherPredicate, otherRole, isValid))
                .ToList();
        }

        /// <summary>
        /// Creates a single radio button view model from an enum value
        /// </summary>
        private static RadioButtonsLabelViewModel CreateRadioButton<TEnum>(
            TEnum enumValue,
            RadioButtonConfig config,
            Func<Enum, bool> isOtherPredicate,
            string? otherRole,
            bool isValid)
            where TEnum : struct, Enum
        {
            var displayName = enumValue.GetDisplayName();
            var isOther = isOtherPredicate(enumValue);

            var radioButton = new RadioButtonsLabelViewModel
            {
                Id = GenerateId(displayName),
                Name = displayName,
                Value = config.UseHashCodeValue ? enumValue.GetHashCode().ToString() : displayName
            };

            if (isOther)
            {
                radioButton.DisplayAsOr = config.DisplayAsOr ? true : null;
                radioButton.Input = CreateTextFieldInput(config, otherRole, isValid);
            }

            return radioButton;
        }

        /// <summary>
        /// Creates a text field input view model for "Other" options
        /// </summary>
        private static TextFieldInputViewModel CreateTextFieldInput(RadioButtonConfig config, string? otherRole, bool isValid)
        {
            return new TextFieldInputViewModel
            {
                Id = config.InputId,
                ValidationMessage = config.ValidationMessage,
                Paragraph = config.Paragraph,
                Value = otherRole,
                IsValid = isValid,
                IsTextArea = false
            };
        }

        /// <summary>
        /// Generates a consistent HTML ID from a display name
        /// </summary>
        private static string GenerateId(string displayName) => displayName.Replace(' ', '-').ToLower();

        // Public methods - now much cleaner and eliminate duplication
        public static IList<RadioButtonsLabelViewModel> GetSchoolRadioButtons(string? otherRole, bool isValid = true)
            => CreateRadioButtons<SchoolOrginisationTypes>(otherRole, isValid);

        public static IList<RadioButtonsLabelViewModel> GetSupportingOrganisationRadioButtons(string? otherRole, bool isValid = true)
            => CreateRadioButtons<SupportOrganisationTypes>(otherRole, isValid);

        public static IList<RadioButtonsLabelViewModel> GetGoverningBodyRadioButtons(string? otherRole, bool isValid = true)
            => CreateRadioButtons<GovernanceBodyTypes>(otherRole, isValid);

        // Extension methods for additional functionality if needed
        public static IList<RadioButtonsLabelViewModel> GetRadioButtonsForEnumType(Type enumType, string? otherRole, bool isValid = true)
        {
            return enumType.Name switch
            {
                nameof(SchoolOrginisationTypes) => GetSchoolRadioButtons(otherRole, isValid),
                nameof(SupportOrganisationTypes) => GetSupportingOrganisationRadioButtons(otherRole, isValid),
                nameof(GovernanceBodyTypes) => GetGoverningBodyRadioButtons(otherRole, isValid),
                _ => throw new ArgumentException($"Unsupported enum type: {enumType.Name}")
            };
        }

        // Utility method for validation - can be used by the validation logic you had earlier
        public static bool IsOtherOptionSelected<TEnum>(TEnum selectedValue) where TEnum : struct, Enum
        {
            var enumType = typeof(TEnum);
            if (!_configurations.TryGetValue(enumType, out var configTuple))
                return false;

            return configTuple.IsOther(selectedValue);
        }

        // Helper method to get the "Other" enum value for a given type
        public static TEnum GetOtherValue<TEnum>() where TEnum : struct, Enum
        {
            return Enum.GetValues<TEnum>().First(value => IsOtherOptionSelected(value));
        }

        // Configuration validation method - useful for unit tests or startup validation
        internal static bool ValidateConfigurations()
        {
            var supportedTypes = new[] { typeof(RolesIds), typeof(SchoolOrginisationTypes), typeof(SupportOrganisationTypes), typeof(GovernanceBodyTypes) };
            return supportedTypes.All(type => _configurations.ContainsKey(type));
        }
    }
}
