namespace Dfe.ManageSchoolImprovement.Domain.ValueObjects;

public class SupportProjectContactDetails
{
    public string Name { get; set; } = string.Empty;
    public RolesIds RoleId { get; set; }
    public string? OtherRoleName { get; set; }
    public string Organisation { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
}
