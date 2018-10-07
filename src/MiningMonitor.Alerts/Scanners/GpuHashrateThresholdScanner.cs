using System.Collections.Generic;
using System.Linq;

using ClaymoreMiner.RemoteManagement.Models;

using MiningMonitor.BackgroundScheduler;
using MiningMonitor.Model;
using MiningMonitor.Model.Alerts;

namespace MiningMonitor.Alerts.Scanners
{
    public class GpuHashrateThresholdScanner : GpuThresholdScanner
    {
        public GpuHashrateThresholdScanner(DataCollectorSchedule dataCollectorSchedule) : base(dataCollectorSchedule)
        {
        }

        public override string DefaultAlertMessage => "GPU hashrate out of range";

        public override bool ShouldScan(AlertDefinition definition)
        {
            return definition.Parameters is GpuThresholdParameters parameters && parameters.Metric == Metric.Hashrate;
        }

        protected override Condition MapToCondition(GpuStats stats, GpuThresholdParameters parameters)
        {
            if (stats.EthereumHashrate / 1000m < parameters.MinValue)
                return Condition.Low;
            if (stats.EthereumHashrate / 1000m > parameters.MaxValue)
                return Condition.High;

            return Condition.Ok;
        }

        protected override AlertMetadata CreateMetadata(Alert alert, GpuThresholdParameters parameters, GpuConditionPeriod conditionPeriod)
        {
            var hashrates = conditionPeriod.GpuStats
                .Select(s => (int?)s.EthereumHashrate)
                .Concat(new[] {alert?.Metadata?.Value?.Min, alert?.Metadata?.Value?.Max})
                .ToList();

            return new AlertMetadata
            {
                GpuIndex = conditionPeriod.GpuIndex,
                Threshold = new AlertThresholdMetadata
                {
                    Min = parameters.MaxValue,
                    Max = parameters.MaxValue,
                    GpuMetric = Metric.Hashrate
                },
                Value = new AlertValueMetadata
                {
                    Condition = conditionPeriod.Condition,
                    Min = hashrates.Min(),
                    Max = hashrates.Max()
                }
            };
        }

        protected override IEnumerable<string> CreateDetailMessages(Alert alert, GpuThresholdParameters parameters)
        {
            var durationMessage = parameters.DurationMinutes == null ? null : $" for more than {parameters.DurationMinutes} minute(s)";

            if (alert.Metadata.Value.Condition == Condition.High)
                yield return $"GPU {alert.Metadata.GpuIndex + 1} hashrate above threshold of {parameters.MaxValue} MH/s{durationMessage}";
            if (alert.Metadata.Value.Condition == Condition.Low)
                yield return $"GPU {alert.Metadata.GpuIndex + 1} hashrate below threshold of {parameters.MinValue} MH/s{durationMessage}";

            yield return $"GPU {alert.Metadata.GpuIndex + 1} min hashrate speed during alert: {alert.Metadata.Value.Min / 1000m} MH/s";
            yield return $"GPU {alert.Metadata.GpuIndex + 1} max hashrate speed during alert: {alert.Metadata.Value.Max / 1000m} MH/s";
        }
    }
}
