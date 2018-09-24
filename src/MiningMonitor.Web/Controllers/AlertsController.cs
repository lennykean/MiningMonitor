using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MiningMonitor.Model.Alerts;
using MiningMonitor.Service.Alerts;

namespace MiningMonitor.Test.Service
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
        public IEnumerable<Alert> Get(bool includeAcknowledged = false)
        {
            return _alertService.Get(includeAcknowledged);
        }

        [HttpGet("miner/{minerId}")]
        public IEnumerable<Alert> GetByMiner(Guid minerId, bool includeAcknowledged = false)
        {
            return _alertService.GetByMiner(minerId, includeAcknowledged);
        }
        
        [HttpGet("{id}")]
        public ObjectResult Get(Guid id)
        {
            var alert = _alertService.GetById(id);

            if (alert == null)
                return NotFound(null);

            return Ok(alert);
        }

        [HttpPost("{id}/acknowledge")]
        public StatusCodeResult Acknowledge(Guid id)
        {
            if (!_alertService.Acknowledge(id))
                return NotFound();

            return Ok();
        }
    }
}
