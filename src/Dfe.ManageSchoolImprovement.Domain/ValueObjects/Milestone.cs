using System.ComponentModel.DataAnnotations;

namespace Dfe.ManageSchoolImprovement.Domain.ValueObjects;

public enum Milestone
{
    [Display(Name = "Formally inform responsible body")]
    FormallyInformResponsibleBody = 0,
    [Display(Name = "First RISE meeting")] FirstRiseMeeting = 1,
    [Display(Name = "Initial diagnosis")] InitialDiagnosis = 2,
    [Display(Name = "Matching complete")] MatchingComplete = 3,
    [Display(Name = "Plans approved")] PlansApproved = 4,
    [Display(Name = "Improvement grant offer letter requested")]
    ImprovementGrantOfferLetterRequested = 5,
    [Display(Name = "Implementation and termly reviews")]
    ImplementationAndTermlyReviews = 6,
    [Display(Name = "Termly reviews")] TermlyReviews = 7
}