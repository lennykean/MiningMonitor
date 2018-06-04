using System;
using System.Collections.Generic;
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
        public async Task<IEnumerable<Miner>> Get()
        {
            return await _minerService.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ObjectResult> Get(Guid id)
        {
            var miner = await _minerService.GetByIdAsync(id);

            if (miner == null)
                return NotFound(null);

            return Ok(miner);
        }

        [HttpPost]
        public async Task<ObjectResult> Post([FromBody]Miner miner)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _minerService.AddAsync(miner);

            return Ok(miner);
        }

        [HttpPut]
        public async Task<ObjectResult> Put([FromBody]Miner miner)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!await _minerService.UpdateAsync(miner))
                return NotFound(null);

            return Ok(miner);
        }

        [HttpDelete("{id}")]
        public async Task<StatusCodeResult> Delete(Guid id)
        {
            if (!await _minerService.DeleteAsync(id))
                return NotFound();

            return NoContent();
        }

        [HttpPost("collector/{collector}"), Authorize(Policy = "Collector")]
        public async Task<StatusCodeResult> Post(string collector, [FromBody]Miner miner)
        {
            var succcess = await _collectorService.MinerSyncAsync(collector, miner);

            if (!succcess)
                return BadRequest();

            return Ok();
        }
    }
}