using MiningMonitor.Model.Alerts.Actions;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;

namespace MiningMonitor.Model.Serialization
{
    public class AlertActionDefinitionConverter : JsonConverter
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
            var value = jsonObject.GetValue(nameof(AlertActionDefinition.Type), StringComparison.OrdinalIgnoreCase);

            if (value.Type == JTokenType.Null)
                return null;

            AlertActionDefinition action;
            switch ((AlertActionType)value.Value<int>())
            {
                case AlertActionType.DisableGpu:
                    action = new DisableGpuAlertActionDefinition();
                    break;
                case AlertActionType.RestartMiner:
                    action = new RestartMinerAlertActionDefinition();
                    break;
                case AlertActionType.WebHook:
                    action = new WebHookAlertActionDefinition();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            serializer.Populate(jsonObject.CreateReader(), action);

            return action;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(AlertActionDefinition);
        }
    }
}
