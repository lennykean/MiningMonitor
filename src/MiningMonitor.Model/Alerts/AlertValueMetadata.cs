namespace MiningMonitor.Model.Alerts
{
    public class AlertValueMetadata
    {
        public GpuThresholdState? GpuThresholdState { get; set; }
        public int? Min { get; set; }
        public int? Max { get; set; }
    }
}