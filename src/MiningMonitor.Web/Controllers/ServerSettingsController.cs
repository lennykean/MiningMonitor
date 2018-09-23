using System.Collections.Generic;
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
        public IDictionary<string, string> Get()
        {
            return _settingsService.GetAll();
        }

        [HttpGet("{setting}")]
        public ObjectResult Get(string setting)
        {
            var result = _settingsService.GetSetting(setting);

            if (!result.success)
                return NotFound(null);

            return Ok(result.setting);
        }

        [HttpPut]
        public ObjectResult Put([FromBody]IDictionary<string, string> settings)
        {
            var result = _settingsService.UpdateSettings(settings);

            if (!result.success)
                return NotFound(null);

            return Ok(result.settings);
        }
    }
}