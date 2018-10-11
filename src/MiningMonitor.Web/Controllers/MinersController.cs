using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MiningMonitor.Model;
using MiningMonitor.Service;

namespace MiningMonitor.Web.Controllers
{
    [Route("api/[controller]"), Authorize(Policy = "Basic")]
    public class MinersController : Controller
    {
        private readonly IMinerService _minerService;
        private readonly ICollectorService _collectorService;

        public MinersController(IMinerService minerService, ICollectorService collectorService)
        {
            _minerService = minerService;
            _collectorService = collectorService;
        }

        [HttpGet]
        public async Task<IEnumerable<Miner>> GetAsync(CancellationToken token = default)
        {
            return await _minerService.GetAllAsync(token);
        }

        [HttpGet("{id}")]
        public async Task<ObjectResult> GetAsync(Guid id, CancellationToken token = default)
        {
            var miner = await _minerService.GetByIdAsync(id, token);

            if (miner == null)
                return NotFound(null);

            return Ok(miner);
        }

        [HttpPost]
        public async Task<ObjectResult> PostAsync([FromBody]Miner miner, CancellationToken token = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _minerService.AddAsync(miner, token);

            return Ok(miner);
        }

        [HttpPut]
        public async Task<ObjectResult> PutAsync([FromBody]Miner miner, CancellationToken token = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!await _minerService.UpdateAsync(miner, token))
                return NotFound(null);

            return Ok(miner);
        }

        [HttpDelete("{id}")]
        public async Task<StatusCodeResult> Delete(Guid id, CancellationToken token = default)
        {
            if (!await _minerService.DeleteAsync(id, token))
                return NotFound();

            return NoContent();
        }

        [HttpPost("collector/{collector}"), Authorize(Policy = "Collector")]
        public async Task<StatusCodeResult> PostAsync(string collector, [FromBody]Miner miner, CancellationToken token = default)
        {
            var success = await _collectorService.MinerSyncAsync(collector, miner, token);

            if (!success)
                return BadRequest();

            return Ok();
        }
    }
}