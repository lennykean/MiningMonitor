using System.Threading;
using System.Threading.Tasks;

namespace MiningMonitor.BackgroundWorker
{
    public interface IBackgroundWorker
    {
        Task DoWorkAsync(CancellationToken cancellationToken);
    }
}