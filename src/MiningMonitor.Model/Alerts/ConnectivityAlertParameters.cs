namespace MiningMonitor.Model.Alerts
{
    public class ConnectivityAlertParameters : AlertParameters
    {
        public override AlertType AlertType => AlertType.Connectivity;

        public int? DurationMinutes { get; set; }
    }
}
