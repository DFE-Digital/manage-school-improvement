namespace Dfe.ManageSchoolImprovement.Domain.ValueObjects;

public class SupportingOrganisationDetails
{
    public DateTime? DateSupportOrganisationChosen { get; set; }
    public string? SupportOrganisationName { get; set; }
    public string? SupportOrganisationIdNumber { get; set; }
    public string? SupportOrganisationType { get; set; }
    public bool? AssessmentToolTwoCompleted { get; set; }
    public string? SupportOrganisationAddress { get; set; }
    public string? SupportOrganisationContactName { get; set; }
    public string? SupportOrganisationContactEmailAddress { get; set; }
    public string? SupportOrganisationContactPhone { get; set; }
    public DateTime? DateSupportingOrganisationContactDetailsAdded { get; set; }
}