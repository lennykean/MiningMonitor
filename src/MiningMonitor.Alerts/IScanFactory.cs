using System;

using MiningMonitor.Model;
using MiningMonitor.Model.Alerts;

namespace MiningMonitor.Alerts
{
    public interface IScanFactory
    {
        IScan CreateScan(AlertDefinition definition, Miner miner, DateTime scanTime);
    }
}