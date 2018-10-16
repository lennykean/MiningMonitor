namespace MiningMonitor.Model.Alerts.Actions
{
    public class AlertActionResult
    {
        public AlertActionState State { get; set; }
        public string ActionName { get; set; }
        public string Message { get; set; }

        public static AlertActionResult Skip(string name, string message)
        {
            return new AlertActionResult
            {
                State = AlertActionState.Skipped,
                ActionName = name,
                Message = message
            };
        }

        public static AlertActionResult Complete(string name, string message)
        {
            return new AlertActionResult
            {
                State = AlertActionState.Completed,
                ActionName = name,
                Message = message
            };
        }

        public static AlertActionResult Error(string name, string message)
        {
            return new AlertActionResult
            {
                State = AlertActionState.Error,
                ActionName = name,
                Message = message
            };
        }
    }
}