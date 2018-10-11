using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

using MiningMonitor.Model;

namespace MiningMonitor.Service
{
    public class ServerService : IServerService
    {
        private readonly Func<HttpClient> _clientFactory;
        private readonly ISettingsService _settingsService;

        public ServerService(Func<HttpClient> clientFactory, ISettingsService settingsService)
        {
            _clientFactory = clientFactory;
            _settingsService = settingsService;
        }

        public async Task<(string id, string token)> RegisterAsCollectorAsync(CancellationToken token = default)
        {
            var client = await SetupConnectionAsync(authorization: false, token: token);
            var (_, name) = await _settingsService.GetSettingAsync("Name", token);

            var response = await client.PostAsJsonAsync("api/collector", new Collector
            {
                Name = name
            }, token);
            response.EnsureSuccessStatusCode();
            
            var registration = await response.Content.ReadAsAsync<RegistrationResponse>(token);

            return (registration.Id, registration.Token);
        }

        public async Task<bool> CheckApprovalAsync(string id, CancellationToken token = default)
        {
            var client = await SetupConnectionAsync(token: token);

            var response = await client.GetAsync($"api/collector/{id}", token);
            response.EnsureSuccessStatusCode();

            var collector = await response.Content.ReadAsAsync<Collector>(token);

            return collector?.Approved ?? false;
        }

        public async Task SyncMinerAsync(string id, Miner miner, CancellationToken token = default)
        {
            var client = await SetupConnectionAsync(token: token);

            var response = await client.PostAsJsonAsync($"api/miners/collector/{id}", miner, token);
            response.EnsureSuccessStatusCode();
        }

        public async Task SyncSnapshotAsync(string id, Snapshot snapshot, CancellationToken token = default)
        {
            var client = await SetupConnectionAsync(token: token);

            var response = await client.PostAsJsonAsync($"api/snapshots/collector/{id}/{snapshot.MinerId}", snapshot, token);
            response.EnsureSuccessStatusCode();
        }

        private async Task<HttpClient> SetupConnectionAsync(bool authorization = true, CancellationToken token = default)
        {
            var (_, serverUrl) = await _settingsService.GetSettingAsync("ServerUrl", token);
            var client = _clientFactory();

            client.BaseAddress = new Uri(serverUrl);

            if (authorization)
            {
                var (_, serverToken) = await _settingsService.GetSettingAsync("ServerToken", token);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", serverToken);
            }
            return client;
        }
    }
}