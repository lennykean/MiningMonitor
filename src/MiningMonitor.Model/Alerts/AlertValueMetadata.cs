namespace MiningMonitor.Model.Alerts
{
    public class AlertValueMetadata
    {
        public Condition? Condition { get; set; }
        public int? Min { get; set; }
        public int? Max { get; set; }
    }
}