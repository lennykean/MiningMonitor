using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MiningMonitor.Model;
using MiningMonitor.Service;

namespace MiningMonitor.Web.Controllers
{
    [Route("api/[controller]")]
    public class CollectorController : Controller
    {
        private readonly ICollectorService _collectorService;

        public CollectorController(ICollectorService collectorService)
        {
            _collectorService = collectorService;
        }

        [HttpGet, Authorize(Policy = "Basic")]
        public async Task<IEnumerable<Collector>> GetAsync(CancellationToken token = default)
        {
            return await _collectorService.GetAllAsync(token);
        }

        [HttpGet("{collector}"), Authorize(Policy = "Collector")]
        public async Task<IActionResult> GetAsync(string collector, CancellationToken token = default)
        {
            var (success, collectorObj) = await _collectorService.GetAsync(collector, token);

            if (!success)
                return NotFound();

            return Ok(collectorObj);
        }

        [HttpPut, Authorize(Policy = "Basic")]
        public async Task<IActionResult> PutAsync([FromBody]Collector collector, CancellationToken token = default)
        {
            var (result, found) = await _collectorService.UpdateAsync(collector, token);

            if (!found)
                return NotFound();
            if (!result.IsValid)
                return BadRequest(result);

            return Ok(collector);
        }

        [HttpPost]
        public async Task<ObjectResult> PostAsync([FromBody]Collector collector, CancellationToken token = default)
        {
            var (result, response) = await _collectorService.CreateCollectorAsync(collector, token);

            if (!result.IsValid)
                return BadRequest(result);

            return Ok(response);
        }

        [HttpDelete("{collector}"), Authorize(Policy = "Basic")]
        public async Task<StatusCodeResult> DeleteAsync(string collector, CancellationToken token = default)
        {
            if (!await _collectorService.DeleteAsync(collector, token))
                return NotFound();

            return NoContent();
        }
    }
}