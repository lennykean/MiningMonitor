using System.Collections.Generic;
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
        public async Task<IDictionary<string, string>> Get()
        {
            return await _settingsService.GetAllAsync();
        }

        [HttpPut]
        public async Task<ObjectResult> Put([FromBody]IDictionary<string, string> settings)
        {
            var result = await _settingsService.UpdateSettingsAsync(settings);

            if (!result.success)
                return NotFound(null);

            return Ok(result.settings);
        }
    }
}