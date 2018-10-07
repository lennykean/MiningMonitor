using System.Threading;
using System.Threading.Tasks;

namespace MiningMonitor.Workers
{
    public abstract class SynchronousWorker : IWorker
    {
        public async Task DoWorkAsync(CancellationToken cancellationToken)
        {
            await Task.Run(() => DoWork(), cancellationToken);
        }

        protected abstract void DoWork();
    }
}