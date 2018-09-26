using MiningMonitor.Model.Alerts;

namespace MiningMonitor.Service.Alerts
{
    public interface IScanFactory
    {
        IScan CreateScan(AlertDefinition definition);
    }
}