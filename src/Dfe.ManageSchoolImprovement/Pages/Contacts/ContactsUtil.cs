using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using Dfe.ManageSchoolImprovement.Utils;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.Contacts
{
    public static class ContactsUtil
    {
        public static IList<RadioButtonsLabelViewModel> GetRadioButtons(string? otherRole, bool isValid = true)
        {
            var list = Enum.GetValues(typeof(RolesIds))
                    .Cast<RolesIds>()
                    .Select(role =>
                    {
                        // Get the Display Name attribute value
                        string displayName = role.GetDisplayName();

                        if (role == RolesIds.Other)
                        {
                            return new RadioButtonsLabelViewModel
                            {
                                Id = displayName.Replace(' ', '-').ToLower(),
                                Name = displayName,
                                Value = role.GetHashCode().ToString(),
                                Input = new TextFieldInputViewModel
                                {
                                    Id = "OtherRole",
                                    ValidationMessage = "Enter a role",
                                    Paragraph = "Enter a role",
                                    Value = otherRole,
                                    IsValid = isValid,
                                    IsTextArea = false
                                }
                            };
                        }
                        else
                        {
                            return new RadioButtonsLabelViewModel
                            {
                                Id = displayName.Replace(' ', '-').ToLower(),
                                Name = displayName,
                                Value = role.GetHashCode().ToString()
                            };
                        }
                    })
                    .ToList();

            return list;
        }

        public static IList<RadioButtonsLabelViewModel> GetSchoolRadioButtons(string? otherRole, bool isValid = true)
        {
            var list = Enum.GetValues(typeof(SchoolOrginisationTypes))
                    .Cast<SchoolOrginisationTypes>()
                    .Select(role =>
                    {
                        // Get the Display Name attribute value
                        string displayName = role.GetDisplayName();

                        if (role == SchoolOrginisationTypes.Other)
                        {
                            return new RadioButtonsLabelViewModel
                            {
                                Id = displayName.Replace(' ', '-').ToLower(),
                                Name = displayName,
                                Value = displayName,
                                DisplayAsOr = true,
                                Input = new TextFieldInputViewModel
                                {
                                    Id = "OtherRole",
                                    ValidationMessage = "Enter a role",
                                    Paragraph = "Enter a role",
                                    Value = otherRole,
                                    IsValid = isValid,
                                    IsTextArea = false
                                }
                            };
                        }
                        else
                        {
                            return new RadioButtonsLabelViewModel
                            {
                                Id = displayName.Replace(' ', '-').ToLower(),
                                Name = displayName,
                                Value = displayName
                            };
                        }
                    })
                    .ToList();

            return list;
        }

        public static IList<RadioButtonsLabelViewModel> GetSupportingOrganisationRadioButtons(string? otherRole, bool isValid = true)
        {
            var list = Enum.GetValues(typeof(SupportOrganisationTypes))
                    .Cast<SupportOrganisationTypes>()
                    .Select(role =>
                    {
                        // Get the Display Name attribute value
                        string displayName = role.GetDisplayName();

                        if (role == SupportOrganisationTypes.Other)
                        {
                            return new RadioButtonsLabelViewModel
                            {
                                Id = displayName.Replace(' ', '-').ToLower(),
                                Name = displayName,
                                Value = displayName,
                                DisplayAsOr = true,
                                Input = new TextFieldInputViewModel
                                {
                                    Id = "OtherRole",
                                    ValidationMessage = "Enter a role",
                                    Paragraph = "Enter a role",
                                    Value = otherRole,
                                    IsValid = isValid,
                                    IsTextArea = false
                                }
                            };
                        }
                        else
                        {
                            return new RadioButtonsLabelViewModel
                            {
                                Id = displayName.Replace(' ', '-').ToLower(),
                                Name = displayName,
                                Value = displayName
                            };
                        }
                    })
                    .ToList();

            return list;
        }

        public static IList<RadioButtonsLabelViewModel> GetGoverningBodyRadioButtons(string? otherRole, bool isValid = true)
        {
            var list = Enum.GetValues(typeof(GovernanceBodyTypes))
                    .Cast<GovernanceBodyTypes>()
                    .Select(role =>
                    {
                        // Get the Display Name attribute value
                        string displayName = role.GetDisplayName();

                        if (role == GovernanceBodyTypes.Other)
                        {
                            return new RadioButtonsLabelViewModel
                            {
                                Id = displayName.Replace(' ', '-').ToLower(),
                                Name = displayName,
                                Value = displayName,
                                DisplayAsOr = true,
                                Input = new TextFieldInputViewModel
                                {
                                    Id = "OtherRole",
                                    ValidationMessage = "Enter a role",
                                    Paragraph = "Enter a role",
                                    Value = otherRole,
                                    IsValid = isValid,
                                    IsTextArea = false
                                }
                            };
                        }
                        else
                        {
                            return new RadioButtonsLabelViewModel
                            {
                                Id = displayName.Replace(' ', '-').ToLower(),
                                Name = displayName,
                                Value = displayName
                            };
                        }
                    })
                    .ToList();

            return list;
        }
    }
}
