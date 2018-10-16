using System.Collections.Generic;
using System.Linq;

using ClaymoreMiner.RemoteManagement.Models;

using MiningMonitor.Model;
using MiningMonitor.Model.Alerts;

namespace MiningMonitor.Alerts.Scanners
{
    public class GpuHashrateThresholdScanner : GpuThresholdScanner
    {
        public GpuHashrateThresholdScanner(IAlertFactory alertFactory) : base(alertFactory)
        {
        }

        public override string DefaultAlertMessage => "GPU hashrate out of range";

        public override bool ShouldScan(AlertDefinition definition)
        {
            return definition.Parameters is GpuThresholdAlertParameters parameters && parameters.Metric == Metric.Hashrate;
        }

        protected override Condition MapToCondition(GpuStats stats, GpuThresholdAlertParameters parameters)
        {
            if (stats.EthereumHashrate / 1000m < parameters.MinValue)
                return Condition.Low;
            if (stats.EthereumHashrate / 1000m > parameters.MaxValue)
                return Condition.High;

            return Condition.Ok;
        }

        protected override AlertMetadata CreateMetadata(AlertMetadata metadata, GpuThresholdAlertParameters parameters, GpuConditionPeriod conditionPeriod)
        {
            var hashrates = conditionPeriod.GpuStats
                .Select(s => (int?) s.EthereumHashrate)
                .Concat(new[] {metadata?.Value?.Min, metadata?.Value?.Max})
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

        protected override IEnumerable<string> CreateDetailMessages(AlertMetadata metadata, GpuThresholdAlertParameters parameters)
        {
            var durationMessage = parameters.DurationMinutes == null ? null : $" for more than {parameters.DurationMinutes} minute(s)";

            if (metadata.Value.Condition == Condition.High)
                yield return $"GPU {metadata.GpuIndex + 1} hashrate above threshold of {parameters.MaxValue} MH/s{durationMessage}";
            if (metadata.Value.Condition == Condition.Low)
                yield return $"GPU {metadata.GpuIndex + 1} hashrate below threshold of {parameters.MinValue} MH/s{durationMessage}";

            yield return $"GPU {metadata.GpuIndex + 1} min hashrate speed during alert: {metadata.Value.Min / 1000m} MH/s";
            yield return $"GPU {metadata.GpuIndex + 1} max hashrate speed during alert: {metadata.Value.Max / 1000m} MH/s";
        }
    }
}
