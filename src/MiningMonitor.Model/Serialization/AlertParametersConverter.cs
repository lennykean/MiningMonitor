using System;

using MiningMonitor.Model.Alerts;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MiningMonitor.Model.Serialization
{
    public class AlertParametersConverter : JsonConverter
    {
        public override bool CanRead => true;
        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            var value = jsonObject.GetValue(nameof(AlertParameters.AlertType), StringComparison.OrdinalIgnoreCase);

            AlertParameters parameters;
            
            switch ((AlertType)value.Value<int>())
            {
                case AlertType.Hashrate:
                    parameters = new HashrateAlertParameters();
                    break;
                case AlertType.Connectivity:
                    parameters = new ConnectivityAlertParameters();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            serializer.Populate(jsonObject.CreateReader(), parameters);

            return parameters;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(AlertParameters);
        }
    }
}
