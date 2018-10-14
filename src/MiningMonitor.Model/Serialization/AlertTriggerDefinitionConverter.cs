using System;

using MiningMonitor.Model.Alerts;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MiningMonitor.Model.Serialization
{
    public class AlertTriggerDefinitionConverter : JsonConverter
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
            var value = jsonObject.GetValue(nameof(AlertTriggerDefinition.Type), StringComparison.OrdinalIgnoreCase);

            if (value.Type == JTokenType.Null)
                return null;

            AlertTriggerDefinition trigger;
            switch ((TriggerType)value.Value<int>())
            {
                case TriggerType.DisableGpu:
                    trigger = new DisableGpuAlertTriggerDefinition();
                    break;
                case TriggerType.RestartMiner:
                    trigger = new RestartMinerAlertTriggerDefinition();
                    break;
                case TriggerType.WebHook:
                    trigger = new WebHookAlertTriggerDefinition();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            serializer.Populate(jsonObject.CreateReader(), trigger);

            return trigger;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(AlertTriggerDefinition);
        }
    }
}
