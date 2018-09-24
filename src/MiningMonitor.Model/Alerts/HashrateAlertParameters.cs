namespace MiningMonitor.Model.Alerts
{
    public class HashrateAlertParameters : AlertParameters
    {
        public override AlertType AlertType => AlertType.Hashrate;
        public decimal? MinValue { get; set; }
    }
}
