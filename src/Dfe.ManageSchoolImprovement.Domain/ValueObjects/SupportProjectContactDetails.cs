namespace Dfe.ManageSchoolImprovement.Domain.ValueObjects;

public class SupportProjectContactDetails
{
    public string Name { get; set; } = string.Empty;
    public string OrganisationTypeSubCategory { get; set; } = string.Empty;
    public string? OrganisationTypeSubCategoryOther { get; set; }
    public string OrganisationType { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
}
