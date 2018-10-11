using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MiningMonitor.Model.Alerts;
using MiningMonitor.Service;

namespace MiningMonitor.Web.Controllers
{
    [Route("api/[controller]"), Authorize(Policy = "Basic")]
    public class AlertsController : Controller
    {
        private readonly IAlertService _alertService;

        public AlertsController(IAlertService alertService)
        {
            _alertService = alertService;
        }

        [HttpGet]
        public async Task<IEnumerable<Alert>> GetAsync(bool includeAcknowledged = false, CancellationToken token = default)
        {
            return await _alertService.GetAsync(includeAcknowledged, token);
        }

        [HttpGet("miner/{minerId}")]
        public async Task<IEnumerable<Alert>> GetByMinerAsync(Guid minerId, bool includeAcknowledged = false, CancellationToken token = default)
        {
            return await _alertService.GetByMinerAsync(minerId, includeAcknowledged, token);
        }
        
        [HttpGet("{id}")]
        public async Task<ObjectResult> GetAsync(Guid id, CancellationToken token = default)
        {
            var alert = await _alertService.GetByIdAsync(id, token);

            if (alert == null)
                return NotFound(null);

            return Ok(alert);
        }

        [HttpPost("{id}/acknowledge")]
        public async Task<StatusCodeResult> AcknowledgeAsync(Guid id, CancellationToken token = default)
        {
            if (!await _alertService.AcknowledgeAsync(id, token))
                return NotFound();

            return Ok();
        }
    }
}
