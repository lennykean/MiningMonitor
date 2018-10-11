using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MiningMonitor.Service;

namespace MiningMonitor.Web.Controllers
{
    [Route("api/[controller]"), Authorize(Policy = "Basic")]
    public class ServerSettingsController : Controller
    {
        private readonly ISettingsService _settingsService;

        public ServerSettingsController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        [HttpGet]
        public async Task<IDictionary<string, string>> GetAsync(CancellationToken token = default)
        {
            return await _settingsService.GetAllAsync(token);
        }

        [HttpGet("{setting}")]
        public async Task<ObjectResult> GetAsync(string setting, CancellationToken token = default)
        {
            var result = await _settingsService.GetSettingAsync(setting, token);

            if (!result.success)
                return NotFound(null);

            return Ok(result.setting);
        }

        [HttpPut]
        public async Task<ObjectResult> Put([FromBody]IDictionary<string, string> settings, CancellationToken token = default)
        {
            var result = await _settingsService.UpdateSettingsAsync(settings, token);

            if (!result.success)
                return NotFound(null);

            return Ok(result.settings);
        }
    }
}