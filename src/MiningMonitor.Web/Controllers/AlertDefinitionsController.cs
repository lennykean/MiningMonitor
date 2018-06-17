using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MiningMonitor.Model;
using MiningMonitor.Service;

namespace MiningMonitor.Test.Service
{
    [Route("api/[controller]"), Authorize(Policy = "Basic")]
    public class AlertDefinitionsController : Controller
    {
        private readonly IAlertService _alertService;

        public AlertDefinitionsController(IAlertService alertService)
        {
            _alertService = alertService;
        }

        [HttpGet("miner/{minerId}")]
        public IEnumerable<AlertDefinition> GetByMiner(Guid minerId)
        {
            return _alertService.GetDefinitionsByMiner(minerId);
        }

        [HttpGet("{id}")]
        public ObjectResult Get(Guid id)
        {
            var alertDefinition = _alertService.GetDefinition(id);

            if (alertDefinition == null)
                return NotFound(null);

            return Ok(alertDefinition);
        }

        [HttpPost]
        public ObjectResult Post([FromBody]AlertDefinition alertDefinition)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _alertService.Add(alertDefinition);

            return Ok(alertDefinition);
        }

        [HttpPut]
        public ObjectResult Put([FromBody]AlertDefinition alertDefinition)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_alertService.Update(alertDefinition))
                return NotFound(null);

            return Ok(alertDefinition);
        }

        [HttpDelete("{id}")]
        public StatusCodeResult Delete(Guid id)
        {
            if (!_alertService.DeleteDefiniton(id))
                return NotFound();

            return NoContent();
        }
    }
}
