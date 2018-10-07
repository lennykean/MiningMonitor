using System.Threading;
using System.Threading.Tasks;

namespace MiningMonitor.Scheduler
{
    public interface IBackgroundWorker
    {
        Task DoWorkAsync(CancellationToken cancellationToken);
    }
}