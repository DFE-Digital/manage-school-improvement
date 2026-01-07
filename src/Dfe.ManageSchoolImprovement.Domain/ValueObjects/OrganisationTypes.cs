namespace Dfe.ManageSchoolImprovement.Domain.ValueObjects
{
    /// <summary>
    /// Constants for organisation types used in contact management
    /// </summary>
    public static class OrganisationTypes
    {
        /// <summary>
        /// School organisation type
        /// </summary>
        public const string School = "School";

        /// <summary>
        /// Supporting organisation type
        /// </summary>
        public const string SupportingOrganisation = "Supporting organisation";

        /// <summary>
        /// Governance bodies organisation type
        /// </summary>
        public const string GovernanceBodies = "Governance bodies";

        /// <summary>
        /// Gets all available organisation types
        /// </summary>
        public static readonly string[] AllTypes =
        {
            School,
            SupportingOrganisation,
            GovernanceBodies
        };
    }
}