namespace MiningMonitor.Model.Alerts
{
    public class TriggerResult
    {
        public TriggerState State { get; set; }
        public string TriggerName { get; set; }
        public string Message { get; set; }

        public static TriggerResult Skip(string name, string message)
        {
            return new TriggerResult
            {
                State = TriggerState.Skipped,
                TriggerName = name,
                Message = message
            };
        }

        public static TriggerResult Complete(string name, string message)
        {
            return new TriggerResult
            {
                State = TriggerState.Completed,
                TriggerName = name,
                Message = message
            };
        }

        public static TriggerResult Error(string name, string message)
        {
            return new TriggerResult
            {
                State = TriggerState.Error,
                TriggerName = name,
                Message = message
            };
        }
    }
}