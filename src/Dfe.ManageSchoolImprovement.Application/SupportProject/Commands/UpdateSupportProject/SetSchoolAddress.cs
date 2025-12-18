using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;

public class SetSchoolAddress
{
    public record SetSchoolAddressCommand(
        SupportProjectId SupportProjectId,
        string? Address
    ) : IRequest<bool>;

    public class SetSchoolAddressHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<SetSchoolAddressCommand, bool>
    {
        public async Task<bool> Handle(SetSchoolAddressCommand request,
            CancellationToken cancellationToken)
        {
            var supportProject =
                await supportProjectRepository.FindAsync(x => x.Id == request.SupportProjectId, cancellationToken);

            if (supportProject is null)
            {
                return false;
            }

            supportProject.SetSchoolAddress(
                request.Address);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return true;
        }
    }
}