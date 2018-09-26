using System;

using LiteDB;

using MiningMonitor.Model.Alerts;

namespace MiningMonitor.Model.Serialization
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
                    var alertType = (AlertType)Enum.Parse(typeof(AlertType), document[nameof(AlertParameters.AlertType)].AsString);

                    switch (alertType)
                    {
                        case AlertType.Hashrate:
                            return ToObject<HashrateAlertParameters>(document);
                        case AlertType.GpuThreshold:
                            return ToObject<GpuThresholdParameters>(document);
                        case AlertType.Connectivity:
                            return ToObject<ConnectivityAlertParameters>(document);
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                });
        }
    }
}
