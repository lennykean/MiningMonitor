using System.Collections.Generic;
using System.Linq;

using ClaymoreMiner.RemoteManagement.Models;

using MiningMonitor.Model.Alerts;

namespace MiningMonitor.Service.Alerts
{
    public class GpuFanSpeedThresholdScanner : GpuThresholdScanner
    {
        public override string DefaultAlertMessage => "GPU fan speed out of range";

        public override bool ShouldScan(AlertDefinition definition)
        {
            return definition.Parameters is GpuThresholdParameters parameters && parameters.Metric == Metric.FanSpeed;
        }

        protected override GpuThresholdState SelectAlertState(GpuStats stats, GpuThresholdParameters parameters)
        {
            if (stats.FanSpeed < parameters.MinValue)
                return GpuThresholdState.Low;
            if (stats.FanSpeed > parameters.MaxValue)
                return GpuThresholdState.High;

            return GpuThresholdState.Ok;
        }

        protected override AlertMetadata CreateMetadata(Alert alert, GpuThresholdParameters parameters, GpuThresholdPeriod statePeriod)
        {
            var fanSpeeds = statePeriod.GpuStats
                .Select(s => (int?) s.FanSpeed)
                .Concat(new[] {alert?.Metadata?.Value?.Min, alert?.Metadata?.Value?.Max})
                .ToList();

            return new AlertMetadata
            {
                GpuIndex = statePeriod.GpuIndex,
                Threshold = new AlertThresholdMetadata
                {
                    Min = parameters.MaxValue,
                    Max = parameters.MaxValue,
                    GpuMetric = Metric.FanSpeed
                },
                Value = new AlertValueMetadata
                {
                    GpuThresholdState = statePeriod.State,
                    Min = fanSpeeds.Min(),
                    Max = fanSpeeds.Max()
                }
            };
        }

        protected override IEnumerable<string> CreateDetailMessages(Alert alert, GpuThresholdParameters parameters)
        {
            var durationMessage = parameters.DurationMinutes == null ? null : $" for more than {parameters.DurationMinutes} minute(s)";

            if (alert.Metadata.Value.GpuThresholdState == GpuThresholdState.High)
                yield return $"GPU {alert.Metadata.GpuIndex + 1} fan speed above threshold of {parameters.MaxValue}%{durationMessage}";
            if (alert.Metadata.Value.GpuThresholdState == GpuThresholdState.Low)
                yield return $"GPU {alert.Metadata.GpuIndex + 1} fan speed below threshold of {parameters.MinValue}%{durationMessage}";

            yield return $"GPU {alert.Metadata.GpuIndex + 1} min fan speed during alert: {alert.Metadata.Value.Min}%";
            yield return $"GPU {alert.Metadata.GpuIndex + 1} max fan speed during alert: {alert.Metadata.Value.Max}%";
        }
    }
}
