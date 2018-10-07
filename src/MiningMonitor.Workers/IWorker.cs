using System.Threading;
using System.Threading.Tasks;

namespace MiningMonitor.Workers
{
    public interface IWorker
    {
        Task DoWorkAsync(CancellationToken cancellationToken);
    }
}