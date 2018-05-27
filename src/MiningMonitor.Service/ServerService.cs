using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
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

        public async Task<string> RegisterAsCollectorAsync()
        {
            var client = await SetupConnection(authorization: false);
            var (_, name) = await _settingsService.GetSettingAsync("Name");

            var response = await client.PostAsJsonAsync("api/collector", new Collector
            {
                Name = name
            });
            response.EnsureSuccessStatusCode();
            
            var registration = await response.Content.ReadAsAsync<RegistrationResponse>();
            await _settingsService.UpdateSettingsAsync(new Dictionary<string, string>
            {
                ["CollectorId"] = registration.Id,
                ["ServerToken"] = registration.Token
            });

            return registration.Id;
        }

        public async Task<bool> CheckApprovalAsync(string id)
        {
            var client = await SetupConnection();

            var response = await client.GetAsync($"api/collector/{id}");
            response.EnsureSuccessStatusCode();

            var collector = await response.Content.ReadAsAsync<Collector>();

            return collector?.Approved ?? false;
        }

        public async Task SyncMinerAsync(string id, Miner miner)
        {
            var client = await SetupConnection();

            var response = await client.PostAsJsonAsync($"api/miners/collector/{id}", miner);
            response.EnsureSuccessStatusCode();
        }

        public async Task SyncSnapshotAsync(string id, Snapshot snapshot)
        {
            var client = await SetupConnection();

            var response = await client.PostAsJsonAsync($"api/snapshots/collector/{id}/{snapshot.MinerId}", snapshot);
            response.EnsureSuccessStatusCode();
        }

        private async Task<HttpClient> SetupConnection(bool authorization = true)
        {
            var (_, serverUrl) = await _settingsService.GetSettingAsync("ServerUrl");
            var client = _clientFactory();

            client.BaseAddress = new Uri(serverUrl);

            if (authorization)
            {
                var (_, serverToken) = await _settingsService.GetSettingAsync("ServerToken");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", serverToken);
            }
            return client;
        }
    }
}