using System.Collections.Generic;
using System.Linq;

using ClaymoreMiner.RemoteManagement.Models;

using MiningMonitor.Model;
using MiningMonitor.Model.Alerts;

namespace MiningMonitor.Alerts.Scanners
{
    public class GpuFanSpeedThresholdScanner : GpuThresholdScanner
    {
        public GpuFanSpeedThresholdScanner(IAlertFactory alertFactory) : base(alertFactory)
        {
        }

        public override string DefaultAlertMessage => "GPU fan speed out of range";

        public override bool ShouldScan(AlertDefinition definition)
        {
            return definition.Parameters is GpuThresholdAlertParameters parameters && parameters.Metric == Metric.FanSpeed;
        }

        protected override Condition MapToCondition(GpuStats stats, GpuThresholdAlertParameters parameters)
        {
            if (stats.FanSpeed < parameters.MinValue)
                return Condition.Low;
            if (stats.FanSpeed > parameters.MaxValue)
                return Condition.High;

            return Condition.Ok;
        }

        protected override AlertMetadata CreateMetadata(AlertMetadata existingMetadata, GpuThresholdAlertParameters parameters, GpuConditionPeriod conditionPeriod)
        {
            var fanSpeeds = conditionPeriod.GpuStats
                .Select(s => (int?) s.FanSpeed)
                .Concat(new[] { existingMetadata?.Value?.Min, existingMetadata?.Value?.Max})
                .ToList();

            return new AlertMetadata
            {
                GpuIndex = conditionPeriod.GpuIndex,
                Threshold = new AlertThresholdMetadata
                {
                    Min = parameters.MaxValue,
                    Max = parameters.MaxValue,
                    GpuMetric = Metric.FanSpeed
                },
                Value = new AlertValueMetadata
                {
                    Condition = conditionPeriod.Condition,
                    Min = fanSpeeds.Min(),
                    Max = fanSpeeds.Max()
                }
            };
        }

        protected override IEnumerable<string> CreateDetailMessages(AlertMetadata metadata, GpuThresholdAlertParameters parameters)
        {
            var durationMessage = parameters.DurationMinutes == null ? null : $" for more than {parameters.DurationMinutes} minute(s)";

            if (metadata.Value.Condition == Condition.High)
                yield return $"GPU {metadata.GpuIndex + 1} fan speed above threshold of {parameters.MaxValue}%{durationMessage}";
            if (metadata.Value.Condition == Condition.Low)
                yield return $"GPU {metadata.GpuIndex + 1} fan speed below threshold of {parameters.MinValue}%{durationMessage}";

            yield return $"GPU {metadata.GpuIndex + 1} min fan speed during alert: {metadata.Value.Min}%";
            yield return $"GPU {metadata.GpuIndex + 1} max fan speed during alert: {metadata.Value.Max}%";
        }
    }
}
