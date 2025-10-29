using GovUK.Dfe.CoreLibs.AsyncProcessing.Interfaces;
using Dfe.ManageSchoolImprovement.Application.Services.BackgroundServices.Events;

namespace Dfe.ManageSchoolImprovement.Application.Services.BackgroundServices.EventHandlers
{
    public class SimpleTaskCompletedEventHandler : IBackgroundServiceEventHandler<CreateReportExampleTaskCompletedEvent>
    {
        public Task Handle(CreateReportExampleTaskCompletedEvent notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Event received for Task: {notification.TaskName}, Message: {notification.Message}");
            return Task.CompletedTask;
        }
    }
}
