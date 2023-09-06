using ETHTPS.Configuration;

using Hangfire;

namespace ETHTPS.TaskRunner.BackgroundServices
{
    /// <summary>
    /// Schedules tasks to run in the background. These are picked up by the Hangfire server.
    /// </summary>
    public class TaskRunnerService : IHostedService
    {
        private readonly ILogger<TaskRunnerService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private bool _cancel = false;
        private static readonly string[] _defaultQueues = new string[] { "default" };

        public TaskRunnerService(ILogger<TaskRunnerService> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("TaskRunnerService: starting...");
#if DEBUG
            BackgroundJob.Enqueue(() => Console.WriteLine("Ethereum"));
#endif
            using var scope = _serviceScopeFactory.CreateScope();
            var configurationProvider = scope.ServiceProvider.GetService<DBConfigurationProviderWithCache>();
            using var server = new BackgroundJobServer(new BackgroundJobServerOptions()
            {
                Queues = configurationProvider?.GetConfigurationStrings("HangfireQueue")?.Select(x => x.Value)?.ToArray() ?? _defaultQueues
            });
            while (!cancellationToken.IsCancellationRequested && !_cancel)
            {
                await Task.Delay(500);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("TaskRunnerService: stopping...");
            _cancel = true;
            return Task.CompletedTask;
        }
    }
}
