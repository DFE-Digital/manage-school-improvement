using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.SupportProjectContacts;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Mediator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dfe.ManageSchoolImprovement.Application.Tests.Common.Mediation;

public class MediatorRegistrationTests
{
    [Fact]
    public void AddApplicationDependencyGroup_RegistersContactCommandHandlers()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection()
            .Build();

        services.AddApplicationDependencyGroup(configuration);

        Assert.Contains(services, descriptor =>
            descriptor.ServiceType == typeof(IRequestHandler<UpdateSupportProjectContactCommand, SupportProjectContactId>)
            && descriptor.ImplementationType == typeof(UpdateSupportProjectContactHandler));

        Assert.Contains(services, descriptor =>
            descriptor.ServiceType == typeof(IRequestHandler<CreateSupportProjectContactCommand, SupportProjectContactId>)
            && descriptor.ImplementationType == typeof(CreateSupportProjectContactHandler));
    }
}
