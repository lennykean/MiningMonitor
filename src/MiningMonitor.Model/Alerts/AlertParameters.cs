namespace MiningMonitor.Model.Alerts
{
    public abstract class AlertParameters
    {
        public abstract AlertType AlertType { get; }
        public string AlertMessage { get; set; }
        public int? DurationMinutes { get; set; }
    }
}
