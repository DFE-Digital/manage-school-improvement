using Dfe.ManageSchoolImprovement.Domain.Interfaces.Repositories;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Utils;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.SupportProjectContacts
{
    public record CreateSupportProjectContactCommand(
       SupportProjectId SupportProjectId,
       string Name,
       string OrganisationTypeSubCategory,
       string? OrganisationTypeSubCategoryOther,
       string Organisation,
       string Email,
       string Phone,
       string Author
   ) : IRequest<SupportProjectContactId>;

    public class CreateSupportProjectContactHandler(ISupportProjectRepository supportProjectRepository, IDateTimeProvider _dateTimeProvider)
        : IRequestHandler<CreateSupportProjectContactCommand, SupportProjectContactId>
    {
        public async Task<SupportProjectContactId> Handle(CreateSupportProjectContactCommand request, CancellationToken cancellationToken)
        {
            var supportProject = await supportProjectRepository.FindAsync(x => x.Id == request.SupportProjectId, cancellationToken);

            var supportProjectContactId = new SupportProjectContactId(Guid.NewGuid());

            var details = new SupportProjectContactDetails
            {
                Name = request.Name,
                OrganisationTypeSubCategory = request.OrganisationTypeSubCategory,
                OrganisationTypeSubCategoryOther = request.OrganisationTypeSubCategoryOther,
                OrganisationType = request.Organisation,
                Email = request.Email,
                Phone = request.Phone
            };


            supportProject.AddContact(supportProjectContactId, details, request.Author, _dateTimeProvider.Now, request.SupportProjectId);

            await supportProjectRepository.UpdateAsync(supportProject, cancellationToken);

            return supportProjectContactId;
        }
    }
}
