using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject
{
    public record SetCompleteAndSaveInitialDiagnosisTemplateCommand(
        SupportProjectId SupportProjectId,
        DateTime? SavedAssessmentTemplateInSharePointDate,
        bool? HasTalkToAdviserAboutFindings,
        bool? HasCompleteAssessmentTemplate
    ) : IRequest<bool>;

    public class SetCompleteAndSaveInitialDiagnosisTemplateCommandHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<SetCompleteAndSaveInitialDiagnosisTemplateCommand, bool>
    {
        public async Task<bool> Handle(SetCompleteAndSaveInitialDiagnosisTemplateCommand request,
            CancellationToken cancellationToken)
        {
            var supportProject = await supportProjectRepository.FindAsync(x => x.Id == request.SupportProjectId, cancellationToken);

            if (supportProject is null)
            {
                return false;
            }

            supportProject.SetCompleteAndSaveInitialDiagnosisTemplate(request.SavedAssessmentTemplateInSharePointDate, request.HasTalkToAdviserAboutFindings, request.HasCompleteAssessmentTemplate);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}
