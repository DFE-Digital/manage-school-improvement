using Dfe.ManageSchoolImprovement.Application.Services.BackgroundServices.Events;
using Dfe.ManageSchoolImprovement.Application.Services.BackgroundServices.Tasks;
using GovUK.Dfe.CoreLibs.AsyncProcessing.Interfaces;
using MediatR;

namespace Dfe.ManageSchoolImprovement.Application.Schools.Commands.CreateReport
{
    /// <summary>
    /// An example of enqueuing a background task
    /// </summary>
    public record CreateReportCommand() : IRequest<string>;

    public class CreateReportCommandHandler(IBackgroundServiceFactory backgroundServiceFactory)
        : IRequestHandler<CreateReportCommand, string>
    {
        public async Task<string> Handle(CreateReportCommand request, CancellationToken cancellationToken)
        {
            var taskName = "Create_Report_Task1";

            var report = await backgroundServiceFactory
                .EnqueueTask(
                    ct => new CreateReportExampleTask().RunAsync(taskName),
                    resultValue => new CreateReportExampleTaskCompletedEvent(taskName, resultValue),
                    cancellationToken
                )
                .ConfigureAwait(false);

            return report;
        }
    }
}
