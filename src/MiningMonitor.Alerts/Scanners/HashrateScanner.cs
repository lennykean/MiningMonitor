using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MiningMonitor.Common;
using MiningMonitor.Model;
using MiningMonitor.Model.Alerts;

namespace MiningMonitor.Alerts.Scanners
{
    public class HashrateScanner : AlertScanner
    {
        private readonly IAlertFactory _alertFactory;

        public HashrateScanner(IAlertFactory alertFactory)
        {
            _alertFactory = alertFactory;
        }

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
                alert.Metadata = CreateMetadata(alert.Metadata, parameters, snapshotsList);
                alert.DetailMessages = CreateDetailMessages(alert.Metadata, parameters);

                return lastLowCondition.Period.HasEnd;
            }
            return conditionPeriods.Any();
        }

        public override async Task<ScanResult> PerformScanAsync(IEnumerable<Alert> activeAlerts, AlertDefinition definition, Miner miner, IEnumerable<Snapshot> snapshots, DateTime scanTime, CancellationToken token)
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
                select CreateAlertAsync(definition, miner, parameters, snapshotsList, token)).ToArray();

            if (alerts.Any())
                return ScanResult.Fail(await Task.WhenAll(alerts));

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

        private static AlertMetadata CreateMetadata(AlertMetadata existingMetadata, HashrateAlertParameters parameters, IEnumerable<Snapshot> snapshots)
        {
            var hashRates = snapshots
                .Select(s => (int?)s.MinerStatistics.Ethereum.Hashrate)
                .Union(new[] { existingMetadata?.Value?.Min, existingMetadata?.Value?.Max })
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

        private static IEnumerable<string> CreateDetailMessages(AlertMetadata metadata, HashrateAlertParameters parameters)
        {
            var durationMessage = parameters.DurationMinutes == null ? null : $" for more than {parameters.DurationMinutes} minute(s)";

            yield return $"Hashrate below threshold of {parameters.MinValue} MH/s{durationMessage}";
            yield return $"Min hashrate speed during alert: {metadata.Value.Min / 1000m} MH/s";
            yield return $"Max hashrate speed during alert: {metadata.Value.Max / 1000m} MH/s";
        }

        private async Task<Alert> CreateAlertAsync(AlertDefinition definition, Miner miner, HashrateAlertParameters parameters, IEnumerable<Snapshot> snapshots, CancellationToken token)
        {
            var metadata = CreateMetadata(null, parameters, snapshots);
            var detailMessages = CreateDetailMessages(metadata, parameters);

            return await _alertFactory.CreateAlertAsync(definition, miner, metadata, "Hashrate too low", detailMessages, token);
        }
    }
}
