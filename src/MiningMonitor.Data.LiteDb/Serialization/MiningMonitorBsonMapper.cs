using System;

using LiteDB;

using MiningMonitor.Model.Alerts;
using MiningMonitor.Model.Alerts.Actions;

namespace MiningMonitor.Data.LiteDb.Serialization
{
    public class MiningMonitorBsonMapper : BsonMapper
    {
        public MiningMonitorBsonMapper(Func<Type, object> customTypeInstantiator = null) : base(customTypeInstantiator)
        {
            RegisterType<AlertParameters>(
                parameters => ToDocument(parameters.GetType(), parameters),
                bson =>
                {
                    var document = bson.AsDocument;
                    if (!Enum.TryParse<AlertType>(document[nameof(AlertParameters.AlertType)].AsString, out var alertType))
                        return null;

                    switch (alertType)
                    {
                        case AlertType.Hashrate:
                            return ToObject<HashrateAlertParameters>(document);
                        case AlertType.GpuThreshold:
                            return ToObject<GpuThresholdAlertParameters>(document);
                        case AlertType.Connectivity:
                            return ToObject<ConnectivityAlertParameters>(document);
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                });
            RegisterType<AlertActionDefinition>(
                parameters => ToDocument(parameters.GetType(), parameters),
                bson =>
                {
                    var document = bson.AsDocument;
                    if (!Enum.TryParse<AlertActionType>(document[nameof(AlertActionDefinition.Type)].AsString, out var action))
                        return null;

                    switch (action)
                    {
                        case AlertActionType.DisableGpu:
                            return ToObject<DisableGpuAlertActionDefinition>(document);
                        case AlertActionType.RestartMiner:
                            return ToObject<RestartMinerAlertActionDefinition>(document);
                        case AlertActionType.WebHook:
                            return ToObject<WebHookAlertActionDefinition>(document);
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                });
        }
    }
}
