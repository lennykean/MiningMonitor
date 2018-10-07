using System;
using System.Collections.Generic;
using System.Linq;

using ClaymoreMiner.RemoteManagement.Models;

using MiningMonitor.BackgroundScheduler;
using MiningMonitor.Common;
using MiningMonitor.Model;
using MiningMonitor.Model.Alerts;

namespace MiningMonitor.Alerts.Scanners
{
    public abstract class GpuThresholdScanner : AlertScanner
    {
        protected GpuThresholdScanner(DataCollectorSchedule dataCollectorSchedule) : base(dataCollectorSchedule)
        {
        }

        public abstract string DefaultAlertMessage { get; }

        public override bool EndAlert(AlertDefinition definition, Miner miner, Alert alert, IEnumerable<Snapshot> snapshots, DateTime scanTime)
        {
            var snapshotsList = snapshots.ToList();

            if (!snapshotsList.Any())
                return false;

            var parameters = (GpuThresholdParameters)definition.Parameters;

            var conditionPeriods = (
                from gpuCondition in snapshotsList.ToGpuConditions(stats => MapToCondition(stats, parameters))
                from gpuConditionPeriod in gpuCondition.Periods
                where gpuConditionPeriod.GpuIndex == alert.Metadata?.GpuIndex
                orderby gpuConditionPeriod.Period.Start
                select gpuConditionPeriod).ToList();

            var conditionPeriod = conditionPeriods.LastOrDefault(s => s.Condition == alert.Metadata?.Value?.Condition);
            if (conditionPeriod != null)
            {
                alert.Metadata = CreateMetadata(alert, parameters, conditionPeriod);
                alert.DetailMessages = CreateDetailMessages(alert, parameters);
                return conditionPeriod.Period.HasEnd;
            }
            return conditionPeriods.Any();
        }

        public override ScanResult PerformScan(IEnumerable<Alert> activeAlerts, AlertDefinition definition, Miner miner, IEnumerable<Snapshot> snapshots, DateTime scanTime)
        {
            if (!miner.CollectData)
                return ScanResult.Skip;

            var parameters = (GpuThresholdParameters)definition.Parameters;
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
                select CreateAlert(definition, parameters, conditionPeriod)).ToList();

            if (alerts.Any())
                return ScanResult.Fail(alerts);

            return ScanResult.Success;
        }

        private Alert CreateAlert(AlertDefinition definition, GpuThresholdParameters parameters, GpuConditionPeriod conditionPeriod)
        {
            var alert = Alert.CreateFromDefinition(definition, definition.Parameters.AlertMessage ?? DefaultAlertMessage);

            alert.Metadata = CreateMetadata(alert, parameters, conditionPeriod);
            alert.DetailMessages = CreateDetailMessages(alert, parameters);

            return alert;
        }

        protected abstract Condition MapToCondition(GpuStats stats, GpuThresholdParameters parameters);

        protected abstract AlertMetadata CreateMetadata(Alert alert, GpuThresholdParameters parameters, GpuConditionPeriod conditionPeriod);
        
        protected abstract IEnumerable<string> CreateDetailMessages(Alert alert, GpuThresholdParameters parameters);
    }
}
