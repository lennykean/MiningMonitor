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
    public class AlertDefinitionsController : Controller
    {
        private readonly IAlertDefinitionService _alertDefinitionService;

        public AlertDefinitionsController(IAlertDefinitionService alertDefinitionService)
        {
            _alertDefinitionService = alertDefinitionService;
        }

        [HttpGet]
        public async Task<IEnumerable<AlertDefinition>> GetAsync([FromQuery]Guid? minerId = null, CancellationToken token = default)
        {
            if (minerId != null)
                return await _alertDefinitionService.GetByMinerAsync((Guid)minerId, token);

            return await _alertDefinitionService.GetAllAsync(token);
        }

        [HttpGet("{id}")]
        public async Task<ObjectResult> GetAsync(Guid id, CancellationToken token = default)
        {
            var alertDefinition = await _alertDefinitionService.GetByIdAsync(id, token);

            if (alertDefinition == null)
                return NotFound(null);

            return Ok(alertDefinition);
        }

        [HttpPost]
        public async Task<ObjectResult> PostAsync([FromBody]AlertDefinition alertDefinition, CancellationToken token = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _alertDefinitionService.AddAsync(alertDefinition, token);

            return Ok(alertDefinition);
        }

        [HttpPut]
        public async Task<ObjectResult> PutAsync([FromBody]AlertDefinition alertDefinition, CancellationToken token = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!await _alertDefinitionService.UpdateAsync(alertDefinition, token))
                return NotFound(null);

            return Ok(alertDefinition);
        }

        [HttpDelete("{id}")]
        public async Task<StatusCodeResult> DeleteAsync(Guid id, CancellationToken token = default)
        {
            if (!await _alertDefinitionService.DeleteAsync(id, token))
                return NotFound();

            return NoContent();
        }
    }
}
