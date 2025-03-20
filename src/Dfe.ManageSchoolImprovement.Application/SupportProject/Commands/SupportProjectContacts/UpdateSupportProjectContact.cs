using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Utils;
using MediatR; 

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.SupportProjectContacts
{ 
    public record UpdateSupportProjectContactCommand(
       SupportProjectId SupportProjectId,
       SupportProjectContactId Id,
       string Name,
       RolesIds RoleId,
       string OtherRoleName,
       string Organisation,
       string Email,
       string Phone,
       string Author
   ) : IRequest<SupportProjectContactId>;

    public class UpdateSupportProjectContactHandler(ISupportProjectRepository supportProjectRepository, IDateTimeProvider _dateTimeProvider)
        : IRequestHandler<UpdateSupportProjectContactCommand, SupportProjectContactId>
    {
        public async Task<SupportProjectContactId> Handle(UpdateSupportProjectContactCommand request, CancellationToken cancellationToken)
        {
            var supportProject = await supportProjectRepository.GetSupportProjectById(request.SupportProjectId, cancellationToken) ?? throw new ArgumentException($"Support project with id {request.SupportProjectId} not found");
            
            supportProject.EditContact(request.Id, request.Name, request.RoleId, request.OtherRoleName, request.Organisation, request.Email, request.Phone, request.Author, _dateTimeProvider.Now);
            
            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return request.Id;
        }
    }
}
