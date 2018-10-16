using System.Collections.Generic;
using System.Linq;

using ClaymoreMiner.RemoteManagement.Models;

using MiningMonitor.Model;
using MiningMonitor.Model.Alerts;

namespace MiningMonitor.Alerts.Scanners
{
    public class GpuTemperatureThresholdScanner : GpuThresholdScanner
    {
        public GpuTemperatureThresholdScanner(IAlertFactory alertFactory) : base(alertFactory)
        {
        }

        public override string DefaultAlertMessage => "GPU temperature out of range";

        public override bool ShouldScan(AlertDefinition definition)
        {
            return definition.Parameters is GpuThresholdAlertParameters parameters && parameters.Metric == Metric.Temperature;
        }

        protected override Condition MapToCondition(GpuStats stats, GpuThresholdAlertParameters parameters)
        {
            if (stats.Temperature < parameters.MinValue)
                return Condition.Low;
            if (stats.Temperature > parameters.MaxValue)
                return Condition.High;

            return Condition.Ok;
        }

        protected override AlertMetadata CreateMetadata(AlertMetadata existingMetadata, GpuThresholdAlertParameters parameters, GpuConditionPeriod conditionPeriod)
        {
            var temps = conditionPeriod.GpuStats
                .Select(s => (int?)s.Temperature)
                .Concat(new[] { existingMetadata?.Value?.Min, existingMetadata?.Value?.Max })
                .ToList();

            return new AlertMetadata
            {
                GpuIndex = conditionPeriod.GpuIndex,
                Threshold = new AlertThresholdMetadata
                {
                    Min = parameters.MaxValue,
                    Max = parameters.MaxValue,
                    GpuMetric = Metric.Temperature
                },
                Value = new AlertValueMetadata
                {
                    Condition = conditionPeriod.Condition,
                    Min = temps.Min(),
                    Max = temps.Max()
                }
            };
        }

        protected override IEnumerable<string> CreateDetailMessages(AlertMetadata metadata, GpuThresholdAlertParameters parameters)
        {
            var durationMessage = parameters.DurationMinutes == null ? null : $" for more than {parameters.DurationMinutes} minute(s)";

            if (metadata.Value.Condition == Condition.High)
                yield return $"GPU {metadata.GpuIndex + 1} temperature above threshold of {parameters.MaxValue}°{durationMessage}";
            if (metadata.Value.Condition == Condition.Low)
                yield return $"GPU {metadata.GpuIndex + 1} temperature below threshold of {parameters.MinValue}°{durationMessage}";

            yield return $"GPU {metadata.GpuIndex + 1} min temperature during alert: {metadata.Value.Min}°";
            yield return $"GPU {metadata.GpuIndex + 1} max temperature during alert: {metadata.Value.Max}°";
        }
    }
}
