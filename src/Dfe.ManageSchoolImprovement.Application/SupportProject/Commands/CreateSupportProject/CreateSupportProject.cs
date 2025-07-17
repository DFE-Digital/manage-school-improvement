using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.CreateSupportProject
{
    public record CreateSupportProjectCommand(
        string schoolName,
        string schoolUrn,
        string localAuthority,
        string region,
        string? trustName,
        string? trustReferenceNumber
    ) : IRequest<SupportProjectId>;

    public class CreateSupportProjectCommandHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<CreateSupportProjectCommand, SupportProjectId>
    {
        public async Task<SupportProjectId> Handle(CreateSupportProjectCommand request, CancellationToken cancellationToken)
        {
            var supportProject = Domain.Entities.SupportProject.SupportProject.Create(
                request.schoolName,
                request.schoolUrn,
                request.localAuthority,
                request.region,
                request.trustName,
                request.trustReferenceNumber);

            await supportProjectRepository.AddAsync(supportProject, cancellationToken);

            return supportProject.Id!;
        }
    }
}
