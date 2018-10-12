using System;

using LiteDB;

using MiningMonitor.Model.Alerts;

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
        }
    }
}
