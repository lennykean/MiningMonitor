using System.Threading.Tasks;

namespace MiningMonitor.Service
{
    public interface ILoginService
    {
        Task<(bool success, string token)> LoginUserAsync(string username, string password);
        string CreateToken(string username);
    }
}