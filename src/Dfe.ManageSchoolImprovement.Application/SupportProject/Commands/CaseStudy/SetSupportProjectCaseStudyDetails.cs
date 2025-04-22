using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Utils;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.CreateSupportProjectNote;

public class SetSupportProjectCaseStudyDetails
{
    public record SetSupportProjectCaseStudyDetailsCommand(
        SupportProjectId SupportProjectId,
        bool? CaseStudyCandidate,
        string? CaseStudyDetails
    ) : IRequest<bool>;

    public class SetSupportProjectCaseStudyDetailsCommandHandler(ISupportProjectRepository supportProjectRepository, IDateTimeProvider _dateTimeProvider)
        : IRequestHandler<SetSupportProjectCaseStudyDetailsCommand, bool>
    {
        public async Task<bool> Handle(SetSupportProjectCaseStudyDetailsCommand request, CancellationToken cancellationToken)
        {

            var supportProject = await supportProjectRepository.FindAsync(x => x.Id == request.SupportProjectId, cancellationToken);

            var supportProjectNoteId = new SupportProjectNoteId(Guid.NewGuid());

            //supportProject.SetCaseStudyDetails(supportProjectNoteId, request.CaseStudyCandidate, request.CaseStudyDetails);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}
