using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.CreateSupportProjectNote;

public class SetSupportProjectCaseStudyDetails
{
    public record SetSupportProjectCaseStudyDetailsCommand(
        SupportProjectId SupportProjectId,
        bool? CaseStudyCandidate,
        string? CaseStudyDetails
    ) : IRequest<bool>;

    public class SetSupportProjectCaseStudyDetailsCommandHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<SetSupportProjectCaseStudyDetailsCommand, bool>
    {
        public async Task<bool> Handle(SetSupportProjectCaseStudyDetailsCommand request, CancellationToken cancellationToken)
        {

            var supportProject = await supportProjectRepository.GetSupportProjectById(request.SupportProjectId, cancellationToken);

            if (supportProject is null)
            {
                return false;
            }

            supportProject.SetCaseStudyDetails(request.CaseStudyCandidate, request.CaseStudyDetails);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}
