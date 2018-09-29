using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MiningMonitor.Model.Alerts;
using MiningMonitor.Service.Alerts;

namespace MiningMonitor.Test.Service
{
    [Route("api/[controller]"), Authorize(Policy = "Basic")]
    public class AlertDefinitionsController : Controller
    {
        private readonly IAlertDefinitionService _alertDefinitionService;

        public AlertDefinitionsController(IAlertDefinitionService alertDefinitionService)
        {
            _alertDefinitionService = alertDefinitionService;
        }

        [HttpGet]
        public IEnumerable<AlertDefinition> Get(Guid? minerId = null)
        {
            if (minerId != null)
                return _alertDefinitionService.GetByMiner((Guid)minerId);

            return _alertDefinitionService.GetAll();
        }

        [HttpGet("{id}")]
        public ObjectResult Get(Guid id)
        {
            var alertDefinition = _alertDefinitionService.GetById(id);

            if (alertDefinition == null)
                return NotFound(null);

            return Ok(alertDefinition);
        }

        [HttpPost]
        public ObjectResult Post([FromBody]AlertDefinition alertDefinition)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _alertDefinitionService.Add(alertDefinition);

            return Ok(alertDefinition);
        }

        [HttpPut]
        public ObjectResult Put([FromBody]AlertDefinition alertDefinition)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_alertDefinitionService.Update(alertDefinition))
                return NotFound(null);

            return Ok(alertDefinition);
        }

        [HttpDelete("{id}")]
        public StatusCodeResult Delete(Guid id)
        {
            if (!_alertDefinitionService.Delete(id))
                return NotFound();

            return NoContent();
        }
    }
}
