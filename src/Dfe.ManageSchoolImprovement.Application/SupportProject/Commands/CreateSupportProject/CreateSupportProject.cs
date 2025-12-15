using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using GovUK.Dfe.CoreLibs.Contracts.Academies.Base;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.CreateSupportProject
{
    public record CreateSupportProjectCommand(
        string schoolName,
        string schoolUrn,
        string localAuthority,
        string region,
        string? trustName,
        string? trustReferenceNumber,
        AddressDto? address
    ) : IRequest<SupportProjectId>;

    public class CreateSupportProjectCommandHandler(ISupportProjectRepository supportProjectRepository)
        : IRequestHandler<CreateSupportProjectCommand, SupportProjectId>
    {
        public async Task<SupportProjectId> Handle(CreateSupportProjectCommand request, CancellationToken cancellationToken)
        {
            var schoolAddress = string.Join(", ", new[]
            {
                request.address?.Street,
                request.address?.Locality,
                request.address?.Town,
                request.address?.County,
                request.address?.Postcode
            }.Where(x => !string.IsNullOrWhiteSpace(x)));
            
            var supportProject = Domain.Entities.SupportProject.SupportProject.Create(
                request.schoolName,
                request.schoolUrn,
                request.localAuthority,
                request.region,
                request.trustName,
                request.trustReferenceNumber,
                schoolAddress);

            await supportProjectRepository.AddAsync(supportProject, cancellationToken);

            return supportProject.Id!;
        }
    }
}
