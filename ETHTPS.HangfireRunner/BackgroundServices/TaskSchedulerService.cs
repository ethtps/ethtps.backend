namespace ETHTPS.TaskRunner.BackgroundServices
{
    /// <summary>
    /// Schedules tasks to run in the background. These are picked up by the Hangfire server.
    /// </summary>
    public class TaskSchedulerService : IHostedService
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
