using System.Collections.Generic;
using System.Linq;

using ClaymoreMiner.RemoteManagement.Models;

using MiningMonitor.Model.Alerts;
using MiningMonitor.Scheduler;

namespace MiningMonitor.Service.Alerts
{
    public class GpuHashrateThresholdScanner : GpuThresholdScanner
    {
        public GpuHashrateThresholdScanner(SnapshotDataCollectorSchedule snapshotDataCollectorSchedule) : base(snapshotDataCollectorSchedule)
        {
        }

        public override string DefaultAlertMessage => "GPU hashrate out of range";

        public override bool ShouldScan(AlertDefinition definition)
        {
            return definition.Parameters is GpuThresholdParameters parameters && parameters.Metric == Metric.Hashrate;
        }

        protected override GpuThresholdState SelectAlertState(GpuStats stats, GpuThresholdParameters parameters)
        {
            if (stats.EthereumHashrate / 1000m < parameters.MinValue)
                return GpuThresholdState.Low;
            if (stats.EthereumHashrate / 1000m > parameters.MaxValue)
                return GpuThresholdState.High;

            return GpuThresholdState.Ok;
        }

        protected override AlertMetadata CreateMetadata(Alert alert, GpuThresholdParameters parameters, GpuThresholdPeriod statePeriod)
        {
            var hashrates = statePeriod.GpuStats
                .Select(s => (int?)s.EthereumHashrate)
                .Concat(new[] {alert?.Metadata?.Value?.Min, alert?.Metadata?.Value?.Max})
                .ToList();

            return new AlertMetadata
            {
                GpuIndex = statePeriod.GpuIndex,
                Threshold = new AlertThresholdMetadata
                {
                    Min = parameters.MaxValue,
                    Max = parameters.MaxValue,
                    GpuMetric = Metric.Hashrate
                },
                Value = new AlertValueMetadata
                {
                    GpuThresholdState = statePeriod.State,
                    Min = hashrates.Min(),
                    Max = hashrates.Max()
                }
            };
        }

        protected override IEnumerable<string> CreateDetailMessages(Alert alert, GpuThresholdParameters parameters)
        {
            var durationMessage = parameters.DurationMinutes == null ? null : $" for more than {parameters.DurationMinutes} minute(s)";

            if (alert.Metadata.Value.GpuThresholdState == GpuThresholdState.High)
                yield return $"GPU {alert.Metadata.GpuIndex + 1} hashrate above threshold of {parameters.MaxValue} MH/s{durationMessage}";
            if (alert.Metadata.Value.GpuThresholdState == GpuThresholdState.Low)
                yield return $"GPU {alert.Metadata.GpuIndex + 1} hashrate below threshold of {parameters.MinValue} MH/s{durationMessage}";

            yield return $"GPU {alert.Metadata.GpuIndex + 1} min hashrate speed during alert: {alert.Metadata.Value.Min / 1000m} MH/s";
            yield return $"GPU {alert.Metadata.GpuIndex + 1} max hashrate speed during alert: {alert.Metadata.Value.Max / 1000m} MH/s";
        }
    }
}
