using System.Collections.Generic;
using System.Linq;

using ClaymoreMiner.RemoteManagement.Models;

using MiningMonitor.BackgroundScheduler;
using MiningMonitor.Model;
using MiningMonitor.Model.Alerts;

namespace MiningMonitor.Alerts.Scanners
{
    public class GpuFanSpeedThresholdScanner : GpuThresholdScanner
    {
        public GpuFanSpeedThresholdScanner(DataCollectorSchedule dataCollectorSchedule) : base(dataCollectorSchedule)
        {
        }

        public override string DefaultAlertMessage => "GPU fan speed out of range";

        public override bool ShouldScan(AlertDefinition definition)
        {
            return definition.Parameters is GpuThresholdParameters parameters && parameters.Metric == Metric.FanSpeed;
        }

        protected override Condition MapToCondition(GpuStats stats, GpuThresholdParameters parameters)
        {
            if (stats.FanSpeed < parameters.MinValue)
                return Condition.Low;
            if (stats.FanSpeed > parameters.MaxValue)
                return Condition.High;

            return Condition.Ok;
        }

        protected override AlertMetadata CreateMetadata(Alert alert, GpuThresholdParameters parameters, GpuConditionPeriod conditionPeriod)
        {
            var fanSpeeds = conditionPeriod.GpuStats
                .Select(s => (int?) s.FanSpeed)
                .Concat(new[] {alert?.Metadata?.Value?.Min, alert?.Metadata?.Value?.Max})
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

        protected override IEnumerable<string> CreateDetailMessages(Alert alert, GpuThresholdParameters parameters)
        {
            var durationMessage = parameters.DurationMinutes == null ? null : $" for more than {parameters.DurationMinutes} minute(s)";

            if (alert.Metadata.Value.Condition == Condition.High)
                yield return $"GPU {alert.Metadata.GpuIndex + 1} fan speed above threshold of {parameters.MaxValue}%{durationMessage}";
            if (alert.Metadata.Value.Condition == Condition.Low)
                yield return $"GPU {alert.Metadata.GpuIndex + 1} fan speed below threshold of {parameters.MinValue}%{durationMessage}";

            yield return $"GPU {alert.Metadata.GpuIndex + 1} min fan speed during alert: {alert.Metadata.Value.Min}%";
            yield return $"GPU {alert.Metadata.GpuIndex + 1} max fan speed during alert: {alert.Metadata.Value.Max}%";
        }
    }
}
