using System;
using System.Collections.Generic;
using System.Linq;

using MiningMonitor.Common;
using MiningMonitor.Model;
using MiningMonitor.Model.Alerts;

namespace MiningMonitor.Alerts.Scanners
{
    public class HashrateScanner : AlertScanner
    {
        public override bool ShouldScan(AlertDefinition definition)
        {
            return definition.Parameters.AlertType == AlertType.Hashrate;
        }
        
        public override bool EndAlert(AlertDefinition definition, Miner miner, Alert alert, IEnumerable<Snapshot> snapshots, DateTime scanTime)
        {
            var snapshotsList = snapshots.ToList();

            if (!snapshotsList.Any())
                return false;

            var parameters = (HashrateAlertParameters)definition.Parameters;
            var conditionPeriods = GetConditionPeriods(snapshotsList, parameters).ToList();

            var lastLowCondition = conditionPeriods.LastOrDefault(s => s.Condition == Condition.Low);
            if (lastLowCondition != null)
            {
                alert.Metadata = CreateMetadata(alert, parameters, snapshotsList);
                alert.DetailMessages = CreateDetailMessages(alert, parameters);

                return lastLowCondition.Period.HasEnd;
            }
            return conditionPeriods.Any();
        }

        public override ScanResult PerformScan(IEnumerable<Alert> activeAlerts, AlertDefinition definition, Miner miner, IEnumerable<Snapshot> snapshots, DateTime scanTime)
        {
            if (activeAlerts.Any() || !miner.CollectData)
                return ScanResult.Skip;

            var snapshotsList = snapshots.ToList();
            var parameters = (HashrateAlertParameters)definition.Parameters;
            var duration = parameters.DurationMinutes.MinutesToTimeSpan();

            var outOfRangePeriods = (
                from conditionPeriod in GetConditionPeriods(snapshotsList, parameters)
                where conditionPeriod.Condition == Condition.Low
                select conditionPeriod).ToList();

            var inEffectOutOfRangePeriods =
                from conditionPeriod in outOfRangePeriods
                where !conditionPeriod.Period.HasEnd
                where duration == null || scanTime - conditionPeriod?.Period.Start > duration
                select conditionPeriod;

            var alerts = (
                from conditionPeriod in inEffectOutOfRangePeriods
                select CreateAlert(definition, parameters, snapshotsList)).ToArray();

            if (alerts.Any())
                return ScanResult.Fail(alerts);

            return ScanResult.Success;
        }

        private static IEnumerable<ConditionPeriod> GetConditionPeriods(IEnumerable<Snapshot> snapshotsList, HashrateAlertParameters parameters)
        {
            Condition MapToCondition(Snapshot snapshot)
            {
                if (snapshot.MinerStatistics.Ethereum.Hashrate / 1000m < parameters.MinValue)
                    return Condition.Low;

                return Condition.Ok;
            }

            return
                from conditionPeriod in snapshotsList.ToConditionPeriods(MapToCondition)
                orderby conditionPeriod.Period.Start
                select conditionPeriod;
        }

        private static AlertMetadata CreateMetadata(Alert alert, HashrateAlertParameters parameters, IEnumerable<Snapshot> snapshots)
        {
            var hashRates = snapshots
                .Select(s => (int?)s.MinerStatistics.Ethereum.Hashrate)
                .Union(new[] { alert.Metadata?.Value?.Min, alert.Metadata?.Value?.Max })
                .ToList();

            var metadata = new AlertMetadata
            {
                Value = new AlertValueMetadata
                {
                    Condition = Condition.Low,
                    Min = hashRates.Min(),
                    Max = hashRates.Max()
                },
                Threshold = new AlertThresholdMetadata
                {
                    Min = parameters.MinValue
                }
            };
            return metadata;
        }

        private static IEnumerable<string> CreateDetailMessages(Alert alert, HashrateAlertParameters parameters)
        {
            var durationMessage = parameters.DurationMinutes == null ? null : $" for more than {parameters.DurationMinutes} minute(s)";

            yield return $"Hashrate below threshold of {parameters.MinValue} MH/s{durationMessage}";
            yield return $"Min hashrate speed during alert: {alert.Metadata.Value.Min / 1000m} MH/s";
            yield return $"Max hashrate speed during alert: {alert.Metadata.Value.Max / 1000m} MH/s";
        }

        private static Alert CreateAlert(AlertDefinition definition, HashrateAlertParameters parameters, IEnumerable<Snapshot> snapshots)
        {
            var alert = Alert.CreateFromDefinition(definition, definition.Parameters.AlertMessage ?? "Hashrate too low");

            alert.Metadata = CreateMetadata(alert, parameters, snapshots);
            alert.DetailMessages = CreateDetailMessages(alert, parameters);

            return alert;
        }
    }
}
