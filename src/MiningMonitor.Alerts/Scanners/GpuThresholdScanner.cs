using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ClaymoreMiner.RemoteManagement.Models;

using MiningMonitor.Common;
using MiningMonitor.Model;
using MiningMonitor.Model.Alerts;

namespace MiningMonitor.Alerts.Scanners
{
    public abstract class GpuThresholdScanner : AlertScanner
    {
        private readonly IAlertFactory _alertFactory;

        protected GpuThresholdScanner(IAlertFactory alertFactory)
        {
            _alertFactory = alertFactory;
        }

        public abstract string DefaultAlertMessage { get; }

        public override bool EndAlert(AlertDefinition definition, Miner miner, Alert alert, IEnumerable<Snapshot> snapshots, DateTime scanTime)
        {
            var snapshotsList = snapshots.ToList();

            if (!snapshotsList.Any())
                return false;

            var parameters = (GpuThresholdAlertParameters)definition.Parameters;

            var conditionPeriods = (
                from gpuCondition in snapshotsList.ToGpuConditions(stats => MapToCondition(stats, parameters))
                from gpuConditionPeriod in gpuCondition.Periods
                where gpuConditionPeriod.GpuIndex == alert.Metadata?.GpuIndex
                orderby gpuConditionPeriod.Period.Start
                select gpuConditionPeriod).ToList();

            var conditionPeriod = conditionPeriods.LastOrDefault(s => s.Condition == alert.Metadata?.Value?.Condition);
            if (conditionPeriod != null)
            {
                alert.Metadata = CreateMetadata(alert?.Metadata, parameters, conditionPeriod);
                alert.DetailMessages = CreateDetailMessages(alert?.Metadata, parameters);
                return conditionPeriod.Period.HasEnd;
            }
            return conditionPeriods.Any();
        }

        public override async Task<ScanResult> PerformScanAsync(IEnumerable<Alert> activeAlerts, AlertDefinition definition, Miner miner, IEnumerable<Snapshot> snapshots, DateTime scanTime, CancellationToken token)
        {
            if (!miner.CollectData)
                return ScanResult.Skip;

            var parameters = (GpuThresholdAlertParameters)definition.Parameters;
            var duration = parameters.DurationMinutes.MinutesToTimeSpan();

            var outOfRangePeriods = 
                from gpuConditions in snapshots.ToGpuConditions(stats => MapToCondition(stats, parameters))
                let lastOutOfRangePeriod = (
                    from conditionPeriod in gpuConditions.Periods
                    where conditionPeriod.Condition != Condition.Ok
                    orderby conditionPeriod.Period.Start
                    select conditionPeriod).LastOrDefault()
                where lastOutOfRangePeriod != null
                select lastOutOfRangePeriod;
            
            var inEffectOutOfRangePeriods = 
                from conditionPeriod in outOfRangePeriods
                where !conditionPeriod.Period.HasEnd
                where duration == null || scanTime - conditionPeriod?.Period.Start > duration
                select conditionPeriod;
            
            var alerts = (
                from conditionPeriod in inEffectOutOfRangePeriods
                where activeAlerts.All(activeAlert => activeAlert.Metadata?.GpuIndex != conditionPeriod.GpuIndex)
                select CreateAlertAsync(definition, miner, parameters, conditionPeriod, token)).ToArray();

            if (alerts.Any())
                return ScanResult.Fail(await Task.WhenAll(alerts));

            return ScanResult.Success;
        }

        private async Task<Alert> CreateAlertAsync(AlertDefinition definition, Miner miner, GpuThresholdAlertParameters parameters, GpuConditionPeriod conditionPeriod, CancellationToken token)
        {
            var metadata = CreateMetadata(null, parameters, conditionPeriod);
            var detailMessages = CreateDetailMessages(metadata, parameters);

            return await _alertFactory.CreateAlertAsync(definition, miner, metadata, DefaultAlertMessage, detailMessages, token);
        }

        protected abstract Condition MapToCondition(GpuStats stats, GpuThresholdAlertParameters parameters);

        protected abstract AlertMetadata CreateMetadata(AlertMetadata existingMetadata, GpuThresholdAlertParameters parameters, GpuConditionPeriod conditionPeriod);
        
        protected abstract IEnumerable<string> CreateDetailMessages(AlertMetadata metadata, GpuThresholdAlertParameters parameters);
    }
}
