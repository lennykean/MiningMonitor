using System;
using System.Collections.Generic;
using System.Linq;

using ClaymoreMiner.RemoteManagement.Models;

using MiningMonitor.Model;
using MiningMonitor.Model.Alerts;
using MiningMonitor.Scheduler;
using MiningMonitor.Service.Alerts.Scanners;

namespace MiningMonitor.Service.Alerts
{
    public abstract class GpuThresholdScanner : AlertScanner
    {
        protected GpuThresholdScanner(SnapshotDataCollectorSchedule snapshotDataCollectorSchedule) : base(snapshotDataCollectorSchedule)
        {
        }

        public abstract string DefaultAlertMessage { get; }

        public override Period CalculateScanPeriod(AlertDefinition definition, DateTime scanTime)
        {
            var durationMinutes = ((GpuThresholdParameters)definition.Parameters).DurationMinutes;
            var duration = durationMinutes != null
                ? TimeSpan.FromMinutes((int)durationMinutes)
                : default(TimeSpan?);

            return CalculateScanPeriod(definition, duration, scanTime);
        }

        public override bool EndAlert(AlertDefinition definition, Miner miner, Alert alert, IEnumerable<Snapshot> snapshots, DateTime scanTime)
        {
            var snapshotsList = snapshots.ToList();

            if (!snapshotsList.Any())
                return false;

            var parameters = (GpuThresholdParameters)definition.Parameters;

            var statePeriods = (
                from gpu in snapshotsList.SelectGpuThresholdStatePeriods(stats => SelectAlertState(stats, parameters))
                from gpuStatePeriod in gpu.StatePeriods
                where gpuStatePeriod.GpuIndex == alert.Metadata?.GpuIndex
                orderby gpuStatePeriod.Period.Start
                select gpuStatePeriod).ToList();

            var statePeriod = statePeriods.LastOrDefault(s => s.State == alert.Metadata?.Value?.GpuThresholdState);
            if (statePeriod != null)
            {
                alert.Metadata = CreateMetadata(alert, parameters, statePeriod);
                alert.DetailMessages = CreateDetailMessages(alert, parameters);
                return statePeriod.Period.HasEnd;
            }
            return statePeriods.Any();
        }

        public override ScanResult PerformScan(IEnumerable<Alert> activeAlerts, AlertDefinition definition, Miner miner, IEnumerable<Snapshot> snapshots, DateTime scanTime)
        {
            if (!miner.CollectData)
                return ScanResult.Skip;

            var parameters = (GpuThresholdParameters)definition.Parameters;
            var durationMinutes = parameters.DurationMinutes;
            var duration = durationMinutes != null
                ? TimeSpan.FromMinutes((int)durationMinutes)
                : default(TimeSpan?);

            var outOfRangePeriods = 
                from statePeriods in snapshots.SelectGpuThresholdStatePeriods(stats => SelectAlertState(stats, parameters))
                let lastOutOfRangePeriod = (
                    from statePeriod in statePeriods.StatePeriods
                    where statePeriod.State != GpuThresholdState.Ok
                    orderby statePeriod.Period.Start
                    select statePeriod).LastOrDefault()
                where lastOutOfRangePeriod != null
                select lastOutOfRangePeriod;
            
            var inEffectOutOfRangePeriods = 
                from statePeriod in outOfRangePeriods
                where !statePeriod.Period.HasEnd
                where duration == null || scanTime - statePeriod?.Period.Start > duration
                select statePeriod;
            
            var alerts = (
                from statePeriod in inEffectOutOfRangePeriods
                where activeAlerts.All(activeAlert => activeAlert.Metadata?.GpuIndex != statePeriod.GpuIndex)
                select CreateAlert(definition, parameters, statePeriod)).ToList();

            if (alerts.Any())
                return ScanResult.Fail(alerts);

            return ScanResult.Success;
        }

        private Alert CreateAlert(AlertDefinition definition, GpuThresholdParameters parameters, GpuThresholdPeriod statePeriod)
        {
            var alert = Alert.CreateFromDefinition(definition, definition.Parameters.AlertMessage ?? DefaultAlertMessage);

            alert.Metadata = CreateMetadata(alert, parameters, statePeriod);
            alert.DetailMessages = CreateDetailMessages(alert, parameters);

            return alert;
        }

        protected abstract GpuThresholdState SelectAlertState(GpuStats stats, GpuThresholdParameters parameters);

        protected abstract AlertMetadata CreateMetadata(Alert alert, GpuThresholdParameters parameters, GpuThresholdPeriod statePeriod);
        
        protected abstract IEnumerable<string> CreateDetailMessages(Alert alert, GpuThresholdParameters parameters);
    }
}
