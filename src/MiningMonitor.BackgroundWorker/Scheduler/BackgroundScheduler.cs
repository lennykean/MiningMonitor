using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

namespace MiningMonitor.BackgroundWorker.Scheduler
{
    public class BackgroundScheduler<TWorker, TSchedule> : BackgroundService
        where TWorker : IBackgroundWorker
        where TSchedule : ISchedule
    {
        private readonly TWorker _worker;
        private readonly TSchedule _schedule;
        private readonly ILogger<BackgroundScheduler<TWorker, TSchedule>> _logger;

        public BackgroundScheduler(TWorker worker, TSchedule schedule, ILogger<BackgroundScheduler<TWorker, TSchedule>> logger)
        {
            _worker = worker;
            _schedule = schedule;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (_schedule.DoWorkOnStartup)
            {
                _logger.LogInformation("Performing startup work cycle");
                await PerformWorkCycle(stoppingToken);
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"Performing next work cycle in {_schedule.Interval}");
                await Task.Delay(_schedule.Interval, stoppingToken);

                _logger.LogInformation($"Performing work cycle");
                await PerformWorkCycle(stoppingToken);
            }
        }

        private async Task PerformWorkCycle(CancellationToken stoppingToken)
        {
            try
            {
                await _worker.DoWorkAsync(stoppingToken);
                _logger.LogInformation("Completed work cycle");
            }
            catch (Exception ex) when (!(ex is OperationCanceledException))
            {
                _logger.LogError("Error while performing work cycle", ex);
            }
        }
    }
}