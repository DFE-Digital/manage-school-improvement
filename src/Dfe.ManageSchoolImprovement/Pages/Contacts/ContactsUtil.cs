using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.ViewModels;
using Dfe.ManageSchoolImprovement.Utils;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.Contacts
{
    public class ContactsUtil
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
                                    ValidationMessage = "You must enter a role",
                                    Paragraph = "Please enter a role",
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

        public static bool IsOtherRoleFieldValidation(int? roleId, string? otherRole)
        {
            return roleId == RolesIds.Other.GetHashCode() && !string.IsNullOrWhiteSpace(otherRole);
        }
    }
}
