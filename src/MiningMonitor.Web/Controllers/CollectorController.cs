using System.Collections.Generic;
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
        public async Task<IEnumerable<Collector>> Get()
        {
            return await _collectorService.GetAllAsync();
        }

        [HttpGet("{collector}"), Authorize(Policy = "Collector")]
        public async Task<Collector> Get(string collector)
        {
            return await _collectorService.Get(collector);
        }

        [HttpPut, Authorize(Policy = "Basic")]
        public async Task<IActionResult> Put([FromBody]Collector collector)
        {
            var success = await _collectorService.UpdateAsync(collector);

            if (!success)
                return NotFound();

            return Ok(collector);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Collector collector)
        {
            var (result, token) = await _collectorService.CreateCollectorAsync(collector);

            if (!result.IsValid)
                return BadRequest(result);

            return Ok(token);
        }
    }
}